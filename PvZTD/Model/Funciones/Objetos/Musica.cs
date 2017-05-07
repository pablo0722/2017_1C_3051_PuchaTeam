using TGC.Core.Sound;
using TGC.Core.Text;
using System.IO;

namespace TGC.Group.Model
{
    public class t_Musica
    {
        /******************************************************************************************/
        /*                                  CONSTANTES
        /******************************************************************************************/
        // Musicas
        public const string PATH_MUSICA = "..\\..\\Media\\Musicas\\Plants vs Zombies Soundtrack. Pool Stage.mp3";










        /******************************************************************************************/
        /*                                  VARIABLES
        /******************************************************************************************/
        private string currentFile = null;
        private TgcMp3Player mp3Player = null;









        /******************************************************************************************/
        /*                                  CONSTRUCTOR
        /******************************************************************************************/
        public t_Musica()
        {
            mp3Player = new TgcMp3Player();
        }










        /******************************************************************************************/
        /*                                  CARGAR MP3
        /******************************************************************************************/
        ///     Cargar un nuevo MP3 si hubo una variacion
        public void Do_Load(string filePath)
        {
            if (currentFile == null || currentFile != filePath)
            {
                currentFile = filePath;

                //Cargar archivo
                mp3Player.closeFile();
                mp3Player.FileName = currentFile;
            }
        }










        /******************************************************************************************/
        /*                                  REPRODUCIR MP3
        /******************************************************************************************/
        public void Do_Play()
        {
            if (currentFile == null || mp3Player == null)
                return;

            var currentState = mp3Player.getStatus();
            if (currentState == TgcMp3Player.States.Paused)
            {
                //Resumir la ejecución del MP3
                mp3Player.resume();
            }
            else if (currentState == TgcMp3Player.States.Stopped)
            {
                //Parar y reproducir MP3
                mp3Player.closeFile();
                mp3Player.play(true);
            }
            else if (currentState == TgcMp3Player.States.Open)
            {
                //Reproducir MP3
                mp3Player.play(true);
            }
        }










        /******************************************************************************************/
        /*                                  PAUSAR MP3
        /******************************************************************************************/
        public void Do_Pause()
        {
            if (currentFile == null || mp3Player != null)
                return;

            var currentState = mp3Player.getStatus();
            if (currentState == TgcMp3Player.States.Playing)
            {
                //Pausar el MP3
                mp3Player.pause();
            }
        }










        /******************************************************************************************/
        /*                                  DETENER MP3
        /******************************************************************************************/
        public void Do_Stop()
        {
            if (currentFile == null || mp3Player != null)
                return;

            var currentState = mp3Player.getStatus();
            if (currentState == TgcMp3Player.States.Playing)
            {
                //Parar el MP3
                mp3Player.stop();
            }
        }
    }
}
