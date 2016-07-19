using System;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Paint.GLSL
{
    abstract class Game 
    {
        public RenderWindow window;
        protected Color clearColor;

        public Vector2u Size;

        Stopwatch _stopwatch = new Stopwatch();


        public int FrameRateLimit = 250;

        public RenderTo RenderTo;

        protected Game(uint width, uint height, string title, Color clearColor, RenderTo renderTo)
        {
            Size = new Vector2u(width,height);
            RenderTo = renderTo;
            this.clearColor = clearColor;


     
            this.window = new RenderWindow(new VideoMode(width, height), title, Styles.Default);
            window.SetActive(true);
            window.Position = new Vector2i(window.Position.X, 0);
            //window.SetFramerateLimit((uint) FrameRateLimit);
            // Set up events
            window.Closed += OnClosed;
            window.Resized += Window_Resized;
  
        }



        private void Window_Resized(object sender, SizeEventArgs e)
        {
            window.Size = Size;

            //Size = window.Size;
        }

        public abstract void Load();

        public abstract void Initialize();

        public abstract void Update();

        public abstract void Render();


        private int _index;

        public void Run()
        {
            Load();
            Initialize();

            _stopwatch.Start();

            while (true)
            {
                
                Update();
                window.DispatchEvents();

                window.Clear(clearColor);
                Render();
                window.Display();
                

                FPS = 1/(float)_stopwatch.ElapsedMilliseconds*1000;


                window.SetTitle($"FPS {FPS:#}");
                _stopwatch.Restart();
            }
        }


        public float FPS;


        private void OnClosed(object sender, EventArgs e)
        {
            window.Close();
        }

    }


    enum RenderTo
    {
        Window, Image
    }
}


