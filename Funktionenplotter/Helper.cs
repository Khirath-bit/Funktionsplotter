using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Funktionenplotter
{
    public static class Helper
    {
        /// <summary>
        /// Calculates the stroke thickness for a specific transform
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public static double CalculateStrokeThicknessByTransform(Transform transform, Axis axis)
        {
            var x = Math.Abs(transform.Value.M11);
            var y = Math.Abs(transform.Value.M22);

            switch (axis)
            {
                case Axis.X:
                    return x > y ? Math.Min(1 / y, 1 / x ) * (x / y) : Math.Min(1 / y, 1 / x);
                case Axis.Y:
                    return y > x ? Math.Min(1 / y, 1 / x) * (y / x) : Math.Min(1 / y, 1 / x);
                case Axis.Line:
                    return (1 / transform.Value.M11 + 1 / transform.Value.M11) / 2;
                default:
                    return 1;
            }
        }

        public static double ParseX(string coordinates)
        {
            var x = coordinates.Split(' ')[1];

            return !double.TryParse(x, out var xVal) ? default : xVal;
        }

        public static double ParseY(string coordinates)
        {
            var y = coordinates.Split(' ')[4];

            return !double.TryParse(y, out var yVal) ? default : yVal;
        }
    }

    public enum Axis
    {
        X,
        Y,
        Line
    }
}
