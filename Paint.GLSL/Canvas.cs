using System.Collections.Generic;
using System.Windows.Documents;
using Paint.GLSL.Brushes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Paint.GLSL
{
    class Canvas :Game
    {
        public readonly MainWindow MainWindow;

        public Texture Texture;
        private RectangleShape _rectangleShape;

        public RenderTexture BackTexture;

        private BrushBase _brush;

        public Canvas(uint width, uint height, MainWindow mainWindow) 
            : base(width, height, "Canvas", Color.White, RenderTo.Window)
        {
            MainWindow = mainWindow;

            MainWindow.BrushesComboBox.SelectionChanged += BrushesComboBox_SelectionChanged;
        }

        private void BrushesComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(_brushes!=null)
                _brush = _brushes[MainWindow.BrushesComboBox.SelectedIndex ];
        }

        public override void Load() { }

        private List<BrushBase> _brushes;

        public void AddBrushes(List<BrushBase> brushes)
        {
            _brushes = brushes;
        }


        public override void Initialize()
        {
            Texture = new Texture(Size.X, Size.Y);

            _rectangleShape = new RectangleShape(new Vector2f(Size.X, Size.Y));
            _rectangleShape.Texture = Texture;

            BackTexture = new RenderTexture(Size.X, Size.Y);
            _brush = _brushes[0];
        }

        public override void Update()
        {
            _brush.Update();
            
        }

        public override void Render()
        {
            window.Draw(_rectangleShape, _brush.RenderStates);
            if (Mouse.IsButtonPressed(Mouse.Button.Left) & window.HasFocus())
                BackTexture.Draw(_rectangleShape, _brush.RenderStates);
        }
    }
}
