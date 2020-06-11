using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Funktionenplotter.DataObjects;
using Funktionenplotter.UserControls;

namespace Funktionenplotter
{
    public class MainWindowContext : INotifyPropertyChanged
    {
        public string FourthCoefficientGraph { get; set; }
        public string ThirdCoefficientGraph { get; set; }
        public string SecondCoefficientGraph{ get; set; }
        public string FirstCoefficientGraph { get; set; }
        public string B { get; set; }
        public GraphMenuContext GraphMenuContext { get; set; }
        public PointCollection Points { get; set; } = new PointCollection();
        public Transform RenderTransform { get; set; } = Transform.Parse("1 0 0 -1 0 0");
        public double StrokeThicknessX { get; set; } = 1;
        public double StrokeThicknessY { get; set; } = 1;
        public double StrokeThicknessLine { get; set; } = 1;
        public int ActualHeightGraph { get; set; }
        public int ActualWidthGraph { get;set; }

        public MainWindowContext()
        {
            GraphMenuContext = new GraphMenuContext(Plot);
        }

        /// <summary>
        /// Plots the graph
        /// </summary>
        private void Plot(object parameter)
        {
            if(!double.TryParse(GraphMenuContext.CalculationAccuracy, out var calcAccuracy))
                return;

            var points = new PointCollection();

            var func = new Function(FourthCoefficientGraph, ThirdCoefficientGraph, SecondCoefficientGraph, FirstCoefficientGraph, B);

            for (var i = GraphMenuContext.GetMinXDouble(); i < GraphMenuContext.GetMaxXDouble(); i+= calcAccuracy)
                points.Add(new Point(i, CalculateYForSinusX(i)));

            Points = points;
            UpdateView();
        }

        public double CalculateYForSinusX(double x)
        {
            return Math.Sin(x);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateView()
        {
            RenderTransform = GraphMenuContext.GetRenderTransform(ActualHeightGraph, ActualWidthGraph);
            StrokeThicknessX = Helper.CalculateStrokeThicknessByTransform(RenderTransform, Axis.X);
            StrokeThicknessY = Helper.CalculateStrokeThicknessByTransform(RenderTransform, Axis.Y);
            StrokeThicknessLine = Helper.CalculateStrokeThicknessByTransform(RenderTransform, Axis.Line);
            OnPropertyChanged("RenderTransform");
            OnPropertyChanged("StrokeThicknessX");
            OnPropertyChanged("StrokeThicknessY");
            OnPropertyChanged("StrokeThicknessLine");
            OnPropertyChanged("Points");
        }
    }
}
