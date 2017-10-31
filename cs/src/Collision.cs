using System;
using System.Collections.Generic;

using Atagoal.Core;

namespace VectorBurnerCalculation
{
    public class Body
    {
        public Point point;
        public List<Point> boundaryLines;

        public Collision Collision(Point velocity, Body target)
        {
            float minDistance = float.MaxValue;
            Collision minDistanceCollision = null;

            foreach (var boundaryLinePoint in boundaryLines)
            {
                var originalPoint = Point.Create(
                    point.x + boundaryLinePoint.x,
                    point.y + boundaryLinePoint.y);
                
                var collision = GetCollision(originalPoint, velocity, target);

                if (collision == null)
                    continue;

                var temporaryVelocity = collision.velocity;

                var temporaryVelocityLength = (float)System.Math.Sqrt(
                    temporaryVelocity.x * temporaryVelocity.x +
                    temporaryVelocity.y * temporaryVelocity.y);

                if (temporaryVelocityLength < minDistance)
                {
                    minDistance = temporaryVelocityLength;
                    minDistanceCollision = collision;
                }
            }
            
            return minDistanceCollision;
        }

        private Collision GetCollision(Point original, Point velocity, Body target)
        {
            var velocityLength = (float)System.Math.Sqrt(
                velocity.x * velocity.x +
                velocity.y * velocity.y);

            float minDistance = float.MaxValue;
            Collision minDistanceCollision = null;

            int boundaryLinesCount = target.boundaryLines.Count;
            for (int i = 0; i < boundaryLinesCount; i++)
            {
                var boundaryLineFrom = target.boundaryLines[(i) % boundaryLinesCount];
                var boundaryLineTo = target.boundaryLines[(i + 1) % boundaryLinesCount];
                var lineFrom = Point.Create(
                    target.point.x + boundaryLineFrom.x,
                    target.point.x + boundaryLineFrom.y);
                var lineTo = Point.Create(
                    target.point.x + boundaryLineTo.x,
                    target.point.x + boundaryLineTo.y);

                var collision = GetCollision(
                    original,
                    velocity,
                    lineFrom,
                    lineTo);

                if (collision == null)
                    continue;

                var temporaryVelocity = collision.velocity;

                var temporaryVelocityLength = (float)System.Math.Sqrt(
                    temporaryVelocity.x * temporaryVelocity.x + 
                    temporaryVelocity.y * temporaryVelocity.y);

                if (temporaryVelocityLength < minDistance)
                {
                    minDistance = temporaryVelocityLength;
                    minDistanceCollision = collision;
                }
            }

            return minDistanceCollision;
        }

        private Collision GetCollision(
            Point targetPoint,
            Point targetVelocity,
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

            var velocityLength = (float)System.Math.Sqrt(
                targetVelocity.x * targetVelocity.x +
                targetVelocity.y * targetVelocity.y);

            var temporaryVelocity = Point.Create(
                    crossPoint.x - targetPoint.x,
                    crossPoint.y - targetPoint.y);
            var temporaryVelocityLength = (float)System.Math.Sqrt(
                temporaryVelocity.x * temporaryVelocity.x +
                temporaryVelocity.y * temporaryVelocity.y);
            var normalizedTemporaryVelocity = Point.Create(
                    temporaryVelocity.x / temporaryVelocityLength,
                    temporaryVelocity.y / temporaryVelocityLength);

            var collisionVelocity = velocityLength > temporaryVelocityLength
                ? temporaryVelocityLength
                : velocityLength;


            return new Collision
            {
                point = crossPoint,
                velocity = Point.Create(
                    normalizedTemporaryVelocity.x * collisionVelocity,
                    normalizedTemporaryVelocity.y * collisionVelocity),
                lineSegment = LineSegment.Create(boundaryLineFrom, boundaryLineTo)
            };
        }
    }

    public class Collision
    {
        public Point point;
        public Point velocity;
        public LineSegment lineSegment;
    }
}