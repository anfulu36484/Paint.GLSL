using System.Collections.Generic;
using Paint.GLSL.Brushes;
using SFML.Graphics;
using SFML.System;

namespace Paint.GLSL
{
    class Canvas :Game
    {

        public readonly MainWindow MainWindow;

        public Texture Texture;
        public RectangleShape RectangleShape;

        private IDrawing _drawing;
        private IDrawing _manualDrawing;
        private IDrawing _autoDrawing;

        public Canvas(uint width, uint height, MainWindow mainWindow, uint frameRateLimit, int sizeOfHistory) 
            : base(width, height, "Canvas", Color.White, frameRateLimit, RenderTo.Window)
        {
            MainWindow = mainWindow;
            mainWindow.DrawingDataIsLoaded += MainWindow_DrawingDataIsLoaded;

            _manualDrawing = new ManualDrawing(this, sizeOfHistory, new RenderTexture(Size.X, Size.Y));
            _autoDrawing =new AutoDrawing(this, new RenderTexture(Size.X, Size.Y));
            _drawing = _manualDrawing;

            ((AutoDrawing) _autoDrawing).EndOfTheListOfDrawingDataReached += Canvas_EndOfTheListOfDrawingDataReached;
        }

        private void Canvas_EndOfTheListOfDrawingDataReached(object sender, System.EventArgs e)
        {
            _drawing = _manualDrawing;
            _manualDrawing.SetBackRenderTexture(_autoDrawing.GetBackRenderTexture());
        }

        private void MainWindow_DrawingDataIsLoaded(object sender, System.EventArgs e)
        {
            _drawing = _autoDrawing;
            _autoDrawing.SetBackRenderTexture(_manualDrawing.GetBackRenderTexture());
            ((AutoDrawing)_autoDrawing).AddDrawingData(((EventsArgDrawingData)e).DrawingDataList);
        }

        public override void Initialize()
        {
            Texture = new Texture(Size.X, Size.Y);

            RectangleShape = new RectangleShape(new Vector2f(Size.X, Size.Y));
            RectangleShape.Texture = Texture;

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
