using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using TGC.Core.Example;
using TGC.Core.Input;
using TGC.Core.Camara;

using System.Collections.Generic;

namespace TGC.Group.Model
{
    public class t_Camara
    {
        /******************************************************************************************/
        /*                                  VARIABLES
        /******************************************************************************************/
        private MyCamara1Persona _CamaraLibre;   // Camara Libre
        private TgcCamera _CamaraAerea;          // Camara Plano Picado
        private TgcExample _example;

        private bool _Is_CamAerea;               // En modo Camara aerea?











        /******************************************************************************************/
        /*                                  CONSTRUCTOR
        /******************************************************************************************/
        public t_Camara(TgcExample example)
        {
            _example = example;

            // Camara Picado
            _CamaraAerea = new TgcCamera();
            _CamaraAerea.SetCamera( new Vector3(0, 0, 1),
                                    Vector3.Empty, new Vector3(0, 1, 0));

            // Camara Primera Persona
            _CamaraLibre = new MyCamara1Persona(new Vector3(0, 0, 1),
                                                10, 10, 0.01F, _example.Input);
            _CamaraLibre.SetCamera( new Vector3(0, 0, 1),
                                    Vector3.Empty, new Vector3(0, 1, 0));

            _Is_CamAerea = true;
            example.Camara = _CamaraAerea;
        }










        /******************************************************************************************/
        /*                                  MODO DE CAMARA
        /******************************************************************************************/
        public bool Modo_Is_CamaraAerea()
        {
            return _Is_CamAerea;
        }

        public void Modo_Change()
        {
            _Is_CamAerea = !_Is_CamAerea;

            if (_Is_CamAerea)
            {
                _example.Camara = _CamaraAerea;
            }
            else
            {
                _example.Camara = _CamaraLibre;
            }
        }

        public void Modo_Aerea()
        {
            _Is_CamAerea = true;
            
            _example.Camara = _CamaraAerea;
        }

        public void Modo_Libre()
        {
            _Is_CamAerea = false;
            
            _example.Camara = _CamaraLibre;
        }










        /******************************************************************************************/
        /*                                  CAMARA AEREA CONFIG
        /******************************************************************************************/
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










        /******************************************************************************************/
        /*                                  UPDATE
        /******************************************************************************************/
        public void Update(float ElapsedTime)
        {
            if (!_Is_CamAerea)
                _CamaraLibre.UpdateCamera(ElapsedTime);
        }
    }
}
