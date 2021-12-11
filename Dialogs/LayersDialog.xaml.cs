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

        List<Dictionary<String, Object>> layers;
        public Canvas canvas;
        public List<Int32> totalStrokesIds;

        public LayersDialog(List<Dictionary<String, Object>> layers, Canvas canvas, List<Int32> totalStrokesIds)
        {
            InitializeComponent();
            this.layers = layers;
            this.canvas = canvas;
            this.totalStrokesIds = totalStrokesIds;
            /*foreach(StackPanel layer in allLayers.Children)
            {
                allLayers.Children.Remove(layer);
            }*/
            foreach (Dictionary<String, Object> layer in layers)
            {
                StackPanel layerContainer = new StackPanel();
                layerContainer.Margin = new Thickness(5, 15, 5, 15);
                Image layerVisibility = new Image();
                layerVisibility.Height = 25;
                BitmapImage eyeSprite = new BitmapImage();
                eyeSprite.BeginInit();
                eyeSprite.UriSource = new Uri("file:///C:/wpfprojects/paint/Assets/toggle_layer_visibility_btn.png", UriKind.Absolute);
                eyeSprite.EndInit();
                layerVisibility.Source = eyeSprite;
                TextBlock layerName = new TextBlock();
                layerName.Margin = new Thickness(15, 5, 15, 0);
                //layerContainer.Background = System.Windows.Media.Brushes.WhiteSmoke;
                layerName.Text = layer["name"].ToString();
                layerContainer.Orientation = Orientation.Horizontal;
                layerContainer.Children.Add(layerVisibility);
                layerContainer.Children.Add(layerName);
                layerContainer.Cursor = Cursors.Hand;
                if (((bool)(layer["isHidden"])))
                {
                    layerVisibility.Opacity = 0.2;
                    layerVisibility.ToolTip = "Скрыт";
                }
                else if (!((bool)(layer["isHidden"])))
                {
                    layerVisibility.ToolTip = "Показан";
                }
                layerContainer.MouseLeftButtonUp += SelectLayerHandler;
                layerContainer.MouseRightButtonUp += ToggleVisibilityLayerHandler;
                allLayers.Children.Add(layerContainer);
            }
            foreach (Dictionary<String, Object> layer in layers)
            {
                if (((bool)layer["isActive"]))
                {
                    ((StackPanel)(allLayers.Children[layers.IndexOf(layer)])).Background = System.Windows.Media.Brushes.WhiteSmoke;
                }
            }
        }
        private void SelectLayerHandler(object sender, MouseButtonEventArgs e)
        {
            foreach (StackPanel layer in allLayers.Children)
            {
                layer.Background = System.Windows.Media.Brushes.Transparent;
            }
            foreach (Dictionary<String, Object> layer in layers)
            {
                layer["isActive"] = false;
            }
            StackPanel currentLayer = (StackPanel)sender;
            int currentLayerIdx = allLayers.Children.IndexOf(currentLayer);
            layers[currentLayerIdx]["isActive"] = true;
            currentLayer.Background = System.Windows.Media.Brushes.WhiteSmoke;
        }
        private void ToggleVisibilityLayerHandler(object sender, MouseButtonEventArgs e)
        {
            StackPanel currentLayer = (StackPanel)sender;
            int currentLayerIdx = allLayers.Children.IndexOf(currentLayer);
            layers[currentLayerIdx]["isHidden"] = !((bool)(layers[currentLayerIdx]["isHidden"]));
            if (((bool)(layers[currentLayerIdx]["isHidden"]))) { 
                ((Image)(currentLayer.Children[0])).ToolTip = "Скрыт";
                ((Image)(currentLayer.Children[0])).Opacity = 0.2;
                /*foreach (UIElement stroke in canvas.Children)
                {
                    if (canvas.Children.IndexOf(stroke) == currentLayerIdx) {
                        canvas.Children.Remove(stroke);
                    }
                }*/
                foreach (Int32 totalStrokesId in totalStrokesIds)
                {
                    if (((List<Int32>)(layers[currentLayerIdx]["strokes"])).Contains(totalStrokesId))
                    {
                        ((UIElement)(canvas.Children[totalStrokesId])).Opacity = 0.0;
                    }
                }
                /*canvas.Children[0].Opacity = 0.0;*/
            } else if (!((bool)(layers[currentLayerIdx]["isHidden"]))) {
                ((Image)(currentLayer.Children[0])).ToolTip = "Показан";
                ((Image)(currentLayer.Children[0])).Opacity = 1.0;
                /*canvas.Children[0].Opacity = 1.0;*/
                foreach (Int32 totalStrokesId in totalStrokesIds)
                {
                    if (((List<Int32>)(layers[currentLayerIdx]["strokes"])).Contains(totalStrokesId))
                    {
                        ((UIElement)(canvas.Children[totalStrokesId])).Opacity = 1.0;
                    }
                }
            }
        }
    }
}
