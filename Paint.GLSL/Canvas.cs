using System.Collections.Generic;
using System.Linq;
using Paint.GLSL.Brushes;
using SFML.Graphics;
using SFML.System;

namespace Paint.GLSL
{
    class Canvas :Game
    {

        public readonly MainWindow MainWindow;

        public BrushBase Brush;

        public Texture Texture;
        public RectangleShape RectangleShape;

        private IDrawing _drawing;
        private IDrawing _manualDrawing;
        private IDrawing _autoDrawing;

        public Canvas(uint width, uint height, MainWindow mainWindow, uint frameRateLimit, int sizeOfHistory) 
            : base(width, height, "Canvas", Color.White, frameRateLimit, RenderTo.Window)
        {
            MainWindow = mainWindow;

            RenderTexture renderTexture = new RenderTexture(Size.X,Size.Y);

            _manualDrawing = new ManualDrawing(this, sizeOfHistory,renderTexture);
            _autoDrawing =new AutoDrawing(this,renderTexture);
            _drawing = _autoDrawing;
        }


        public override void Initialize()
        {
            Texture = new Texture(Size.X, Size.Y);

            RectangleShape = new RectangleShape(new Vector2f(Size.X, Size.Y));
            RectangleShape.Texture = Texture;

            Brush = Brushes.Values.First();

            _manualDrawing.Initialize();
            _autoDrawing.Initialize();
        }


        public Dictionary<string, BrushBase> Brushes;

        public void AddBrushes(Dictionary<string, BrushBase> brushes)
        {
            Brushes = brushes;
        }


        public Texture GetBackTexture()
        {
            return _drawing.GetBackTexture();
        }

        public override void Update()
        {
            _drawing.Update();
        }

        public override void Render()
        {
            _drawing.Render();
        }
    }
}
