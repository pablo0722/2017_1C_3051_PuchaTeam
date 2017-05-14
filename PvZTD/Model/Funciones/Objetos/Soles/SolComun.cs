using Microsoft.DirectX;
using System.Collections.Generic;


namespace TGC.Group.Model
{
    public class t_SolComun
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ = "..\\..\\Media\\Objetos\\sol-TgcScene.xml";
        private const float ROTACION_SEG_POR_VUELTA = 2;
        private const float VELOCIDAD_CAIDA = -10F;
        private const int SOL_VALOR = 25;   // Cuanto suma agarrar un sol










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private t_Objeto3D _Sol;
        private GameModel _game;
        static int _SolN; // Se usa para ir creando los soles conforme transcurre el tiempo










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_SolComun( GameModel game)
        {
            _game = game;

            _Sol = t_Objeto3D.Crear(_game, PATH_OBJ);

            _Sol.Set_Transform(0, 100000, 0,
                                (float)0.075, (float)0.075, (float)0.075,
                                0, 0, 0);
        }

        public static t_SolComun Crear(GameModel game)
        {
            if (game != null)
            {
                return new t_SolComun(game);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      COLISION
        /******************************************************************************************/
        public t_Objeto3D.t_instancia Is_MouseOver()
        {
            return _game._colision.MouseMesh(_Sol);
        }










        /******************************************************************************************/
        /*                                      CREACION DE SOLES
        /******************************************************************************************/
        public void Do_CreateSol()
        {
            _Sol.Inst_CreateAndSelect(0, 65, _game._rand.Next(-60, 60));
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public void Update(bool ShowBoundingBoxWithKey, int CantSegundosSegundosAEsperarParaCrearSol)
        {
            _Sol.Update(ShowBoundingBoxWithKey);

            _Sol.Inst_RotateAll(_game.ElapsedTime * ROTACION_SEG_POR_VUELTA / (2*GameModel.PI), 0, 0);
            
            if (_game.Input.keyPressed(Microsoft.DirectX.DirectInput.Key.K))
            {
                _game._soles += 500;
            }

            // Caida
            for (int i = _Sol._instancias.Count-1; i >= 0; i--)
            {
                if (_Sol._instancias[i].pos.Y < 6)
                {
                    Vector3 PosAux = _Sol._instancias[i].pos;
                    _Sol._instancias[i].pos = new Vector3(PosAux.X, PosAux.Y + _game.ElapsedTime * VELOCIDAD_CAIDA * (1F / 10F), PosAux.Z);
                }
                else if (_Sol._instancias[i].pos.Y < -6)
                {
                    _Sol._instancias.Remove(_Sol._instancias[i]);
                }
                else
                {
                    Vector3 PosAux = _Sol._instancias[i].pos;
                    _Sol._instancias[i].pos = new Vector3(PosAux.X, PosAux.Y + _game.ElapsedTime * VELOCIDAD_CAIDA, PosAux.Z);
                }
            }

            if (_game._TiempoTranscurrido >= CantSegundosSegundosAEsperarParaCrearSol * (_SolN+1))
            {
                Do_CreateSol();

                _SolN++;
            }

            if (_game._mouse.ClickIzq_RisingDown())
            {
                t_Objeto3D.t_instancia SolActual = Is_MouseOver();

                if (SolActual != null)
                {
                    _game._soles += SOL_VALOR;
                    _Sol.Inst_Delete(Is_MouseOver());
                }
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            _Sol.Render(true);
        }
    }
}
