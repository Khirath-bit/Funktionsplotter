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

namespace Funktionenplotter.UserControls
{
    /// <summary>
    /// Interaction logic for ValueTable.xaml
    /// </summary>
    public partial class ValueTable : Window
    {
        public ValueTable(List<Point> points)
        {
            InitializeComponent();
            Points = points;
        }

        /// <summary>
        /// The dependency property of <see cref="HeadlineText"/>
        /// </summary>
        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
            nameof(Points), typeof(List<Point>), typeof(ValueTable), new PropertyMetadata(default(List<Point>)));

        /// <summary>
        /// Gets or sets the text of the headline
        /// </summary>
        public List<Point> Points
        {
            get => (List<Point>)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
    }
}
