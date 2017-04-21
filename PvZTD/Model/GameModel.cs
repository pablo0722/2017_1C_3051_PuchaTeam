using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Utils;
using System.Threading.Tasks;


namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
    public partial class GameModel : TgcExample
    {
        /******************************************************************************************/
        /*                                  CONSTANTES
        /******************************************************************************************/
        public const float PI = 3.14159265359F;

        //      SCREEN
        public const int WIDTH = 1366;
        public const int HEIGHT = 768;
        public const int CANT_FILAS = 5;        // De plantas
        public const int CANT_COLUMNAS = 12;    // De plantas


        







        /******************************************************************************************/
        /*                                  VARIABLES
        /******************************************************************************************/
        // CAMARA
        public t_Camara _camara;
        public t_Mouse _mouse;
        public t_Colision _colision;
        public t_EscenarioBase _EscenarioBase;
        public float _TiempoTranscurrido;
        public System.Random _rand;
        public int _soles;  // Cantidad de soles






        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            //Device de DirectX para crear primitivas.
            //var d3dDevice = D3DDevice.Instance.Device;

            _rand = new System.Random(System.Guid.NewGuid().GetHashCode());
            _TiempoTranscurrido = 0;
            _soles = 0;

            _camara = new t_Camara(this);
            _mouse = new t_Mouse(this);
            _colision = new t_Colision(this);

            pablo_init();
            jose_init();
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();

            _camara.Update(ElapsedTime);

            pablo_update();
            jose_update();
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();
            DrawText.drawText("Soles:", 100, 0, Color.Yellow);
            DrawText.drawText(_soles.ToString(), 150, 0, Color.Yellow);

            pablo_render();
            jose_render();

            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            PostRender();
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            pablo_dispose();
            jose_dispose();
        }
    }
}