using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Media
{
    [Icon(Images.Shape)]
    [Serializable]
    public class Shape : BaseNamable
    {
        #region (enum) CombineMode

        public enum CombineMode
        {
            Complement,
            Replace,
            Xor,
            Intersect,
            Union,
            Exclude,
        }

        #endregion

        #region Properties

        public int Count => Points?.Count ?? 0;

        readonly List<Point> preservedPoints = new();

        [NonSerialized]
        PointCollection points = new();
        public PointCollection Points
        {
            get => points;
            set => this.Change(ref points, value);
        }

        #endregion

        #region Shape

        public Shape() : base()
        {
            Points = new PointCollection();
        }

        public Shape(IEnumerable<Point> input) : this()
            => input?.ForEach(i => Points.Add(i));

        public Shape(IList<System.Drawing.Point> input) : this()
            => input?.ForEach(i => Points.Add(new(i.X, i.Y)));

        public Shape(params Point[] input) : this()
            => input?.ForEach(i => Points.Add(i));

        public Shape(params Vector2<int>[] input) : this()
            => input?.ForEach(i => Points.Add(new(i.X, i.Y)));

        public Shape(PointCollection input) : this()
            => input?.ForEach(i => Points.Add(i));

        public Shape(Shape input) : this() 
            => Points = new(input.Points);

        public Shape(string name) : this() 
            => Name = name;

        public Shape(string name, IEnumerable<Point> input) : this(input)
            => Name = name;

        public Shape(string name, IList<System.Drawing.Point> input) : this(input)
            => Name = name;

        public Shape(string name, PointCollection input) : this(input)
            => Name = name;

        public Shape(string name, Shape input) : this(input)
            => Name = name;

        #endregion

        #region Methods

        #region Public

        public PathGeometry Geometry(bool closed = false)
        {
            var pathFigures = new List<PathFigure>();
            var pathSegments = new List<PathSegment>();

            for (var i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                    break;

                var a = points[i];
                var b = points[i + 1];

                pathSegments.Add(new LineSegment(b, true));
                pathFigures.Add(new PathFigure(a, pathSegments, false));
                pathSegments.Clear();
            }

            return new PathGeometry(pathFigures);
        }

        /// <summary>
        /// Converts points of existing range to points of range [0, 1].
        /// </summary>
        public void Normalize()
        {
            Normalize(points);
            this.Changed(() => Points);
        }

        public void Preserve()
        {
            preservedPoints.Clear();
            points.ForEach(i => preservedPoints.Add(i));
        }

        public void Restore()
        {
            points = new PointCollection();
            preservedPoints.ForEach(i => points.Add(i));
        }

        public void Scale(System.Drawing.Point scale)
        {
            Scale(points, scale);
            this.Changed(() => Points);
        }

        public void Translate(Quadrants quadrant = Quadrants.I)
        {
            var bounds = GetBounds(points);

            var x = bounds.Width / 2;
            var y = bounds.Height / 2;

            Points = Translate(points, x, y);
        }

        #endregion

        #region Static

        public static Int32Region GetBounds(int[] points)
        {
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            Each(points, point =>
            {
                minX = Math.Min(minX, point.X);
                minY = Math.Min(minY, point.Y);

                maxX = Math.Max(maxX, point.X);
                maxY = Math.Max(maxY, point.Y);
            });

            //bottom left = minX, minY
            //bottom right = maxX, minY
            //top left = minX, maxY <------------------ What we want!
            //top right = maxX, maxY
            return new Int32Region(minX, maxY, maxX - minX, maxY - minY);
        }

        public static DoubleRegion GetBounds(IList<Point> points)
        {
            double minX = double.MaxValue, minY = double.MaxValue, maxX = double.MinValue, maxY = double.MinValue;
            points.ForEach(i =>
            {
                minX = Math.Min(minX, i.X);
                minY = Math.Min(minY, i.Y);

                maxX = Math.Max(maxX, i.X);
                maxY = Math.Max(maxY, i.Y);
            });
            return new DoubleRegion(minX, minY, maxX - minX, maxY - minY);
        }

        //...

        static double GetConcaveRadius(uint num_points, int skip)
        {
            // For really small numbers of points.
            if (num_points < 5) return 0.33f;

            // Calculate angles to key points.
            double dtheta = 2 * Math.PI / num_points;
            double theta00 = -Math.PI / 2;
            double theta01 = theta00 + dtheta * skip;
            double theta10 = theta00 + dtheta;
            double theta11 = theta10 - dtheta * skip;

            // Find the key points.
            Point pt00 = new(Math.Cos(theta00), Math.Sin(theta00));
            Point pt01 = new(Math.Cos(theta01), Math.Sin(theta01));
            Point pt10 = new(Math.Cos(theta10), Math.Sin(theta10));
            Point pt11 = new(Math.Cos(theta11), Math.Sin(theta11));

            // See where the segments connecting the points intersect.
            GetIntersection(pt00, pt01, pt10, pt11,
                out bool lines_intersect,
                out bool segments_intersect,
                out Point intersection,
                out Point close_p1,
                out Point close_p2);

            // Calculate the distance between the
            // point of intersection and the center.
            return Math.Sqrt(intersection.X * intersection.X + intersection.Y * intersection.Y);
        }

        /// <summary>
        /// Find the point of intersection between the lines p1 --> p2 and p3 --> p4.
        /// </summary>
        static void GetIntersection(Point p1, Point p2, Point p3, Point p4, out bool lines_intersect, out bool segments_intersect, out Point intersection, out Point close_p1, out Point close_p2)
        {
            // Get the segments' parameters.
            double dx12 = p2.X - p1.X;
            double dy12 = p2.Y - p1.Y;
            double dx34 = p4.X - p3.X;
            double dy34 = p4.Y - p3.Y;

            // Solve for t1 and t2
            double denominator = (dy12 * dx34 - dx12 * dy34);

            double t1 = ((p1.X - p3.X) * dy34 + (p3.Y - p1.Y) * dx34) / denominator;
            if (double.IsInfinity(t1))
            {
                // The lines are parallel (or close enough to it).
                lines_intersect = false;
                segments_intersect = false;
                intersection = new Point(double.NaN, double.NaN);
                close_p1 = new Point(double.NaN, double.NaN);
                close_p2 = new Point(double.NaN, double.NaN);
                return;
            }
            lines_intersect = true;

            double t2 = ((p3.X - p1.X) * dy12 + (p1.Y - p3.Y) * dx12) / -denominator;

            // Find the point of intersection.
            intersection = new Point(p1.X + dx12 * t1, p1.Y + dy12 * t1);

            // The segments intersect if t1 and t2 are between 0 and 1.
            segments_intersect =
                ((t1 >= 0) && (t1 <= 1) &&
                 (t2 >= 0) && (t2 <= 1));

            // Find the closest points on the segments.
            if (t1 < 0)
            {
                t1 = 0;
            }
            else if (t1 > 1)
            {
                t1 = 1;
            }

            if (t2 < 0)
            {
                t2 = 0;
            }
            else if (t2 > 1)
            {
                t2 = 1;
            }

            close_p1 = new Point(p1.X + dx12 * t1, p1.Y + dy12 * t1);
            close_p2 = new Point(p3.X + dx34 * t2, p3.Y + dy34 * t2);
        }

        //...

        public static IEnumerable<Point> Close(IEnumerable<Point> input)
        {
            Point? j = null;
            foreach (var i in input)
            {
                j ??= i;
                yield return i;
            }
            if (j != null)
                yield return j.Value;
        }

        public static bool Contains(IList<Point> path, Point point)
        {
            var c = path.Skip(1).Select((p, i) => (point.Y - path[i].Y) * (p.X - path[i].X) - (point.X - path[i].X) * (p.Y - path[i].Y)).ToList();
            if (c.Any(p => p == 0))
                return true;

            for (int i = 1; i < c.Count(); i++)
            {
                if (c[i] * c[i - 1] < 0)
                    return false;
            }
            return true;
        }

        public static int[] Copy(int[] input)
        {
            var result = new int[input.Length];
            for (var i = 0; i < input.Length; i++)
                result[i] = input[i];

            return result;
        }

        public static void Each(int[] input, Action<System.Drawing.Point> action)
        {
            for (var i = 0; i < input.Length; i += 2)
                action(new System.Drawing.Point(input[i], input[i + 1]));
        }

        public static void Each(int[] input, Func<System.Drawing.Point, System.Drawing.Point> action)
        {
            for (var i = 0; i < input.Length; i += 2)
            {
                var result = action(new System.Drawing.Point(input[i], input[i + 1]));
                input[i] = result.X;
                input[i + 1] = result.Y;
            }
        }

        public static int[] From(System.Drawing.Point[] input)
        {
            var count = input.Length * 2;
            var result = new int[count + 2];

            for (var i = 0; i < input.Length; i++)
            {
                result[i * 2]
                    = input[i].X;
                result[(i * 2) + 1]
                    = input[i].Y;
            }

            result[count] = result[0];
            result[count + 1] = result[1];
            return result;
        }

        public static int[] From(IList<Point> input)
        {
            var count = input.Count * 2;
            var result = new int[count];

            for (var i = 0; i < input.Count; i++)
            {
                result[i * 2]
                    = input[i].X.Int32();
                result[(i * 2) + 1]
                    = input[i].Y.Int32();
            }
            return result;
        }

        /// <summary>
        /// Converts points of existing range to points of range [0, 1].
        /// </summary>
        public static void Normalize(IList<Point> input)
        {
            var bounds = GetBounds(input);

            var xRange = new DoubleRange(bounds.X, bounds.X + bounds.Width);
            var yRange = new DoubleRange(bounds.Y, bounds.Y + bounds.Height);

            for (var i = 0; i < input.Count; i++)
                input[i] = new Point(xRange.Convert(0, 1, input[i].X), yRange.Convert(0, 1, input[i].Y));
        }

        public static void Reflect(ref double x, ref double y, int quadrant)
        {
            switch (quadrant)
            {
                case 0:
                    y = -y;
                    break;
                case 1:
                    break;
                case 2:
                    x = -x;
                    break;
                case 3:
                    x = -x;
                    y = -y;
                    break;
            }
        }

        public static IEnumerable<Point> Round(IList<Point> points)
        {
            for (var i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                {
                    yield return points[i];
                    break;
                }

                var a = points[i].Int32().Double();
                var b = points[i + 1].Int32().Double();

                if (a == b)
                    continue;

                var c = b - a;
                var d = a;

                var ax = c.X.Absolute();
                var ay = c.Y.Absolute();

                var ix = (c.X / ax);
                var iy = (c.Y / ay);

                var cx = ax;
                var cy = ay;

                yield return d;

                if (ax == ay)
                {
                    while (d != b)
                    {
                        if (cx > 0)
                        {
                            d = new Point(d.X + ix, d.Y);
                            yield return d;
                            cx--;
                        }
                        if (cy > 0)
                        {
                            d = new Point(d.X, d.Y + iy);
                            yield return d;
                            cy--;
                        }
                    }
                }
                else if (ax > ay)
                {
                    //For every n ax, do ay
                    while (cy > 0)
                    {
                        cx = ax / ay;
                        while (cx > 0)
                        {
                            d = new Point(d.X + ix, d.Y);
                            yield return d;
                            cx--;
                        }
                        d = new Point(d.X, d.Y + iy);
                        yield return d;
                        cy--;
                    }
                }
                else if (ax < ay)
                {
                    //For every n ay, do ax
                    while (cx > 0)
                    {
                        cy = ay / ax;
                        while (cy > 0)
                        {
                            d = new Point(d.X, d.Y + iy);
                            yield return d;
                            cy--;
                        }
                        d = new Point(d.X + ix, d.Y);
                        yield return d;
                        cx--;
                    }
                }
            }
        }

        public static void Scale(int[] input, System.Drawing.Point scale)
        {
            var bounds = Shape.GetBounds(input);
            Each(input, point => new System.Drawing.Point((point.X * scale.X / bounds.Width), (point.Y * scale.Y / bounds.Height)));
        }

        public static void Scale(IList<Point> input, System.Drawing.Point scale)
        {
            for (var i = 0; i < input.Count; i++)
                input[i] = new Point(input[i].X * scale.X, input[i].Y * scale.Y);
        }

        //...

        /// <summary>
        /// Moves all points in the direction of the origin until each lives in quadrant I.
        /// </summary>
        public static void Translate(int[] points, Quadrants quadrant = Quadrants.I)
        {
            var bounds = GetBounds(points);

            var x = (bounds.Width.Double() / 2.0).Int32();
            var y = (bounds.Height.Double() / 2.0).Int32();

            Translate(points, new System.Drawing.Point(x, y));
        }

        public static void Translate(int[] input, System.Drawing.Point newCenter)
        {
            var bounds = GetBounds(input);
            var oldCenter = bounds.Center;

            var x = (newCenter.X - oldCenter.X);
            var y = (newCenter.Y - oldCenter.Y);

            Each(input, point => new System.Drawing.Point(point.X + x, point.Y + y));
        }

        public static PointCollection Translate(IList<Point> input, double xCenter, double yCenter)
        {
            var result = new PointCollection();

            var bounds = GetBounds(input);
            var oldCenter = bounds.Center;

            var x = xCenter - oldCenter.X;
            var y = yCenter - oldCenter.Y;

            for (var i = 0; i < input.Count; i++)
                result.Add(new Point(input[i].X + x, input[i].Y + y));

            return result;
        }

        //...

        public static IEnumerable<Point> GetEllipse(Int32Region region, bool close = false)
        {
            var ellipse = new EllipseGeometry()
            {
                Center = new Point(region.Width / 2.0, region.Height / 2.0),
                RadiusX = region.Width / 2.0,
                RadiusY = region.Height / 2.0
            };

            var points = new List<Point>();
            var segments = ellipse.GetFlattenedPathGeometry(0.01, ToleranceType.Absolute).Figures[0].Segments;
            foreach (var segment in segments)
            {
                if (segment is PolyLineSegment)
                {
                    foreach (var p in (segment as PolyLineSegment)?.Points ?? Enumerable.Empty<Point>())
                        points.Add(new Point((p.X + region.X).Round(), (p.Y + region.Y).Round()));
                }
            }

            Point? first = null;
            foreach (var i in Round(points))
            {
                first ??= i;
                yield return i;
            }

            if (close)
                yield return first.Value;
        }

        //...

        public static IEnumerable<Point> GetPolygon(Int32Region region, double angle, uint sides, int quadrant)
        {
            var centerX = region.X + (region.Width / 2.0);
            var centerY = region.Y + (region.Height / 2.0);

            var center = new Point(centerX, centerY);
            var size = new Size(region.Width.Coerce(int.MaxValue), region.Height.Coerce(int.MaxValue));

            var oldPoints
                = GetPolygon(angle.GetRadian(), center, size, sides, quadrant);

            for (var i = 0; i < oldPoints.Length; i++)
                yield return new Point(oldPoints[i].X, oldPoints[i].Y);
        }

        public static System.Drawing.Point[] GetPolygon(double startAngle, Point center, Size size, uint sides, int quadrant)
        {
            sides.Coerce(32, 3);

            var a = 2.0 * Math.PI / sides.Double();
            var b = startAngle;
            double x = 0, y = 0;

            var points = new System.Drawing.Point[sides + 1];
            for (int i = 0; i < sides; i++)
            {
                x = size.Width * Math.Cos(b);
                y = size.Height * Math.Sin(b);

                Reflect(ref x, ref y, quadrant);

                x += center.X;
                y += center.Y;

                points[i] = new System.Drawing.Point(x.Round().Int32(), y.Round().Int32());
                b += a;
            }

            points[sides] = points[0];
            return points;
        }

        //...

        public static IEnumerable<Point> GetStar(Int32Region region, double angle, uint sides, int indent, int quadrant)
        {
            var centerX = region.X + (region.Width / 2.0);
            var centerY = region.Y + (region.Height / 2.0);

            var center = new Point(centerX, centerY);
            var size = new Size(region.Width.Coerce(int.MaxValue), region.Height.Coerce(int.MaxValue));

            var oldPoints
                = GetStar(angle.GetRadian(), sides, indent, new Rect(center.X, center.Y, size.Width, size.Height), quadrant);

            for (var i = 0; i < oldPoints.Length; i++)
                yield return new Point(oldPoints[i].X, oldPoints[i].Y);
        }

        public static System.Drawing.Point[] GetStar(double startAngle, uint sides, int skip, Rect rect, int quadrant)
        {
            sides.Coerce(32, 2);

            double theta, dtheta;
            System.Drawing.Point[] result;

            double rx = rect.Width.Single() / 2f;
            double ry = rect.Height.Single() / 2f;
            double cx = rect.X.Single() + rx;
            double cy = rect.Y.Single() + ry;

            // If this is a polygon, don't bother with concave points.
            if (skip == 1)
            {
                result = new System.Drawing.Point[sides];
                theta = startAngle;
                dtheta = 2 * Math.PI / sides;
                for (int i = 0; i < sides; i++)
                {
                    var x = rx * Math.Cos(theta);
                    var y = ry * Math.Sin(theta);

                    Reflect(ref x, ref y, quadrant);

                    result[i] = new System.Drawing.Point(x.Int32() + cx.Int32(), y.Int32() + cy.Int32());
                    theta += dtheta;
                }
                return result;
            }

            // Find the radius for the concave vertices.
            double concave_radius = GetConcaveRadius(sides, skip);

            // Make the points.
            result = new System.Drawing.Point[2 * sides + 1];
            theta = startAngle;
            dtheta = Math.PI / sides;
            for (int i = 0; i < sides; i++)
            {
                var x = (cx + rx * Math.Cos(theta)).Int32();
                var y = (cy + ry * Math.Sin(theta)).Int32();
                result[2 * i] = new System.Drawing.Point(x, y);
                theta += dtheta;

                x = (cx + rx * Math.Cos(theta) * concave_radius).Int32();
                y = (cy + ry * Math.Sin(theta) * concave_radius).Int32();
                result[2 * i + 1] = new System.Drawing.Point(x, y);
                theta += dtheta;
            }

            result[2 * sides] = result[0];
            return result;
        }

        //...

        public static IEnumerable<Point> GetRectangle(Int32Region region, bool close = false)
        {
            yield return new Point(region.X, region.Y);
            yield return new Point(region.X, region.Y + region.Height);
            yield return new Point(region.X + region.Width, region.Y + region.Height);
            yield return new Point(region.X + region.Width, region.Y);
            if (close) yield return new Point(region.X, region.Y);
        }

        public static IEnumerable<Point> GetRoundedRectangle(Int32Region region, double xRadius, double yRadius, bool close = false)
        {
            var rectangle = new RectangleGeometry()
            {
                Rect = new Rect(region.X, region.Y, region.Width, region.Height),
                RadiusX = xRadius,
                RadiusY = yRadius
            };

            var points = new List<Point>();
            var segments = rectangle.GetFlattenedPathGeometry(0.01, ToleranceType.Absolute).Figures[0].Segments;

            foreach (var segment in segments)
            {
                if (segment is PolyLineSegment)
                {
                    foreach (var p in (segment as PolyLineSegment)?.Points ?? Enumerable.Empty<Point>())
                        points.Add(new Point(p.X.Round(), p.Y.Round()));
                }
            }

            var finalPoints = Shape.Round(points);

            Point? firstPoint = null;
            foreach (var i in finalPoints)
            {
                firstPoint ??= i;
                yield return i;
            }

            if (close && firstPoint != null)
                yield return firstPoint.Value;
        }

        #endregion

        #region Legacy

        /*
        public static GraphicsPath ClipPath(GraphicsPath subjectPath, CombineMode combineMode, GraphicsPath clipPath)
        {
            GpcWrapper.Polygon.Validate(combineMode);

            GpcWrapper.Polygon basePoly = new GpcWrapper.Polygon(subjectPath);

            GraphicsPath clipClone = (GraphicsPath)clipPath.Clone();
            clipClone.CloseAllFigures();
            GpcWrapper.Polygon clipPoly = new GpcWrapper.Polygon(clipClone);
            clipClone.Dispose();

            GpcWrapper.Polygon clippedPoly = GpcWrapper.Polygon.Clip(combineMode, basePoly, clipPoly);

            GraphicsPath returnPath = clippedPoly.ToGraphicsPath();
            returnPath.CloseAllFigures();
            return returnPath;
        }

        public static PointSelection Combine(PointSelection subjectPath, CombineMode combineMode, PointSelection clipPath)
        {
            switch (combineMode)
            {
                case CombineMode.Complement:
                    return Combine(clipPath, CombineMode.Exclude, subjectPath);

                case CombineMode.Replace:
                    return clipPath.Clone();

                case CombineMode.Xor:
                case CombineMode.Intersect:
                case CombineMode.Union:
                case CombineMode.Exclude:
                    if (subjectPath.Count == 0 && clipPath.Count == 0)
                    {
                        return new PointSelection(); // empty path
                    }
                    else if (subjectPath.IsEmpty)
                    {
                        switch (combineMode)
                        {
                            case CombineMode.Xor:
                            case CombineMode.Union:
                                return clipPath.Clone();

                            case CombineMode.Intersect:
                            case CombineMode.Exclude:
                                return new PointSelection();

                            default:
                                throw new InvalidEnumArgumentException();
                        }
                    }
                    else if (clipPath.IsEmpty)
                    {
                        switch (combineMode)
                        {
                            case CombineMode.Exclude:
                            case CombineMode.Xor:
                            case CombineMode.Union:
                                return subjectPath.Clone();

                            case CombineMode.Intersect:
                                return new PointSelection();

                            default:
                                throw new InvalidEnumArgumentException();
                        }
                    }
                    else
                    {
                        GraphicsPath resultPath = ClipPath(subjectPath, combineMode, clipPath);
                        return new PointSelection(resultPath);
                    }

                default:
                    throw new InvalidEnumArgumentException();
            }
        }
        */

        #endregion

        #endregion
    }
}