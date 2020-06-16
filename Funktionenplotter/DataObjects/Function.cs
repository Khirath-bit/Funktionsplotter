using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Funktionenplotter.Utility;

namespace Funktionenplotter.DataObjects
{
    public class Function
    {
        public Function(string fourthCo, string thirdCo, string secondCo, string firstCo, string b)
        {
            if (!string.IsNullOrWhiteSpace(fourthCo)
                && double.TryParse(fourthCo, out var fourthCoe))
            {
                if(Level == FunctionLevel.None)
                    Level = FunctionLevel.Fourth;
                Polynom.Add(fourthCoe);
            } 

            if (!string.IsNullOrWhiteSpace(thirdCo)
                && double.TryParse(thirdCo, out var thirdCoe))
            {
                if (Level == FunctionLevel.None)
                    Level = FunctionLevel.Third;
                Polynom.Add(thirdCoe);
            } else if (Polynom.Count > 0)
            {
                Polynom.Add(0);
            }

            if (!string.IsNullOrWhiteSpace(secondCo)
                && double.TryParse(secondCo, out var secondCoe))
            {
                if (Level == FunctionLevel.None)
                    Level = FunctionLevel.Second;
                Polynom.Add(secondCoe);
            }
            else if (Polynom.Count > 0)
            {
                Polynom.Add(0);
            }

            if (!string.IsNullOrWhiteSpace(firstCo)
                && double.TryParse(firstCo, out var firstCoe))
            {
                if (Level == FunctionLevel.None)
                    Level = FunctionLevel.First;
                Polynom.Add(firstCoe);
            }
            else if (Polynom.Count > 0)
            {
                Polynom.Add(0);
            }

            if (string.IsNullOrWhiteSpace(b))
                Polynom.Add(0);
            else if (double.TryParse(b, out var yOffset))
            {
                Polynom.Add(yOffset);
            }

            FirstDerivative = new Derivative(Polynom);

            if(Level != FunctionLevel.First)
                SecondDerivative = new Derivative(FirstDerivative.Polynom);

            CalculateZeroPoints();
            CalculateTurningPoints();
        }

        public List<double> Polynom { get; set; } = new List<double>();
        public FunctionLevel Level { get; set; }
        public List<Point> ExtremePoints { get; set; }
        public List<Point> TurningPoints { get; set; }
        public List<Point> SaddlePoints { get; set; }
        public List<Point> ZeroPoints { get; set; } = new List<Point>();
        public Derivative FirstDerivative { get; set; }
        public Derivative SecondDerivative { get; set; }

        private void CalculateTurningPoints()
        {
            TurningPoints = new List<Point>();
            SaddlePoints = new List<Point>();

            if(Level == FunctionLevel.First || Level == FunctionLevel.None || Level == FunctionLevel.Second)
                return;

            var xs = SecondDerivative.CalculateXsForY(0);
            

            foreach (var x in xs)
            {
                if (SecondDerivative.CalculateYForX(x) == 0)
                    SaddlePoints.Add(new Point(x, CalculateYForX(x)));
                else
                    TurningPoints.Add(new Point(x, CalculateYForX(x)));
            }
        }
        private void CalculateZeroPoints()
        {
            switch (Level)
            {
                case FunctionLevel.First:
                    CalculateZeroPointsFirstLevel();
                    break;
                case FunctionLevel.Second:
                    CalculateZeroPointsSecondLevel();
                    break;
                case FunctionLevel.Third:
                    CalculateZeroPointsThirdLevel();
                    break;
                case FunctionLevel.Fourth:
                    CalculateZeroPointsFourthLevel();
                    break;
                default:
                    return;
            }
        }
        private void CalculateZeroPointsFirstLevel()
        {
            ZeroPoints.Add(new Point(0, Polynom.Last()));

            if(Polynom.Last() == 0)
                return;

            var x = -Polynom.Last() / Polynom.First();

            ZeroPoints.Add(new Point(x, 0));
        }
        private void CalculateZeroPointsSecondLevel()
        {
            var sqrt = Math.Sqrt(Math.Pow(Polynom[1], 2) - 4 * Polynom.First() * Polynom.Last());

            if (double.IsNaN(sqrt))
                return;

            var x1 = (-Polynom[1] + sqrt) / (2 * Polynom.First());

            var x2 = (-Polynom[1] - sqrt) / (2 * Polynom.First());

            ZeroPoints.Add(new Point(x1, 0));
            ZeroPoints.Add(new Point(x2, 0));
            ZeroPoints.Add(new Point(0, Polynom.Last())); //x = 0 == b
        }
        private void CalculateZeroPointsThirdLevel()
        {
            var x = 4.0;

            //Newton verfahren
            for (var i = 0; i < 100; i++)
                x -= CalculateYForX(x) / FirstDerivative.CalculateYForX(x);

            //Polynom division
            var func = MathOperations.PolynomialDivision(Polynom,
                new List<double> { 1, -x });

            //Mitternachtsformel
            var zeros = MathOperations.ApplyQuadraticFormula(func);

            if(zeros != null)
                ZeroPoints.AddRange(zeros);

            ZeroPoints.Add(new Point(x, 0));
            ZeroPoints.Add(new Point(0, Polynom.Last()));
        }
        private void CalculateZeroPointsFourthLevel()
        {
            var x = 4.0;

            //Newton verfahren
            for (var i = 0; i < 100; i++)
                x -= CalculateYForX(x) / FirstDerivative.CalculateYForX(x);

            //Polynom division
            var func = MathOperations.PolynomialDivision(Polynom,
                new List<double> { 1, -x });

            var funct = new Function("", func[0].ToString(), func[1].ToString(), func[2].ToString(), func[3].ToString());
            funct.ZeroPoints?.RemoveAt(funct.ZeroPoints.Count-1); //y zero point is wrong for this graph

            if (funct.ZeroPoints != null)
                ZeroPoints.AddRange(funct.ZeroPoints);

            ZeroPoints.Add(new Point(0, Polynom.Last())); //x=0 remains only B
            ZeroPoints.Add(new Point(x, 0));
        }
        public double CalculateYForX(double x)
        {
            var result = 0.0;

            for (var i = 0; i < Polynom.Count - 1; i++)
            {
                result += Polynom[i] * Math.Pow(x, Polynom.Count - 1 - i);
            }

            result += Polynom.Last();

            return result;
        }

        public override string ToString()
        {
            switch (Level)
            {
                case FunctionLevel.None:
                    return "";
                case FunctionLevel.First:
                    return $"f(x) = {Polynom.First()}x + {Polynom.Last()}";
                case FunctionLevel.Second:
                    return $"f(x) = {Polynom.First()}x² + {Polynom[1]}x + {Polynom.Last()}";
                case FunctionLevel.Third:
                    return $"f(x) = {Polynom.First()}x³ + {Polynom[1]}x² + {Polynom[2]}x + {Polynom.Last()}";
                case FunctionLevel.Fourth:
                    return $"f(x) = {Polynom.First()}x{'\u2074'} + {Polynom[1]}x³ + {Polynom[2]}x² + {Polynom[3]}x + {Polynom.Last()}"; //Superscript chars
                default:
                    return "";
            }
        }
    }

    public enum FunctionLevel
    {
        None,
        First,
        Second,
        Third,
        Fourth
    }
}
