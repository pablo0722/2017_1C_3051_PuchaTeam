using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;

using System.Collections.Generic;

namespace TGC.Group.Model
{
    public partial class GameModel : TgcExample
    {
        /******************************************************************************************
         *                                  CONSTANTES
         ******************************************************************************************/
        // Camara Libre
        private const float P_CAM_POS_X_INIT = 20;
        private const float P_CAM_POS_Y_INIT = 20;
        private const float P_CAM_POS_Z_INIT = 0;
        private const float P_CAM_VEL = 50;
        private const float P_CAM_JUMP = 50;
        private const float P_CAM_ROT = (float)0.02;

        // Camara Plano Picado
        private const float P_CAM_PICADO_X = 100;
        private const float P_CAM_PICADO_Y = 100;
        private const float P_CAM_PICADO_Z = 0;
        private const float P_CAM_UP_X = -1;
        private const float P_CAM_UP_Y = 1;
        private const float P_CAM_UP_Z = 0;










        /******************************************************************************************
         *                                  INICIALIZA CAMARA
         ******************************************************************************************/
        private void Func_Init_Camara()
        {
            // Camara Picado
            CamaraPicado = new Core.Camara.TgcCamera();
            CamaraPicado.SetCamera(new Vector3(P_CAM_PICADO_X, P_CAM_PICADO_Y, P_CAM_PICADO_Z),
                Vector3.Empty, new Vector3(P_CAM_UP_X, P_CAM_UP_Y, P_CAM_UP_Z));

            // Camara Primera Persona
            Camara1Persona = new MyCamara1Persona(new Vector3(P_CAM_POS_X_INIT, P_CAM_POS_Y_INIT, P_CAM_POS_Z_INIT),
                P_CAM_VEL, P_CAM_JUMP, P_CAM_ROT, Input);

            Is_CamPicado = true;
            Camara = CamaraPicado;
        }










        /******************************************************************************************
         *                                  ACTUALIZA CAMARA
         ******************************************************************************************/
        private void Func_Update_Cam()
        {
            if (!Is_CamPicado)
                Camara1Persona.UpdateCamera(ElapsedTime);

            if (Input.keyPressed(Key.H))
            {
                Is_CamPicado = !Is_CamPicado;

                if (Is_CamPicado)
                {
                    Camara = CamaraPicado;
                }
                else
                {
                    Camara = Camara1Persona;
                }
            }
        }
    }
}
