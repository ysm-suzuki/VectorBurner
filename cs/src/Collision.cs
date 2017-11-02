using System;
using System.Collections.Generic;

using Atagoal.Core;

namespace VectorBurnerCalculation
{
    public class Body
    {
        public Point point;

        public float radius = -1;

        public bool isCircle
        {
            get { return radius > 0; }
        }

        public List<Point> vertices
        {
            get
            {
                var points = new List<Point>();
                for (int i = 0; i < _boundaryLines.Count; i++)
                    points.Add(_boundaryLines[i].from);
                return points;
            }
            set
            {
                _boundaryLines.Clear();
                for (int i = 0; i < value.Count; i++)
                {
                    var from = value[(i) % value.Count];
                    var to = value[(i + 1) % value.Count];
                    _boundaryLines.Add(LineSegment.Create(from, to));
                }
            }
        }
        public List<LineSegment> boundaryLines
        {
            get { return _boundaryLines; }
            set
            {
                _boundaryLines = value;
            }
        }

        private List<LineSegment> _boundaryLines = new List<LineSegment>();


        public Collision GetCollision(Vector velocity, Body target)
        {
            if (isCircle)
                return GetCollision(
                            point + velocity.GetUnit() * radius,
                            velocity,
                            target);
                    
            float minDistance = float.MaxValue;
            Collision minDistanceCollision = Collision.Non;
            
            foreach (var vertex in vertices)
            {
                var originalPoint = Point.Create(
                    point.x + vertex.x,
                    point.y + vertex.y);


                var collision = GetCollision(originalPoint, velocity, target);

                if (collision.hasNoCollision)
                    continue;
                if (target.IsVertex(collision.point))
                    continue;

                var collisionVelocityLength = (float)System.Math.Sqrt(collision.velocity.GetPower());
                if (collisionVelocityLength < minDistance)
                {
                    minDistance = collisionVelocityLength;
                    minDistanceCollision = collision;
                }
            }

            return minDistanceCollision;
        }

        private Collision GetCollision(Point original, Vector velocity, Body target)
        {
            if (target.isCircle)
            {
                var line = LineSegment.Create(original, original + velocity);
                Point boundaryPoint;

                if (VectorBurnerCalculation.Math.IsOnLine(target.point, line))
                {
                    boundaryPoint = target.point + velocity.GetUnit() * (float)(-1.0f * radius);
                    return new Collision
                    {
                        point = boundaryPoint,
                        velocity = Vector.Create(original, boundaryPoint),
                        target = target,
                    };
                }
                else
                {
                    var orthogonalPoint = VectorBurnerCalculation.Math
                                            .GetProjectionPoint(target.point, line);
                    var orthogonalPointVector = Vector.Create(target.point, orthogonalPoint);

                    if ((float)System.Math.Sqrt(orthogonalPointVector.GetPower()) > target.radius)
                        return Collision.Non;


                    var orthogonalPointVector2 = Vector.Create(original, orthogonalPoint);
                    var boundaryPointVectorLength =
                        System.Math.Sqrt(orthogonalPointVector2.GetPower()) -
                        System.Math.Sqrt(target.radius * target.radius - orthogonalPointVector.GetPower());

                    boundaryPoint = original + velocity.GetUnit() * (float)boundaryPointVectorLength;
                }

                return new Collision
                {
                    point = boundaryPoint,
                    velocity = Vector.Create(original, boundaryPoint),
                    target = target,
                };
            }

            float minDistance = float.MaxValue;
            Collision minDistanceCollision = Collision.Non;

            int boundaryLinesCount = target.boundaryLines.Count;
            for (int i = 0; i < boundaryLinesCount; i++)
            {
                var boundaryLine = target.boundaryLines[i];
                var lineFrom = Point.Create(
                    target.point.x + boundaryLine.from.x,
                    target.point.y + boundaryLine.from.y);
                var lineTo = Point.Create(
                    target.point.x + boundaryLine.to.x,
                    target.point.y + boundaryLine.to.y);

                var nextPoint = original + velocity;

                if (VectorBurnerCalculation.Math.IsOnLineSegment(original, boundaryLine)
                    && !VectorBurnerCalculation.Math.IsWithIn(nextPoint, target.boundaryLines))
                    continue;
                
                var collision = GetCollision(
                    original,
                    velocity,
                    lineFrom,
                    lineTo);

                if (collision.hasNoCollision)
                    continue;
                
                var collisionVelocityLength = (float)System.Math.Sqrt(collision.velocity.GetPower());
                if (collisionVelocityLength >= minDistance)
                    continue;

                minDistance = collisionVelocityLength;
                minDistanceCollision = collision;
                minDistanceCollision.target = target;
            }

            return minDistanceCollision;
        }

        private Collision GetCollision(
            Point targetPoint,
            Vector targetVelocity,
            Point boundaryLineFrom,
            Point boundaryLineTo)
        {
            var destination = Point.Create(
                targetPoint.x + targetVelocity.x,
                targetPoint.y + targetVelocity.y);

            var crossPoint = VectorBurnerCalculation.Math.Intersection(
                LineSegment.Create(targetPoint, destination),
                LineSegment.Create(boundaryLineFrom, boundaryLineTo));

            if (crossPoint.IsInvalidPoint())
                return Collision.Non;


            var temporaryVelocity = Vector.Create(targetPoint, crossPoint);
            var temporaryVelocityLength = (float)System.Math.Sqrt(temporaryVelocity.GetPower());
            
            var velocityLength = (float)System.Math.Sqrt(targetVelocity.GetPower());

            var normalizedTemporaryVelocity = temporaryVelocity.GetUnit();

            var collisionVelocityLength = velocityLength > temporaryVelocityLength
                ? temporaryVelocityLength
                : velocityLength;

            return new Collision
            {
                point = crossPoint,
                velocity = normalizedTemporaryVelocity * collisionVelocityLength,
                lineSegment = LineSegment.Create(boundaryLineFrom, boundaryLineTo),
            };
        }


        public bool IsVertex(Point target)
        {
            foreach (var vertex in vertices)
                if (Point.Create(
                    point.x + vertex.x,
                    point.y + vertex.y)
                    .FuzzyEquals(target, 0.001f))
                    return true;

            return false;
        }
    }

    public class Collision
    {
        public Point point = null;
        public Vector velocity = null;

        // has value when collides.
        public LineSegment lineSegment = null;
        public Body target = null;

        public bool hasNoCollision = false;

        public static Collision Non
        {
            get
            {
                return new Collision
                {
                    hasNoCollision = true
                };
            }
        }

        public Vector lineVector
        {
            get
            {
                return lineSegment != null
                    ? Vector.Create(lineSegment.from, lineSegment.to)
                    : Vector.Create(0, 0);
            }
        }
    }
}