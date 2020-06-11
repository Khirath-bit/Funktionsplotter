using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
