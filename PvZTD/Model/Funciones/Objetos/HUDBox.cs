using Microsoft.DirectX;
using TGC.Core.Geometry;
using TGC.Core.Textures;

namespace TGC.Group.Model
{
    public class t_HUDBox
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const float PI = 3.14159265359F;
        private const float P_HUD_BOX_SIZE = 5;
        private const float P_HUD_BOX_POS_X = 20;
        private const float P_HUD_BOX_POS_Y = 62;
        private const float P_HUD_BOX_POS_Z = -55;
        private const float P_HUD_BOX_ROT = PI * (float)0.17;










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // ESTATICAS
        private static int s_n; // Se usa como campo de bits.
                                // cada bit es una posicion distinta del BoxHUD de cada planta.
                                // Si un bit esta en '1', significa que esa posicion esta ocupada.
                                // Si un bit esta en '0', significa que esa posicion esta libre.
        private static Vector3 _HUDSize = new Vector3(P_HUD_BOX_SIZE, P_HUD_BOX_SIZE, P_HUD_BOX_SIZE);

        // NO ESTATICAS
        private int _n;         // Numero (posicion) del BoxHUD
        private TgcBox _Mesh_HUDBoxOn;
        private TgcBox _Mesh_HUDBoxOff;
        private TgcBox _Mesh_HUDBoxActual;
        private t_Colision _colision;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_HUDBox(string PathObj, string PathTexturaOn, string PathTexturaOff, t_Colision colision, byte n)
        {
            s_n = s_n | (1 << n);   // Reserva lugar en el campo de bits
            _n = n;

            _colision = colision;

            TgcTexture TexturaOn = TgcTexture.createTexture(PathTexturaOn);

            _Mesh_HUDBoxOn = TgcBox.fromSize(new Vector3(P_HUD_BOX_POS_X, P_HUD_BOX_POS_Y, P_HUD_BOX_POS_Z + P_HUD_BOX_SIZE * _n), _HUDSize, TexturaOn);
            _Mesh_HUDBoxOn.rotateZ(P_HUD_BOX_ROT);

            TgcTexture TexturaOff = TgcTexture.createTexture(PathTexturaOff);

            _Mesh_HUDBoxOff = TgcBox.fromSize(new Vector3(P_HUD_BOX_POS_X, P_HUD_BOX_POS_Y, P_HUD_BOX_POS_Z + P_HUD_BOX_SIZE * _n), _HUDSize, TexturaOff);
            _Mesh_HUDBoxOff.rotateZ(P_HUD_BOX_ROT);

            _Mesh_HUDBoxActual = _Mesh_HUDBoxOff;
        }

        public static t_HUDBox CrearHUDBox(string PathObj, string PathTexturaOn, string PathTexturaOff, t_Colision colision, byte n)
        {
            if((PathObj != null) && (PathTexturaOn != null) && (PathTexturaOff != null) && (colision != null) && 
                ((s_n & (1 << n)) == 0)) // Verifica lugar en el campo de bits
            {
                return new t_HUDBox(PathObj, PathTexturaOn, PathTexturaOff, colision, n);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      DESTRUCTOR
        /******************************************************************************************/
        ~t_HUDBox()
        {
            s_n = s_n & (~(1 << _n));   // Se borra del campo de bits
            _Mesh_HUDBoxOn.dispose();
            _Mesh_HUDBoxOff.dispose();
        }










        /******************************************************************************************/
        /*                                      COLISION
        /******************************************************************************************/
        public bool Is_MouseOver()
        {
            if(_colision.MouseBox(_Mesh_HUDBoxOff) || _colision.MouseBox(_Mesh_HUDBoxOn))
            {
                return true;
            }

            return false;
        }










        /******************************************************************************************/
        /*                                      TEXTURAS
        /******************************************************************************************/
        public void Set_Textura_On()
        {
            _Mesh_HUDBoxActual = _Mesh_HUDBoxOn;
        }

        public void Set_Textura_Off()
        {
            _Mesh_HUDBoxActual = _Mesh_HUDBoxOff;
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        // Renderiza un TgcBox
        private void Func_BoxRender(TgcBox box)
        {
            //Siempre antes de renderizar el modelo necesitamos actualizar la matriz de transformacion.
            //Debemos recordar el orden en cual debemos multiplicar las matrices, en caso de tener modelos jerárquicos, tenemos control total.
            box.Transform = Matrix.Scaling(box.Scale) *
                            Matrix.RotationYawPitchRoll(box.Rotation.Y, box.Rotation.X, box.Rotation.Z) *
                            Matrix.Translation(box.Position);

            box.render();
        }

        // Renderiza todos los objetos relativos a la clase
        public void Render()
        {
            Func_BoxRender(_Mesh_HUDBoxActual);
        }
    }
}
