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

        /// <summary>
        /// Integrates a polynom
        /// </summary>
        /// <returns></returns>
        public static double Integrate(List<double> upperPolynomial, double upperBorder, double lowerBorder)
        {
            var antiDerivative = GetAntiDerivative(upperPolynomial);

            return Math.Abs(CalculateYForX(antiDerivative, upperBorder) - CalculateYForX(antiDerivative, lowerBorder)); //An area is always absolute
        }

        /// <summary>
        /// Calculates the anti derivative (Stammfunktion)
        /// </summary>
        /// <returns>anti derivative</returns>
        public static List<double> GetAntiDerivative(List<double> polynom)
        {
            var antiDerivative = new List<double>();

            for (var i = 0; i < polynom.Count; i++)
            {
                var asdd = 1.0 / (polynom.Count - i);
                var asd = polynom[i] * asdd;

                antiDerivative.Add(asd);
            }

            antiDerivative.Add(0); //b hinzufügen damit nach der logik auch funktion n+1 grades ist

            return antiDerivative;
        }

        /// <summary>
        /// Calculates y for x with any given function
        /// </summary>
        /// <param name="polynom">Any given polynomial function</param>
        /// <param name="x">x</param>
        /// <returns></returns>
        public static double CalculateYForX(List<double> polynom, double x)
        {
            var result = 0.0;

            for (var i = 0; i < polynom.Count - 1; i++)
            {
                result += polynom[i] * Math.Pow(x, polynom.Count - 1 - i);
            }

            result += polynom.Last();

            return result;
        }
    }
}
