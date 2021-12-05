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
    /// Логика взаимодействия для CanvasSizeDialog.xaml
    /// </summary>
    public partial class CanvasSizeDialog : Window
    {

        public Canvas canvas;

        public CanvasSizeDialog(Canvas canvas)
        {
            InitializeComponent();
            this.canvas = canvas;
            width.Text = canvas.Width.ToString();
            height.Text = canvas.Height.ToString();
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SetCanvasSizeHandler(object sender, RoutedEventArgs e)
        {
            canvas.Width = Int32.Parse(width.Text);
            canvas.Height = Int32.Parse(height.Text);
            this.Close();
        }
    }
}
