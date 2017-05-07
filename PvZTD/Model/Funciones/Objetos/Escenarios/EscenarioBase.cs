using TGC.Core.Direct3D;


namespace TGC.Group.Model
{
    public class t_EscenarioBase
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        public const float PASTO_POS_X_INICIAL = -33.2F;
        public const float PASTO_POS_Y_INICIAL = -13;
        public const float PASTO_POS_Z_INICIAL = -70;
        public const float PASTO_RAZON = 13;
        public const float PASTO_AJUSTE = 1.6F;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // ESTATICAS
        public static int MouseX;
        public static int MouseY;
        private static int _PastoLugar1;   // Primer campo de bits para saber la ocupacion por plantas de cada cuadrado de pasto. (0 -> 31)
        private static int _PastoLugar2;   // Segundo campo de bits para saber la ocupacion por plantas de cada cuadrado de pasto. (0 -> 31)

        // NO ESTATICAS
        private t_Objeto3D _Casa;
        private t_Objeto3D _Plano;
        private t_Objeto3D _PastoOscuro;
        private t_Objeto3D _PastoMedio;
        private t_Objeto3D _PastoClaro;
        private t_Objeto3D _Cerebro;
        private GameModel _game;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        protected t_EscenarioBase(  GameModel game, string PathCasa, string PathPlano,
                                    string PathPastoClaro, string PathPastoMedio, string PathPastoOscuro,
                                    string PathCerebro)
        {
            _game = game;

            _PastoLugar1 = 0;
            _PastoLugar2 = 0;

            // Casa
            _Casa = t_Objeto3D.Crear(_game, PathCasa);

            _Casa.Set_Position(7.5F, -7, -115);
            _Casa.Set_Size(0.5F, 0.5F, 0.5F);
            _Casa.Set_Rotation(0, GameModel.PI / 2, 0);

            _Casa.Inst_Create();

            // Plano
            _Plano = t_Objeto3D.Crear(_game, PathPlano);

            _Plano.Set_Size(4, 1, 4);

            _Plano.Inst_Create(0, 0, 0);

            // Pasto
            _PastoClaro = t_Objeto3D.Crear(_game, PathPastoOscuro);
            _PastoMedio = t_Objeto3D.Crear(_game, PathPastoMedio);
            _PastoOscuro = t_Objeto3D.Crear(_game, PathPastoClaro);

            _PastoClaro.Set_Size((PASTO_RAZON / 10) * PASTO_AJUSTE, (float)PASTO_RAZON / 10, (float)PASTO_RAZON / 10);
            _PastoMedio.Set_Size((PASTO_RAZON / 10) * PASTO_AJUSTE, (float)PASTO_RAZON / 10, (float)PASTO_RAZON / 10);
            _PastoOscuro.Set_Size((PASTO_RAZON / 10) * PASTO_AJUSTE, (float)PASTO_RAZON / 10, (float)PASTO_RAZON / 10);

            for (int j = 0; j < GameModel.CANT_COLUMNAS; j+=2)
            {
                for (int i = 0; i < GameModel.CANT_FILAS; i+=2)
                {
                    _PastoMedio.Inst_Create(PASTO_POS_X_INICIAL + (PASTO_RAZON * PASTO_AJUSTE) * i, PASTO_POS_Y_INICIAL, PASTO_POS_Z_INICIAL + (PASTO_RAZON * j));
                    _PastoOscuro.Inst_Create(PASTO_POS_X_INICIAL + (PASTO_RAZON * PASTO_AJUSTE) * i, PASTO_POS_Y_INICIAL, PASTO_POS_Z_INICIAL + PASTO_RAZON * (j+1));
                }
                for (int i = 1; i < GameModel.CANT_FILAS; i+=2)
                {
                    _PastoClaro.Inst_Create(PASTO_POS_X_INICIAL + (PASTO_RAZON * PASTO_AJUSTE) * i, PASTO_POS_Y_INICIAL, PASTO_POS_Z_INICIAL + (PASTO_RAZON * j));
                    _PastoMedio.Inst_Create(PASTO_POS_X_INICIAL + (PASTO_RAZON * PASTO_AJUSTE) * i, PASTO_POS_Y_INICIAL, PASTO_POS_Z_INICIAL + (PASTO_RAZON * (j + 1 )));
                }
            }

            // Cerebros
            _Cerebro = t_Objeto3D.Crear(_game, PathCerebro);

            _Cerebro.Set_Position(50, 5, -70);
            _Cerebro.Set_Size(0.03F, 0.03F, 0.03F);

            for(int i=0; i<5; i++)
            {
                _Cerebro.Inst_CreateAndSelect();
                _Cerebro.Inst_Move(-13 * 1.6F * i, 0, 0);
            }
        }

