﻿using System.Collections.Generic;



namespace TGC.Group.Model
{
    public class t_Patatapum : t_Planta
    {
        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public struct t_PatatapumInstancia
        {
        };










        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string    PATH_OBJ =          "..\\..\\Media\\Objetos\\Nuez-TgcScene.xml";
        private const string    PATH_TEXTURA_ON =   "..\\..\\Media\\Texturas\\HUD_nut_sel.jpg";
        private const string    PATH_TEXTURA_OFF =  "..\\..\\Media\\Texturas\\HUD_nut.jpg";
        private const int       PLANTA_VALOR =      200;
        private const float     VIDA_PLANTA =       15;
        private const float     VIDA_SUPER =        30;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        public GameModel _game;
        public List<t_PatatapumInstancia> _InstPatatapum;
        public bool Is_Personal = false;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Patatapum(GameModel game, byte n) : base(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, game, n, PLANTA_VALOR, VIDA_PLANTA)
        {
            _game = game;

            _Planta.Set_Transform(0, 0, 0,
                                    0.4F, 0.4F, 0.4F,
                                    0, GameModel.PI, 0);

            _Planta.Mesh_Select(0);
            _Planta.Mesh_Color(64, 64, 64);

            _Planta.Mesh_Select(1);
            _Planta.Mesh_Color(228, 214, 153);

            _Planta.Mesh_Select(2);
            _Planta.Mesh_Color(228, 214, 153);

            _Planta.Mesh_Select(3);
            _Planta.Mesh_Color(228, 214, 153);

            _Planta.Mesh_Select(4);
            _Planta.Mesh_Color(95, 40, 0);

            _Planta.Mesh_Select(5);
            _Planta.Mesh_Color(255, 0, 0);

            _Planta.Mesh_Select(6);
            _Planta.Mesh_Color(60, 27, 0);


            _InstPatatapum = new List<t_PatatapumInstancia>();
        }

        public static t_Patatapum Crear(GameModel game, byte n)
        {
            if (t_HUDBox.Is_Libre(n) && game != null)
            {
                return new t_Patatapum(game, n);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public new void Update(bool ShowBoundingBoxWithKey)
        {
            int PatatapumCreado = base.Update(ShowBoundingBoxWithKey);

            if (_game._camara.Modo_Is_CamaraPersonal())
            {
                if (PatatapumCreado == 3)
                {
                    // Se seleccionó una planta para controlar

                    Is_Personal = true;
                }
            }
            else
            {
                Is_Personal = false;
            }

            if (Is_Personal && PatatapumCreado == 4)
            {
                // Se activo la super
                int i;
                for (i = 0; i < _Planta._instancias.Count; i++)
                {
                    if (_Planta._instancias[i] == _instPersonal)
                    {
                        t_PlantaInstancia planta = _InstPlanta[i];
                        planta.vida = VIDA_SUPER;
                        _Planta.Inst_Select(_Planta._instancias[i]);
                        _Planta.Inst_ShaderSuperNuez(true);
                        _InstPlanta[i] = planta;
                        break;
                    }
                }
                //_InstPatatapum[i].super();
            }

            if (PatatapumCreado == 2)
            {
                // Girasol ubicado
                t_PatatapumInstancia Patatapum = new t_PatatapumInstancia();
                _InstPatatapum.Add(Patatapum);
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public new void Render()
        {
            base.Render();
        }
    }
}
