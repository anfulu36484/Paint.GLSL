using System;
using System.Collections.Generic;
using System.Windows;
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

        //Random random = new Random();

        public void Initialize()
        {
            /*_drawingDataList = new List<DrawingData>();

            for (int i = 0; i < 1000; i++)
            {
                _drawingDataList.Add(new DrawingData("Brush"+random.Next(5,6), random.Next(1,20),
                    new Vector2f(random.Next((int)_canvas.Window.Size.X), random.Next((int)_canvas.Window.Size.Y)), 
                    new Color((byte)random.Next(10,20), 
                    (byte)random.Next(10, 70), (byte)random.Next(10, 120))));
            }*/


            _defaultBrush = new Brush(_canvas,"defaultBrush", Properties.Resources.Turn);
            _brush = _defaultBrush;
        }

        private BrushBase _brush;

        public void Update()
        {
            try
            {
                if (index < _drawingDataList.Count)
                {
                    _brush = _canvas.Brushes[_drawingDataList[index].NameOfBrush];

                    _brush.Update(
                        _drawingDataList[index].SizeOfBrush,
                        TransformPosition(_drawingDataList[index].Position),
                        _drawingDataList[index].Color
                        );
                    index++;
                }
                else
                {
                    index = 0;
                    _drawingDataList.Clear();
                    EndOfTheListOfDrawingDataReached?.Invoke(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                EndOfTheListOfDrawingDataReached?.Invoke(this, new EventArgs());
            }
        }


        Vector2f TransformPosition(Vector2f input)
        {
            return new Vector2f(_canvas.Window.Size.X*input.X,_canvas.Window.Size.Y*input.Y);
        }

        public event EventHandler EndOfTheListOfDrawingDataReached;

        public void Render()
        {
            if (index <= _drawingDataList.Count)
            {
                _canvas.Window.Draw(_canvas.RectangleShape,
                    _brush.RenderStates);
                _renderTexture.Draw(_canvas.RectangleShape,
                    _brush.RenderStates);
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

        public RenderTexture GetBackRenderTexture()
        {
            return _renderTexture;
        }

        public void SetBackRenderTexture(RenderTexture renderTexture)
        {
            _renderTexture = renderTexture;
        }
    }
}
