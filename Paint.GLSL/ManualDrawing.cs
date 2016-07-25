using System.IO;
using System.Linq;
using Paint.GLSL.Brushes;
using SFML.Graphics;
using SFML.Window;

namespace Paint.GLSL
{
    class ManualDrawing:IDrawing
    {
        private readonly Canvas _canvas;
        private readonly int _sizeOfHistory;
        private readonly RenderTexture _startRenderTexture;


        public HistoryCollection<RenderTexture> BackHistory;
        public HistoryCollection<RenderTexture> ForwardHistory;

        private Shader _turnShader;

        public ManualDrawing(Canvas canvas, int sizeOfHistory, RenderTexture startRenderTexture)
        {
            _canvas = canvas;
            _sizeOfHistory = sizeOfHistory;
            _startRenderTexture = startRenderTexture;

            _canvas.MainWindow.BrushesComboBox.SelectionChanged += BrushesComboBox_SelectionChanged;
            _canvas.MainWindow.UndoImage.MouseLeftButtonUp += UndoImage_MouseLeftButtonUp;
            _canvas.MainWindow.RendoImage.MouseLeftButtonUp += RendoImage_MouseLeftButtonUp;

            _canvas.Window.MouseButtonReleased += Window_MouseButtonReleased;
        }

        private BrushBase _brush;

        public void Initialize()
        {
            BackHistory = new HistoryCollection<RenderTexture>(_sizeOfHistory);
            BackHistory.Push(_startRenderTexture);
            ForwardHistory = new HistoryCollection<RenderTexture>(_sizeOfHistory);

            _turnShader = new Shader(new MemoryStream(Properties.Resources.VertexShader),
                new MemoryStream(Properties.Resources.Turn));

            Window_MouseButtonReleased(null, new MouseButtonEventArgs(new MouseButtonEvent())
            {
                Button = Mouse.Button.Left,
            });

            _brush = _canvas.Brushes.Values.First();
        }

        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                RenderTexture renderTexture = new RenderTexture(_canvas.Size.X, _canvas.Size.Y);

                Texture texture = BackHistory.Peek().Texture;

                RenderStates renderStates = new RenderStates(_turnShader);
                renderStates.Texture = texture;

                _canvas.RectangleShape.Texture = BackHistory.Peek().Texture;

                _turnShader.SetParameter("texture", texture);
                renderTexture.Draw(_canvas.RectangleShape, renderStates);
                BackHistory.Push(renderTexture);
                ForwardHistory.Clear();
            }
        }

        public Texture GetBackTexture()
        {
            return BackHistory.Peek().Texture;
        }

        public RenderTexture GetBackRenderTexture()
        {
            return BackHistory.Pop();
        }

        public void SetBackRenderTexture(RenderTexture renderTexture)
        {
            BackHistory.Push(renderTexture);
        }

        private void RendoImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (/*!IsTherewindowAboveGivenWindow() & */ForwardHistory.Count > 0)
                BackHistory.Push(ForwardHistory.Pop());
        }

        private void UndoImage_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (/*!IsTherewindowAboveGivenWindow() &*/ BackHistory.Count > 1)
                ForwardHistory.Push(BackHistory.Pop());
        }

        private void BrushesComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (_canvas.Brushes != null)
                _brush = _canvas.Brushes[_canvas.MainWindow.BrushesComboBox.Text];
        }

        public void Update()
        {
            _brush.Update(
                _canvas.MainWindow.size,
                Mouse.GetPosition(_canvas.Window).ConvertToVector2f(),
                _canvas.MainWindow.color);
        }

        public void Render()
        {
            _canvas.Window.Draw(_canvas.RectangleShape, _brush.RenderStates);
            if (Mouse.IsButtonPressed(Mouse.Button.Left) & _canvas.Window.HasFocus())
                BackHistory.Peek().Draw(_canvas.RectangleShape, _brush.RenderStates);
        }
    }
}
