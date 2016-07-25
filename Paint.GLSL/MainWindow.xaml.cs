using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Paint.GLSL.Brushes;
using SFML.Window;
using Window = System.Windows.Window;

namespace Paint.GLSL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //BrushesComboBox.Items.Add("");
        }

        public float size;
        public SFML.Graphics.Color color;

        private void OnSizeChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                size =(float) Convert.ToDouble(SizeTextBox.Text);
            }
            catch (Exception)
            {
                size = (float)Convert.ToDouble(e.UndoAction); 
            }
            
        }

        private void OnColorChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                color = new SFML.Graphics.Color(
                Convert.ToByte(ColorTextBoxRed.Text),
                Convert.ToByte(ColorTextBoxGreen.Text),
                Convert.ToByte(ColorTextBoxBlue.Text));
            }
            catch (Exception)
            {

            }
            
        }


        public static volatile List<IntPtr> Handles = new List<IntPtr>();



        private void  CreateNewWindowButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.Show();
            settingsWindow.OkButton.Click +=(s, o)=>
            {
                var screenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

                uint width = 0;
                uint height = 0;
                uint FPSLimit = 0;
                int sizeOfHistory = 50;

                try { width = Convert.ToUInt32(settingsWindow.WidthTextBox.Text); } catch { }
                try { height = Convert.ToUInt32(settingsWindow.HeightTextBox.Text); } catch { }
                try { FPSLimit = Convert.ToUInt32(settingsWindow.FPSLimitTextBox.Text); } catch { }
                try { sizeOfHistory = (int)Convert.ToUInt32(settingsWindow.SizeOfTheHistoryTextBox.Text); } catch { }

                if (width == 0) width = (uint)screenSize.Size.Width - 50;
                if (height == 0) height = (uint)screenSize.Size.Height - 100;

                Task.Factory.StartNew(() =>
                {
                    Canvas canvas = new Canvas(width, height, this, FPSLimit, sizeOfHistory);

          

                    Dictionary<string, BrushBase> brushesCollection =new Dictionary<string, BrushBase>
                    { 
                        ["Brush1"]= new Brush(canvas, "Brush1", Properties.Resources.BackBuffer1),
                        ["Brush2"] = new Brush(canvas, "Brush2", Properties.Resources.BackBuffer2),
                        ["Brush3"] = new Brush(canvas, "Brush3", Properties.Resources.BackBuffer3),
                        ["Brush4"] = new Brush(canvas, "Brush4", Properties.Resources.BackBuffer4),
                        ["Brush5"] = new Brush(canvas, "Brush5", Properties.Resources.BackBuffer5),
                        ["Brush6"] = new Brush(canvas, "Brush6", Properties.Resources.BackBuffer6)
                    };


                    Dispatcher.Invoke(() =>
                    {

                        if (BrushesComboBox.Items.Count == 0)
                        {
                            foreach (var brush in brushesCollection)
                            {
                                BrushesComboBox.Items.Add(brush.Key);
                            }
                            BrushesComboBox.SelectedIndex = 0;
                        }
                    });
                    canvas.AddBrushes(brushesCollection);
                    Handles.Add(canvas.Window.SystemHandle);

                    canvas.Window.MouseButtonPressed += Window_MouseButtonPressed;
                    canvas.Window.KeyPressed += Window_KeyPressed;

                    canvas.Run();
                });
                settingsWindow.Close();
            };
        }

        void ChangeVisibility()
        {
            if (Visibility == Visibility.Visible)
            {
                Visibility = Visibility.Hidden;
                return;
            }
            if (Visibility == Visibility.Hidden)
                Visibility = Visibility.Visible;
        }


        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (e.Code == Keyboard.Key.Escape)
                    ChangeVisibility();
            });
        }

        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if (e.Button == Mouse.Button.Right)
                    ChangeVisibility();
            });
        }


        public event EventHandler DrawingDataIsLoaded;

        private void OpenFile_ButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            bool? result = openFileDialog.ShowDialog();

            if (result.HasValue)
            {
                DataReader dataReader = new DataReader();
                List<DrawingData> list = new List<DrawingData>();

                try{ list=dataReader.ReadFile(openFileDialog.FileName); }
                catch (Exception ex) {  MessageBox.Show(ex.Message); }
                
                if(list.Count>0)
                    DrawingDataIsLoaded?.Invoke(this,new EventsArgDrawingData(list));
            }
                


        }
    }

    class EventsArgDrawingData : EventArgs
    {
        public List<DrawingData> DrawingDataList;

        public EventsArgDrawingData(List<DrawingData> drawingDataList)
        {
            DrawingDataList = drawingDataList;
        }
    }
}
