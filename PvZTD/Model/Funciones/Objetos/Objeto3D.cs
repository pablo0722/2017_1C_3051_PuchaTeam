using Microsoft.DirectX;
using TGC.Core.SceneLoader;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using System.Collections.Generic;


namespace TGC.Group.Model
{
    public class t_Objeto3D
    {
        /******************************************************************************************/
        /*                                      ESTRUCTURAS
        /******************************************************************************************/
        public class t_instancia
        {
            public class t_ShadersHabilitados
            {
                public bool Girar = false;
                public bool BolaDeFuego = false;
                public bool BolaDeExplosion = false;
                public bool Explosion = false;
                public bool SuperGirasol = false;
                public bool GirarSoles = false;
            };


            public Vector3 pos;
            public Vector3 rot;
            public Vector3 size;
            public Color color;
            public t_ShadersHabilitados shaders;

            public t_instancia()
            {
                pos = new Vector3(0, 0, 0);
                rot = new Vector3(0, 0, 0);
                size = new Vector3(1, 1, 1);
                color = Color.FromArgb(0, 255, 255, 255);
                shaders = new t_ShadersHabilitados();
            }
            public t_instancia(t_instancia inst)
            {
                pos = new Vector3(inst.pos.X, inst.pos.Y, inst.pos.Z);
                rot = new Vector3(inst.rot.X, inst.rot.Y, inst.rot.Z);
                size = new Vector3(inst.size.X, inst.size.Y, inst.size.Z);
                color = Color.FromArgb(inst.color.A, inst.color.R, inst.color.G, inst.color.B);
                shaders = new t_ShadersHabilitados();
            }
        };

        public class t_mesh
        {
            public List<TgcMesh> mesh;
            public List<Color> color;


            public t_mesh(string PathObj)
            {
                mesh = new TgcSceneLoader().loadSceneFromFile(PathObj).Meshes;
                color = new List<Color>();
                for (int i = 0; i < mesh.Count; i++)
                {
                    color.Add(Color.FromArgb(0, 255, 255, 255));
                }
            }
        };










        /******************************************************************************************/
        /*                                      VARIABLES
        /******************************************************************************************/
        // ESTATICAS
        public static bool _ShowBoundingBox = false;

        // NO ESTATICAS
        public t_mesh _meshes;
        public List<t_instancia> _instancias;
        public t_instancia _instanciaActual;
        private t_instancia _InstanciaBase;     // Todas las instancias se crean como copias de la _InstanciaBase
        private int _MeshSel; // Mesh seleccionado
        private GameModel _game;
        private bool _ChangeColorMesh;
        private bool _ChangeColorInst;










        /******************************************************************************************/
        /*                                      CONSTRUCTOR
        /******************************************************************************************/
        private t_Objeto3D(GameModel game, string PathObj)
        {
            _game = game;

            _instanciaActual = null;
            _meshes = new t_mesh(PathObj);
            _instancias = new List<t_instancia>();
            _InstanciaBase = new t_instancia();
            _ChangeColorMesh = false;
            _ChangeColorInst = false;
        }

        public static t_Objeto3D Crear(GameModel game, string PathObj)
        {
            if ((PathObj != null) && (game != null))
            {
                return new t_Objeto3D(game, PathObj);
            }

            return null;
        }










