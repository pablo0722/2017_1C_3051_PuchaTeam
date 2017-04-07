using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Example;
using TGC.Core.Input;
using TGC.Core.Camara;

using System.Collections.Generic;

namespace TGC.Group.Model
{
    public partial class GameModel : TgcExample
    {
        /******************************************************************************************/
        /*                                  CONSTANTES
        /******************************************************************************************/
        // Camara Libre
        private const float P_CAM_LIBRE_POS_X = 150;
        private const float P_CAM_LIBRE_POS_Y = 40;
        private const float P_CAM_LIBRE_POS_Z = 0;
        private const float P_CAM_LIBRE_MOVE = 50;
        private const float P_CAM_LIBRE_JUMP = 50;
        private const float P_CAM_LIBRE_ROT = 0.02F;

        // Camara Plano Picado
        private const float P_CAM_AEREA_POS_X = 100;
        private const float P_CAM_AEREA_POS_Y = 100;
        private const float P_CAM_AEREA_POS_Z = 0;
        private const float P_CAM_AEREA_UP_X = -1;
        private const float P_CAM_AEREA_UP_Y = 1;
        private const float P_CAM_AEREA_UP_Z = 0;










        /******************************************************************************************/
        /*                                  INICIALIZA CAMARA
        /******************************************************************************************/
        private void p_Func_Camara_Init()
        {
            _camara.Aerea_Posicion(P_CAM_AEREA_POS_X, P_CAM_AEREA_POS_Y, P_CAM_AEREA_POS_Z);
            _camara.Aerea_LookAt(0, 0, 0);
            _camara.Aerea_Up(P_CAM_AEREA_UP_X, P_CAM_AEREA_UP_Y, P_CAM_AEREA_UP_Z);

            _camara.Libre_MoveSpeed(P_CAM_LIBRE_MOVE);
            _camara.Libre_JumpSpeed(P_CAM_LIBRE_JUMP);
            _camara.Libre_RotationSpeed(P_CAM_LIBRE_ROT);
            _camara.Libre_Posicion(P_CAM_LIBRE_POS_X, P_CAM_LIBRE_POS_Y, P_CAM_LIBRE_POS_Z);
        }










        /******************************************************************************************/
        /*                                  ACTUALIZA CAMARA
        /******************************************************************************************/
        private void p_Func_Camara_Update()
        {
            if (Input.keyPressed(Key.H))
            {
                _camara.Modo_Change();
            }
        }
    }
}
