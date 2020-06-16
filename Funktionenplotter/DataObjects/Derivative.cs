using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funktionenplotter.Utility;

namespace Funktionenplotter.DataObjects
{
    public class Derivative
    {
        public Derivative(List<double> polynom)
        {
            var derivative = new List<double>();

            for (var i = 0; i < polynom.Count - 1; i++)
            {
                derivative.Add(polynom[i] * (polynom.Count - 1 - i));
            }

            Polynom = derivative;
        }

        public List<double> Polynom { get; set; }

        public double CalculateYForX(double x)
        {
            var result = 0.0;

            for (var i = 0; i < Polynom.Count-1; i++)
            {
                result += Polynom[i] * Math.Pow(x, Polynom.Count-1-i);
            }

            result += Polynom.Last();

            return result;
        }

        public List<double> CalculateXsForY(double y)
        {
            if (Polynom.Count == 2)
            {
                y -= Polynom.Last(); // -b

                y /= Polynom.First(); // divide coefficient of x

                return new List<double> { y };
            }

            var data = MathOperations.ApplyQuadraticFormula(Polynom);

            return data == null ? new List<double>() : data.Select(x => x.X).ToList();
        }

        public override string ToString()
        {
            switch (Polynom.Count)
            {
                case 1:
                    return $"f(x) = {Polynom.First()}";
                case 2:
                    return $"f(x) = {Polynom.First()}x + {Polynom.Last()}";
                case 3:
                    return $"f(x) = {Polynom.First()}x² + {Polynom[1]}x + {Polynom.Last()}";
                case 4:
                    return $"f(x) = {Polynom.First()}x³ + {Polynom[1]}x² + {Polynom[2]}x + {Polynom.Last()}"; 
                default:
                    return "";
            }
        }
    }
}
