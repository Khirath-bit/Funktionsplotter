using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Funktionenplotter.Utility
{
    public static class MathOperations
    {
        /// <summary>
        /// Provides division on second pol up to 2 sequences
        /// </summary>
        /// <param name="pol1"></param>
        /// <param name="pol2"></param>
        /// <returns></returns>
        public static List<double> PolynomialDivision(List<double> pol1, List<double> pol2)
        {
            var result = new List<double>();

            result.Add(pol1[0] / pol2[0]);

            var recentStep = new List<double>
            {
                result.Last() * pol2[0],
                result.Last() * pol2[1]
            };

            for (var i = 1; i < pol1.Count - 1; i++)
            {
                var currentStep = new List<double>
                {
                    pol1[i] - recentStep[1],
                    pol1[i+1]
                };

                result.Add(currentStep[0] / pol2[0]);

                recentStep = new List<double>
                {
                    result[i] * pol2[0],
                    result[i] * pol2[1]
                };
            }

            return result;
        }

        /// <summary>
        /// Uses pq formula to calculate two null points
        /// </summary>
        /// <param name="polynomial"></param>
        /// <returns>zeropoints or null if invalid</returns>
        public static List<Point> ApplyQuadraticFormula(List<double> polynomial)
        {
            if (polynomial.Count > 3)
                return null;

            var sqrt = Math.Sqrt(Math.Pow(polynomial[1], 2) - 4 * polynomial[0] * polynomial[2]);

            if (double.IsNaN(sqrt))
                return null;

            var result = new List<Point>
            {
                new Point((-polynomial[1] + sqrt) / (2 * polynomial[0]), 0),
                new Point((-polynomial[1] - sqrt) / (2 * polynomial[0]), 0)
            };

            return result;
        }
    }
}
