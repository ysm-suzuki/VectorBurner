using System;
using System.Collections.Generic;

using Atagoal.Core;

namespace VectorBurnerCalculation
{
    public class Math
    {
        // Inner Product
        public static float Inner(Vector vector1, Vector vector2)
        {
            if (vector1 == null
                || vector2 == null)
                return 0;

            return vector1.x * vector2.x + vector2.y * vector1.y;
        }

        // Cross Product
        public static float Cross(Vector vector1, Vector vector2)
        {
            if (vector1 == null
                || vector2 == null)
                return 0;

            return vector1.x * vector2.y - vector2.x * vector1.y;
        }

        public static Point Intersection(LineSegment line1, LineSegment line2)
        {
            Vector vector1 = Vector.Create(line1.from, line1.to);
            Vector vector2 = Vector.Create(line1.from, line2.from);
            Vector vector3 = Vector.Create(line1.from, line2.to);

            Vector vector4 = Vector.Create(line2.from, line2.to);
            Vector vector5 = Vector.Create(line2.from, line1.from);
            Vector vector6 = Vector.Create(line2.from, line1.to);

            double cross1 = Cross(vector1, vector2);
            double cross2 = Cross(vector1, vector3);

            double cross3 = Cross(vector4, vector5);
            double cross4 = Cross(vector4, vector6);

            // has no intersection (out of line segment.)
            if (cross1 * cross2 > 0
                || cross3 * cross4 > 0
            // has no intersection (line segments are parallel.)
                || (cross1 == 0 && cross2 == 0))
                return Point.CreateInvalidPoint();

            double ratio = System.Math.Abs(cross1) / (System.Math.Abs(cross1) + System.Math.Abs(cross2));

            Point intersection = new Point
            {
                x = line2.from.x + vector4.x * (float)ratio,
                y = line2.from.y + vector4.y * (float)ratio
            };

            // has an intersection point.
            if (0 <= ratio && ratio <= 1)
                    return intersection;

            // has no intersection (out of line segment.)
            return Point.CreateInvalidPoint();
        }
        
        public static Vector Rotate(Vector vector, double radian)
        {
            var sin = System.Math.Sin(radian);
            var cos = System.Math.Cos(radian);

            return Vector.Create(
                            (float)(vector.x * cos - vector.y * sin),
                            (float)(vector.x * sin + vector.y * cos));
        }

        // shape: A convex polygon with clockwise rotation only.
        public static bool IsWithIn(Point point, List<LineSegment> shape)
        {
            foreach (LineSegment line in shape)
                if (Cross(Vector.Create(point, line.from), Vector.Create(point, line.to)) <= 0)
                    return false;

            return true;
        }
        // line: A convex polygon with clockwise rotation only.
        public static bool IsInside(Point point, LineSegment line)
        {
            return Cross(Vector.Create(point, line.from), Vector.Create(point, line.to)) > 0;
        }

        public static bool IsOnLine(Point point, LineSegment line)
        {
            var vector1 = Vector.Create(point, line.from);
            var vector2 = Vector.Create(point, line.to);

            if (Cross(vector1.GetUnit(), vector2.GetUnit()) != 0)
                return false;
            
            var linVector = Vector.Create(line.from, line.to);

            return 
                System.Math.Sqrt(vector1.GetPower()) + System.Math.Sqrt(vector2.GetPower()) 
                <=
                System.Math.Sqrt(linVector.GetPower());
        }

        public static Point GetProjectionPoint(Point point, LineSegment line)
        {
            if (IsOnLine(point, line))
                return point;

            var vector1 = Vector.Create(point, line.from);
            var vector2 = Vector.Create(point, line.to);
            var lineVector = Vector.Create(line.from, line.to);
            var crossedLineVector = Vector
                                        .Create(line.from, line.to);
            Rotate(crossedLineVector, (float)System.Math.PI / 2);
            crossedLineVector -= vector1;

            var cross1 = Cross(crossedLineVector, vector1);
            var cross2 = Cross(crossedLineVector, vector2);

            
            if (cross1 == 0 && cross2 == 0)
                Point.CreateInvalidPoint();

            double ratio = System.Math.Sqrt(vector1.GetPower()) 
                / 
                (System.Math.Sqrt(vector1.GetPower()) + System.Math.Sqrt(vector2.GetPower()));

            var projectionPoin = line.from + lineVector * (float)ratio;

            return projectionPoin;
        }
    }
}