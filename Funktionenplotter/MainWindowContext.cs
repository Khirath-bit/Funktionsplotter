using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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
        public string LogText { get; set; }
        public string FourthCoefficientGraph { get; set; } = "";
        public string ThirdCoefficientGraph { get; set; } = "1";
        public string SecondCoefficientGraph { get; set; } = "";
        public string FirstCoefficientGraph { get; set; } = "-9";
        public string B { get; set; } = "0";
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
        public List<string> TurningPoints { get; set; }
        public List<string> SaddlePoints { get; set; }
        public List<string> ExtremePoints { get; set; }
        public List<string> IntegralInfos { get; set; }
        public List<string> Functions { get; set; }
        public Function CurrentFunction { get; set; }
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
            SaddlePoints = new List<string>();
            ExtremePoints = new List<string>();
            TurningPoints = new List<string>();
            IntegralInfos = new List<string>();
            Functions = new List<string>();
            RedPoints = new PointCollection();
            GreenPoints = new PointCollection();
            BluePoints = new PointCollection();
            var bluePoints = new PointCollection();
            var redPoints = new PointCollection();
            var greenPoints = new PointCollection();


            var func = new Function(FourthCoefficientGraph, ThirdCoefficientGraph, SecondCoefficientGraph, FirstCoefficientGraph, B);

            Functions.Add($"Integral: {Helper.FunctionToString(MathOperations.GetAntiDerivative(func.Polynom))}");
            Functions.Add($"Root:     {func}");
            Functions.Add($"1. Abl:   {func.FirstDerivative}");
            Functions.Add($"2. Abl:   {func.SecondDerivative}");

            foreach (var p in func.ZeroPoints)
                ZeroPoints.Add($"X: {p.X:0.00} | Y: {p.Y:0.00}");

            foreach (var p in func.TurningPoints)
                TurningPoints.Add($"X: {p.X:0.00} | Y: {p.Y:0.00}");

            foreach (var p in func.SaddlePoints)
                SaddlePoints.Add($"X: {p.X:0.00} | Y: {p.Y:0.00}");

            foreach (var p in func.ExtremePoints)
                ExtremePoints.Add($"X: {p.X:0.00} | Y: {p.Y:0.00}");

            if (GraphMenuContext.SavedIntegralSettings)
            {
                var area = MathOperations.Integrate(func.Polynom, GraphMenuContext.UpperIntegralBorder,
                    GraphMenuContext.LowerIntegralBorder);

                IntegralInfos.Add($"Fläche unter x: {area:0.00}FE²");
            }

            var antiDerivative = MathOperations.GetAntiDerivative(func.Polynom); //Stammfunktion

            for (var i = GraphMenuContext.GetMinXDouble(); i < GraphMenuContext.GetMaxXDouble(); i += calcAccuracy)
            {
                bluePoints.Add(new Point(i, func.CalculateYForX(i)));
                if(GraphMenuContext.PlotIntegral)
                    redPoints.Add(new Point(i, MathOperations.CalculateYForX(antiDerivative, i)));
                if (SinusChecked)
                    redPoints.Add(new Point(i, CalculateYForSinusX(i)));
                if (CosinusChecked)
                    greenPoints.Add(new Point(i, CalculateYForCoSinusX(i)));
            }

            if (GraphMenuContext.CalculateFirstDerivative && func.FirstDerivative != null)
                for (var i = GraphMenuContext.GetMinXDouble(); i < GraphMenuContext.GetMaxXDouble(); i += calcAccuracy)
                    greenPoints.Add(new Point(i, func.FirstDerivative.CalculateYForX(i)));

            if (GraphMenuContext.CalculateSecondDerivative && func.SecondDerivative != null)
                for (var i = GraphMenuContext.GetMinXDouble(); i < GraphMenuContext.GetMaxXDouble(); i += calcAccuracy)
                    redPoints.Add(new Point(i, func.SecondDerivative.CalculateYForX(i)));

            ToValueTableBtnEnabled = true;
            RedPoints = redPoints;
            GreenPoints = greenPoints;
            BluePoints = bluePoints;
            UpdateView();

            CurrentFunction = func;
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
            OnPropertyChanged("TurningPoints");
            OnPropertyChanged("SaddlePoints");
            OnPropertyChanged("ExtremePoints");
            OnPropertyChanged("IntegralInfos");
            OnPropertyChanged("Functions");
            OnPropertyChanged("LogText");
        }
    }
}
