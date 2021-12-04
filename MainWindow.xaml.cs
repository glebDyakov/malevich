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
                if (activeTool.Text == "Кисть")
                {
                    pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                    currentStroke.Points = pointCollection;
                } else if (activeTool.Text == "Ластик")
                {
                    pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                    currentStroke.Points = pointCollection;
                } else if (activeTool.Text == "Выделение")
                {
                    /*selection.Width = Mouse.GetPosition(canvas).X - startSelectionPoint.X;
                    selection.Height = Mouse.GetPosition(canvas).Y - startSelectionPoint.Y;*/
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
            if (activeTool.Text == "Заливка")
            {
                canvas.Background = System.Windows.Media.Brushes.Red;
            }
            if (activeTool.Text == "Выделение")
            {
                canvas.Children.Remove(selection);
                startSelectionPoint = new Point(0, 0);
            }
        }

        private void BrushDownHandler(object sender, MouseButtonEventArgs e)
        {
            isDrawing = true;

            if (activeTool.Text == "Кисть")
            {
                currentStroke = new Polyline();
                currentStroke.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                pointCollection = new PointCollection();
                pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                currentStroke.StrokeThickness = 8;
                currentStroke.Points = pointCollection;
                canvas.Children.Add(currentStroke);
            } else if (activeTool.Text == "Ластик")
            {
                currentStroke = new Polyline();
                currentStroke.Stroke = System.Windows.Media.Brushes.White;
                pointCollection = new PointCollection();
                pointCollection.Add(new Point(Mouse.GetPosition(canvas).X, Mouse.GetPosition(canvas).Y));
                currentStroke.StrokeThickness = 8;
                currentStroke.Points = pointCollection;
                canvas.Children.Add(currentStroke);
            } else if (activeTool.Text == "Выделение")
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
                } else if (e.Delta < 0)
                {
                    zoom.Width = zoom.Width - 10 * (e.Delta / 120 * -1);
                    zoom.Height = zoom.Height - 10 * (e.Delta / 120 * -1);
                }
            }
        }

        private void HoverToolHandler(object sender, MouseEventArgs e)
        {
            TextBlock currentTool = (TextBlock)sender;
            if (activeTool.Text != currentTool.Text)
            {
                currentTool.Background = System.Windows.Media.Brushes.Gray;
            }
        }

        private void HoutToolHandler(object sender, MouseEventArgs e)
        {
            TextBlock currentTool = (TextBlock)sender;
            if (activeTool.Text != currentTool.Text)
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
            if (currentTool.Text == "Кисть")
            {
                canvas.Cursor = Cursors.Pen;
            } else if (currentTool.Text == "Ластик")
            {
                canvas.Cursor = Cursors.UpArrow;
            } else if (currentTool.Text == "Рука")
            {
                canvas.Cursor = Cursors.Hand;
            }
            else if (currentTool.Text == "Заливка")
            {
                canvas.Cursor = Cursors.AppStarting;
            }
            else if (currentTool.Text == "Дополнительно")
            {
                canvas.Cursor = Cursors.ArrowCD;
            }
            else if (currentTool.Text == "Лупа")
            {
                canvas.Cursor = Cursors.No;
            }
            else if (currentTool.Text == "Фигуры")
            {
                canvas.Cursor = Cursors.None;
            }
            else if (currentTool.Text == "Текст")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.Text == "Переключить цвета")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.Text == "Курсор")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.Text == "Палец")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.Text == "Освещение")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.Text == "Кисть истории")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.Text == "Восстанавливающая кисть")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.Text == "Пипетка")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.Text == "Квадрат")
            {
                canvas.Cursor = Cursors.IBeam;
            }
            else if (currentTool.Text == "Выделение")
            {
                canvas.Cursor = Cursors.SizeWE;
            }
            activeTool = currentTool;
        }
    }
}
