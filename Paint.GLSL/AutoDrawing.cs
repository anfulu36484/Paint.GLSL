using System;
using System.Collections.Generic;
using Paint.GLSL.Brushes;
using SFML.Graphics;
using SFML.System;

namespace Paint.GLSL
{
    class AutoDrawing:IDrawing
    {
        private readonly Canvas _canvas;
        private RenderTexture _renderTexture;

        private List<DrawingData> _drawingDataList;

        private BrushBase _defaultBrush; 

        public AutoDrawing(Canvas canvas, RenderTexture renderTexture)
        {
            _canvas = canvas;
            _renderTexture = renderTexture;
        }

        private int index;

        public void AddDrawingData(List<DrawingData> drawingDataList)
        {
            _drawingDataList = drawingDataList;
        }

        Random random = new Random();

        public void Initialize()
        {
            _drawingDataList = new List<DrawingData>
            {
                /*new DrawingData("Brush4", 10, new Vector2f(0, 0), new Color(160, 20, 10)),
                new DrawingData("Brush4", 10, new Vector2f(0, 1), new Color(160, 10, 10)),
                new DrawingData("Brush1", 1, new Vector2f(400, 300), new Color(10, 160, 10)),
                new DrawingData("Brush1", 10, new Vector2f(100, 800), new Color(160, 20, 10)),
                new DrawingData("Brush4", 10, new Vector2f(800, 500), new Color(110, 10, 10)),
                new DrawingData("Brush4", 1, new Vector2f(900, 300), new Color(10, 160, 10))*/
            };

            for (int i = 0; i < 1000; i++)
            {
                _drawingDataList.Add(new DrawingData("Brush"+random.Next(5,6), random.Next(1,20),
                    new Vector2f(random.Next((int)_canvas.Window.Size.X), random.Next((int)_canvas.Window.Size.Y)), 
                    new Color((byte)random.Next(10,20), 
                    (byte)random.Next(10, 70), (byte)random.Next(10, 120))));
            }


            _defaultBrush = new Brush(_canvas,"defaultBrush", Properties.Resources.Turn);
        }

        private BrushBase _brush;

        public void Update()
        {
            if (index < _drawingDataList.Count)
            {
                _brush=_canvas.Brushes[_drawingDataList[index].NameOfBrush];

                _brush.Update(
                        _drawingDataList[index].SizeOfBrush,
                        _drawingDataList[index].Position,
                        _drawingDataList[index].Color
                    );
                index++;
            }
            else
            {
                EndOfTheListOfDrawingDataReached?.Invoke(this,new EventArgs());
            }
        }


        public event EventHandler EndOfTheListOfDrawingDataReached;

        public void Render()
        {
            if (index < _drawingDataList.Count)
            {
                _canvas.Window.Draw(_canvas.RectangleShape,
                    _brush.RenderStates);
                _renderTexture.Draw(_canvas.RectangleShape,
                    _brush.RenderStates);
                //_renderTexture.Texture.CopyToImage().SaveToFile(index + ".bmp");
                
            }
            else
            {
                _canvas.Window.Draw(_canvas.RectangleShape,_defaultBrush.RenderStates);
            }
        }

        public Texture GetBackTexture()
        {
            return _renderTexture.Texture;
        }
    }
}
