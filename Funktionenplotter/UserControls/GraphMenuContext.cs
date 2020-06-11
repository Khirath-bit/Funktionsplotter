using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Funktionenplotter.Utility;

namespace Funktionenplotter.UserControls
{
    public class GraphMenuContext 
    {
        /// <summary>
        /// Contains the plotter method
        /// </summary>
        private readonly Action<object> _plotterFunction;

        public string XMin { get; set; }

        public string XMax { get; set; }

        public string YMax { get; set; }

        public string YMin { get; set; }

        public string CalculationAccuracy { get; set; } = "1";

        public GraphMenuContext(Action<object> plotterFunction)
        {
            _plotterFunction = plotterFunction;
        }

        /// <summary>
        /// The plotter command delegation
        /// </summary>
        public ICommand PlotterCommand => new DelegateCommand(ExecutePlotterCommand);

        /// <summary>
        /// Executes the plotter command
        /// </summary>
        /// <param name="param"></param>
        private void ExecutePlotterCommand(object param)
        {
            _plotterFunction?.Invoke(param);
        }

        /// <summary>
        /// Gets the new transform according to x and y min and max
        /// </summary>
        /// <param name="height">Actual height of the canvas</param>
        /// <param name="width">Actual width of the canvas</param>
        /// <returns>Render transform</returns>
        public Transform GetRenderTransform(double height, double width)
        {

            if (!int.TryParse(XMin, out var xMin) ||
                !int.TryParse(YMin, out var yMin) ||
                !int.TryParse(XMax, out var xMax) ||
                !int.TryParse(YMax, out var yMax))
                return default;

            var yTotalLength = Math.Abs(yMin) + yMax;
            var xTotalLength = Math.Abs(xMin) + xMax; //Das funktioniert immer, wenn zb xmin = -10 und xmax = -5 dann xtotal = 5

            var scalingY = Math.Max(height / yTotalLength, 0.1);
            var scalingX = Math.Max(width / xTotalLength, 0.1);

            var xOffsetForY = -(xMin + xMax) * scalingX;

            var yOffsetForX = -(yMin + yMax) * scalingY;

            var transform = $"{scalingX} 0 0 -{scalingY} {xOffsetForY} {yOffsetForX}";

            return Transform.Parse(transform);
        }

        public double GetMinXDouble() => double.TryParse(XMin, out var xMin) ? xMin : default;

        public double GetMaxXDouble() => double.TryParse(XMax, out var xMax) ? xMax : default;

    }
}
