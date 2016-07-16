using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Paint.GLSL.Brushes;
using SFML.Graphics;

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

        private Task task;

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




        private void  CreateNewWindowButton_Click(object sender, RoutedEventArgs e)
        {
            SizeDialogWindow sizeDialogWindow = new SizeDialogWindow();
            sizeDialogWindow.Show();
            sizeDialogWindow.OkButton.Click += (send, eventsArg) =>
            {
                var screenSize = System.Windows.Forms.Screen.PrimaryScreen.Bounds;

                uint width = 0;
                uint height = 0;

                try { width = Convert.ToUInt32(sizeDialogWindow.WidthTextBox.Text) ; } catch { }
                try { height = Convert.ToUInt32(sizeDialogWindow.HeightTextBox.Text); } catch { }

                if(width==0)  width = (uint)screenSize.Size.Width-50;
                if(height==0) height = (uint)screenSize.Size.Height-100;

                Task.Factory.StartNew(() =>
                {
                    Canvas canvas = new Canvas(width,height, this);

                    List<BrushBase> brushesCollection = new List<BrushBase>
                    {
                        new Brushes.Brush(canvas,"Brush1",Properties.Resources.BackBuffer1),
                        new Brushes.Brush(canvas,"Brush2",Properties.Resources.BackBuffer2),
                        new Brushes.Brush(canvas,"Brush3",Properties.Resources.BackBuffer3),
                        new Brushes.Brush(canvas,"Brush4",Properties.Resources.BackBuffer4),
                        new Brushes.Brush(canvas,"Brush5",Properties.Resources.BackBuffer5),
                        new Brushes.Brush(canvas,"Brush5",Properties.Resources.BackBuffer6)
                    };

                    Dispatcher.Invoke(() =>
                    {

                        if (BrushesComboBox.Items.Count == 0)
                        {
                            foreach (var brush in brushesCollection)
                            {
                                BrushesComboBox.Items.Add(brush.Name);
                            }
                            BrushesComboBox.SelectedIndex = 0;
                        }
                    });
                    canvas.AddBrushes(brushesCollection);
                    canvas.Run();
                });
                sizeDialogWindow.Close();
            };
        }

    }
}