        /******************************************************************************************/
        /*                                      DESTRUCTOR
        /******************************************************************************************/
        ~t_Objeto3D()
        {
            for (int i = 0; i < _meshes.mesh.Count; i++)
            {
                _meshes.mesh[i].dispose();
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

        public void Set_Color(int Alpha, int Red, int Green, int Blue)
        {
            _InstanciaBase.color = Color.FromArgb(Alpha, Red, Green, Blue);
        }

        public void Set_Color(int Red, int Green, int Blue)
        {
            _InstanciaBase.color = Color.FromArgb(0, Red, Green, Blue);
        }










        /******************************************************************************************/
        /*                                      MESHES
        /******************************************************************************************/
        //  SELECCION DE MESH
        public void Mesh_Select(int i)
        {
            if (_MeshSel < 0 || _MeshSel >= _meshes.mesh.Count)
                Mesh_SelectNone();

            _MeshSel = i;
        }
        public void Mesh_SelectNone()
        {
            _MeshSel = -1;
        }

        //  COLOR MESHES
        public void Mesh_Color(int Alpha, int Red, int Green, int Blue)
        {
            if (_MeshSel < 0 || _MeshSel >= _meshes.mesh.Count) return;

            _meshes.color[_MeshSel] = Color.FromArgb(Alpha, Red, Green, Blue);
            _ChangeColorMesh = true;
        }

        public void Mesh_Color(int Red, int Green, int Blue)
        {
            if (_MeshSel < 0 || _MeshSel >= _meshes.mesh.Count) return;

            _meshes.color[_MeshSel] = Color.FromArgb(0, Red, Green, Blue);
            _ChangeColorMesh = true;
        }

        public void Mesh_ColorAll(int Red, int Green, int Blue)
        {
            for (int i = 0; i < _meshes.mesh.Count; i++)
            {
                _meshes.color[i] = Color.FromArgb(0, Red, Green, Blue);
                _ChangeColorMesh = true;
            }
        }










        /******************************************************************************************/
        /*                                      INSTANCIAS
        /******************************************************************************************/

        //  CREACION DE INSTANCIAS
        public t_instancia Inst_Create()
        {
            t_instancia inst = new t_instancia(_InstanciaBase);

            _instancias.Add(inst);
            _ChangeColorMesh = true;

            _instanciaActual = inst;

            return inst;
        }

        public t_instancia Inst_Create(float PosX, float PosY, float PosZ)
        {
            t_instancia inst = new t_instancia(_InstanciaBase);

            inst.pos = new Vector3(PosX, PosY, PosZ);

            _instancias.Add(inst);
            _ChangeColorMesh = true;

            _instanciaActual = inst;

            return inst;
        }

        public t_instancia Inst_Create(Vector3 Pos)
        {
            t_instancia inst = new t_instancia(_InstanciaBase);

            inst.pos = new Vector3(Pos.X, Pos.Y, Pos.Z);

            _instancias.Add(inst);
            _ChangeColorMesh = true;

            _instanciaActual = inst;

            return inst;
        }

        //  CREACION Y SELECCION DE INSTANCIAS
        public t_instancia Inst_CreateAndSelect()
        {
            return _instanciaActual = Inst_Create();
        }

        public t_instancia Inst_CreateAndSelect(float PosX, float PosY, float PosZ)
        {
            return _instanciaActual = Inst_Create(PosX, PosY, PosZ);
        }

        public t_instancia Inst_CreateAndSelect(Vector3 Pos)
        {
            return _instanciaActual = Inst_Create(Pos);
        }

        //  SELECCION DE INSTANCIAS
        public void Inst_Select(t_instancia inst)
        {
            _instanciaActual = inst;
        }
        public void Inst_SelectNone()
        {
            _instanciaActual = null;
        }

        //  ELIMINACION DE INSTANCIAS
        public void Inst_Delete()
        {
            if (_instanciaActual == null) return;

            _instancias.Remove(_instanciaActual);
        }
        public void Inst_DeleteAll()
        {
            _instancias.Clear();
        }

        public void Inst_Delete(t_instancia inst)
        {
            if (inst == null) return;

            _instancias.Remove(inst);
        }

        //  POSICION DE INSTANCIAS
        public void Inst_Set_Position(float PosX, float PosY, float PosZ)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.pos = new Vector3(PosX, PosY, PosZ);
        }

        public void Inst_Set_PositionX(float PosX)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.pos = new Vector3(PosX, _instanciaActual.pos.Y, _instanciaActual.pos.Z);
        }

        public void Inst_Set_PositionY(float PosY)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.pos = new Vector3(_instanciaActual.pos.X, PosY, _instanciaActual.pos.Z);
        }

