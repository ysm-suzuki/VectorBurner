using System;
using System.Collections.Generic;

using Atagoal.Core;

using VectorBurnerCalculation;

public class VectorBurner
{   
    public Point GetDestination(
        Body target,
        Point velocity,
        List<Body> barricades)
    {
        var calculatedVelocity = GetVelocity(target, velocity, barricades);

        var velocityLength = (float)System.Math.Sqrt(
            velocity.x * velocity.x +
            velocity.y * velocity.y);

        var calculatedVelocityLength = (float)System.Math.Sqrt(
            calculatedVelocity.x * calculatedVelocity.x +
            calculatedVelocity.y * calculatedVelocity.y);

        if (calculatedVelocityLength == velocityLength)
            return Point.Create(
                target.point.x + calculatedVelocity.x,
                target.point.y + calculatedVelocity.y);

        if (calculatedVelocityLength <= 0)
            return target.point;

        var newTarget = new Body
        {
            point = Point.Create(
                target.point.x + calculatedVelocity.x,
                target.point.y + calculatedVelocity.y),
            boundaryLines = target.boundaryLines
        };

        var newVelocity = Point.Create(
                velocity.x * calculatedVelocityLength / velocityLength,
                velocity.y * calculatedVelocityLength / velocityLength);

        return GetDestination(
            newTarget,
            newVelocity,
            barricades);
    }


    public Point GetDestination(
        Point targetPoint,
        List<Point> targetBoundaryLines,
        Point velocity,
        List<Point> barricadePoints,
        List<List<Point>> barricadeBoundaryLines)
    {
        return GetDestination(
            new Body
            {
                point = targetPoint,
                boundaryLines = targetBoundaryLines
            },
            velocity,
            ConvertBodies(barricadePoints, barricadeBoundaryLines));
    }



    // ------------------------------------------------------

    private Body _target = null;
    private List<Body> _barricades = null;

    public VectorBurner SetTarget(Body target)
    {
        _target = target;

        return this;
    }

    public VectorBurner SetTarget(
        Point targetPoint,
        List<Point> targetBoundaryLines)
    {
        _target = new Body
        {
            point = targetPoint,
            boundaryLines = targetBoundaryLines
        };

        return this;
    }

    public VectorBurner SetBarricades(List<Body> barricades)
    {
        _barricades = barricades;

        return this;
    }
    public VectorBurner SetBarricades(
        List<Point> barricadePoints,
        List<List<Point>> barricadeBoundaryLines)
    {
        _barricades = ConvertBodies(barricadePoints, barricadeBoundaryLines);

        return this;
    }
    public VectorBurner AddBarricade(Body barricade)
    {
        if (_barricades == null)
            _barricades = new List<Body>();
        _barricades.Add(barricade);

        return this;
    }
    public Point GetDestination(Point velocity)
    {
        var destination = GetDestination(_target, velocity, _barricades);
        _target = null;
        _barricades = null;
        return destination;
    }


    // ===============================================================

    private Point GetVelocity(
        Body target,
        Point velocity,
        List<Body> barricades)
    {
        float minDistance = float.MaxValue;

        foreach (var barricade in barricades)
        {
            var temporaryVelocity = target.GetVelocity(velocity, barricade);

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

        var calculatedVelocity = Point.Create(
            unitVelocity.x * minDistance,
            unitVelocity.y * minDistance);

        return calculatedVelocity;
    }


    private List<Body> ConvertBodies(
        List<Point> barricadePoints,
        List<List<Point>> barricadeBoundaryLines)
    {
        var barricades = new List<Body>();
        for (int i = 0; i < barricadePoints.Count; i++)
            barricades.Add(new Body
            {
                point = barricadePoints[i],
                boundaryLines = barricadeBoundaryLines[i]
            });

        return barricades;
    }
}