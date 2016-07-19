using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public RenderTexture RenderTexture;

        private RectangleShape _rectangleShape;

        private BrushBase _brush;

        public Stack<Texture> BackHistory;
        public Stack<Texture> ForwardHistory;

        public Texture Texture;

        public Canvas(uint width, uint height, MainWindow mainWindow) 
            : base(width, height, "Canvas", Color.White, RenderTo.Window)
        {
            MainWindow = mainWindow;
            MainWindow.BrushesComboBox.SelectionChanged += BrushesComboBox_SelectionChanged;
            MainWindow.UndoImage.MouseLeftButtonUp += UndoImage_MouseLeftButtonUp;
            MainWindow.RendoImage.MouseLeftButtonUp += RendoImage_MouseLeftButtonUp;

            window.MouseButtonReleased += Window_MouseButtonReleased;

            Texture = new Texture(Size.X, Size.Y);
        }

        private Shader _turnShader;


        public override void Initialize()
        {
            BackHistory = new Stack<Texture>();
            BackHistory.Push(Texture);

            ForwardHistory = new Stack<Texture>();
            
            RenderTexture = new RenderTexture(Size.X, Size.Y);

            _rectangleShape = new RectangleShape(new Vector2f(Size.X, Size.Y));
            _rectangleShape.Texture = Texture;

            _brush = _brushes[0];

            _turnShader = new Shader(new MemoryStream(Properties.Resources.VertexShader),
                new MemoryStream(Properties.Resources.Turn));



            Window_MouseButtonReleased(null, new MouseButtonEventArgs(new MouseButtonEvent())
            {
                Button = Mouse.Button.Left,
            });

        }

        private int index;
        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                
                Texture texture = new Texture(RenderTexture.Texture);
                
                BackHistory.Push(texture);
                ForwardHistory.Clear();

                for (int i = 0; i < ForwardHistory.Count; i++)
                {
                    Texture t = ForwardHistory.Pop();
                    t.Dispose();
                }
    
            }
        }

        private void RendoImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ( /*!IsTherewindowAboveGivenWindow() & */ForwardHistory.Count > 0)
            {
                BackHistory.Push(ForwardHistory.Pop());


                Texture texture = BackHistory.Peek();

                _turnShader.SetParameter("texture", texture);

                RenderStates renderStates = new RenderStates(_turnShader);
                renderStates.Texture = Texture;

                RectangleShape rectangle = new RectangleShape(new Vector2f(Size.X, Size.Y));
                rectangle.Texture = Texture;

                RenderTexture.Draw(_rectangleShape, renderStates);

                RenderTexture.Display();
            }
        }

        private void UndoImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ( /*!IsTherewindowAboveGivenWindow() &*/ BackHistory.Count > 1)
            {
                ForwardHistory.Push(BackHistory.Pop());
                //ForwardHistory.TrimExcess();

                Texture texture = BackHistory.Peek();

                texture.CopyToImage().SaveToFile(++index + ".bmp");

                _turnShader.SetParameter("texture", texture);

                RenderStates renderStates = new RenderStates(_turnShader);
                renderStates.Texture = Texture;

                RectangleShape rectangle = new RectangleShape(new Vector2f(Size.X, Size.Y));
                rectangle.Texture = Texture;

                RenderTexture.Draw(_rectangleShape, renderStates);

                RenderTexture.Display();
            }
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
            {
                RenderTexture.Draw(_rectangleShape, _brush.RenderStates);
                RenderTexture.Display();
            }
        }
    }
}