        public void Inst_Set_PositionZ(float PosZ)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.pos = new Vector3(_instanciaActual.pos.X, _instanciaActual.pos.Y, PosZ);
        }

        //  MOVER INSTANCIAS
        public void Inst_Move(float PosX, float PosY, float PosZ)
        {
            if (_instanciaActual == null) return;

            Vector3 PosAux = _instanciaActual.pos;
            _instanciaActual.pos = new Vector3(PosAux.X + PosX, PosAux.Y + PosY, PosAux.Z + PosZ);
        }

        public void Inst_MoveAll(float PosX, float PosY, float PosZ)
        {
            for (int i = 0; i < _instancias.Count; i++)
            {
                Vector3 PosAux = _instancias[i].pos;
                _instancias[i].pos = new Vector3(PosAux.X + PosX, PosAux.Y + PosY, PosAux.Z + PosZ);
            }
        }

        //  ROTAR INSTANCIAS
        public void Inst_Rotate(float RotX, float RotY, float RotZ)
        {
            if (_instanciaActual == null) return;

            Vector3 RotAux = _instanciaActual.rot;
            _instanciaActual.rot = new Vector3(RotAux.X + RotX, RotAux.Y + RotY, RotAux.Z + RotZ);
        }

        public void Inst_RotateAll(float RotX, float RotY, float RotZ)
        {
            for (int i = 0; i < _instancias.Count; i++)
            {
                Vector3 RotAux = _instancias[i].rot;
                _instancias[i].rot = new Vector3(RotAux.X + RotX, RotAux.Y + RotY, RotAux.Z + RotZ);
            }
        }

        //  COLOR INSTANCIAS
        public void Inst_Color(int Alpha, int Red, int Green, int Blue)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.color = Color.FromArgb(Alpha, Red, Green, Blue);
            _ChangeColorInst = true;
        }

        public void Inst_Color(int Red, int Green, int Blue)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.color = Color.FromArgb(0, Red, Green, Blue);
            _ChangeColorInst = true;
        }

        public void Inst_ColorAll(int Red, int Green, int Blue)
        {
            for (int i = 0; i < _instancias.Count; i++)
            {
                _instancias[i].color = Color.FromArgb(0, Red, Green, Blue);
                _ChangeColorInst = true;
            }
        }

        //  SHADER INSTANCIAS
        public void Inst_ShaderGirar(bool activate)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.shaders.Girar = activate;
        }
        
        public void Inst_ShaderBolaDeFuego(bool activate)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.shaders.BolaDeFuego = activate;
        }

        public void Inst_ShaderBolaDeExplosion(bool activate)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.shaders.BolaDeExplosion = activate;
        }

        public void Inst_ShaderExplosion(bool activate)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.shaders.Explosion = activate;
        }

        public void Inst_ShaderSuperGirasol(bool activate)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.shaders.SuperGirasol = activate;
        }

        public void Inst_ShaderAllSuperGirasol(bool activate)
        {
            for (int i = 0; i < _instancias.Count; i++)
            {
                _instancias[i].shaders.SuperGirasol = activate;
            }
        }
        public void Inst_ShaderGirarSoles(bool activate)
        {
            if (_instanciaActual == null) return;

            _instanciaActual.shaders.GirarSoles = activate;
        }










        /******************************************************************************************/
        /*                                      UPDATE
        /******************************************************************************************/
        public void Update(bool ShowBoundingBoxWithKey)
        {
            _ShowBoundingBox = false;

            if (ShowBoundingBoxWithKey)
            {
                if (_game.Input.keyDown(Key.F))
                {
                    _ShowBoundingBox = true;
                }
            }
        }










        /******************************************************************************************/
        /*                                      RENDER
        /******************************************************************************************/
        public void Render(bool ocultar)
        {
            for (int i = 0; i < _instancias.Count; i++)
            {
                if(_instancias[i].pos.Y < -90) return;

                if (_instancias[i].pos.Z >= _game._camara._CamaraAerea.Position.Z+20 || !_game._camara.Modo_Is_CamaraPersonal() || !ocultar)
                {
                    for (int j = 0; j < _meshes.mesh.Count; j++)
                    {
                        if (_ChangeColorMesh)
                        {
                            _meshes.mesh[j].setColor(_meshes.color[j]);
                        }
                        else if (_ChangeColorInst)
                        {
                            _meshes.mesh[j].setColor(_instancias[i].color);
                        }
                        _meshes.mesh[j].Position = _instancias[i].pos;
                        _meshes.mesh[j].Rotation = _instancias[i].rot;
                        _meshes.mesh[j].Scale = _instancias[i].size;
                        
                        // RENDERIZA TODOS LOS SHADERS
                        _game.shader.Render(_meshes.mesh[j], _instancias[i].shaders);


                        _meshes.mesh[j].UpdateMeshTransform();
                        _meshes.mesh[j].render();

                        if (_ShowBoundingBox)
                        {
                            _meshes.mesh[j].BoundingBox.render();
                        }
                    }
                }
            }

            _ChangeColorMesh = false;
            _ChangeColorInst = false;
        }
    }
}
