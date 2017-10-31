using System;
using System.Collections.Generic;

using Atagoal.Core;

namespace VectorBurnerCalculation
{
    public class Body
    {
        public Point point;
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


        public Collision Collision(Vector velocity, Body target)
        {
            float minDistance = float.MaxValue;
            Collision minDistanceCollision = null;

            foreach (var vertex in vertices)
            {
                var originalPoint = Point.Create(
                    point.x + vertex.x,
                    point.y + vertex.y);
                
                var collision = GetCollision(originalPoint, velocity, target);

                if (collision == null)
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
            var velocityLength = (float)System.Math.Sqrt(
                velocity.x * velocity.x +
                velocity.y * velocity.y);

            float minDistance = float.MaxValue;
            Collision minDistanceCollision = null;

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

                var collision = GetCollision(
                    original,
                    velocity,
                    lineFrom,
                    lineTo);

                if (collision == null)
                    continue;
                
                var collisionVelocityLength = (float)System.Math.Sqrt(collision.velocity.GetPower());
                if (collisionVelocityLength < minDistance)
                {
                    minDistance = collisionVelocityLength;
                    minDistanceCollision = collision;
                }
                
                // check if original + velocity is with in inner target boundary.
                var nextPoint = Point.Create(
                minDistanceCollision.point.x + minDistanceCollision.velocity.x,
                minDistanceCollision.point.y + minDistanceCollision.velocity.y);
                
                if (VectorBurnerCalculation.Math.IsWithIn(nextPoint, target.boundaryLines))
                {
                    return new Collision
                    {
                        point = minDistanceCollision.point,
                        velocity = Vector.Create(0, 0),
                        lineSegment = minDistanceCollision.lineSegment
                    };
                }
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
                return null;

            var velocityLength = (float)System.Math.Sqrt(targetVelocity.GetPower());
            var temporaryVelocity = Vector.Create(targetPoint, crossPoint);
            var temporaryVelocityLength = (float)System.Math.Sqrt(temporaryVelocity.GetPower());
            if (temporaryVelocityLength == 0)
                return new Collision
                {
                    point = crossPoint,
                    velocity = Vector.Create(0, 0),
                    lineSegment = LineSegment.Create(boundaryLineFrom, boundaryLineTo)
                };

            var normalizedTemporaryVelocity = temporaryVelocity.GetUnit();

            var collisionVelocityLength = velocityLength > temporaryVelocityLength
                ? temporaryVelocityLength
                : velocityLength;

            return new Collision
            {
                point = crossPoint,
                velocity = normalizedTemporaryVelocity * collisionVelocityLength,
                lineSegment = LineSegment.Create(boundaryLineFrom, boundaryLineTo)
            };
        }
    }

    public class Collision
    {
        public Point point;
        public Vector velocity;
        public LineSegment lineSegment;
    }
}