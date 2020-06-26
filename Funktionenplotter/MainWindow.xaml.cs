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

        }

        //private void Graph_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (!(DataContext is MainWindowContext context))
        //        return;

        //    //var currentPos = e.GetPosition(MainFunctionLine);

        //    //DisplayPointX.Text = $"X: {currentPos.X}";
        //    //DisplayPointY.Text = $"Y: {currentPos.Y}";


        //    //var y1 = context.CurrentFunction.FirstDerivative.CalculateYForX(currentPos.X);

        //    //if (y1 == 0)
        //    //    Steigung.Text = "Waagerecht";
        //    //else
        //    //    Steigung.Text = y1 < 0
        //    //        ? "Senkung"
        //    //        : "Steigung";


        //    //if (context.CurrentFunction.Level == FunctionLevel.First)
        //    //    return;

        //    //var y = context.CurrentFunction.SecondDerivative.CalculateYForX(currentPos.X);

        //    //if (y == 0)
        //    //    Krümmung.Text = "Keine Krümmung";
        //    //else
        //    //{
        //    //    Krümmung.Text = y < 0
        //    //        ? "Rechtskrümmung"
        //    //        : "Linkskrümmung";
        //    //}
        //}

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!(DataContext is MainWindowContext context))
                return;

            context.ActualHeightGraph = (int)GraphRow.ActualHeight;
            context.ActualWidthGraph = (int)GraphCol.ActualWidth;
        }

        private void PointLists_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if(!(DataContext is MainWindowContext context))
                return;

            ZeroPointList.SelectedIndex = -1;
            IntegralInfos.SelectedIndex = -1;
            TurningPointList.SelectedIndex = -1;
            ExtremePointList.SelectedIndex = -1;
            SaddlePointList.SelectedIndex = -1;

            context.CoordinateSystem.RemoveChildrenByTag("cross");
            context.CoordinateSystem.RemoveChildrenByTag("polygon");
        }

        private void Lists_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (ListView) sender;

            if (list.SelectedIndex == -1)
                return;

            if (!(DataContext is MainWindowContext context))
                return;

            var pointFivePercent = (context.GraphMenuContext.GetMaxXDouble() - context.GraphMenuContext.GetMinXDouble()) * 0.005;

            var x = Helper.ParseX(list.SelectedValue?.ToString());
            var y = Helper.ParseY(list.SelectedValue?.ToString());

            var redStroke = new SolidColorBrush(Color.FromRgb(255, 0, 0));

            var line1 = new Line
            {
                X1 = x + pointFivePercent,
                Y1 = y + pointFivePercent,
                X2 = x - pointFivePercent,
                Y2 = y - pointFivePercent,
                Stroke = redStroke,
                Tag = "cross"
            };

            var line2 = new Line
            {
                X1 = x - pointFivePercent,
                Y1 = y + pointFivePercent,
                X2 = x + pointFivePercent,
                Y2 = y - pointFivePercent,
                Stroke = redStroke,
                Tag = "cross"
            };

            context.CoordinateSystem.AddMark(line1, line2);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(DataContext is MainWindowContext context) || !double.TryParse(context.GraphMenuContext.CalculationAccuracy, out var calcAccuracy))
                return;

            var polygon = new Polygon();
            var points = new PointCollection();

            polygon.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            polygon.Opacity = 0.1;
            polygon.Tag = "polygon";

            points.Add(new Point(context.GraphMenuContext.UpperIntegralBorder, 0));
            points.Add(new Point(context.GraphMenuContext.LowerIntegralBorder, 0));

            for (var i = context.GraphMenuContext.LowerIntegralBorder; i < context.GraphMenuContext.UpperIntegralBorder; i += calcAccuracy)
                points.Add(new Point(i, context.CurrentFunction.CalculateYForX(i)));

            polygon.Points = points;

            context.CoordinateSystem.AddPolygon(polygon);
        }
    }
}
