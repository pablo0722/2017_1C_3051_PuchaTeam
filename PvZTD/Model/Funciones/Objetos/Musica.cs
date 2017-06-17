using TGC.Core.Sound;
using System.Collections.Generic;
using Microsoft.DirectX;

namespace TGC.Group.Model
{
    public class t_Sonidos
    {
        /******************************************************************************************/
        /*                                  CONSTANTES
        /******************************************************************************************/
        // MUSICA
        private const string MUSICA_PATH = "..\\..\\Media\\Sonidos\\PvZ_Pool_Stage.wav";
        static public int MUSICA_ID; // Se carga en el constructor

        // WALK
        private const string WALK_PATH =    "..\\..\\Media\\Sonidos\\footstepsLeaves.wav";
        static public int WALK_ID; // Se carga en el constructor

        // EAT
        private const string EAT_PATH = "..\\..\\Media\\Sonidos\\zombieEating.wav";
        static public int EAT_ID; // Se carga en el constructor

        // ROAR
        private const string ROAR_PATH = "..\\..\\Media\\Sonidos\\zombieRoar.wav";
        static public int ROAR_ID; // Se carga en el constructor

        // Control
        static public int CANT_ID; // Se carga en el constructor










        /******************************************************************************************/
        /*                                  VARIABLES
        /******************************************************************************************/
        private List<Tgc3dSound> sonidos;
        private GameModel _game;
        public int musicvolume = 100; // de 0  a 100
        public int fxvolume = 50;  // de 0 a 100









        /******************************************************************************************/
        /*                                  CONSTRUCTOR
        /******************************************************************************************/
        public t_Sonidos(GameModel game)
        {
            _game = game;

            sonidos = new List<Tgc3dSound>();

            Do_Load();
            
            Do_PlayLoop(MUSICA_ID);
            Do_Stop(WALK_ID);
            Do_Stop(EAT_ID);
            Do_Stop(ROAR_ID);
        }









        /******************************************************************************************/
        /*                                  DESTRUCTOR
        /******************************************************************************************/
        ~t_Sonidos()
        {
            foreach (var sound in sonidos)
            {
                sound.dispose();
            }
        }










        /******************************************************************************************/
        /*                                  CARGAR MP3
        /******************************************************************************************/
        ///     Cargar un nuevo MP3 si hubo una variacion
        private void Do_Load()
        {
            Tgc3dSound sound;
            int id = 0;

            sound = new Tgc3dSound(MUSICA_PATH, new Vector3(0, 0, 0), _game.DirectSound.DsDevice);
            //Hay que configurar la mínima distancia a partir de la cual se empieza a atenuar el sonido 3D
            sound.MinDistance = 50f;
            sonidos.Add(sound);
            MUSICA_ID = id;
            id++;

            sound = new Tgc3dSound(WALK_PATH, new Vector3(0, 0, 0), _game.DirectSound.DsDevice);
            //Hay que configurar la mínima distancia a partir de la cual se empieza a atenuar el sonido 3D
            sound.MinDistance = 50f;
            sonidos.Add(sound);
            WALK_ID = id;
            id++;

            sound = new Tgc3dSound(EAT_PATH, new Vector3(0, 0, 0), _game.DirectSound.DsDevice);
            //Hay que configurar la mínima distancia a partir de la cual se empieza a atenuar el sonido 3D
            sound.MinDistance = 50f;
            sonidos.Add(sound);
            EAT_ID = id;
            id++;

            sound = new Tgc3dSound(ROAR_PATH, new Vector3(0, 0, 0), _game.DirectSound.DsDevice);
            //Hay que configurar la mínima distancia a partir de la cual se empieza a atenuar el sonido 3D
            sound.MinDistance = 50f;
            sonidos.Add(sound);
            ROAR_ID = id;
            id++;

            CANT_ID = id;

            Update();
        }










        /******************************************************************************************/
        /*                                  REPRODUCIR SONIDOS
        /******************************************************************************************/
        public void Do_PlayLoop(int id)
        {
            if (id < 0 || id >= CANT_ID) return;

            //Ejecuta en loop
            sonidos[id].play(true);
        }

        public void Do_PlayOnce(int id)
        {
            if (id < 0 || id >= CANT_ID) return;

            //Ejecuta una sola vez
            sonidos[id].play(false);
        }










        /******************************************************************************************/
        /*                                  UPDATE SONIDOS
        /******************************************************************************************/
        public void Update()
        {
            if (musicvolume == 0)
                musicvolume = -1000;

            if (fxvolume == 0)
                fxvolume = -1000;

            sonidos[MUSICA_ID].Position = new Vector3(500 - musicvolume * 5, 0, 0);
            sonidos[WALK_ID].Position = new Vector3(500 - fxvolume * 5, 0, 0);
            sonidos[EAT_ID].Position = new Vector3(500 - fxvolume * 5, 0, 0);
            sonidos[ROAR_ID].Position = new Vector3(500 - fxvolume * 5, 0, 0);
        }










        /******************************************************************************************/
        /*                                  DETENER SONIDO
        /******************************************************************************************/
        public void Do_Stop(int id)
        {
            if (id < 0 || id >= CANT_ID) return;

            //Ejecuta una sola vez
            sonidos[id].stop();
        }
    }
}
