using System.Collections.Generic;



namespace TGC.Group.Model
{
    public class t_Jalapenio : t_Planta
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\Pimiento-TgcScene.xml";
        private const string PATH_OBJ_FUEGO = "..\\..\\Media\\Objetos\\fuego-TgcScene.xml";
        private const string PATH_TEXTURA_ON = "..\\..\\Media\\Texturas\\pimiento_hudSelect.png";
        private const string PATH_TEXTURA_OFF = "..\\..\\Media\\Texturas\\pimiento_hud.png";
        private const float PLANTA_INIT_Y = 10;
        private const int PLANTA_VALOR = 125;
        private const float VIDA_PLANTA = 3;
        private const float SUPER_TIEMPO_PREV = 3;
        private const float SUPER_TIEMPO_POST = 4;
        private const float DANIO_SUPER = 15;










        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public class t_JalapenioInstancia
        {
            public GameModel _game;
            // SHADER FUEGO
            private float _superTiempoPrev = 0;
            private float _superTiempoPost = 0;
            public int filaFuego = 0;
            public t_Objeto3D _FuegoObj = null;
            public t_Objeto3D _JalapenioPlanta;
            public t_Objeto3D.t_instancia _Jalapenio;
            public t_PlantaInstancia _planta;
            public bool fuego = false;
            public bool killMe = false;

            public t_JalapenioInstancia(GameModel game, t_PlantaInstancia planta, t_Objeto3D.t_instancia Jalapenio, t_Objeto3D JalapenioPlanta)
            {
                _game = game;
                _planta = planta;
                _Jalapenio = Jalapenio;
                _JalapenioPlanta = JalapenioPlanta;

                _FuegoObj = t_Objeto3D.Crear(_game, PATH_OBJ_FUEGO);
                _FuegoObj.Set_Size(1, 0.5F, 0.5F);

                for (int i = GameModel.CANT_COLUMNAS + 1; i >= 0; i--)
                {
                    _FuegoObj.Inst_CreateAndSelect(t_EscenarioBase.PASTO_POS_X_INICIAL + _planta.fila * t_EscenarioBase.PASTO_RAZON * t_EscenarioBase.PASTO_AJUSTE - i * 0.1F,
                                                    0,
                                                    t_EscenarioBase.PASTO_POS_Z_INICIAL + (i - 1) * t_EscenarioBase.PASTO_RAZON);
                }
            }

            public void super()
            {
                _superTiempoPrev = SUPER_TIEMPO_PREV;
                _superTiempoPost = 0;
                _JalapenioPlanta.Inst_Select(_Jalapenio);
                _game.shader.timeExplota = 0;
                _JalapenioPlanta.Inst_ShaderJalapenioExplota(true);
            }

            public void update(bool ShowBoundingBoxWithKey)
            {
                _FuegoObj.Update(ShowBoundingBoxWithKey);

                if (_game._camara.Modo_Is_CamaraPersonal())
                {
                    _FuegoObj.Inst_ShaderAllFuegoJalapenio(false);
                    _FuegoObj.Inst_ShaderAllFuegoJalapenioGirado(true);
                }
                else
                {
                    _FuegoObj.Inst_ShaderAllFuegoJalapenioGirado(false);
                    _FuegoObj.Inst_ShaderAllFuegoJalapenio(true);
                }


                // SUPER: PREVIO A EXPLOTAR
                if (!fuego)
                {
                    if (_superTiempoPrev > 0)
                    {
                        _superTiempoPrev -= _game.ElapsedTime;
                    }
                    else
                    {
                        _superTiempoPrev = 0;
                        _superTiempoPost = SUPER_TIEMPO_POST;
                        fuego = true;

                        _JalapenioPlanta.Inst_Delete(_Jalapenio);
                        _game._EscenarioBase.Set_PastoDesocupado(_planta.fila, _planta.columna);

                        QuemarZombies(_game._zombie, _planta.fila, true);
                        QuemarZombies(_game._zombieCono, _planta.fila, true);
                        QuemarZombies(_game._zombieBalde, _planta.fila, true);
                    }
                }

                // SUPER: LUEGO DE EXPLOTAR
                if (fuego)
                {
                    if (_superTiempoPost > 0)
                    {
                        _superTiempoPost -= _game.ElapsedTime;
                    }
                    else
                    {
                        _superTiempoPost = 0;
                        fuego = false;

                        QuemarZombies(_game._zombie, _planta.fila, false);
                        QuemarZombies(_game._zombieCono, _planta.fila, false);
                        QuemarZombies(_game._zombieBalde, _planta.fila, false);

                        killMe = true;
                    }
                }
            }

            private void QuemarZombies(t_ZombieComun zombies, int FilaCenter, bool quemar)
            {
                for (int i = zombies._InstZombie.Count - 1; i >= 0; i--)
                {
                    t_ZombieComun.t_ZombieInstancia zombie = zombies._InstZombie[i];

                    if (zombie.fila == FilaCenter)
                    {
                        zombie.vida -= DANIO_SUPER;
                        zombies._InstZombie[i] = zombie;
                        zombies._Zombie.Inst_Select(zombie.zombie);
                        zombies._Zombie.Inst_ShaderZombieQuemado(quemar);

                        if (zombie.vida <= 0)
                        {
                            t_ZombieComun.removeZombie(zombies, zombie);
                        }
                    }
                }
            }

            public void render()
            {
                if (fuego && _superTiempoPost > 0)
                {
                    _FuegoObj.Render(false);
                }
            }
        };










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        public GameModel _game;
        public List<t_JalapenioInstancia> _InstJalapenio;
        public bool Is_Personal = false;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Jalapenio(GameModel game, byte n) : base(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, game, n, PLANTA_VALOR, VIDA_PLANTA)
        {
            _game = game;
            
            _Planta.Set_Transform(0, -5.9F, 0,
                                    1.5F, 1.5F, 1.5F,
                                    0, GameModel.PI, 0);


            _InstJalapenio = new List<t_JalapenioInstancia>();
        }

