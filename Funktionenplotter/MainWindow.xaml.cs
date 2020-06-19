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
using Funktionenplotter.DataObjects;
using Funktionenplotter.UserControls;

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
            if (!(DataContext is MainWindowContext context))
                return;

            var currentPos = e.GetPosition(MainFunctionLine);

            DisplayPointX.Text = $"X: {currentPos.X}";
            DisplayPointY.Text = $"Y: {currentPos.Y}";


            var y1 = context.CurrentFunction.FirstDerivative.CalculateYForX(currentPos.X);

            if (y1 == 0)
                Steigung.Text = "Waagerecht";
            else
                Steigung.Text = y1 < 0
                    ? "Senkung"
                    : "Steigung";


            if (context.CurrentFunction.Level == FunctionLevel.First)
                return;

            var y = context.CurrentFunction.SecondDerivative.CalculateYForX(currentPos.X);

            if (y == 0)
                Krümmung.Text = "Keine Krümmung";
            else
            {
                Krümmung.Text = y < 0
                    ? "Rechtskrümmung"
                    : "Linkskrümmung";
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is MainWindowContext context))
                return;

            context.ActualHeightGraph = (int)GraphRow.ActualHeight;
            context.ActualWidthGraph = (int)GraphCol.ActualWidth;
        }

        private void ZeroPointList_OnMouseLeave(object sender, MouseEventArgs e)
        {
            var remove = new List<UIElement>();

            for (var i = 0; i < Graph.Children.Count; i++)
            {
                if (Graph.Children[i] is Line line && (string)line.Tag == "cross"
                   || Graph.Children[i] is Polygon poly && (string)poly.Tag == "polygon")
                    remove.Add(Graph.Children[i]);
            }

            foreach (var child in remove)
            {
                Graph.Children.Remove(child);
            }

            ZeroPointList.SelectedIndex = -1;
            IntegralInfos.SelectedIndex = -1;
        }

        private void ZeroPointList_OnMouseMove(object sender, SelectionChangedEventArgs e)
        {
            if (ZeroPointList.SelectedIndex == -1)
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
                if (child is Line line && ReferenceEquals(line.Tag, "cross"))
                    childsToRemove.Add(line);
            }

            foreach (var child in childsToRemove)
            {
                Graph.Children.Remove(child);
            }

            Graph.Children.Add(line2);
            Graph.Children.Add(line1);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(DataContext is MainWindowContext context) || !double.TryParse(context.GraphMenuContext.CalculationAccuracy, out var calcAccuracy))
                return;

            var polygon = new Polygon();
            var points = new PointCollection();

            polygon.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            polygon.Opacity = 0.1;
            polygon.StrokeThickness = MainFunctionLine.StrokeThickness;
            polygon.Tag = "polygon";

            points.Add(new Point(context.GraphMenuContext.UpperIntegralBorder, 0));
            points.Add(new Point(context.GraphMenuContext.LowerIntegralBorder, 0));

            for (var i = context.GraphMenuContext.LowerIntegralBorder; i < context.GraphMenuContext.UpperIntegralBorder; i += calcAccuracy)
                points.Add(new Point(i, context.CurrentFunction.CalculateYForX(i)));

            polygon.Points = points;

            Graph.Children.Add(polygon);
        }
    }
}
