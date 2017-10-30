using System;
using System.Collections.Generic;

using Atagoal.Core;

namespace VectorBurnerCalculation
{
    public class Body
    {
        public Point point;
        public List<Point> boundaryLines;

        public Point GetVelocity(Point velocity_, Body target)
        {
            Point velocity = Point.Create
                (
                velocity_.x,
                velocity_.y
                );

            float minDistance = float.MaxValue;

            foreach (var boundaryLinePoint in boundaryLines)
            {
                var originalPoint = Point.Create(
                    point.x + boundaryLinePoint.x,
                    point.y + boundaryLinePoint.y);
                var temporaryVelocity = GetVelocity(originalPoint, velocity, target);

                var temporaryVelocityLength = (float)System.Math.Sqrt(
                    temporaryVelocity.x * temporaryVelocity.x +
                    temporaryVelocity.y * temporaryVelocity.y);

                if (temporaryVelocityLength < minDistance)
                {
                    minDistance = temporaryVelocityLength;
                }
            }

            var velocityLength = (float)System.Math.Sqrt(
                    velocity.x * velocity.x +
                    velocity.y * velocity.y);
            var unitVelocity = Point.Create(
                velocity.x / velocityLength,
                velocity.y / velocityLength);

            return Point.Create(
                unitVelocity.x * minDistance,
                unitVelocity.y * minDistance);
        }

        private Point GetVelocity(Point original, Point velocity, Body target)
        {
            float minDistance = float.MaxValue;

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

                var temporaryVelocity = GetVelocity(
                    original,
                    velocity,
                    lineFrom,
                    lineTo);

                var temporaryVelocityLength = (float)System.Math.Sqrt(
                    temporaryVelocity.x * temporaryVelocity.x + 
                    temporaryVelocity.y * temporaryVelocity.y);

                if (temporaryVelocityLength < minDistance)
                {
                    minDistance = temporaryVelocityLength;
                }
            }

            var velocityLength = (float)System.Math.Sqrt(
                    velocity.x * velocity.x +
                    velocity.y * velocity.y);
            var unitVelocity = Point.Create(
                velocity.x / velocityLength,
                velocity.y / velocityLength);

            return Point.Create(
                unitVelocity.x * minDistance,
                unitVelocity.y * minDistance);
        }

        private Point GetVelocity(
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
                return targetVelocity;

            return Point.Create(
                crossPoint.x - targetPoint.x,
                crossPoint.y - targetPoint.y);
        }
    }

    public class Collision
    {

    }
}