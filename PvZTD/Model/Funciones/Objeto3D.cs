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
    public class Objeto3D
    {
        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        private class c_instancia
        {
            public Vector3 pos;
            public Vector3 rot;
            public Vector3 size;
        };










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private List<TgcMesh> _meshes;
        private List<c_instancia> _instancia;
        private c_instancia _InstanciaBase;     // Todas las instancias se crean como copias de la _InstanciaBase
        private int _InstSel; //Instancia seleccionada










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        public Objeto3D(string path)
        {
            _InstSel = -1;
            _meshes = new TgcSceneLoader().loadSceneFromFile(path).Meshes;
            _instancia = new List<c_instancia>();
        }










        /******************************************************************************************/
        /*                                      DESTRUCTOR
        /******************************************************************************************/
        ~Objeto3D()
        {
            for (int i = 0; i < _meshes.Count; i++)
            {
                _meshes[i].dispose();
            }
        }










        /******************************************************************************************/
        /*                                      TRANSFORMACIONES
        /******************************************************************************************/
        public void Transform(  float PosX, float PosY, float PosZ,
                                float SizeX, float SizeY, float SizeZ,
                                float RotX, float RotY, float RotZ)
        {
            Position(PosX, PosY, PosZ);
            Size(SizeX, SizeY, SizeZ);
            Rotation(RotX, RotY, RotZ);
        }

        public void Position(float X, float Y, float Z)
        {
            _InstanciaBase.pos = new Vector3(X, Y, Z);
        }

        public void Size(float X, float Y, float Z)
        {
            _InstanciaBase.size = new Vector3(X, Y, Z);
        }

        public void Rotation(float X, float Y, float Z)
        {
            _InstanciaBase.rot = new Vector3(X, Y, Z);
        }










        /******************************************************************************************/
        /*                                      INSTANCIAS
        /******************************************************************************************/

        //  CREACION DE INSTANCIAS
        public int Inst_Create(float PosX, float PosY, float PosZ)
        {
            c_instancia inst = new c_instancia();
            inst.pos = new Vector3(_InstanciaBase.pos.X, _InstanciaBase.pos.Y, _InstanciaBase.pos.Z);
            inst.rot = new Vector3(_InstanciaBase.rot.X, _InstanciaBase.rot.Y, _InstanciaBase.rot.Z);
            inst.size = new Vector3(_InstanciaBase.size.X, _InstanciaBase.size.Y, _InstanciaBase.size.Z);

            _instancia.Add(inst);

            return _instancia.Count - 1;
        }

        public int Inst_CreateAndSelect(float PosX, float PosY, float PosZ)
        {
            return _InstSel = Inst_Create(PosX, PosY, PosZ);
        }

        //  SELECCION DE INSTANCIAS
        public void Inst_Select(int i)
        {
            _InstSel = i;
        }

        //  MOVIMIENTO DE INSTANCIAS
        public void Inst_Position(float PosX, float PosY, float PosZ)
        {
            if (_InstSel < 0) return;


            _instancia[_InstSel].pos = new Vector3(0, 0, 0);
            _position[_InstSel] = new Vector3(PosX, PosY, PosZ);
        }

        //  ROTACION DE INSTANCIAS
        public void Inst_RotateAll(float RotX, float RotY, float RotZ)
        {
            for (int i = 0; i < _rotation.Count; i++)
            {
                _rotation[i] = new Vector3(_rotation[i].X + RotX, _rotation[i].Y + RotY, _rotation[i].Z + RotZ);
            }
        }











        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            for (int i = 0; i < _position.Count; i++)
            {
                for (int j = 0; j < _meshes.Count; j++)
                {
                    _meshes[j].Position = _position[i];
                    _meshes[j].Rotation = _rotation[i];
                    _meshes[j].UpdateMeshTransform();
                    _meshes[j].render();
                }
            }

        }
    }
}
