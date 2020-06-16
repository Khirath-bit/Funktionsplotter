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

        private void ZeroPointList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            return;
            if (!(DataContext is MainWindowContext context))
                return;
            
            var pointFivePercent = (context.GraphMenuContext.GetMaxXDouble() - context.GraphMenuContext.GetMinXDouble()) * 0.005;

            var x = Helper.ParseX(ZeroPointList.SelectedValue?.ToString());
            var y = Helper.ParseY(ZeroPointList.SelectedValue?.ToString());

            var redStroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            var line1 = new Line
            {
                X1 = x + pointFivePercent,
                Y1 = y + pointFivePercent,
                X2 = x - pointFivePercent,
                Y2 = y - pointFivePercent,
                StrokeThickness = MainFunctionLine.StrokeThickness,
                Stroke = redStroke,
                Tag = "cross"
            };

            var line2 = new Line
            {
                X1 = x - pointFivePercent,
                Y1 = y + pointFivePercent,
                X2 = x + pointFivePercent,
                Y2 = y - pointFivePercent,
                StrokeThickness = MainFunctionLine.StrokeThickness,
                Stroke = redStroke,
                Tag = "cross"
            };

            var childsToRemove = new List<UIElement>();

            foreach (var child in Graph.Children)
            {
                if(child is Line line && ReferenceEquals(line.Tag, "cross"))
                    childsToRemove.Add(line);
            }

            foreach (var child in childsToRemove)
            {
                Graph.Children.Remove(child);
            }

            Graph.Children.Add(line2);
            Graph.Children.Add(line1);
        }
    }
}
