using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Documents;
using Paint.GLSL.Brushes;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Paint.GLSL
{
    class Canvas :Game
    {
        /*[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        public static extern IntPtr GetNextWindow(IntPtr hWnd, uint wCmd);


        public bool IsTherewindowAboveGivenWindow()
        {
            IntPtr handle = GetNextWindow(window.SystemHandle, 3);
            return MainWindow.Handles.Contains(handle);
        }*/

        public readonly MainWindow MainWindow;

        public Texture Texture;
        private RectangleShape _rectangleShape;

        private BrushBase _brush;

        public Stack<RenderTexture> BackHistory;
        public Stack<RenderTexture> ForwardHistory; 

        public Canvas(uint width, uint height, MainWindow mainWindow) 
            : base(width, height, "Canvas", Color.White, RenderTo.Window)
        {
            MainWindow = mainWindow;
            MainWindow.BrushesComboBox.SelectionChanged += BrushesComboBox_SelectionChanged;
            MainWindow.UndoImage.MouseLeftButtonUp += UndoImage_MouseLeftButtonUp;
            MainWindow.RendoImage.MouseLeftButtonUp += RendoImage_MouseLeftButtonUp;

            window.MouseButtonReleased += Window_MouseButtonReleased;

            
        }

        private Shader _turnShader;


        public override void Initialize()
        {
            Texture = new Texture(Size.X, Size.Y);

            _rectangleShape = new RectangleShape(new Vector2f(Size.X, Size.Y));
            _rectangleShape.Texture = Texture;

            
            BackHistory = new Stack<RenderTexture>();
            BackHistory.Push(new RenderTexture(Size.X, Size.Y));
            ForwardHistory = new Stack<RenderTexture>();
            _brush = _brushes[0];

            _turnShader = new Shader(new MemoryStream(Properties.Resources.VertexShader),
                new MemoryStream(Properties.Resources.Turn));

            Window_MouseButtonReleased(null, new MouseButtonEventArgs(new MouseButtonEvent())
            {
                Button = Mouse.Button.Left,
            });

        }


        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                RenderTexture renderTexture = new RenderTexture(Size.X, Size.Y);

           
            
                Texture texture = BackHistory.Peek().Texture;

                RenderStates renderStates = new RenderStates(_turnShader);
                renderStates.Texture = texture;

                _rectangleShape.Texture = BackHistory.Peek().Texture;

                _turnShader.SetParameter("texture", texture);
                renderTexture.Draw(_rectangleShape,renderStates);

                BackHistory.Push(renderTexture);
                ForwardHistory.Clear();

    
            }
        }

        private void RendoImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if(/*!IsTherewindowAboveGivenWindow() & */ForwardHistory.Count>0)
                BackHistory.Push(ForwardHistory.Pop());
        }

        private void UndoImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (/*!IsTherewindowAboveGivenWindow() &*/ BackHistory.Count > 1)
                ForwardHistory.Push(BackHistory.Pop());
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


        public override void Update()
        {
            _brush.Update();
        }

        public override void Render()
        {
            window.Draw(_rectangleShape, _brush.RenderStates);
            if (Mouse.IsButtonPressed(Mouse.Button.Left) & window.HasFocus())
                BackHistory.Peek().Draw(_rectangleShape, _brush.RenderStates);
        }
    }
}
