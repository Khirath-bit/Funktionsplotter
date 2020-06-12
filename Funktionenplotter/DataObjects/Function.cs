using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Funktionenplotter.DataObjects
{
    public class Function
    {

        public Function(string fourthCo, string thirdCo, string secondCo, string firstCo, string b)
        {
            if (string.IsNullOrWhiteSpace(b))
                B = 0;
            else if (double.TryParse(b, out var yOffset))
            {
                B = yOffset;
            }

            if (!string.IsNullOrWhiteSpace(firstCo)
                && double.TryParse(firstCo, out var firstCoe))
            {
                Level = FunctionLevel.First;
                FirstCoefficient = firstCoe;
            }

            if (!string.IsNullOrWhiteSpace(secondCo)
                && double.TryParse(secondCo, out var secondCoe))
            {
                Level = FunctionLevel.Second;
                SecondCoefficient = secondCoe;
            }

            if (!string.IsNullOrWhiteSpace(thirdCo)
                && double.TryParse(thirdCo, out var thirdCoe))
            {
                Level = FunctionLevel.Third;
                ThirdCoefficient = thirdCoe;
            }

            if (!string.IsNullOrWhiteSpace(fourthCo)
                && double.TryParse(fourthCo, out var fourthCoe))
            {
                Level = FunctionLevel.Fourth;
                FourthCoefficient = fourthCoe;
            }

            CalculateExtremePoints();
            CalculateZeroPoints();
        }
        /// <summary>
        /// Contains the Function Level
        /// </summary>
        public FunctionLevel Level { get; set; }

        public double FourthCoefficient { get; set; }
        public double ThirdCoefficient { get; set; }
        public double SecondCoefficient { get; set; }
        public double FirstCoefficient { get; set; }
        public double B { get; set; }

        public List<Point> ExtremePoints { get; set; }
        public List<Point> ZeroPoints { get; set; } = new List<Point>();

        private void CalculateZeroPoints()
        {
            switch (Level)
            {
                case FunctionLevel.First:
                    ZeroPoints.Add(new Point(B, 0));
                    break;
                case FunctionLevel.Second:
                    CalculateZeroPointsSecondLevel();
                    break;
                case FunctionLevel.Third:
                    CalculateZeroPointsThirdLevel();
                    break;
                case FunctionLevel.Fourth:
                    break;
                default:
                    return;
            }
        }

        private void CalculateZeroPointsSecondLevel()
        {
            var sqrt = Math.Sqrt(Math.Pow(FirstCoefficient, 2) - 4 * SecondCoefficient * B);

            if (double.IsNaN(sqrt))
                return;

            var x1 = (-FirstCoefficient + sqrt) / (2 * SecondCoefficient);

            var x2 = (-FirstCoefficient - sqrt) / (2 * SecondCoefficient);

            ZeroPoints.Add(new Point(x1, 0));
            ZeroPoints.Add(new Point(x2, 0));
        }

        private void CalculateZeroPointsThirdLevel()
        {
            //Polynomdivision

            ZeroPoints.Add(new Point(x1, B));
            ZeroPoints.Add(new Point(x2, 0));
            ZeroPoints.Add(new Point(x3, 0));
        }

        private void CalculateExtremePoints()
        {
            switch (Level)
            {
                case FunctionLevel.First:
                    return;
                case FunctionLevel.Second:
                    CalculateExtremePointsSecondLevel();
                    break;
                case FunctionLevel.Third:
                    break;
                case FunctionLevel.Fourth:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void CalculateExtremePointsSecondLevel()
        {
            var secondCoefficient = SecondCoefficient * 2;
            var firstCoefficient = FirstCoefficient;// 0 = secondCo * x + firstCo

            var x = -(firstCoefficient / secondCoefficient); // sum = x;
        }

        public double CalculateFirstDerivative(double x)
        {
            switch (Level)
            {
                case FunctionLevel.First:
                    return -1;
                case FunctionLevel.Second:
                    return 2 * SecondCoefficient + FirstCoefficient;
                case FunctionLevel.Third:
                    return 3 * ThirdCoefficient * Math.Pow(x, 2) + 2 * SecondCoefficient * x + FirstCoefficient;
                case FunctionLevel.Fourth:
                    return 4 * FourthCoefficient * Math.Pow(x, 3) + 3 * ThirdCoefficient * Math.Pow(x, 2) +
                           2 * SecondCoefficient * x + FirstCoefficient;
                default:
                    return -1;
            }
        }

        public double CalculateSecondDerivative(double x)
        {
            switch (Level)
            {
                case FunctionLevel.First:
                    return -1;
                case FunctionLevel.Second:
                    return -1;
                case FunctionLevel.Third:
                    return 6 * ThirdCoefficient * x + 2 * SecondCoefficient;
                case FunctionLevel.Fourth:
                    return 12 * FirstCoefficient * Math.Pow(x, 2) + 6 * ThirdCoefficient * x + 2 * SecondCoefficient;
                default:
                    return -1;
            }
        }

        public double CalculateYForX(double x)
        {
            switch (Level)
            {
                case FunctionLevel.First:
                    return CalculateYFirstLevel(x);
                case FunctionLevel.Second:
                    return CalculateYSecondLevel(x);
                case FunctionLevel.Third:
                    return CalculateYThirdLevel(x);
                case FunctionLevel.Fourth:
                    return CalculateYFourthLevel(x);
                default:
                    return default;
            }
        }

        private double CalculateYFourthLevel(double x)
        {
            return FourthCoefficient * Math.Pow(x, 4) + ThirdCoefficient * Math.Pow(x, 3) +
                   SecondCoefficient * Math.Pow(x, 2) + FirstCoefficient * x + B;
        }

        private double CalculateYThirdLevel(double x)
        {
            return ThirdCoefficient * Math.Pow(x, 3) +
                   SecondCoefficient * Math.Pow(x, 2) + FirstCoefficient * x + B;
        }

        private double CalculateYSecondLevel(double x)
        {
            return SecondCoefficient * Math.Pow(x, 2) + FirstCoefficient * x + B;
        }

        private double CalculateYFirstLevel(double x)
        {
            return FirstCoefficient * x + B;
        }
    }

    public enum FunctionLevel
    {
        First,
        Second,
        Third,
        Fourth
    }
}
