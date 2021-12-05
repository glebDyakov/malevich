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
using System.Windows.Shapes;

namespace paint.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для LayersDialog.xaml
    /// </summary>
    public partial class LayersDialog : Window
    {

        List<Int32> layers;

        public LayersDialog(List<Int32> layers)
        {
            InitializeComponent();
            this.layers = layers;
            foreach(StackPanel layer in allLayers.Children)
            {
                allLayers.Children.Remove(layer);
            }
            foreach (Int32 layer in layers)
            {
                StackPanel layerContainer = new StackPanel();
                Image layerVisibility = new Image();
                BitmapImage eyeSprite = new BitmapImage();
                eyeSprite.BeginInit();
                eyeSprite.UriSource = new Uri("file:///C:/wpfprojects/paint/Assets/toggle_layer_visibility_btn.png", UriKind.Absolute);
                eyeSprite.EndInit();
                layerVisibility.Source = eyeSprite;
                layerVisibility.Width = 150;
                layerVisibility.Height = 150;
                TextBlock layerName = new TextBlock();
                layerName.Text = "Фон";
                layerContainer.Children.Add(layerVisibility);
                layerContainer.Children.Add(layerName);
                allLayers.Children.Add(layerContainer);
            }
        }
    }
}
