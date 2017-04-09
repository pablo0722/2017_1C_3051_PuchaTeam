using Microsoft.DirectX;
using TGC.Core.SceneLoader;

using System.Collections.Generic;

namespace TGC.Group.Model
{
    public class t_Objeto3D
    {
        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        private class t_instancia
        {
            public Vector3 pos;
            public Vector3 rot;
            public Vector3 size;

            public t_instancia()
            {
                pos = new Vector3(0, 0, 0);
                rot = new Vector3(0, 0, 0);
                size = new Vector3(1, 1, 1);
            }
            public t_instancia(t_instancia inst)
            {
                pos = new Vector3(inst.pos.X, inst.pos.Y, inst.pos.Z);
                rot = new Vector3(inst.rot.X, inst.rot.Y, inst.rot.Z);
                size = new Vector3(inst.size.X, inst.size.Y, inst.size.Z);
            }
        };










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        private List<TgcMesh> _meshes;
        private List<t_instancia> _instancias;
        private t_instancia _InstanciaBase;     // Todas las instancias se crean como copias de la _InstanciaBase
        private int _InstSel; //Instancia seleccionada










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Objeto3D(string PathObj)
        {
            _InstSel = -1;
            _meshes = new TgcSceneLoader().loadSceneFromFile(PathObj).Meshes;
            _instancias = new List<t_instancia>();
            _InstanciaBase = new t_instancia();
        }

        public static t_Objeto3D CrearObjeto3D(string PathObj)
        {
            if ((PathObj != null))
            {
                return new t_Objeto3D(PathObj);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      DESTRUCTOR
        /******************************************************************************************/
        ~t_Objeto3D()
        {
            for (int i = 0; i < _meshes.Count; i++)
            {
                _meshes[i].dispose();
            }
        }










        /******************************************************************************************/
        /*                                      TRANSFORMACIONES
        /******************************************************************************************/
        public void Set_Transform(  float PosX, float PosY, float PosZ,
                                float SizeX, float SizeY, float SizeZ,
                                float RotX, float RotY, float RotZ)
        {
            _InstanciaBase.pos = new Vector3(PosX, PosY, PosZ);
            _InstanciaBase.size = new Vector3(SizeX, SizeY, SizeZ);
            _InstanciaBase.rot = new Vector3(RotX, RotY, RotZ);
        }

        public void Set_Position(float X, float Y, float Z)
        {
            _InstanciaBase.pos = new Vector3(X, Y, Z);
        }

        public void Set_Size(float X, float Y, float Z)
        {
            _InstanciaBase.size = new Vector3(X, Y, Z);
        }

        public void Set_Rotation(float X, float Y, float Z)
        {
            _InstanciaBase.rot = new Vector3(X, Y, Z);
        }










        /******************************************************************************************/
        /*                                      INSTANCIAS
        /******************************************************************************************/

        //  CREACION DE INSTANCIAS
        public int Inst_Create()
        {
            t_instancia inst = new t_instancia(_InstanciaBase);

            _instancias.Add(inst);

            return _instancias.Count - 1;
        }

        public int Inst_Create(float PosX, float PosY, float PosZ)
        {
            t_instancia inst = new t_instancia(_InstanciaBase);

            inst.pos = new Vector3(PosX, PosY, PosZ);

            _instancias.Add(inst);

            return _instancias.Count - 1;
        }

        public int Inst_Create(Vector3 Pos)
        {
            t_instancia inst = new t_instancia(_InstanciaBase);

            inst.pos = new Vector3(Pos.X, Pos.Y, Pos.Z);

            _instancias.Add(inst);

            return _instancias.Count - 1;
        }

        //  CREACION Y SELECCION DE INSTANCIAS
        public int Inst_CreateAndSelect()
        {
            return _InstSel = Inst_Create();
        }

        public int Inst_CreateAndSelect(float PosX, float PosY, float PosZ)
        {
            return _InstSel = Inst_Create(PosX, PosY, PosZ);
        }

        public int Inst_CreateAndSelect(Vector3 Pos)
        {
            return _InstSel = Inst_Create(Pos);
        }

        //  SELECCION DE INSTANCIAS
        public void Inst_Select(int i)
        {
            _InstSel = i;
        }
        public void Inst_SelectNone()
        {
            _InstSel = -1;
        }

        //  POSICION DE INSTANCIAS
        public void Inst_Set_Position(float PosX, float PosY, float PosZ)
        {
            if (_InstSel < 0) return;

            _instancias[_InstSel].pos = new Vector3(PosX, PosY, PosZ);
        }

        public void Inst_Set_PositionX(float PosX)
        {
            if (_InstSel < 0) return;

            _instancias[_InstSel].pos = new Vector3(PosX, _instancias[_InstSel].pos.Y, _instancias[_InstSel].pos.Z);
        }

        public void Inst_Set_PositionY(float PosY)
        {
            if (_InstSel < 0) return;

            _instancias[_InstSel].pos = new Vector3(_instancias[_InstSel].pos.X, PosY, _instancias[_InstSel].pos.Z);
        }

        public void Inst_Set_PositionZ(float PosZ)
        {
            if (_InstSel < 0) return;

            _instancias[_InstSel].pos = new Vector3(_instancias[_InstSel].pos.X, _instancias[_InstSel].pos.Y, PosZ);
        }

        //  ROTAR DE INSTANCIAS
        public void Inst_Rotate(float RotX, float RotY, float RotZ)
        {
            if (_InstSel < 0) return;

            Vector3 RotAux = _instancias[_InstSel].rot;
            _instancias[_InstSel].rot = new Vector3(RotAux.X + RotX, RotAux.Y + RotY, RotAux.Z + RotZ);
        }

        public void Inst_RotateAll(float RotX, float RotY, float RotZ)
        {
            for (int i = 0; i < _instancias.Count; i++)
            {
                Vector3 RotAux = _instancias[i].rot;
                _instancias[i].rot = new Vector3(RotAux.X + RotX, RotAux.Y + RotY, RotAux.Z + RotZ);
            }
        }











        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render()
        {
            for (int i = 0; i < _instancias.Count; i++)
            {
                for (int j = 0; j < _meshes.Count; j++)
                {
                    _meshes[j].Position = _instancias[i].pos;
                    _meshes[j].Rotation = _instancias[i].rot;
                    _meshes[j].Scale = _instancias[i].size;
                    _meshes[j].UpdateMeshTransform();
                    _meshes[j].render();
                }
            }

        }
    }
}
