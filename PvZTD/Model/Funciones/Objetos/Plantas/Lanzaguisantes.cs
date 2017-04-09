namespace TGC.Group.Model
{
    public class t_Lanzaguisantes
    {
        /******************************************************************************************/
        /*                                      CONSTANTES
        /******************************************************************************************/
        private const string PATH_OBJ = "../../Media/Objetos/Girasol-TgcScene.xml";
        private const string PATH_TEXTURA_ON = "../../Media/Texturas/HUD_Girasol_sel.jpg";
        private const string PATH_TEXTURA_OFF = "../../Media/Texturas/HUD_Girasol.jpg";










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        t_Objeto3D _Lanzaguisantes;
        t_HUDBox _HUDBox;
        t_Colision _colision;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Lanzaguisantes(t_Colision colision, byte n)
        {
            _colision = colision;
            _HUDBox = t_HUDBox.CrearHUDBox(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, colision, n);
            _Lanzaguisantes = t_Objeto3D.CrearObjeto3D(PATH_OBJ);
        }

        public static t_Lanzaguisantes CrearGirasol(t_Colision colision, byte n)
        {
            if (t_HUDBox.CrearHUDBox(PATH_OBJ, PATH_TEXTURA_ON, PATH_TEXTURA_OFF, colision, n) != null && colision != null)
            {
                return new t_Lanzaguisantes(colision, n);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public void Update()
        {
            if (_HUDBox.Is_MouseOver())
            {
                _HUDBox.Set_Textura_On();
            }
            else
            {
                _HUDBox.Set_Textura_Off();
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            _HUDBox.Render();
            _Lanzaguisantes.Render();
        }
    }
}
