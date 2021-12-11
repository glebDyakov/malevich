using Microsoft.Win32;
using Syncfusion.Windows.Tools.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace paint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool isDrawing = false;
        public double initialWidth = 0;
        public double initialHeight = 0;
        public Polyline currentStroke;
        public PointCollection pointCollection;
        public TextBlock activeTool;
        public Rectangle selection;
        public Point startSelectionPoint;
        public Brush foreGroundColor;
        public Brush backGroundColor;
        public Point handPosition;
        public System.Windows.Shapes.Path penCurve;
        public PathSegmentCollection pathSegmentCollection;
        public BezierSegment bezierSegment;
        public  Point previousPoint;
        public List<Dictionary<String, Object>> layers;
        public bool isHand = false;
        public TextBlock lastTool;
        public SpeechSynthesizer debugger;
        public int brushSizePts = 10;
        public double brushOpacityPrsnts = 100;
        public bool isHandleHotKeys = true;
        public string textFont = "Verdana";
        public TextBox rasterText;
        public string textWeight = "Normal";
        public int textSizePnts = 14;
        public Brush textColor;
        public string translateSelect = "Слой";
        public string selectionStyle = "Normal";
        public string cropArea = "Произвольно";
        public bool removeCropPixels = true;
        public bool cropWithContent = false;
        public string pippeteTemplate = "Все слои";
        public bool pippetShowRingExample = true;
        public string recoveryMode = "Normal";
        public bool zoomScaleDrag = true;
        public bool zoomAllWindows = false;
        public bool zoomSettedScreenScale = false;
        public bool handScrollAllWindows = false;
        public string cursorSelect = "Активные слои";
        public string lightRange = "Средние тона";
        public double lightExpon = 10;
        public List<Int32> totalStrokesIds;

        public MainWindow()
        {
            InitializeComponent();

            initialWidth = zoom.Width;
            initialHeight = zoom.Height;
            activeTool = startActiveTool;
            foreGroundColor = System.Windows.Media.Brushes.Black;
            backGroundColor = System.Windows.Media.Brushes.White;
            Dictionary<String, Object>  defaultLayer = new Dictionary<String, Object>();
            defaultLayer.Add("name", "Фон");
            defaultLayer.Add("isActive", true);
            defaultLayer.Add("isHidden", false);
            List<Int32> layerStrokes = new List<Int32>();
            defaultLayer.Add("strokes", layerStrokes);
            layers = new List<Dictionary<String, Object>>() {
                defaultLayer
            };
            debugger = new SpeechSynthesizer();
            totalStrokesIds = new List<Int32>();
        }

        private void BrushMoveHandler(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                if (activeTool.ToolTip.ToString() == "Кисть")
                {
                    pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                    currentStroke.Points = pointCollection;
                } else if (activeTool.ToolTip.ToString() == "Ластик")
                {
                    pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                    currentStroke.Points = pointCollection;
                } else if (activeTool.ToolTip.ToString() == "Выделение")
                {
                    /*selection.Width = Mouse.GetPosition(canvas).X - startSelectionPoint.X;
                    selection.Height = Mouse.GetPosition(canvas).Y - startSelectionPoint.Y;*/
                    selection.Width = Mouse.GetPosition(canvas).X;
                    selection.Height = Mouse.GetPosition(canvas).Y;
                }
                else if (activeTool.ToolTip.ToString() == "Кадрировать")
                {
                    selection.Width = Mouse.GetPosition(canvas).X;
                    selection.Height = Mouse.GetPosition(canvas).Y;
                } else if (activeTool.ToolTip.ToString() == "Лассо")
                {
                    pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                    currentStroke.Points = pointCollection;
                } else if (activeTool.ToolTip.ToString() == "Палец")
                {
                    pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                    currentStroke.Points = pointCollection;
                } else if (activeTool.ToolTip.ToString() == "Рука")
                {
                    translate.X = e.GetPosition(workSpace).X - handPosition.X;
                    translate.Y = e.GetPosition(workSpace).Y - handPosition.Y;
                } else if (activeTool.ToolTip.ToString() == "Перо")
                {
                    /*pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                    currentStroke.Points = pointCollection;*/
                    if (penCurve != null)
                    {
                        bezierSegment.Point1 = previousPoint;
                        bezierSegment.Point2 = Mouse.GetPosition(canvas);
                    }
                }

            }
        }

        private void BrushUpHandler(object sender, MouseButtonEventArgs e)
        {
            if (isDrawing) {
                isDrawing = false;
            }
            if (activeTool.ToolTip.ToString() == "Заливка")
            {
                canvas.Background = foreGroundColor;
                canvas.Opacity = brushOpacityPrsnts;
            } else if (activeTool.ToolTip.ToString() == "Выделение")
            {
                canvas.Children.Remove(selection);
                startSelectionPoint = new Point(0, 0);
            }
            else if (activeTool.ToolTip.ToString() == "Кадрировать")
            {
                canvas.Width = selection.Width;
                canvas.Height = selection.Height;
                canvas.Children.Remove(selection);
            }
            else if (activeTool.ToolTip.ToString() == "Лупа")
            {
                if ((Keyboard.Modifiers & ModifierKeys.Alt) > 0)
                {
                    zoom.Width = zoom.Width - 10 * 1;
                    zoom.Height = zoom.Height - 10 * 1;
                } else {
                    zoom.Width = zoom.Width + 10 * 1;
                    zoom.Height = zoom.Height + 10 * 1;
                }
            } else if (activeTool.ToolTip.ToString() == "Перо")
            {
                if (penCurve == null)
                {
                    penCurve = new System.Windows.Shapes.Path();
                    penCurve.Stroke = foreGroundColor;
                    /*penCurve.StrokeThickness = 2;*/
                    penCurve.StrokeThickness = brushSizePts;
                    PathGeometry pathGeometry = new PathGeometry();
                    PathFigureCollection pathFigureCollection = new PathFigureCollection();
                    PathFigure pathFigure = new PathFigure();
                    /*PathSegmentCollection pathSegmentCollection = new PathSegmentCollection();*/
                    pathSegmentCollection = new PathSegmentCollection();
                    /*BezierSegment bezierSegment = new BezierSegment();
                    bezierSegment.Point1 = new Point(100, 0);
                    bezierSegment.Point2 = new Point(200, 200);
                    bezierSegment.Point3 = new Point(300, 100);
                    PathSegment pathSegment = bezierSegment;
                    pathSegmentCollection.Add(pathSegment);*/
                    pathFigure.Segments = pathSegmentCollection;
                    pathFigure.StartPoint = Mouse.GetPosition(canvas);
                    pathFigureCollection.Add(pathFigure);
                    pathGeometry.Figures = pathFigureCollection;
                    penCurve.Data = pathGeometry;
                    canvas.Children.Add(penCurve);
                } else
                {
                    previousPoint = Mouse.GetPosition(canvas);
                }
            }

        }

        private void BrushDownHandler(object sender, MouseButtonEventArgs e)
        {
            handPosition = e.GetPosition(workSpace);

            isDrawing = true;
            if (activeTool.ToolTip.ToString() == "Кисть")
            {
                currentStroke = new Polyline();
                /*currentStroke.Stroke = System.Windows.Media.Brushes.LightSteelBlue;*/
                currentStroke.Stroke = foreGroundColor;
                pointCollection = new PointCollection();
                pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                currentStroke.StrokeThickness = brushSizePts;
                currentStroke.Opacity = brushOpacityPrsnts;
                currentStroke.Points = pointCollection;
                canvas.Children.Add(currentStroke);

                int newStrokeId = totalStrokesIds.Count;
                totalStrokesIds.Add(newStrokeId);
                foreach (Dictionary<String, Object> layer in layers) {
                    if (((bool)(layer["isActive"])))
                    {
                        List<Int32> layerStrokes = ((List<Int32>)(layer["strokes"]));
                        layerStrokes.Add(newStrokeId);
                        layer["strokes"] = layerStrokes;
                    }
                }
            } else if (activeTool.ToolTip.ToString() == "Ластик")
            {
                currentStroke = new Polyline();
                currentStroke.Stroke = System.Windows.Media.Brushes.White;
                pointCollection = new PointCollection();
                pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                /*currentStroke.StrokeThickness = 8;*/
                currentStroke.StrokeThickness = brushSizePts;
                currentStroke.Points = pointCollection;
                canvas.Children.Add(currentStroke);
            } else if (activeTool.ToolTip.ToString() == "Выделение")
            {
                selection = new Rectangle();
                selection.Stroke = System.Windows.Media.Brushes.Black;
                selection.StrokeDashArray = new DoubleCollection(new List<Double>() { 0.7 });
                selection.RenderTransformOrigin = new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y);
                canvas.Children.Add(selection);
                Canvas.SetTop(selection, Mouse.GetPosition(canvas).Y);
                Canvas.SetLeft(selection, Mouse.GetPosition(canvas).X);
                startSelectionPoint = new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y);
            }
            else if (activeTool.ToolTip.ToString() == "Кадрировать")
            {
                selection = new Rectangle();
                selection.Stroke = System.Windows.Media.Brushes.Black;
                selection.StrokeDashArray = new DoubleCollection(new List<Double>() { 0.7 });
                selection.RenderTransformOrigin = new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y);
                canvas.Children.Add(selection);
                Canvas.SetTop(selection, Mouse.GetPosition(canvas).Y);
                Canvas.SetLeft(selection, Mouse.GetPosition(canvas).X);
                startSelectionPoint = new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y);
            } else if (activeTool.ToolTip.ToString() == "Лассо")
            {
                currentStroke = new Polyline();
                currentStroke.StrokeDashArray = new DoubleCollection(new List<Double>() { 0.7 });
                currentStroke.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                pointCollection = new PointCollection();
                pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                currentStroke.StrokeThickness = 8;
                currentStroke.Points = pointCollection;
                canvas.Children.Add(currentStroke);
            } else if (activeTool.ToolTip.ToString() == "Палец")
            {
                currentStroke = new Polyline();
                BlurBitmapEffect fingerEffect = new BlurBitmapEffect();
                fingerEffect.Radius = 10;
                currentStroke.BitmapEffect = fingerEffect;
                currentStroke.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                pointCollection = new PointCollection();
                pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                currentStroke.StrokeThickness = 8;
                currentStroke.Points = pointCollection;
                canvas.Children.Add(currentStroke);
            } else if (activeTool.ToolTip.ToString() == "Освещение")
            {
                EmbossBitmapEffect lightEffect = new EmbossBitmapEffect();
                lightEffect.LightAngle = 180;
                canvas.BitmapEffect = lightEffect;
            } else if (activeTool.ToolTip.ToString() == "Текст")
            {
                rasterText = new TextBox();
                rasterText.BorderThickness = new Thickness(0);
                rasterText.AcceptsReturn = true;
                rasterText.Text = "Lorem ipsum";
                canvas.Children.Add(rasterText);
                Canvas.SetTop(rasterText, Mouse.GetPosition(canvas).Y);
                Canvas.SetLeft(rasterText, Mouse.GetPosition(canvas).X);
                rasterText.Focus();
                isHandleHotKeys = false;
                /*
                 * Параметры текста
                 */
                /*
                 * Принятие текста
                 */
                TextBlock applyTextBtn = new TextBlock();
                applyTextBtn.Text = "✓";
                applyTextBtn.Cursor = Cursors.Arrow;
                applyTextBtn.FontSize = 20;
                applyTextBtn.Foreground = System.Windows.Media.Brushes.White;
                applyTextBtn.Width = 75;
                applyTextBtn.Margin = new Thickness(25, 5, 0, 5);
                applyTextBtn.MouseUp += SetApplyText;
                toolParams.Children.Add(applyTextBtn);
                /*
                 * конец принятия текста
                 */
                /*
                * Принятие текста
                */
                TextBlock deniesTextBtn = new TextBlock();
                deniesTextBtn.Text = "⊗";
                deniesTextBtn.Cursor = Cursors.Arrow;
                deniesTextBtn.FontSize = 20;
                deniesTextBtn.Foreground = System.Windows.Media.Brushes.White;
                deniesTextBtn.Width = 75;
                deniesTextBtn.Margin = new Thickness(25, 5, 0, 5);
                deniesTextBtn.MouseUp += SetDeniesText;
                toolParams.Children.Add(deniesTextBtn);
                /*
                 * конец принятия текста
                 */ 
                /*
                 *  конец параметров текста
                 */
            }
            else if (activeTool.ToolTip.ToString() == "Перо")
            {
                /*currentStroke = new Polyline();
                currentStroke.Stroke = foreGroundColor;
                pointCollection = new PointCollection();
                pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                currentStroke.StrokeThickness = 2;
                currentStroke.Points = pointCollection;
                canvas.Children.Add(currentStroke);*/
                if (penCurve != null)
                {
                    bezierSegment = new BezierSegment();
                    /*bezierSegment.Point1 = Mouse.GetPosition(canvas);*/
                    bezierSegment.Point1 = previousPoint;
                    bezierSegment.Point2 = Mouse.GetPosition(canvas);
                    bezierSegment.Point3 = Mouse.GetPosition(canvas);
                    PathSegment pathSegment = bezierSegment;
                    pathSegmentCollection.Add(pathSegment);
                }
            }
        }

        private void SetZoomHandler(object sender, MouseWheelEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Alt) > 0) {
                if (e.Delta > 0)
                {
                    zoom.Width = zoom.Width + 10 * (e.Delta / 120);
                    zoom.Height = zoom.Height + 10 * (e.Delta / 120);
                    /*canvas.Width = canvas.Width + 10 * (e.Delta / 120);
                    canvas.Height = canvas.Height + 10 * (e.Delta / 120);*/
                } else if (e.Delta < 0)
                {
                    zoom.Width = zoom.Width - 10 * (e.Delta / 120 * -1);
                    zoom.Height = zoom.Height - 10 * (e.Delta / 120 * -1);
                    /*canvas.Width = canvas.Width - 10 * (e.Delta / 120 * -1);
                    canvas.Height = canvas.Height - 10 * (e.Delta / 120 * -1);*/
                }
            }
        }

        private void HoverToolHandler(object sender, MouseEventArgs e)
        {
            TextBlock currentTool = (TextBlock)sender;
            if (activeTool.ToolTip.ToString() != currentTool.ToolTip.ToString())
            {
                currentTool.Background = System.Windows.Media.Brushes.Gray;
            }
        }

        private void HoutToolHandler(object sender, MouseEventArgs e)
        {
            TextBlock currentTool = (TextBlock)sender;
            if (activeTool.ToolTip.ToString() != currentTool.ToolTip.ToString())
            {
                currentTool.Background = System.Windows.Media.Brushes.Transparent;
            } else
            {
                currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
            }
        }

        private void SetActiveTool(object sender, MouseButtonEventArgs e)
        {
            foreach(UIElement tool in tools.Children)
            {
                if (tool is TextBlock) {
                    ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                }
            }
            TextBlock currentTool = (TextBlock)sender;
            currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
            
            toolParams.Children.RemoveRange(0, toolParams.Children.Count);
            
            if (currentTool.ToolTip.ToString() == "Кисть")
            {
                canvas.Cursor = Cursors.Pen;
                /*
                 * Параметры кисти
                 */
                /*
                 * размер кисти
                 */
                ComboBox brushSize = new ComboBox();
                ComboBoxItem brushSize10Pt = new ComboBoxItem();
                brushSize10Pt.Content = "10";
                brushSize.Items.Add(brushSize10Pt);
                ComboBoxItem brushSize25Pt = new ComboBoxItem();
                brushSize25Pt.Content = "25";
                brushSize.Items.Add(brushSize25Pt);
                ComboBoxItem brushSize100Pt = new ComboBoxItem();
                brushSize100Pt.Content = "100";
                brushSize.Items.Add(brushSize100Pt);
                brushSize.SelectedIndex = 0;
                brushSize.Width = 75;
                brushSize.Margin = new Thickness(25, 15, 0, 15);
                brushSize.SelectionChanged += SetBrushSize;
                toolParams.Children.Add(brushSize);
                /*
                 * конец размера кисти
                 */
                /*
                 * прозрачность кисти
                 */
                ComboBox brushOpacity = new ComboBox();
                ComboBoxItem brushOpacity0Prsnts = new ComboBoxItem();
                brushOpacity0Prsnts.Content = "0";
                brushOpacity.Items.Add(brushOpacity0Prsnts);
                ComboBoxItem brushOpacity10Prsnts = new ComboBoxItem();
                brushOpacity10Prsnts.Content = "10";
                brushOpacity.Items.Add(brushOpacity10Prsnts);
                ComboBoxItem brushOpacity20Prsnts = new ComboBoxItem();
                brushOpacity20Prsnts.Content = "20";
                brushOpacity.Items.Add(brushOpacity20Prsnts);
                ComboBoxItem brushOpacity30Prsnts = new ComboBoxItem();
                brushOpacity30Prsnts.Content = "30";
                brushOpacity.Items.Add(brushOpacity30Prsnts);
                ComboBoxItem brushOpacity40Prsnts = new ComboBoxItem();
                brushOpacity40Prsnts.Content = "40";
                brushOpacity.Items.Add(brushOpacity40Prsnts);
                ComboBoxItem brushOpacity50Prsnts = new ComboBoxItem();
                brushOpacity50Prsnts.Content = "50";
                brushOpacity.Items.Add(brushOpacity50Prsnts);
                ComboBoxItem brushOpacity60Prsnts = new ComboBoxItem();
                brushOpacity60Prsnts.Content = "60";
                brushOpacity.Items.Add(brushOpacity60Prsnts);
                ComboBoxItem brushOpacity70Prsnts = new ComboBoxItem();
                brushOpacity70Prsnts.Content = "70";
                brushOpacity.Items.Add(brushOpacity70Prsnts);
                ComboBoxItem brushOpacity80Prsnts = new ComboBoxItem();
                brushOpacity80Prsnts.Content = "80";
                brushOpacity.Items.Add(brushOpacity80Prsnts);
                ComboBoxItem brushOpacity90Prsnts = new ComboBoxItem();
                brushOpacity90Prsnts.Content = "90";
                brushOpacity.Items.Add(brushOpacity90Prsnts);
                ComboBoxItem brushOpacity100Prsnts = new ComboBoxItem();
                brushOpacity100Prsnts.Content = "100";
                brushOpacity.Items.Add(brushOpacity100Prsnts);
                brushOpacity.SelectedIndex = 10;
                brushOpacity.Width = 75;
                brushOpacity.Margin = new Thickness(25, 15, 0, 15);
                brushOpacity.SelectionChanged += SetBrushOpacity;
                toolParams.Children.Add(brushOpacity);
                /*
                 * конец прозрачности кисти
                 */
                /*
                 * Конец параметров кисти
                 */
            }
            else if (currentTool.ToolTip.ToString() == "Ластик")
            {
                canvas.Cursor = Cursors.UpArrow;
                /*
                 * Параметры ластика
                 */
                /*
                 * размер ластика
                 */
                ComboBox brushSize = new ComboBox();
                ComboBoxItem brushSize10Pt = new ComboBoxItem();
                brushSize10Pt.Content = "10";
                brushSize.Items.Add(brushSize10Pt);
                ComboBoxItem brushSize25Pt = new ComboBoxItem();
                brushSize25Pt.Content = "25";
                brushSize.Items.Add(brushSize25Pt);
                ComboBoxItem brushSize100Pt = new ComboBoxItem();
                brushSize100Pt.Content = "100";
                brushSize.Items.Add(brushSize100Pt);
                brushSize.SelectedIndex = 0;
                brushSize.Width = 75;
                brushSize.Margin = new Thickness(25, 15, 0, 15);
                brushSize.SelectionChanged += SetBrushSize;
                toolParams.Children.Add(brushSize);
                /*
                 * конец размера ластика
                 */
                /*
                 * Конец параметров ластика
                 */
            }
            else if (currentTool.ToolTip.ToString() == "Рука")
            {
                canvas.Cursor = Cursors.Hand;
                /*
                 * параметры руки
                 */
                /*
                 * рука прокрутка во всех окнах
                 */
                CheckBox handScrollAllWindowsCheckbox = new CheckBox();
                handScrollAllWindowsCheckbox.IsChecked = false;
                handScrollAllWindowsCheckbox.Width = 25;
                handScrollAllWindowsCheckbox.Margin = new Thickness(10, 15, 0, 15);
                handScrollAllWindowsCheckbox.MouseUp += SetHandScrollAllWindows;
                toolParams.Children.Add(handScrollAllWindowsCheckbox);
                TextBlock handScrollAllWindowsLabel = new TextBlock();
                handScrollAllWindowsLabel.Text = "Прокрутка во всех окнах";
                handScrollAllWindowsLabel.Width = 75;
                handScrollAllWindowsLabel.Margin = new Thickness(0, 15, 0, 15);
                toolParams.Children.Add(handScrollAllWindowsLabel);
                /*
                 * конец лупа настр. размер окна
                 */
                /*
                 * конец параметров руки
                 */

            }
            else if (currentTool.ToolTip.ToString() == "Заливка")
            {
                canvas.Cursor = Cursors.AppStarting;
                /*
                 * Параметры заливки
                 */
                /*
                 * прозрачность заливки
                 */
                ComboBox brushOpacity = new ComboBox();
                ComboBoxItem brushOpacity0Prsnts = new ComboBoxItem();
                brushOpacity0Prsnts.Content = "0";
                brushOpacity.Items.Add(brushOpacity0Prsnts);
                ComboBoxItem brushOpacity10Prsnts = new ComboBoxItem();
                brushOpacity10Prsnts.Content = "10";
                brushOpacity.Items.Add(brushOpacity10Prsnts);
                ComboBoxItem brushOpacity20Prsnts = new ComboBoxItem();
                brushOpacity20Prsnts.Content = "20";
                brushOpacity.Items.Add(brushOpacity20Prsnts);
                ComboBoxItem brushOpacity30Prsnts = new ComboBoxItem();
                brushOpacity30Prsnts.Content = "30";
                brushOpacity.Items.Add(brushOpacity30Prsnts);
                ComboBoxItem brushOpacity40Prsnts = new ComboBoxItem();
                brushOpacity40Prsnts.Content = "40";
                brushOpacity.Items.Add(brushOpacity40Prsnts);
                ComboBoxItem brushOpacity50Prsnts = new ComboBoxItem();
                brushOpacity50Prsnts.Content = "50";
                brushOpacity.Items.Add(brushOpacity50Prsnts);
                ComboBoxItem brushOpacity60Prsnts = new ComboBoxItem();
                brushOpacity60Prsnts.Content = "60";
                brushOpacity.Items.Add(brushOpacity60Prsnts);
                ComboBoxItem brushOpacity70Prsnts = new ComboBoxItem();
                brushOpacity70Prsnts.Content = "70";
                brushOpacity.Items.Add(brushOpacity70Prsnts);
                ComboBoxItem brushOpacity80Prsnts = new ComboBoxItem();
                brushOpacity80Prsnts.Content = "80";
                brushOpacity.Items.Add(brushOpacity80Prsnts);
                ComboBoxItem brushOpacity90Prsnts = new ComboBoxItem();
                brushOpacity90Prsnts.Content = "90";
                brushOpacity.Items.Add(brushOpacity90Prsnts);
                ComboBoxItem brushOpacity100Prsnts = new ComboBoxItem();
                brushOpacity100Prsnts.Content = "100";
                brushOpacity.Items.Add(brushOpacity100Prsnts);
                brushOpacity.SelectedIndex = 10;
                brushOpacity.Width = 75;
                brushOpacity.Margin = new Thickness(25, 15, 0, 15);
                brushOpacity.SelectionChanged += SetBrushOpacity;
                toolParams.Children.Add(brushOpacity);
                /*
                 * конец прозрачности заливки
                 */
                /*
                 * Конец параметров заливки
                 */
            }
            else if (currentTool.ToolTip.ToString() == "Дополнительно")
            {
                canvas.Cursor = Cursors.ArrowCD;
            }
            else if (currentTool.ToolTip.ToString() == "Лупа")
            {
                canvas.Cursor = Cursors.No;
                /*
                 *
                 */
                /*
                 * лупа настр. размер окна
                 */
                CheckBox zoomSettedScreenScaleCheckbox = new CheckBox();
                zoomSettedScreenScaleCheckbox.IsChecked = false;
                zoomSettedScreenScaleCheckbox.Width = 25;
                zoomSettedScreenScaleCheckbox.Margin = new Thickness(10, 15, 0, 15);
                zoomSettedScreenScaleCheckbox.MouseUp += SetZoomSettedScreenScale;
                toolParams.Children.Add(zoomSettedScreenScaleCheckbox);
                TextBlock zoomSettedScreenScaleLabel = new TextBlock();
                zoomSettedScreenScaleLabel.Text = "Настр. размер окна";
                zoomSettedScreenScaleLabel.Width = 75;
                zoomSettedScreenScaleLabel.Margin = new Thickness(0, 15, 0, 15);
                toolParams.Children.Add(zoomSettedScreenScaleLabel);
                /*
                 * конец лупа настр. размер окна
                 */
                /*
                 * лупы во всех окнах
                 */
                CheckBox zoomAllWindowsCheckbox = new CheckBox();
                zoomAllWindowsCheckbox.IsChecked = false;
                zoomAllWindowsCheckbox.Width = 25;
                zoomAllWindowsCheckbox.Margin = new Thickness(10, 15, 0, 15);
                zoomAllWindowsCheckbox.MouseUp += SetZoomAllWindows;
                toolParams.Children.Add(zoomAllWindowsCheckbox);
                TextBlock zoomAllWindowsLabel = new TextBlock();
                zoomAllWindowsLabel.Text = "Во всех окнах";
                zoomAllWindowsLabel.Width = 75;
                zoomAllWindowsLabel.Margin = new Thickness(0, 15, 0, 15);
                toolParams.Children.Add(zoomAllWindowsLabel);
                /*
                 * конец лупы во всех окнах
                 */
                /*
                 * лупа масштабирования перетаскиванием
                 */
                CheckBox zoomScaleDragCheckbox = new CheckBox();
                zoomScaleDragCheckbox.IsChecked = false;
                zoomScaleDragCheckbox.Width = 25;
                zoomScaleDragCheckbox.Margin = new Thickness(10, 15, 0, 15);
                zoomScaleDragCheckbox.MouseUp += SetZoomScaleDrag;
                toolParams.Children.Add(zoomScaleDragCheckbox);
                TextBlock zoomScaleDragLabel = new TextBlock();
                zoomScaleDragLabel.Text = "Масшт. перетаскиванием";
                zoomScaleDragLabel.Width = 75;
                zoomScaleDragLabel.Margin = new Thickness(0, 15, 0, 15);
                toolParams.Children.Add(zoomScaleDragLabel);
                /*
                 * конец лупы масштабирования перетаскиванием
                 */
                /*
                 *
                 */
            }
            else if (currentTool.ToolTip.ToString() == "Фигуры")
            {
                canvas.Cursor = Cursors.Pen;
            }
            else if (currentTool.ToolTip.ToString() == "Текст")
            {
                canvas.Cursor = Cursors.IBeam;
                /*
                 *  начало параметров текста
                 */
                /*
                 * начало шрифта текста
                 */
                ComboBox textFont = new ComboBox();
                ComboBoxItem textFontVerdana = new ComboBoxItem();
                textFontVerdana.Content = "Verdana";
                textFont.Items.Add(textFontVerdana);
                ComboBoxItem textFontTimesNewRoman = new ComboBoxItem();
                textFontTimesNewRoman.Content = "Times New Roman";
                textFont.Items.Add(textFontTimesNewRoman);
                ComboBoxItem textFontArial = new ComboBoxItem();
                textFontArial.Content = "Arial";
                textFont.Items.Add(textFontArial);
                textFont.SelectedIndex = 0;
                textFont.Width = 75;
                textFont.Margin = new Thickness(25, 15, 0, 15);
                textFont.SelectionChanged += SetTextFont;
                toolParams.Children.Add(textFont);
                /*
                 * конец шрифта текста
                 */
                /*
                 * начало жирности текста
                 */
                ComboBox textWeight = new ComboBox();
                ComboBoxItem textWeightNormal = new ComboBoxItem();
                textWeightNormal.Content = "Normal";
                textWeight.Items.Add(textWeightNormal);
                ComboBoxItem textWeightBolder = new ComboBoxItem();
                textWeightBolder.Content = "Bolder";
                textWeight.Items.Add(textWeightBolder);
                textWeight.SelectedIndex = 0;
                textWeight.Width = 75;
                textWeight.Margin = new Thickness(25, 15, 0, 15);
                textWeight.SelectionChanged += SetTextWeight;
                toolParams.Children.Add(textWeight);
                /*
                 * конец жирности текста
                 */
                /*
                 * начало размера текста
                 */
                ComboBox textSize = new ComboBox();
                ComboBoxItem textSize14Pts = new ComboBoxItem();
                textSize14Pts.Content = "14";
                textSize.Items.Add(textSize14Pts);
                ComboBoxItem textSize16Pts = new ComboBoxItem();
                textSize16Pts.Content = "16";
                textSize.Items.Add(textSize16Pts);
                textSize.SelectedIndex = 0;
                textSize.Width = 75;
                textSize.Margin = new Thickness(25, 15, 0, 15);
                textSize.SelectionChanged += SetTextSize;
                toolParams.Children.Add(textSize);
                /*
                 * конец размера текста
                 */
                /*
                 * начало размера текста
                 */
                /*ColorPickerPalette с = new ColorPickerPalette();
                c.SelectedBrushChanged += SetTextColor;*/
                var color = new ColorPickerPalette();
                color.SelectedBrushChanged += SetTextColor;
                toolParams.Children.Add(color);
                /*
                 * конец размера текста
                 */
                /*
                 *  конец параметров текста
                 */
            }
            else if (currentTool.ToolTip.ToString() == "Переключить цвета")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.ToolTip.ToString() == "Курсор")
            {
                canvas.Cursor = Cursors.IBeam;
                /*
                 * парметры выбора
                 */
                ComboBox cursorSelect = new ComboBox();
                ComboBoxItem cursorSelectActiveLayers = new ComboBoxItem();
                cursorSelectActiveLayers.Content = "Активные слои";
                cursorSelect.Items.Add(cursorSelectActiveLayers);
                ComboBoxItem cursorSelectAllLayers = new ComboBoxItem();
                cursorSelectAllLayers.Content = "Все слои";
                cursorSelect.Items.Add(cursorSelectAllLayers);
                cursorSelect.SelectedIndex = 0;
                cursorSelect.Width = 75;
                cursorSelect.Margin = new Thickness(25, 15, 0, 15);
                cursorSelect.SelectionChanged += SetCursorSelect;
                toolParams.Children.Add(cursorSelect);
                /*
                 * конец парметра выбора
                 */

            }
            else if (currentTool.ToolTip.ToString() == "Палец")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.ToolTip.ToString() == "Освещение")
            {
                canvas.Cursor = Cursors.IBeam;
                /*
                 * Параметры освещения
                 */
                /*
                 * диапозон освещения
                 */
                TextBlock lightRangeLabel = new TextBlock();
                lightRangeLabel.Text = "Диапозон";
                lightRangeLabel.Width = 75;
                lightRangeLabel.Margin = new Thickness(25, 15, 0, 15);
                toolParams.Children.Add(lightRangeLabel);
                ComboBox lightRangeCheckbox = new ComboBox();
                ComboBoxItem lightRangeCheckboxShadows = new ComboBoxItem();
                lightRangeCheckboxShadows.Content = "Тени";
                lightRangeCheckbox.Items.Add(lightRangeCheckboxShadows);
                ComboBoxItem lightRangeCheckboxMiddleTones = new ComboBoxItem();
                lightRangeCheckboxMiddleTones.Content = "Средние тона";
                lightRangeCheckbox.Items.Add(lightRangeCheckboxMiddleTones);
                ComboBoxItem lightRangeCheckboxHighlights = new ComboBoxItem();
                lightRangeCheckboxHighlights.Content = "Подсветка";
                lightRangeCheckbox.Items.Add(lightRangeCheckboxHighlights);
                lightRangeCheckbox.SelectedIndex = 1;
                lightRangeCheckbox.Width = 75;
                lightRangeCheckbox.Margin = new Thickness(25, 15, 0, 15);
                lightRangeCheckbox.SelectionChanged += SetLightRange;
                toolParams.Children.Add(lightRangeCheckbox);
                /*
                 * конец диапозона освещения
                 */
                /*
                 * экспонирование освещения
                 */
                TextBlock lightExponLabel = new TextBlock();
                lightExponLabel.Text = "Экспонир.";
                lightExponLabel.Width = 75;
                lightExponLabel.Margin = new Thickness(25, 15, 0, 15);
                toolParams.Children.Add(lightExponLabel);
                ComboBox lightExpon = new ComboBox();
                ComboBoxItem lightExpon10Prsnts = new ComboBoxItem();
                lightExpon10Prsnts.Content = "10";
                lightExpon.Items.Add(lightExpon10Prsnts);
                ComboBoxItem lightExpon20Prsnts = new ComboBoxItem();
                lightExpon20Prsnts.Content = "20";
                lightExpon.Items.Add(lightExpon20Prsnts);
                ComboBoxItem lightExpon30Prsnts = new ComboBoxItem();
                lightExpon30Prsnts.Content = "30";
                lightExpon.Items.Add(lightExpon30Prsnts);
                ComboBoxItem lightExpon40Prsnts = new ComboBoxItem();
                lightExpon40Prsnts.Content = "40";
                lightExpon.Items.Add(lightExpon40Prsnts);
                ComboBoxItem lightExpon50Prsnts = new ComboBoxItem();
                lightExpon50Prsnts.Content = "50";
                lightExpon.Items.Add(lightExpon50Prsnts);
                ComboBoxItem lightExpon60Prsnts = new ComboBoxItem();
                lightExpon60Prsnts.Content = "60";
                lightExpon.Items.Add(lightExpon60Prsnts);
                ComboBoxItem lightExpon70Prsnts = new ComboBoxItem();
                lightExpon70Prsnts.Content = "70";
                lightExpon.Items.Add(lightExpon70Prsnts);
                ComboBoxItem lightExpon80Prsnts = new ComboBoxItem();
                lightExpon80Prsnts.Content = "80";
                lightExpon.Items.Add(lightExpon80Prsnts);
                ComboBoxItem lightExpon90Prsnts = new ComboBoxItem();
                lightExpon90Prsnts.Content = "90";
                lightExpon.Items.Add(lightExpon90Prsnts);
                ComboBoxItem lightExpon100Prsnts = new ComboBoxItem();
                lightExpon100Prsnts.Content = "100";
                lightExpon.Items.Add(lightExpon100Prsnts);
                lightExpon.SelectedIndex = 0;
                lightExpon.Width = 75;
                lightExpon.Margin = new Thickness(25, 15, 0, 15);
                lightExpon.SelectionChanged += SetLightExpon;
                toolParams.Children.Add(lightExpon);
                /*
                 * конец экспонирования освещения
                 */
                /*
                 * конец параметров освещения
                 */

            }
            else if (currentTool.ToolTip.ToString() == "Кисть истории")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.ToolTip.ToString() == "Восстанавливающая кисть")
            {
                canvas.Cursor = Cursors.IBeam;
                /*
                 * востанавливающая кисть
                 */
                /*
                 * начало режима востанавливающей кисти
                 */
                TextBlock recoveryModeLabel = new TextBlock();
                recoveryModeLabel.Text = "Режим";
                recoveryModeLabel.Width = 75;
                recoveryModeLabel.Margin = new Thickness(25, 15, 0, 15);
                toolParams.Children.Add(recoveryModeLabel);
                ComboBox recoveryMode = new ComboBox();
                ComboBoxItem recoveryModeNormal = new ComboBoxItem();
                recoveryModeNormal.Content = "Normal";
                recoveryMode.Items.Add(recoveryModeNormal);
                ComboBoxItem recoveryModeReplace = new ComboBoxItem();
                recoveryModeReplace.Content = "Replace";
                recoveryMode.Items.Add(recoveryModeReplace);
                recoveryMode.SelectedIndex = 0;
                recoveryMode.Width = 75;
                recoveryMode.Margin = new Thickness(25, 15, 0, 15);
                recoveryMode.SelectionChanged += SetRecoveryMode;
                toolParams.Children.Add(recoveryMode);
                /*
                 * конец режима востанавливающей кисти
                 */
                /*
                 * показать кольцо пробы пипетки
                 */
                CheckBox templateAllLayersCheckbox = new CheckBox();
                templateAllLayersCheckbox.IsChecked = false;
                templateAllLayersCheckbox.Width = 25;
                templateAllLayersCheckbox.Margin = new Thickness(10, 15, 0, 15);
                templateAllLayersCheckbox.MouseUp += SetRecoveryTemplateAllLayers;
                toolParams.Children.Add(templateAllLayersCheckbox);
                TextBlock templateAllLayersLabel = new TextBlock();
                templateAllLayersLabel.Text = "Образец со всех слоев";
                templateAllLayersLabel.Width = 75;
                templateAllLayersLabel.Margin = new Thickness(0, 15, 0, 15);
                toolParams.Children.Add(templateAllLayersLabel);
                /*
                 * конец показать кольцо пробы пипетки
                 */
                /*
                 * конец востанавливающей кисти
                 */
            }
            else if (currentTool.ToolTip.ToString() == "Пипетка")
            {
                canvas.Cursor = Cursors.IBeam;
                /*
                 * параметры пипетки
                 */
                /*
                 * размер образца пипетки
                 */
                TextBlock pippetTemplateSizeLabel = new TextBlock();
                pippetTemplateSizeLabel.Text = "Размер образца";
                pippetTemplateSizeLabel.Width = 75;
                pippetTemplateSizeLabel.Margin = new Thickness(25, 15, 0, 15);
                toolParams.Children.Add(pippetTemplateSizeLabel);
                ComboBox pippetTemplateSize = new ComboBox();
                ComboBoxItem pippetTemplateSizePoint = new ComboBoxItem();
                pippetTemplateSizePoint.Content = "Точка";
                pippetTemplateSize.Items.Add(pippetTemplateSizePoint);
                ComboBoxItem pippetTemplateSizeMiddle3X3 = new ComboBoxItem();
                pippetTemplateSizeMiddle3X3.Content = "Среднее3X3";
                pippetTemplateSize.Items.Add(pippetTemplateSizeMiddle3X3);
                ComboBoxItem pippetTemplateSizeMiddle5X5 = new ComboBoxItem();
                pippetTemplateSizeMiddle5X5.Content = "Среднее5X5";
                pippetTemplateSize.Items.Add(pippetTemplateSizeMiddle5X5);
                ComboBoxItem pippetTemplateSizeMiddle11X11 = new ComboBoxItem();
                pippetTemplateSizeMiddle11X11.Content = "Среднее11X11";
                pippetTemplateSize.Items.Add(pippetTemplateSizeMiddle11X11);
                ComboBoxItem pippetTemplateSizeMiddle31X31 = new ComboBoxItem();
                pippetTemplateSizeMiddle31X31.Content = "Среднее31X31";
                pippetTemplateSize.Items.Add(pippetTemplateSizeMiddle31X31);
                ComboBoxItem pippetTemplateSizeMiddle51X51 = new ComboBoxItem();
                pippetTemplateSizeMiddle51X51.Content = "Среднее51X51";
                pippetTemplateSize.Items.Add(pippetTemplateSizeMiddle51X51);
                ComboBoxItem wizardTemplateSizeMiddle101X101 = new ComboBoxItem();
                wizardTemplateSizeMiddle101X101.Content = "Среднее101X101";
                pippetTemplateSize.Items.Add(wizardTemplateSizeMiddle101X101);
                pippetTemplateSize.SelectedIndex = 0;
                pippetTemplateSize.Width = 75;
                pippetTemplateSize.Margin = new Thickness(25, 15, 0, 15);
                pippetTemplateSize.SelectionChanged += SetTranslateSelect;
                toolParams.Children.Add(pippetTemplateSize);
                /*
                 * конец размер образца пипетки
                 */
                /*
                 * образца пипетки
                 */
                TextBlock pippetTemplateLabel = new TextBlock();
                pippetTemplateLabel.Text = "Образец";
                pippetTemplateLabel.Width = 75;
                pippetTemplateLabel.Margin = new Thickness(25, 15, 0, 15);
                toolParams.Children.Add(pippetTemplateLabel);
                ComboBox pippetTemplate = new ComboBox();
                ComboBoxItem pippetTemplateAllLayers = new ComboBoxItem();
                pippetTemplateAllLayers.Content = "Все слои";
                pippetTemplate.Items.Add(pippetTemplateAllLayers);
                pippetTemplate.SelectedIndex = 0;
                pippetTemplate.Width = 75;
                pippetTemplate.Margin = new Thickness(25, 15, 0, 15);
                pippetTemplate.SelectionChanged += SetPipeteTemplate;
                toolParams.Children.Add(pippetTemplate);
                /*
                 * конец образца пипетки
                 */
                /*
                 * показать кольцо пробы пипетки
                 */
                CheckBox showRingExampleCheckbox = new CheckBox();
                showRingExampleCheckbox.IsChecked = true;
                showRingExampleCheckbox.Width = 25;
                showRingExampleCheckbox.Margin = new Thickness(10, 15, 0, 15);
                showRingExampleCheckbox.MouseUp += SetPippetShowRingExample;
                toolParams.Children.Add(showRingExampleCheckbox);
                TextBlock showRingExampleLabel = new TextBlock();
                showRingExampleLabel.Text = "Показать кольцо пробы";
                showRingExampleLabel.Width = 75;
                showRingExampleLabel.Margin = new Thickness(0, 15, 0, 15);
                toolParams.Children.Add(showRingExampleLabel);
                /*
                 * конец показать кольцо пробы пипетки
                 */
                /*
                 * конец параметров пипетки
                 */

            }
            else if (currentTool.ToolTip.ToString() == "Квадрат")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.ToolTip.ToString() == "Выделение")
            {
                canvas.Cursor = Cursors.SizeWE;
                /*
                 *  начало параметров выделения
                 */
                /*
                 * начало растушевки выделения
                 */
                TextBlock selectionShadingLabel = new TextBlock();
                selectionShadingLabel.Text = "Растушевка";
                selectionShadingLabel.Width = 75;
                selectionShadingLabel.Margin = new Thickness(25, 15, 0, 15);
                toolParams.Children.Add(selectionShadingLabel);
                TextBox selectionShadingInput = new TextBox();
                selectionShadingInput.Text = "0";
                selectionShadingInput.Width = 75;
                selectionShadingInput.Margin = new Thickness(25, 15, 0, 15);
                selectionShadingInput.FocusableChanged += SetSelectionShading;
                toolParams.Children.Add(selectionShadingInput);
                /*
                 * конец растушевки выделения
                 */
                /*
                 * начало стиля выделения
                 */
                TextBlock selectionStyleLabel = new TextBlock();
                selectionStyleLabel.Text = "Растушевка";
                selectionStyleLabel.Width = 75;
                selectionStyleLabel.Margin = new Thickness(25, 15, 0, 15);
                toolParams.Children.Add(selectionStyleLabel);
                ComboBox selectionStyle = new ComboBox();
                ComboBoxItem selectionStyleNormal = new ComboBoxItem();
                selectionStyleNormal.Content = "Normal";
                selectionStyle.Items.Add(selectionStyleNormal);
                ComboBoxItem selectionStyleSettedProportions = new ComboBoxItem();
                selectionStyleSettedProportions.Content = "Setted proportions";
                selectionStyle.Items.Add(selectionStyleSettedProportions);
                ComboBoxItem selectionStyleSettedSize = new ComboBoxItem();
                selectionStyleSettedSize.Content = "Setted size";
                selectionStyle.Items.Add(selectionStyleSettedSize);
                selectionStyle.SelectedIndex = 0;
                selectionStyle.Width = 75;
                selectionStyle.Margin = new Thickness(25, 15, 0, 15);
                selectionStyle.SelectionChanged += SetSelectionStyle;
                toolParams.Children.Add(selectionStyle);
                /*
                 * конец растушевки выделения
                 */
                /*
                 * конец выделения
                 */
            }
            else if (currentTool.ToolTip.ToString() == "Кадрировать")
            {
                canvas.Cursor = Cursors.SizeAll;
                /*
                 * Параметры кадрировать
                 */
                /*
                 * область кадрирования
                 */
                ComboBox cropArea = new ComboBox();
                ComboBoxItem cropAreaFree = new ComboBoxItem();
                cropAreaFree.Content = "Произвольно";
                cropArea.Items.Add(cropAreaFree);
                ComboBoxItem cropAreaRatio = new ComboBoxItem();
                cropAreaRatio.Content = "В соотношении";
                cropArea.Items.Add(cropAreaRatio);
                cropArea.SelectedIndex = 0;
                cropArea.Width = 75;
                cropArea.Margin = new Thickness(25, 15, 0, 15);
                cropArea.SelectionChanged += SetCropArea;
                toolParams.Children.Add(cropArea);
                /*
                 * конец область кадрирования
                 */
                /*
                 * удалить отсеч. пикс. кадрирования
                 */
                CheckBox removeCropPixelsCheckbox = new CheckBox();
                removeCropPixelsCheckbox.IsChecked = true;
                removeCropPixelsCheckbox.Width = 25;
                removeCropPixelsCheckbox.Margin = new Thickness(10, 15, 0, 15);
                removeCropPixelsCheckbox.MouseUp += SetWizardSmoothing;
                toolParams.Children.Add(removeCropPixelsCheckbox);
                TextBlock removeCropPixelsLabel = new TextBlock();
                removeCropPixelsLabel.Text = "Удалить отсеч. пикс.";
                removeCropPixelsLabel.Width = 75;
                removeCropPixelsLabel.Margin = new Thickness(0, 15, 0, 15);
                toolParams.Children.Add(removeCropPixelsLabel);
                /*
                 * конец удалить отсеч. пикс. кадрирования
                 */
                /*
                 * удалить отсеч. пикс. кадрирования
                 */
                CheckBox withContentCheckbox = new CheckBox();
                withContentCheckbox.IsChecked = true;
                withContentCheckbox.Width = 25;
                withContentCheckbox.Margin = new Thickness(10, 15, 0, 15);
                withContentCheckbox.MouseUp += SetCropWithContent;
                toolParams.Children.Add(withContentCheckbox);
                TextBlock withContentLabel = new TextBlock();
                withContentLabel.Text = "С учетом содержимого";
                withContentLabel.Width = 75;
                withContentLabel.Margin = new Thickness(0, 15, 0, 15);
                toolParams.Children.Add(withContentLabel);
                /*
                 * конец удалить отсеч. пикс. кадрирования
                 */
                /*
                 * конец кадрировать
                 */
            }
            else if (currentTool.ToolTip.ToString() == "Перо")
            {
                canvas.Cursor = Cursors.Pen;
                /*
                 * Параметры пера
                 */
                /*
                 * размер пера
                 */
                ComboBox brushSize = new ComboBox();
                ComboBoxItem brushSize10Pt = new ComboBoxItem();
                brushSize10Pt.Content = "10";
                brushSize.Items.Add(brushSize10Pt);
                ComboBoxItem brushSize25Pt = new ComboBoxItem();
                brushSize25Pt.Content = "25";
                brushSize.Items.Add(brushSize25Pt);
                ComboBoxItem brushSize100Pt = new ComboBoxItem();
                brushSize100Pt.Content = "100";
                brushSize.Items.Add(brushSize100Pt);
                brushSize.SelectedIndex = 0;
                brushSize.Width = 75;
                brushSize.Margin = new Thickness(25, 15, 0, 15);
                brushSize.SelectionChanged += SetBrushSize;
                toolParams.Children.Add(brushSize);
                /*
                 * конец размера пера
                 */
                /*
                 * Конец параметров пера
                 */

            } else if (currentTool.ToolTip.ToString() == "Перемещение")
            {
                canvas.Cursor = Cursors.SizeAll;
                /*
                 * Параметры перемещения
                 */
                /*
                 * автовыбор перемещения
                 */
                CheckBox autoSelectCheckbox = new CheckBox();
                autoSelectCheckbox.IsChecked = true;
                autoSelectCheckbox.Width = 25;
                autoSelectCheckbox.Margin = new Thickness(10, 15, 0, 15);
                toolParams.Children.Add(autoSelectCheckbox);
                TextBlock autoSelectLabel = new TextBlock();
                autoSelectLabel.Text = "Автовыбор";
                autoSelectLabel.Width = 75;
                autoSelectLabel.Margin = new Thickness(0, 15, 0, 15);
                autoSelectCheckbox.MouseUp += SetTranslateAutoSelect;
                toolParams.Children.Add(autoSelectLabel);
                /*
                 * конец автовыбора перемещения
                 */

                /*
                 * выбор перемещения
                 */
                ComboBox translateSelect = new ComboBox();
                ComboBoxItem translateSelectLayer = new ComboBoxItem();
                translateSelectLayer.Content = "Слой";
                translateSelect.Items.Add(translateSelectLayer);
                ComboBoxItem translateSelectGroup = new ComboBoxItem();
                translateSelectGroup.Content = "Группа";
                translateSelect.Items.Add(translateSelectGroup);
                translateSelect.SelectedIndex = 0;
                translateSelect.Width = 75;
                translateSelect.Margin = new Thickness(25, 15, 0, 15);
                translateSelect.SelectionChanged += SetTranslateSelect;
                toolParams.Children.Add(translateSelect);
                /*
                 * конец выбора перемещения
                 */
                /*
                 * Параметры перемещения
                 */
                /*
                 * показать элем. упр. перемещения
                 */
                CheckBox showControlsCheckbox = new CheckBox();
                showControlsCheckbox.IsChecked = false;
                showControlsCheckbox.Width = 25;
                showControlsCheckbox.Margin = new Thickness(10, 15, 0, 15);
                toolParams.Children.Add(showControlsCheckbox);
                TextBlock showControlsLabel = new TextBlock();
                showControlsLabel.Text = "Показать элем. упр.";
                showControlsLabel.Width = 75;
                showControlsLabel.Margin = new Thickness(0, 15, 0, 15);
                showControlsCheckbox.MouseUp += SetTranslateShowControls;
                toolParams.Children.Add(showControlsLabel);
                /*
                 * конец показать элем. упр. перемещения
                 */


                /*
                 * Конец параметров перемещения
                 */
            }
            else if (currentTool.ToolTip.ToString() == "Лассо")
            {
                canvas.Cursor = Cursors.SizeWE;
                /*
                 *  начало параметров лассо
                 */
                /*
                 * начало растушевки лассо
                 */
                TextBlock selectionShadingLabel = new TextBlock();
                selectionShadingLabel.Text = "Растушевка";
                selectionShadingLabel.Width = 75;
                selectionShadingLabel.Margin = new Thickness(25, 15, 0, 15);
                toolParams.Children.Add(selectionShadingLabel);
                TextBox selectionShadingInput = new TextBox();
                selectionShadingInput.Text = "0";
                selectionShadingInput.Width = 75;
                selectionShadingInput.Margin = new Thickness(25, 15, 0, 15);
                selectionShadingInput.FocusableChanged += SetSelectionShading;
                toolParams.Children.Add(selectionShadingInput);
                /*
                 * конец растушевки лассо
                 */
                /*
                 * сглаживание лассо
                 */
                CheckBox smoothingLassoCheckbox = new CheckBox();
                smoothingLassoCheckbox.IsChecked = false;
                smoothingLassoCheckbox.Width = 25;
                smoothingLassoCheckbox.Margin = new Thickness(10, 15, 0, 15);
                toolParams.Children.Add(smoothingLassoCheckbox);
                TextBlock smoothingLassoLabel = new TextBlock();
                smoothingLassoLabel.Text = "Сглаживание";
                smoothingLassoLabel.Width = 75;
                smoothingLassoLabel.Margin = new Thickness(0, 15, 0, 15);
                smoothingLassoCheckbox.MouseUp += SetLassoSmoothing;
                toolParams.Children.Add(smoothingLassoLabel);
                /*
                 * конец сглаживание лассо
                 */
                /*
                 * конец лассо
                 */
            }
            else if (currentTool.ToolTip.ToString() == "Волшебная палочка")
            {
                canvas.Cursor = Cursors.SizeAll;
                /*
                 * Параметры волшебной палочки
                 */
                /*
                 * допуск волшебной палочки
                 */
                TextBlock wizardThresholdLabel = new TextBlock();
                wizardThresholdLabel.Text = "Допуск";
                wizardThresholdLabel.Width = 75;
                wizardThresholdLabel.Margin = new Thickness(0, 15, 0, 15);
                wizardThresholdLabel.MouseUp += SetTranslateShowControls;
                toolParams.Children.Add(wizardThresholdLabel); 
                TextBox wizardThresholdInput = new TextBox();
                wizardThresholdInput.Width = 125;
                wizardThresholdInput.Margin = new Thickness(10, 15, 0, 15);
                toolParams.Children.Add(wizardThresholdInput);
                /*
                 * конец допуска волшебной палочки
                 */
                /*
                 * размер образца волшебной палочки
                 */
                TextBlock wizardTemplateSizeLabel = new TextBlock();
                wizardTemplateSizeLabel.Text = "Размер образца";
                wizardTemplateSizeLabel.Width = 75;
                wizardTemplateSizeLabel.Margin = new Thickness(25, 15, 0, 15);
                toolParams.Children.Add(wizardTemplateSizeLabel);
                ComboBox wizardTemplateSize = new ComboBox();
                ComboBoxItem wizardTemplateSizePoint = new ComboBoxItem();
                wizardTemplateSizePoint.Content = "Точка";
                wizardTemplateSize.Items.Add(wizardTemplateSizePoint);
                ComboBoxItem wizardTemplateSizeMiddle3X3 = new ComboBoxItem();
                wizardTemplateSizeMiddle3X3.Content = "Среднее3X3";
                wizardTemplateSize.Items.Add(wizardTemplateSizeMiddle3X3);
                ComboBoxItem wizardTemplateSizeMiddle5X5 = new ComboBoxItem();
                wizardTemplateSizeMiddle5X5.Content = "Среднее5X5";
                wizardTemplateSize.Items.Add(wizardTemplateSizeMiddle5X5);
                ComboBoxItem wizardTemplateSizeMiddle11X11 = new ComboBoxItem();
                wizardTemplateSizeMiddle11X11.Content = "Среднее11X11";
                wizardTemplateSize.Items.Add(wizardTemplateSizeMiddle11X11);
                ComboBoxItem wizardTemplateSizeMiddle31X31 = new ComboBoxItem();
                wizardTemplateSizeMiddle31X31.Content = "Среднее31X31";
                wizardTemplateSize.Items.Add(wizardTemplateSizeMiddle31X31);
                ComboBoxItem wizardTemplateSizeMiddle51X51 = new ComboBoxItem();
                wizardTemplateSizeMiddle51X51.Content = "Среднее51X51";
                wizardTemplateSize.Items.Add(wizardTemplateSizeMiddle51X51);
                ComboBoxItem wizardTemplateSizeMiddle101X101 = new ComboBoxItem();
                wizardTemplateSizeMiddle101X101.Content = "Среднее101X101";
                wizardTemplateSize.Items.Add(wizardTemplateSizeMiddle101X101);
                wizardTemplateSize.SelectedIndex = 0;
                wizardTemplateSize.Width = 75;
                wizardTemplateSize.Margin = new Thickness(25, 15, 0, 15);
                wizardTemplateSize.SelectionChanged += SetTranslateSelect;
                toolParams.Children.Add(wizardTemplateSize);
                /*
                 * конец размер образца волшебной палочки
                 */
                /*
                 * сглаживание волшебной палочки
                 */
                CheckBox wizardSmoothingCheckbox = new CheckBox();
                wizardSmoothingCheckbox.IsChecked = true;
                wizardSmoothingCheckbox.Width = 25;
                wizardSmoothingCheckbox.Margin = new Thickness(10, 15, 0, 15);
                toolParams.Children.Add(wizardSmoothingCheckbox);
                TextBlock wizardSmoothingLabel = new TextBlock();
                wizardSmoothingLabel.Text = "Сглаживание";
                wizardSmoothingLabel.Width = 75;
                wizardSmoothingLabel.Margin = new Thickness(0, 15, 0, 15);
                wizardSmoothingLabel.MouseUp += SetWizardSmoothing;
                toolParams.Children.Add(wizardSmoothingLabel);
                /*
                 * конец сглаживание волшебной палочки
                 */
                /*
                 * смеж пикс. волшебной палочки
                 */
                CheckBox neightboursPixelsCheckbox = new CheckBox();
                neightboursPixelsCheckbox.IsChecked = true;
                neightboursPixelsCheckbox.Width = 25;
                neightboursPixelsCheckbox.Margin = new Thickness(10, 15, 0, 15);
                toolParams.Children.Add(neightboursPixelsCheckbox);
                TextBlock neightboursPixelsLabel = new TextBlock();
                neightboursPixelsLabel.Text = "Смеж. пикс.";
                neightboursPixelsLabel.Width = 75;
                neightboursPixelsLabel.Margin = new Thickness(0, 15, 0, 15);
                neightboursPixelsLabel.MouseUp += SetWizardNeightbourPixels;
                toolParams.Children.Add(neightboursPixelsLabel);
                /*
                 * конец смеж пикс. волшебной палочки
                 */
                /*
                 * образец со всех слоев волшебной палочки
                 */
                CheckBox templateAllLayersCheckbox = new CheckBox();
                templateAllLayersCheckbox.IsChecked = false;
                templateAllLayersCheckbox.Width = 25;
                templateAllLayersCheckbox.Margin = new Thickness(10, 15, 0, 15);
                toolParams.Children.Add(templateAllLayersCheckbox);
                TextBlock templateAllLayersLabel = new TextBlock();
                templateAllLayersLabel.Text = "Образец со всех слоев";
                templateAllLayersLabel.Width = 75;
                templateAllLayersLabel.Margin = new Thickness(0, 15, 0, 15);
                templateAllLayersCheckbox.MouseUp += SetWizardTemplateAllLayers;
                toolParams.Children.Add(templateAllLayersLabel);
                /*
                 * конец образец со всех слоев волшебной палочки
                 */

                /*
                 * Конец волшебной палочки
                 */
            }
            activeTool = currentTool;

        }

        private void ColorOfPageHandler(object sender, Syncfusion.Windows.Tools.Controls.SelectedBrushChangedEventArgs e)
        {
            foreGroundColor = e.NewBrush;
        }

        private void OpenBlurDialog(object sender, RoutedEventArgs e)
        {
            Dialogs.GaussFilterDialog gaussFilterDialog = new Dialogs.GaussFilterDialog(canvas);
            gaussFilterDialog.Show();
        }

        private void CreateLayerHandler(object sender, RoutedEventArgs e)
        {
            Dialogs.NewLayerDialog newLayerDialog = new Dialogs.NewLayerDialog(layers);
            newLayerDialog.Show();
        }

        private void GlobalHotKeyHandler(object sender, KeyEventArgs e)
        {
            /*
             * Выбор инструментов начало
             */
            if (isHandleHotKeys)
            {
                if (e.Key == Key.B)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Кисть")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.Pen;
                    activeTool = currentTool;
                } else if (e.Key == Key.E)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Ластик")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    if (currentTool.ToolTip.ToString() == "Ластик")
                    {
                        canvas.Cursor = Cursors.UpArrow;
                    }
                    else if (currentTool.ToolTip.ToString() == "Рука")
                    {
                        canvas.Cursor = Cursors.Hand;
                    }
                    else if (currentTool.ToolTip.ToString() == "Заливка")
                    {
                        canvas.Cursor = Cursors.AppStarting;
                    }
                    else if (currentTool.ToolTip.ToString() == "Дополнительно")
                    {
                        canvas.Cursor = Cursors.ArrowCD;
                    }
                    else if (currentTool.ToolTip.ToString() == "Лупа")
                    {
                        canvas.Cursor = Cursors.No;
                    }
                    else if (currentTool.ToolTip.ToString() == "Фигуры")
                    {
                        canvas.Cursor = Cursors.Pen;
                    }
                    else if (currentTool.ToolTip.ToString() == "Текст")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Переключить цвета")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Курсор")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Палец")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Освещение")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Кисть истории")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Восстанавливающая кисть")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Пипетка")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Квадрат")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Выделение")
                    {
                        canvas.Cursor = Cursors.SizeWE;
                    }
                    else if (currentTool.ToolTip.ToString() == "Кадрировать")
                    {
                        canvas.Cursor = Cursors.SizeAll;
                    }
                    activeTool = currentTool;
                }
                else if (e.Key == Key.E)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Ластик")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.UpArrow;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.H)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Рука")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.Hand;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.G)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Заливка")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.AppStarting;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.Z)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Лупа")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.No;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.U)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Фигуры")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.None;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.T)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Текст")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.IBeam;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.X)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Переключить цвета")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.IBeam;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.A)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Курсор")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.IBeam;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.O)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Освещение")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.IBeam;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.Y)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Кисть истории")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.IBeam;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.J)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Восстанавливающая кисть")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.IBeam;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.I)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Пипетка")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.IBeam;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.K)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Квадрат")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.IBeam;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.M)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Выделение")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.SizeWE;
                    activeTool = currentTool;
                }
                else if (e.Key == Key.C)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Кадрировать")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.SizeAll;
                    activeTool = currentTool;
                } else if (e.Key == Key.W)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Волшебная палочка")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.SizeAll;
                    activeTool = currentTool;
                } else if (e.Key == Key.P)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Перо")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.SizeAll;
                    activeTool = currentTool;
                } else if (e.Key == Key.S)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Штамп")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.SizeAll;
                    activeTool = currentTool;
                } else if (e.Key == Key.L)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Лассо")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.SizeAll;
                    activeTool = currentTool;
                } else if (e.Key == Key.V)
                {
                    TextBlock currentTool = null;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                            if (((TextBlock)tool).ToolTip.ToString() == "Перемещение")
                            {
                                currentTool = (TextBlock)tool;
                            }
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    canvas.Cursor = Cursors.SizeAll;
                    activeTool = currentTool;
                } else if (e.Key == Key.Space)
                {
                    isHand = false;
                    TextBlock currentTool = lastTool;
                    activeTool = lastTool;
                    foreach (UIElement tool in tools.Children)
                    {
                        if (tool is TextBlock)
                        {
                            ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                        }
                    }
                    currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                    if (currentTool.ToolTip.ToString() == "Кисть")
                    {
                        canvas.Cursor = Cursors.Pen;
                    }
                    else if (currentTool.ToolTip.ToString() == "Ластик")
                    {
                        canvas.Cursor = Cursors.UpArrow;
                    }
                    else if (currentTool.ToolTip.ToString() == "Рука")
                    {
                        canvas.Cursor = Cursors.Hand;
                    }
                    else if (currentTool.ToolTip.ToString() == "Заливка")
                    {
                        canvas.Cursor = Cursors.AppStarting;
                    }
                    else if (currentTool.ToolTip.ToString() == "Дополнительно")
                    {
                        canvas.Cursor = Cursors.ArrowCD;
                    }
                    else if (currentTool.ToolTip.ToString() == "Лупа")
                    {
                        canvas.Cursor = Cursors.No;
                    }
                    else if (currentTool.ToolTip.ToString() == "Фигуры")
                    {
                        canvas.Cursor = Cursors.None;
                    }
                    else if (currentTool.ToolTip.ToString() == "Текст")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Переключить цвета")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Курсор")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Палец")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Освещение")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Кисть истории")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Восстанавливающая кисть")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Пипетка")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Квадрат")
                    {
                        canvas.Cursor = Cursors.IBeam;
                    }
                    else if (currentTool.ToolTip.ToString() == "Выделение")
                    {
                        canvas.Cursor = Cursors.SizeWE;
                    }
                    else if (currentTool.ToolTip.ToString() == "Кадрировать")
                    {
                        canvas.Cursor = Cursors.SizeAll;
                    }

                } else if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Control) > 0)
                {
                    canvas.Children.Remove(selection);
                }
                /*
                 * Выбор инструментов конец
                 */
            }
        }

        private void HandSwitchHander(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && !isHand)
            {
                lastTool = activeTool;
                isHand = true;
                TextBlock currentTool = null;
                foreach (UIElement tool in tools.Children)
                {
                    if (tool is TextBlock)
                    {
                        ((TextBlock)tool).Background = System.Windows.Media.Brushes.Transparent;
                        if (((TextBlock)tool).ToolTip.ToString() == "Рука")
                        {
                            currentTool = (TextBlock)tool;
                        }
                    }
                }
                currentTool.Background = System.Windows.Media.Brushes.DarkSlateGray;
                canvas.Cursor = Cursors.Hand;
                activeTool = currentTool;
            }
        }

        private void OpenCanvasSizeDialogHandler(object sender, RoutedEventArgs e)
        {
            Dialogs.CanvasSizeDialog canvasSizeDialog = new Dialogs.CanvasSizeDialog(canvas);
            canvasSizeDialog.Show();
        }

        private void SelectAllHandler(object sender, RoutedEventArgs e)
        {
            selection = new Rectangle();
            selection.Stroke = System.Windows.Media.Brushes.Black;
            selection.StrokeDashArray = new DoubleCollection(new List<Double>() { 0.7 });
            selection.Width = canvas.Width;
            selection.Height = canvas.Height;
            canvas.Children.Add(selection);
            Canvas.SetTop(selection, 0);
            Canvas.SetLeft(selection, 0);

        }

        private void OpenLayersDialogHandler(object sender, RoutedEventArgs e)
        {
            Dialogs.LayersDialog layersDialog = new Dialogs.LayersDialog(layers, canvas, totalStrokesIds);
            layersDialog.Show();
        }

        private void SetBrushSize(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newSize = (ComboBox)sender;
            brushSizePts = Int32.Parse(((ComboBoxItem)(newSize.Items[newSize.SelectedIndex])).Content.ToString());
        }

        private void SetBrushOpacity(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newOpacity = (ComboBox)sender;
            brushOpacityPrsnts = ((double)(Double.Parse(((ComboBoxItem)(newOpacity.Items[newOpacity.SelectedIndex])).Content.ToString()) / 100));
        }

        private void SetApplyText(object sender, RoutedEventArgs e)
        {
            Keyboard.ClearFocus();
            toolParams.Children.RemoveAt(toolParams.Children.Count - 2);
            toolParams.Children.RemoveAt(toolParams.Children.Count - 1);
            isHandleHotKeys = true;
            tools.Focus();
        }

        private void SetDeniesText(object sender, RoutedEventArgs e)
        {
            Keyboard.ClearFocus();
            canvas.Children.Remove(rasterText);
            toolParams.Children.RemoveAt(toolParams.Children.Count - 2);
            toolParams.Children.RemoveAt(toolParams.Children.Count - 1);
            isHandleHotKeys = true;
            tools.Focus();
        }

        private void SetTextFont(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newOpacity = (ComboBox)sender;
            textFont = ((ComboBoxItem)(newOpacity.Items[newOpacity.SelectedIndex])).Content.ToString();
            if (textFont == "Verdana") { 
                rasterText.FontFamily = new System.Windows.Media.FontFamily("Verdana");
            } else if (textFont == "Arial")
            {
                rasterText.FontFamily = new System.Windows.Media.FontFamily("Arial");
            } else if (textFont == "Times New Roman")
            {
                rasterText.FontFamily = new System.Windows.Media.FontFamily("Times New Roman");
            }
        }

        private void SetTextWeight(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newOpacity = (ComboBox)sender;
            textWeight = ((ComboBoxItem)(newOpacity.Items[newOpacity.SelectedIndex])).Content.ToString();
            if (textWeight == "Normal")
            {
                rasterText.FontWeight = FontWeights.Normal;
            }
            else if (textWeight == "Bolder")
            {
                rasterText.FontWeight = FontWeights.ExtraBold;
            }
        }
        private void SetTextSize(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newTextSize = (ComboBox)sender;
            textSizePnts = (Int32.Parse(((ComboBoxItem)(newTextSize.Items[newTextSize.SelectedIndex])).Content.ToString()));
            rasterText.FontSize = textSizePnts;
        }
        
        private void SetTextColor(object sender, Syncfusion.Windows.Tools.Controls.SelectedBrushChangedEventArgs e)
        {
            textColor = e.NewBrush;
            rasterText.Foreground = textColor;
        }
        
        private void SetTranslateSelect(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newTextSize = (ComboBox)sender;
            translateSelect = (((ComboBoxItem)(newTextSize.Items[newTextSize.SelectedIndex])).Content.ToString());
        }

        private void SetTranslateAutoSelect(object sender, RoutedEventArgs e)
        {
            
        }
        private void SetTranslateShowControls(object sender, RoutedEventArgs e)
        {

        }

        private void SetSelectionShading(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void SetSelectionStyle(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newTextSize = (ComboBox)sender;
            selectionStyle = ((ComboBoxItem)(newTextSize.Items[newTextSize.SelectedIndex])).Content.ToString();
        }

        private void SetLassoSmoothing(object sender, RoutedEventArgs e)
        {

        }
        private void SetWizardTemplateAllLayers(object sender, RoutedEventArgs e)
        {

        }

        private void SetWizardNeightbourPixels(object sender, RoutedEventArgs e)
        {

        }

        private void SetWizardSmoothing(object sender, RoutedEventArgs e)
        {

        }

        private void SetCropArea(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newTextSize = (ComboBox)sender;
            cropArea = ((ComboBoxItem)(newTextSize.Items[newTextSize.SelectedIndex])).Content.ToString();
        }

        private void SetRemoveCropPixels(object sender, RoutedEventArgs e)
        {
            CheckBox newRemoveCropPixels = (CheckBox)sender;
            removeCropPixels = ((bool)newRemoveCropPixels.IsChecked);
        }

        private void SetCropWithContent(object sender, RoutedEventArgs e)
        {
            CheckBox newCropWithContent = (CheckBox)sender;
            cropWithContent = ((bool)newCropWithContent.IsChecked);
        }

        private void SetPipeteTemplate(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newTextSize = (ComboBox)sender;
            pippeteTemplate = (((ComboBoxItem)(newTextSize.Items[newTextSize.SelectedIndex])).Content.ToString());
        }

        private void SetPippetShowRingExample(object sender, RoutedEventArgs e)
        {
            CheckBox newCropWithContent = (CheckBox)sender;
            pippetShowRingExample = ((bool)newCropWithContent.IsChecked);
        }
        
        private void SetRecoveryTemplateAllLayers(object sender, RoutedEventArgs e)
        {
            CheckBox newCropWithContent = (CheckBox)sender;
            pippetShowRingExample = ((bool)newCropWithContent.IsChecked);
        }

        private void SetRecoveryMode(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newTextSize = (ComboBox)sender;
            recoveryMode = ((ComboBoxItem)(newTextSize.Items[newTextSize.SelectedIndex])).Content.ToString();
        }

        private void SetZoomScaleDrag(object sender, RoutedEventArgs e)
        {
            CheckBox newCropWithContent = (CheckBox)sender;
            zoomScaleDrag = ((bool)newCropWithContent.IsChecked);
        }
        private void SetZoomAllWindows(object sender, RoutedEventArgs e)
        {
            CheckBox newCropWithContent = (CheckBox)sender;
            zoomAllWindows = ((bool)newCropWithContent.IsChecked);
        }

        private void SetZoomSettedScreenScale(object sender, RoutedEventArgs e)
        {
            CheckBox newCropWithContent = (CheckBox)sender;
            zoomSettedScreenScale = ((bool)newCropWithContent.IsChecked);
        }

        private void SetHandScrollAllWindows(object sender, RoutedEventArgs e)
        {
            CheckBox newCropWithContent = (CheckBox)sender;
            handScrollAllWindows = ((bool)newCropWithContent.IsChecked);
        }

        private void SetCursorSelect(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newTextSize = (ComboBox)sender;
            cursorSelect = ((ComboBoxItem)(newTextSize.Items[newTextSize.SelectedIndex])).Content.ToString();
        }

        private void SetLightRange(object sender, SelectionChangedEventArgs e)
        {
            ComboBox newTextSize = (ComboBox)sender;
            lightRange = ((ComboBoxItem)(newTextSize.Items[newTextSize.SelectedIndex])).Content.ToString();
        }

        private void SetLightExpon(object sender, SelectionChangedEventArgs e)
        {
            ComboBox expon = (ComboBox)sender;
            lightExpon = ((double)(Double.Parse(((ComboBoxItem)(expon.Items[expon.SelectedIndex])).Content.ToString()) / 100));
        }

        private void createMALHandler(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = "Новый документ";
            sfd.DefaultExt = ".mal";
            sfd.Filter = "Malevich documents (.mal)|*.mal";
            bool? res = sfd.ShowDialog();
            if (res != false)
            {

                using (Stream s = File.Open(sfd.FileName, FileMode.OpenOrCreate))
                {
                    using (StreamWriter sw = new StreamWriter(s))
                    {
                        sw.Write("malevich documents");
                    }
                }
            }
        }

        private void openMALHandler(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Новый документ";
            ofd.DefaultExt = ".mal";
            ofd.Filter = "Malevich documents (.mal)|*.mal";
            bool? res = ofd.ShowDialog();
            if (res != false)
            {
                Stream myStream;
                if ((myStream = ofd.OpenFile()) != null)
                {
                    string file_name = ofd.FileName;
                    string file_text = File.ReadAllText(file_name);

                    

                }
            }
        }

    }
}
