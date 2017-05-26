using System.Drawing;
using TGC.Core.Example;
using TGC.Group.Model.Funciones.Objetos;


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
        public const int CANT_SOLES_INIT = 50;//1366

        //      SCREEN
        public const int CANT_FILAS = 5;        // De plantas
        public const int CANT_COLUMNAS = 12;    // De plantas

        // NIVELES
        public const string TXT_NIVEL_1 = "..\\..\\Media\\Txt\\Nivel_1.txt";
        public const string TXT_NIVEL_2 = "..\\..\\Media\\Txt\\Nivel_2.txt";










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
        public t_Musica _musica;
        public string _NivelActual = null;
        public Drawer2D _spriteDrawer;
        public t_Hordas _Hordas;
        public Menu _Menu;
        public t_Super _Super;
        public int FirstRender = 2;







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
            _Menu = Menu.Crear(this);
           
            _spriteDrawer = new Drawer2D();
            _Hordas = new t_Hordas(this);
            _Super = new t_Super(this);
            _rand = new System.Random(System.Guid.NewGuid().GetHashCode());
            _TiempoTranscurrido = 0;
            _soles = CANT_SOLES_INIT;

            _camara = new t_Camara(this);
            _mouse = new t_Mouse(this);
            _colision = new t_Colision(this);
            _musica = new t_Musica();

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

            if (FirstRender == 0)
            {
                _TiempoTranscurrido += ElapsedTime;
            }

            if (Menu.IniciarJuego)
            {
                _camara.Update(ElapsedTime);

                _Hordas.Update();
                _Super.Update();
                pablo_update();
                jose_update();
            }
            else
            {
                _camara.UpdateMenu(_TiempoTranscurrido);
                _Menu.Update();
                _NivelActual = TXT_NIVEL_1;
            }

        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            if (FirstRender > 0)
            {
                FirstRender--;
            }

            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();
            
            pablo_render();
            jose_render();

            if (Menu.IniciarJuego)
            {
                DrawText.drawText("Soles:", 100, 0, Color.Yellow);
                DrawText.drawText(_soles.ToString(), 150, 0, Color.Yellow);
                _Super.Render();
                _Hordas.Render();
            }
            else
            {
                _Menu.Render();
             //   DrawText.drawText(_mouse.Position().X.ToString(), 100, 0, Color.Yellow);
               // DrawText.drawText(_mouse.Position().Y.ToString(), 150, 0, Color.Yellow);
            }

            t_ToonShading.RenderGlobal();

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