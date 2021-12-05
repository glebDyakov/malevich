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
        public Path penCurve;
        public PathSegmentCollection pathSegmentCollection;
        public BezierSegment bezierSegment;
        public  Point previousPoint;
        public List<Int32> layers;
        public bool isHand = false;
        public TextBlock lastTool;
        public SpeechSynthesizer debugger;

        public MainWindow()
        {
            InitializeComponent();

            initialWidth = zoom.Width;
            initialHeight = zoom.Height;
            activeTool = startActiveTool;
            foreGroundColor = System.Windows.Media.Brushes.Black;
            backGroundColor = System.Windows.Media.Brushes.White;
            layers = new List<Int32>() {
                0
            };
            debugger = new SpeechSynthesizer();
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
                    penCurve = new Path();
                    penCurve.Stroke = foreGroundColor;
                    penCurve.StrokeThickness = 2;
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
                currentStroke.StrokeThickness = 8;
                currentStroke.Points = pointCollection;
                canvas.Children.Add(currentStroke);
            } else if (activeTool.ToolTip.ToString() == "Ластик")
            {
                currentStroke = new Polyline();
                currentStroke.Stroke = System.Windows.Media.Brushes.White;
                pointCollection = new PointCollection();
                pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                currentStroke.StrokeThickness = 8;
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
                TextBox rasterText = new TextBox();
                rasterText.BorderThickness = new Thickness(0);
                rasterText.AcceptsReturn = true;
                rasterText.Text = "Lorem ipsum";
                canvas.Children.Add(rasterText);
                Canvas.SetTop(rasterText, Mouse.GetPosition(canvas).Y);
                Canvas.SetLeft(rasterText, Mouse.GetPosition(canvas).X);
                rasterText.Focus();
            } else if (activeTool.ToolTip.ToString() == "Перо")
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
            if (currentTool.ToolTip.ToString() == "Кисть")
            {
                canvas.Cursor = Cursors.Pen;
            } else if (currentTool.ToolTip.ToString() == "Ластик")
            {
                canvas.Cursor = Cursors.UpArrow;
            } else if (currentTool.ToolTip.ToString() == "Рука")
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
            layers.Add(layers.Count);
            debugger.Speak(layers.Count.ToString());
        }

        private void GlobalHotKeyHandler(object sender, KeyEventArgs e)
        {
            /*
             * Выбор инструментов начало
             */
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
            Dialogs.LayersDialog layersDialog = new Dialogs.LayersDialog(layers);
            layersDialog.Show();
        }
    }
}
