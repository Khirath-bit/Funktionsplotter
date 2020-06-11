using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            switch (axis)
            {
                case Axis.X:
                    return 1 / transform.Value.M11;//M11 = x
                case Axis.Y:
                    return 1 / Math.Abs(transform.Value.M22); //M22 = y
                case Axis.Line:
                    return (1 / transform.Value.M11 + 1 / transform.Value.M11) / 2;
                default:
                    return 1;
            }
        }
    }

    public enum Axis
    {
        X,
        Y,
        Line
    }
}
