using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace paint.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для NewLayerDialog.xaml
    /// </summary>
    public partial class NewLayerDialog : Window
    {

        public List<Dictionary<String, Object>> layers;
        public SpeechSynthesizer debugger;

        public NewLayerDialog(List<Dictionary<String, Object>> layers)
        {
            InitializeComponent();
            this.layers = layers;
            debugger = new SpeechSynthesizer();
        }

        private void CreateLayerHandler(object sender, RoutedEventArgs e)
        {
            foreach(Dictionary<String, Object> layer in layers)
            {
                layer["isActive"] = false;
            }
            Dictionary<String, Object> newLayer = new Dictionary<String, Object>();
            newLayer.Add("name", name.Text);
            newLayer.Add("isActive", true);
            newLayer.Add("isHidden", false);
            List<Int32> layerStrokes = new List<Int32>();
            newLayer.Add("strokes", layerStrokes);
            layers.Add(newLayer);
            this.Close();
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
