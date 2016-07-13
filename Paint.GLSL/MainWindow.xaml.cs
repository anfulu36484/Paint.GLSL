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
            BrushesComboBox.Items.Add("Test");
            /*task = Task.Factory.StartNew(() =>
            {
                //Game game = new Experiment4_BackBuffer_SetMousePositon(RenderTo.Window, this);
                Game game = new Brush5(RenderTo.Window, this);
                game.Run();
            });*/
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

        private void CreateNewWindowButton_Click(object sender, RoutedEventArgs e)
        {
            SizeDialogWindow sizeDialogWindow = new SizeDialogWindow();
            sizeDialogWindow.Show();
            sizeDialogWindow.OkButton.Click += (send, eventsArg) =>
            {
                int width = 0;
                int height = 0;

                try{ width = Convert.ToInt32(sizeDialogWindow.WidthTextBox.Text) ; } catch { }
                try { height = Convert.ToInt32(sizeDialogWindow.HeightTextBox.Text); } catch { }


                Task.Factory.StartNew(() =>
                {
                    //Game game = new Experiment4_BackBuffer_SetMousePositon(RenderTo.Window, this);
                    Game game = new Brush5(RenderTo.Window, this);
                    game.Run();
                });
                sizeDialogWindow.Close();
            };
        }

    }
}
