using System.IO;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Paint.GLSL.Brushes
{
    class Brush :BrushBase
    {
        private readonly Canvas _canvas;
        private Shader _shader;

        private float _time;

        public Brush(Canvas canvas, string name, byte[] fragShader)
        {
            Name = name;

            _canvas = canvas;
            _shader = new Shader(new MemoryStream(Properties.Resources.VertexShader),
                new MemoryStream(fragShader));

            _shader.SetParameter("time", _time);
            _shader.SetParameter("resolution", new Vector2f(_canvas.Size.X, _canvas.Size.Y));
            _shader.SetParameter("size", _canvas.MainWindow.size);
            RenderStates = new RenderStates(_shader);
            RenderStates.Texture = _canvas.Texture;
        }

        public override void Update()
        {
            _shader.SetParameter("time", _time);
            _shader.SetParameter("texture", _canvas.BackTexture.Texture);
            _shader.SetParameter("mouse", Mouse.GetPosition(_canvas.window).ConvertToVector2f());
            _shader.SetParameter("size", _canvas.MainWindow.size);
            _shader.SetParameter("resolution", new Vector2f(_canvas.Size.X, _canvas.Size.Y));
            _shader.SetParameter("input_color", _canvas.MainWindow.color);
            _time += 0.005f;
        }


    }
}
