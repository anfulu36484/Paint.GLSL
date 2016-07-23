using System;
using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Paint.GLSL
{
    abstract class Game 
    {
        public RenderWindow Window;
        protected Color clearColor;

        public Vector2u Size;

        Stopwatch _stopwatch = new Stopwatch();


        public RenderTo RenderTo;

        protected Game(uint width, uint height, string title, Color clearColor, uint frameRateLimit, RenderTo renderTo)
        {
            Size = new Vector2u(width,height);
            RenderTo = renderTo;
            this.clearColor = clearColor;


            if (RenderTo == RenderTo.Window)
            {
                this.Window = new RenderWindow(new VideoMode(width, height), title, Styles.Default);
                Window.SetActive(true);
                Window.Position = new Vector2i(Window.Position.X, 0);

                if (frameRateLimit>0)
                    Window.SetFramerateLimit(frameRateLimit);
                // Set up events
                Window.Closed += OnClosed;
                Window.Resized += Window_Resized;
            }
            else
                RenderTexture = new RenderTexture(Size.X, Size.Y);
         
        }



        private void Window_Resized(object sender, SizeEventArgs e)
        {
            Window.Size = Size;

            //Size = window.Size;
        }

        public abstract void Initialize();

        public abstract void Update();

        public abstract void Render();


        public RenderTexture RenderTexture;

        private int _index;

        public void Run()
        {
            Initialize();

            _stopwatch.Start();

            while (true)
            {
                
                Update();

                    if (RenderTo == RenderTo.Window)
                    {
                        Window.DispatchEvents();

                        Window.Clear(clearColor);
                        Render();
                        Window.Display();

                    }
                    else
                    {
                        RenderTexture.Clear(clearColor);
                        Render();
                        Texture texture = RenderTexture.Texture;
                        Image image = texture.CopyToImage();
                        image.SaveToFile($"data\\img{_index}.png");
                        image.Dispose();

                    }

                _index++;


                FPS = 1/(float)_stopwatch.ElapsedMilliseconds*1000;


                Window.SetTitle($"FPS {FPS:#}");
                _stopwatch.Restart();
            }
        }


        public float FPS;


        private void OnClosed(object sender, EventArgs e)
        {
            Window.Close();
        }

    }


    enum RenderTo
    {
        Window, Image
    }
}


