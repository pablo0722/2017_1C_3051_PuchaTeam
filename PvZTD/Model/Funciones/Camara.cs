using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Example;
using TGC.Core.Input;
using TGC.Core.Camara;
using TGC.Core.Utils;
using System.Collections.Generic;

namespace TGC.Group.Model
{
    public class t_Camara
    {
        /******************************************************************************************/
        /*                                  CONSTANTES
        /******************************************************************************************/
        // Camara Libre
        private const float P_CAM_LIBRE_POS_X = 0;
        private const float P_CAM_LIBRE_POS_Y = 20;
        private const float P_CAM_LIBRE_POS_Z = -50;
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
        /*                                  VARIABLES
        /******************************************************************************************/
        private MyCamara1Persona _CamaraLibre;   // Camara Libre
        public TgcCamera _CamaraAerea;          // Camara Plano Picado
        private TgcExample _example;

        private bool _Is_CamLibre;               // En modo Camara libre?
        private bool _Is_CamAerea;               // En modo Camara aerea?
        private bool _Is_CamPersonal;            // En modo Camara personal?











        /******************************************************************************************/
        /*                                  CONSTRUCTOR
        /******************************************************************************************/
        public t_Camara(TgcExample example)
        {
            _example = example;

            // Camara Aerea
            _CamaraAerea = new TgcCamera();
            _CamaraAerea.SetCamera( new Vector3(0, 0, 1),
                                    Vector3.Empty, new Vector3(0, 1, 0));

            // Camara Libre
            _CamaraLibre = new MyCamara1Persona(new Vector3(0, 0, 1),
                                                10, 10, 0.01F, _example.Input);
            _CamaraLibre.SetCamera( new Vector3(0, 0, 1),
                                    new Vector3(0,10,50), new Vector3(0, 1, 0));

            Aerea_Posicion(P_CAM_AEREA_POS_X, P_CAM_AEREA_POS_Y, P_CAM_AEREA_POS_Z);
            Aerea_LookAt(0, 0, 0);
            Aerea_Up(P_CAM_AEREA_UP_X, P_CAM_AEREA_UP_Y, P_CAM_AEREA_UP_Z);


            Libre_MoveSpeed(P_CAM_LIBRE_MOVE);
            Libre_JumpSpeed(P_CAM_LIBRE_JUMP);
            Libre_RotationSpeed(P_CAM_LIBRE_ROT);
            Libre_Posicion(P_CAM_LIBRE_POS_X, P_CAM_LIBRE_POS_Y, P_CAM_LIBRE_POS_Z);
            // Libre_SetLookAt(new Vector3(0, 0, 1));

            _Is_CamAerea = true;
            _Is_CamLibre = false;
            _Is_CamPersonal = false;
            example.Camara = _CamaraAerea;
        }










        /******************************************************************************************/
        /*                                  MODO DE CAMARA
        /******************************************************************************************/
        public bool Modo_Is_CamaraAerea()
        {
            return _Is_CamAerea;
        }

        public bool Modo_Is_CamaraLibre()
        {
            return _Is_CamLibre;
        }

        public bool Modo_Is_CamaraPersonal()
        {
            return _Is_CamPersonal;
        }

        public void Modo_Change()
        {
            if (_Is_CamAerea)
                Modo_Libre();
            else if (_Is_CamLibre)
                Modo_Aerea();
        }

        public void Modo_Aerea()
        {
            _Is_CamAerea = true;
            _Is_CamLibre = false;
            _Is_CamPersonal = false;

            Aerea_Reset();
            _example.Camara = _CamaraAerea;
        }

        public void Modo_Libre()
        {
            _Is_CamAerea = false;
            _Is_CamLibre = true;
            _Is_CamPersonal = false;

            _example.Camara = _CamaraLibre;
        }

        public void Modo_Personal()
        {
            _Is_CamAerea = false;
            _Is_CamLibre = false;
            _Is_CamPersonal = true;

            _example.Camara = _CamaraAerea;
        }










        /******************************************************************************************/
        /*                                  CAMARA AEREA CONFIG
        /******************************************************************************************/
        public void Aerea_Reset()
        {
            Aerea_Posicion(P_CAM_AEREA_POS_X, P_CAM_AEREA_POS_Y, P_CAM_AEREA_POS_Z);
            Aerea_LookAt(0, 0, 0);
            Aerea_Up(P_CAM_AEREA_UP_X, P_CAM_AEREA_UP_Y, P_CAM_AEREA_UP_Z);
        }

        public void Aerea_Posicion(float X, float Y, float Z)
        {
            _CamaraAerea.SetCamera( new Vector3(X, Y, Z),
                                    _CamaraAerea.LookAt,
                                    _CamaraAerea.UpVector);
        }

        public void Aerea_LookAt(float X, float Y, float Z)
        {
            _CamaraAerea.SetCamera( _CamaraAerea.Position,
                                    new Vector3(X, Y, Z),
                                    _CamaraAerea.UpVector);
        }

        public void Aerea_Up(float X, float Y, float Z)
        {
            _CamaraAerea.SetCamera( _CamaraAerea.Position,
                                    _CamaraAerea.LookAt, 
                                    new Vector3(X, Y, Z));
        }

  


        /******************************************************************************************/
        /*                                  CAMARA LIBRE CONFIG
        /******************************************************************************************/
        public void Libre_Posicion(float X, float Y, float Z)
        {
            _CamaraLibre = new MyCamara1Persona(new Vector3(X, Y, Z),
                                                _CamaraLibre.MovementSpeed, _CamaraLibre.JumpSpeed, _CamaraLibre.RotationSpeed,
                                                _example.Input);
        }

        public void Libre_MoveSpeed(float speed)
        {
            _CamaraLibre = new MyCamara1Persona(_CamaraLibre.Position,
                                                speed, _CamaraLibre.JumpSpeed, _CamaraLibre.RotationSpeed,
                                                _example.Input);
        }

        public void Libre_JumpSpeed(float speed)
        {
            _CamaraLibre = new MyCamara1Persona(_CamaraLibre.Position,
                                                _CamaraLibre.MovementSpeed, speed, _CamaraLibre.RotationSpeed,
                                                _example.Input);
        }

        public void Libre_RotationSpeed(float speed)
        {
            _CamaraLibre = new MyCamara1Persona(_CamaraLibre.Position,
                                                _CamaraLibre.MovementSpeed, _CamaraLibre.JumpSpeed, speed,
                                                _example.Input);
        }

        public void Libre_SetLookAt(Vector3 lookAt)
        {
            _CamaraLibre.SetCamera(_CamaraLibre.Position, lookAt);
        }










        /******************************************************************************************/
        /*                                  UPDATE
        /******************************************************************************************/
        public void Update(float ElapsedTime)
        {
            if (_Is_CamLibre)
                _CamaraLibre.UpdateCamera(ElapsedTime);
        }



        public void UpdateMenu(float _TiempoTranscurrido)
        {
            
            Aerea_Posicion(150*FastMath.Cos(_TiempoTranscurrido)+(FastMath.Cos(_TiempoTranscurrido/2) * 100), 100, 50 * FastMath.Sin(_TiempoTranscurrido/2) + (FastMath.Sin(_TiempoTranscurrido/2) * 100));
        }




    }
}
