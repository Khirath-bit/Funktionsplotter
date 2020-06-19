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
    /// Interaction logic for IntegralSettings.xaml
    /// </summary>
    public partial class IntegralSettings : Window
    {
        public double UpperIntegralBorderValue { get; set; }

        public double LowerIntegralBorderValue { get; set; }

        public bool PlotIntegralValue { get; set; }

        public bool Saved { get; set; }

        public IntegralSettings()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(UpperIntegralBorder.Text, out var upper))
                UpperIntegralBorderValue = upper;

            if (double.TryParse(LowerIntegralBorder.Text, out var lower))
                LowerIntegralBorderValue = lower;

            PlotIntegralValue = PlotIntegral.IsChecked == true;

            Saved = true;

            Close();
        }
    }
}
