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
        private const float VELOCIDAD_CAIDA = -0.02F;
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

            /*
            _Sol.Inst_Create(0, 50, 0);
            _Sol.Inst_Create(20, 50, 30);
            */
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
        public int Is_MouseOver()
        {
            return _game._colision.MouseMesh(_Sol);
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public void Update(bool ShowBoundingBoxWithKey, List<int> CantSegundosSegundosAEsperarParaCrearSol, bool GeneracionInfinitaDeSoles)
        {
            _Sol.Update(ShowBoundingBoxWithKey);

            _Sol.Inst_RotateAll(_game.ElapsedTime * ROTACION_SEG_POR_VUELTA / (2*GameModel.PI), 0, 0);
            _Sol.Inst_MoveAll(0, VELOCIDAD_CAIDA, 0);

            if(GeneracionInfinitaDeSoles)
            {
                if (_game._TiempoTranscurrido >= CantSegundosSegundosAEsperarParaCrearSol[0] * (_SolN+1))
                {
                    _Sol.Inst_CreateAndSelect(_game._rand.Next(-50, 50), 80, _game._rand.Next(-60, 60));

                    _SolN++;
                }
            }
            else if (_SolN < CantSegundosSegundosAEsperarParaCrearSol.Count)
            {
                if (_game._TiempoTranscurrido >= CantSegundosSegundosAEsperarParaCrearSol[_SolN])
                {
                    _Sol.Inst_CreateAndSelect(_game._rand.Next(-50, 50), 80, _game._rand.Next(-60, 60));

                    _SolN++;
                }
            }

            if (_game._mouse.ClickIzq_RisingDown())
            {
                int SolActual = Is_MouseOver();

                if (SolActual >= 0)
                {
                    if (_Sol._instancias[SolActual].pos.Y >= 0)
                    {
                        _game._soles += SOL_VALOR;
                    }
                    _Sol.Inst_Delete(Is_MouseOver());
                }
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            _Sol.Render();
        }
    }
}
