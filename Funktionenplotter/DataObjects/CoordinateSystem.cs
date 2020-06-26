using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Funktionenplotter.UserControls;

namespace Funktionenplotter.DataObjects
{
    public class CoordinateSystem : Canvas
    {
        private bool _autoClear;

        private Polyline _selectedPolyline;

        private List<Line> Axis { get; set; }

        public CoordinateSystem(bool autoClear = false)
        {
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            _autoClear = autoClear;

            Polylines = new List<Polyline>();
            Axis = new List<Line>
            {
                new Line {Y1 = 0, Y2 = 0, X2 = 800, X1 = -800, Stroke = Brushes.White, Name = "X", StrokeThickness = 1, Tag = Funktionenplotter.Axis.X},
                new Line {X1 = 0, X2 = 0, Y2 = 800, Y1 = -800, Stroke = Brushes.White, Name = "Y", StrokeThickness = 1, Tag = Funktionenplotter.Axis.Y}
            };
        }

        public List<Polyline> Polylines { get; }

        private void CreateAxisLabels(GraphMenuContext context, Transform renderTransform)
        {
            var yLineWidth = (context.GetMaxXDouble() - context.GetMinXDouble()) * 0.02;
            var xLineWidth = (context.GetMaxYDouble() - context.GetMinYDouble()) * 0.02;
            for (var i = context.GetMinXDouble(); i < context.GetMaxXDouble(); i += context.GetLineMarginAxis())
            {
                var label = new Line
                {
                    X1 = i,
                    Y1 = xLineWidth / 2,
                    X2 = i,
                    Y2 = -xLineWidth / 2,
                    StrokeThickness = Helper.CalculateStrokeThicknessByTransform(renderTransform, Funktionenplotter.Axis.Line) * context.GetLineThickness(),
                    Stroke = Brushes.White
                };

                Children.Add(label);
            }

            for (var i = context.GetMinYDouble(); i < context.GetMaxYDouble(); i += context.GetLineMarginAxis())
            {
                var label = new Line
                {
                    X1 = yLineWidth / 2,
                    Y1 = i,
                    X2 = -yLineWidth / 2,
                    Y2 = i,
                    StrokeThickness = Helper.CalculateStrokeThicknessByTransform(renderTransform, Funktionenplotter.Axis.Line) * context.GetLineThickness(),
                    Stroke = Brushes.White
                };

                Children.Add(label);
            }
        }

        public void Plot(Transform renderTransform, GraphMenuContext context)
        {
            if (_autoClear)
                ClearChildren();

            CreateAxisLabels(context, renderTransform);

            RenderTransform = renderTransform;

            foreach (var ax in Axis) //Draw x and y axis
            {
                ax.StrokeThickness = Helper.CalculateStrokeThicknessByTransform(renderTransform, (Axis)ax.Tag);
                Children.Add(ax);
            }

            foreach (var child in Polylines.Where(x => x.Points?.Any() == true)) //Draw custom objects
            {
                child.StrokeThickness =
                    Helper.CalculateStrokeThicknessByTransform(renderTransform, Funktionenplotter.Axis.Line);
                Children.Add(child);
            }

            Polylines.First().StrokeThickness *= 3;

            UpdateLayout();
        }

        public Polyline GetSelectedPolyline()
        {
            return _selectedPolyline ?? Polylines?.First();
        }

        public void ClearPolylines()
        {
            Polylines.Clear();
        }

        public void AddPolyline(PointCollection points, Brush color)
        {
            var line = new Polyline { Points = points, Stroke = color };
            line.MouseLeftButtonDown += (sender, args) =>
            {
                _selectedPolyline = line;

                foreach (var p in Polylines)
                    p.StrokeThickness = Helper.CalculateStrokeThicknessByTransform(RenderTransform, Funktionenplotter.Axis.Line);

                line.StrokeThickness *= 3;
            };

            Polylines.Add(line);
        }

        public void AddMark(Line lineOne, Line lineTwo)
        {
            RemoveChildrenByTag("cross");

            lineOne.StrokeThickness = lineTwo.StrokeThickness =
                Helper.CalculateStrokeThicknessByTransform(RenderTransform, Funktionenplotter.Axis.Line) * 3;

            Children.Add(lineOne);
            Children.Add(lineTwo);
        }

        public void AddPolygon(Polygon polygon)
        {
            RemoveChildrenByTag("polygon");

            polygon.StrokeThickness =
                Helper.CalculateStrokeThicknessByTransform(RenderTransform, Funktionenplotter.Axis.Line);

            Children.Add(polygon);
        }

        public void RemoveChildrenByTag(string tag)
        {
            var childsToRemove = new List<UIElement>();

            foreach (var child in Children)
            {
                if (child is Line line && ReferenceEquals(line.Tag, tag))
                    childsToRemove.Add(line);
            }

            foreach (var child in childsToRemove)
            {
                Children.Remove(child);
            }
        }

        public void ClearChildren()
        {
            Children.Clear();
        }
    }
}
