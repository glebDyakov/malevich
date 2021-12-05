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
        public MainWindow()
        {
            InitializeComponent();

            initialWidth = zoom.Width;
            initialHeight = zoom.Height;
            activeTool = startActiveTool;
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
                canvas.Background = System.Windows.Media.Brushes.Red;
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
            
        }

        private void BrushDownHandler(object sender, MouseButtonEventArgs e)
        {
            isDrawing = true;

            if (activeTool.ToolTip.ToString() == "Кисть")
            {
                currentStroke = new Polyline();
                currentStroke.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
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
            foreach(TextBlock tool in tools.Children)
            {
                tool.Background = System.Windows.Media.Brushes.Transparent;
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
    }
}
