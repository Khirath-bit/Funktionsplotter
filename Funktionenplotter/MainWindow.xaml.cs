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

namespace Funktionenplotter
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            MainFunctionLine.MouseMove += Graph_MouseMove;
        }

        private void Graph_MouseMove(object sender, MouseEventArgs e)
        {
            logWindow.Text = $"X: {X.StrokeThickness} | Y: {Y.StrokeThickness}";

            var currentPos = e.GetPosition(MainFunctionLine);

            DisplayPointX.Text = $"X: {currentPos.X}";
            DisplayPointY.Text = $"Y: {currentPos.Y}";
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is MainWindowContext context))
                return;

            context.ActualHeightGraph = (int)GraphRow.ActualHeight;
            context.ActualWidthGraph = (int)GraphCol.ActualWidth;
        }
    }
}
