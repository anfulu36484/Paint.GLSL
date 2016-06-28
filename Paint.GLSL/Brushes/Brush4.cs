using System.IO;
using System.Windows.Forms.VisualStyles;
using SFML.Graphics;
using SFML.System;
using SFML.Window;


namespace Paint.GLSL.Brushes
{
    class Brush4 : Game
    {
        private readonly MainWindow _mainWindow;

        public Brush4(RenderTo render,MainWindow mainWindow) 
            : base(1500, 800, "Brush4", Color.White,render)
        {
            _mainWindow = mainWindow;
        }

        private Texture _texture;
        private RectangleShape _rectangleShape;

        private Shader _shader;

        private float _time;

        private RenderStates _rState;


        private RenderTexture _backTexture;

        public override void Load()
        {

        }

        public override void Initialize()
        {
            _texture = new Texture(Size.X, Size.Y);

            _rectangleShape = new RectangleShape(new Vector2f(Size.X, Size.Y));
            _rectangleShape.Texture = _texture;

            _shader = new Shader(new MemoryStream(Properties.Resources.VertexShader),
                new MemoryStream(Properties.Resources.BackBuffer5));

            _shader.SetParameter("time", _time);
            _shader.SetParameter("resolution",new Vector2f(Size.X, Size.Y));
            _shader.SetParameter("size", _mainWindow.size);
            _shader.SetParameter("input_color", _mainWindow.color);
            _rState = new RenderStates(_shader);
            _rState.Texture = _texture;

            _backTexture = new RenderTexture(Size.X, Size.Y);
            sizeStart = Size.ConvertToVector2f();
        }

        private Vector2f sizeStart;

        public override void Update()
        {
            _shader.SetParameter("time", _time);
            _shader.SetParameter("texture",_backTexture.Texture);
            _shader.SetParameter("mouse",Mouse.GetPosition(window).ConvertToVector2f());
            _shader.SetParameter("size", _mainWindow.size);
            _shader.SetParameter("resolution", new Vector2f(Size.X, Size.Y));
            _shader.SetParameter("input_color", _mainWindow.color);
            _time += 0.005f;
        }

        public override void Render()
        {
             window.Draw(_rectangleShape, _rState);
             if (Mouse.IsButtonPressed(Mouse.Button.Left) & window.HasFocus())
                 _backTexture.Draw(_rectangleShape, _rState);
        }
    }
}