        public static t_Jalapenio Crear(GameModel game, byte n)
        {
            if (t_HUDBox.Is_Libre(n) && game != null)
            {
                return new t_Jalapenio(game, n);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public new void Update(bool ShowBoundingBoxWithKey)
        {
            int i;

            int JalapenioCreado = base.Update(ShowBoundingBoxWithKey);

            _Planta.Inst_Set_PositionY(PLANTA_INIT_Y);

            if (_game._camara.Modo_Is_CamaraPersonal())
            {
                if (JalapenioCreado == 3)
                {
                    // Se seleccionó una planta para controlar

                    Is_Personal = true;
                }
            }
            else
            {
                Is_Personal = false;
            }

            if (Is_Personal && JalapenioCreado == 4)
            {
                // Se activo la super
            }

            if (JalapenioCreado == 2)
            {
                // Jalapenio ubicado
                t_JalapenioInstancia Jalapenio = new t_JalapenioInstancia(_game, _InstPlanta[_InstPlanta.Count - 1], _Planta._instancias[_Planta._instancias.Count - 1], _Planta);
                _InstJalapenio.Add(Jalapenio);

                // Apenas se crea, se activa la super (explota y crea fuego)
                Jalapenio.super();
            }


            for (i = _InstJalapenio.Count-1; i >= 0; i--)
            {
                _InstJalapenio[i].update(ShowBoundingBoxWithKey);

                if (_InstJalapenio[i].killMe)
                {
                    // Ya exploto e hizo lo que tenia que hacer. Ahora lo borro

                    _InstJalapenio.Remove(_InstJalapenio[i]);
                }
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public new void Render()
        {
            base.Render();
            
            for (int i = 0; i < _InstJalapenio.Count; i++)
            {
                _InstJalapenio[i].render();
            }
        }
    }
}
