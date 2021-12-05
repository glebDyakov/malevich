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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace paint.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для GaussFilterDialog.xaml
    /// </summary>
    public partial class GaussFilterDialog : Window
    {
        public Canvas canvas;

        public GaussFilterDialog(Canvas canvas)
        {
            InitializeComponent();
            this.canvas = canvas;
        }

        private void ApplyBlurFilter(object sender, RoutedEventArgs e)
        {
            BlurBitmapEffect gausBlurFilter = new BlurBitmapEffect();
            gausBlurFilter.Radius = Int32.Parse(radius.Text);
            canvas.BitmapEffect = gausBlurFilter;
            this.Close();
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