        public static t_EscenarioBase Crear(GameModel game, string PathCasa, string PathPlano,
                                            string PathPastoClaro, string PathPastoMedio, string PathPastoOscuro,
                                            string PathCerebro)
        {
            if (game != null && PathCasa != null && PathPlano != null &&
                PathPastoClaro != null && PathPastoMedio != null && PathPastoOscuro != null && PathCerebro != null)
            {
                return new t_EscenarioBase(game, PathCasa, PathPlano, PathPastoClaro, PathPastoMedio, PathPastoOscuro, PathCerebro);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      PASTO LUGAR
        /******************************************************************************************/
        // Elige un cuadrado de pasto para aplicarle un efecto que indique que está seleccionado.
        // 0 <= fila <= CANT_FILAS - 1
        // 0 <= columna <= CANT_COLUMNAS - 1
        public void Do_PastoSelect(int fila, int columna)
        {
            Do_PastoSelectNone();

            if (columna%2 == 0)
            {
                if(fila%2 == 0)
                {
                    _PastoMedio.Inst_Select(_PastoMedio._instancias[(columna / 2) * GameModel.CANT_FILAS + fila / 2]);
                    if(Is_PastoOcupado(fila, columna))
                    {
                        _PastoMedio.Inst_Color(255, 0, 0);
                    }
                    else
                    {
                        _PastoMedio.Inst_Color(0, 255, 0);
                    }
                }
                else
                {
                    _PastoClaro.Inst_Select(_PastoClaro._instancias[(columna / 2) * (GameModel.CANT_FILAS / 2) + fila / 2]); //aca rompe
                    if (Is_PastoOcupado(fila, columna))
                    {
                        _PastoClaro.Inst_Color(255, 0, 0);
                    }
                    else
                    {
                        _PastoClaro.Inst_Color(0, 255, 0);
                    }
                }
            }
            else
            {
                if (fila % 2 == 0)
                {
                    _PastoOscuro.Inst_Select(_PastoOscuro._instancias[(columna / 2) * ((GameModel.CANT_FILAS+1) / 2) + fila / 2]);
                    if (Is_PastoOcupado(fila, columna))
                    {
                        _PastoOscuro.Inst_Color(255, 0, 0);
                    }
                    else
                    {
                        _PastoOscuro.Inst_Color(0, 255, 0);
                    }
                }
                else
                {
                    _PastoMedio.Inst_Select(_PastoMedio._instancias[(columna / 2) * GameModel.CANT_FILAS + fila / 2 + (GameModel.CANT_FILAS+1)/2]);
                    if (Is_PastoOcupado(fila, columna))
                    {
                        _PastoMedio.Inst_Color(255, 0, 0);
                    }
                    else
                    {
                        _PastoMedio.Inst_Color(0, 255, 0);
                    }
                }
            }
        }

        // Resetea el color de todos los cuadrados de pasto
        public void Do_PastoSelectNone()
        {
            _PastoClaro.Inst_ColorAll(255, 255, 255);
            _PastoMedio.Inst_ColorAll(255, 255, 255);
            _PastoOscuro.Inst_ColorAll(255, 255, 255);
        }

        // Elige un cuadrado de pasto para setear como ocupado con una planta u otra cosa, evitando que se pueda colocar otra planta en ese lugar
        // 0 <= fila <= CANT_FILAS - 1
        // 0 <= columna <= CANT_COLUMNAS - 1
        public void Set_PastoOcupado(int fila, int columna)
        {
            if (columna < GameModel.CANT_COLUMNAS / 2)
            {
                _PastoLugar1 |= 1 << (columna * GameModel.CANT_FILAS + fila);
            }
            else
            {
                _PastoLugar2 |= 1 << ((columna - GameModel.CANT_COLUMNAS / 2) * GameModel.CANT_FILAS + fila);
            }
        }

        // Elige un cuadrado de pasto para setear como desocupado, permitiendo que se pueda colocar otra planta en ese lugar
        // 0 <= fila <= CANT_FILAS - 1
        // 0 <= columna <= CANT_COLUMNAS - 1
        public void Set_PastoDesocupado(int fila, int columna)
        {
            if (columna < GameModel.CANT_COLUMNAS / 2)
            {
                _PastoLugar1 &= ~(1 << (columna * GameModel.CANT_FILAS + fila));
            }
            else
            {
                _PastoLugar2 &= ~(1 << ((columna - GameModel.CANT_COLUMNAS / 2) * GameModel.CANT_FILAS + fila));
            }
        }

        // Define si un cuadrado de pasto ya esta ocupado
        // 0 <= fila <= CANT_FILAS - 1
        // 0 <= columna <= CANT_COLUMNAS - 1
        public static bool Is_PastoOcupado(int fila, int columna)
        {
            if (columna < GameModel.CANT_COLUMNAS / 2)
            {
                if ((_PastoLugar1 & (1 << (columna * GameModel.CANT_FILAS + fila))) != 0)
                {
                    return true;
                }

                return false;
            }
            else
            {
                if ((_PastoLugar2 & (1 << ((columna - GameModel.CANT_COLUMNAS / 2) * GameModel.CANT_FILAS + fila))) != 0)
                {
                    return true;
                }

                return false;
            }
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        bool PickedAnterior = false;
        public void Update(bool ShowBoundingBoxWithKey, float Rotacion_Cerebros_SegundosPorVuelta)
        {
            if (t_HUDBox.Is_AnyBoxPicked())
            {
                MouseX = ((int)_game.Input.Xpos) * GameModel.CANT_COLUMNAS / (D3DDevice.Instance.Device.Viewport.Width + 1);
                MouseY = ((int)_game.Input.Ypos) * GameModel.CANT_FILAS / (D3DDevice.Instance.Device.Viewport.Height + 1);

                _game._EscenarioBase.Do_PastoSelect(MouseY, MouseX);

                PickedAnterior = true;
            }
            else if(PickedAnterior)
            {
                _game._EscenarioBase.Do_PastoSelectNone();
                _game._EscenarioBase.Set_PastoOcupado(MouseY, MouseX);

                PickedAnterior = false;
            }

            _Casa.Update(ShowBoundingBoxWithKey);
            _Plano.Update(ShowBoundingBoxWithKey);
            _PastoClaro.Update(ShowBoundingBoxWithKey);
            _PastoMedio.Update(ShowBoundingBoxWithKey);
            _PastoOscuro.Update(ShowBoundingBoxWithKey);
            _Cerebro.Update(ShowBoundingBoxWithKey);

            _Cerebro.Inst_RotateAll(0, _game.ElapsedTime * Rotacion_Cerebros_SegundosPorVuelta / (2 * GameModel.PI), 0);
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            _Casa.Render();
            _Plano.Render();
            _PastoClaro.Render();
            _PastoMedio.Render();
            _PastoOscuro.Render();
            _Cerebro.Render();
        }
    }
}
