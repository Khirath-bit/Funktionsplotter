using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Funktionenplotter.DataObjects;
using Funktionenplotter.UserControls;
using Funktionenplotter.Utility;

namespace Funktionenplotter
{
    public class MainWindowContext : INotifyPropertyChanged
    {
        public string FourthCoefficientGraph { get; set; } = "0.5";
        public string ThirdCoefficientGraph { get; set; } = "-3";
        public string SecondCoefficientGraph { get; set; } = "5";
        public string FirstCoefficientGraph { get; set; } = "-2";
        public string B { get; set; } = "0.5";
        public GraphMenuContext GraphMenuContext { get; set; }
        public PointCollection BluePoints { get; set; } = new PointCollection();
        public PointCollection RedPoints { get; set; } = new PointCollection();
        public PointCollection GreenPoints { get; set; } = new PointCollection();
        public Transform RenderTransform { get; set; } = Transform.Parse("1 0 0 -1 0 0");
        public double StrokeThicknessX { get; set; } = 1;
        public double StrokeThicknessY { get; set; } = 1;
        public double StrokeThicknessLine { get; set; } = 1;
        public int ActualHeightGraph { get; set; }
        public int ActualWidthGraph { get; set; }
        public bool ToValueTableBtnEnabled { get; set; }
        public bool SinusChecked { get; set; }
        public bool CosinusChecked { get; set; }
        public List<string> ZeroPoints { get; set; }
        public ICommand ShowValueTableCommand => new DelegateCommand(ShowValueTableForGraph);

        public MainWindowContext()
        {
            GraphMenuContext = new GraphMenuContext(Plot);
        }

        private void ShowValueTableForGraph(object parameter)
        {
            new ValueTable(BluePoints.ToList()).Show();
        }

        /// <summary>
        /// Plots the graph
        /// </summary>
        private void Plot(object parameter)
        {
            if (!double.TryParse(GraphMenuContext.CalculationAccuracy, out var calcAccuracy))
                return;

            ZeroPoints = new List<string>();
            RedPoints = new PointCollection();
            GreenPoints = new PointCollection();
            BluePoints = new PointCollection();

            var bluePoints = new PointCollection();
            var redPoints = new PointCollection();
            var greenPoints = new PointCollection();


            var func = new Function(FourthCoefficientGraph, ThirdCoefficientGraph, SecondCoefficientGraph, FirstCoefficientGraph, B);

            foreach (var p in func.ZeroPoints)
            {
                ZeroPoints.Add($"X: {p.X:0.00} | Y: {p.Y:0.00}");
            }

            for (var i = GraphMenuContext.GetMinXDouble(); i < GraphMenuContext.GetMaxXDouble(); i += calcAccuracy)
            {
                bluePoints.Add(new Point(i, func.CalculateYForX(i)));
                if (SinusChecked)
                    redPoints.Add(new Point(i, CalculateYForSinusX(i)));
                if (CosinusChecked)
                    greenPoints.Add(new Point(i, CalculateYForCoSinusX(i)));
            }

            if (GraphMenuContext.CalculateFirstDerivative && func.Level != FunctionLevel.First)
                for (var i = GraphMenuContext.GetMinXDouble(); i < GraphMenuContext.GetMaxXDouble(); i += calcAccuracy)
                    greenPoints.Add(new Point(i, func.CalculateFirstDerivative(i)));

            if (GraphMenuContext.CalculateSecondDerivative && (func.Level == FunctionLevel.Third || func.Level == FunctionLevel.Fourth))
                for (var i = GraphMenuContext.GetMinXDouble(); i < GraphMenuContext.GetMaxXDouble(); i += calcAccuracy)
                    redPoints.Add(new Point(i, func.CalculateSecondDerivative(i)));

            ToValueTableBtnEnabled = true;
            RedPoints = redPoints;
            GreenPoints = greenPoints;
            BluePoints = bluePoints;
            UpdateView();
        }

        public double CalculateYForSinusX(double x)
        {
            return Math.Sin(x);
        }

        public double CalculateYForCoSinusX(double x)
        {
            return Math.Cos(x);
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
            OnPropertyChanged("ToValueTableBtnEnabled");
            OnPropertyChanged("BluePoints");
            OnPropertyChanged("GreenPoints"); 
            OnPropertyChanged("RedPoints");
            OnPropertyChanged("ZeroPoints");
        }
    }
}
