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
using SFML_shaders_experiments.Experiment4_BackBuffer;

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
            task = Task.Factory.StartNew(() =>
            {
                Game game = new Experiment4_BackBuffer_SetMousePositon(RenderTo.Window, this);
                game.Run();
            });
        }

        public int size;

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {

            size = Convert.ToInt32(SizeTextBox.Text);
        }
    }
}
