using System;
using System.Collections.Generic;

using Atagoal.Core;

public class VectorBurner
{
    public class Body
    {
        public Point point;
        public List<Point> boundaryLines;
    }

    private Body _target = null;
    private List<Body> _barricades = null;



    // ===============================================================

    public Point GetVelocity(
        Body target,
        Point velocity,
        List<Body> barricades)
    {
        return Point.Create(0, 0);
    }

    public Point GetVelocity(
        Point targetPoint,
        List<Point> targetBoundaryLines,
        Point velocity,
        List<Point> barricadePoints,
        List<List<Point>> barricadeBoundaryLines)
    {
        return GetVelocity(
            new Body
            {
                point = targetPoint,
                boundaryLines = targetBoundaryLines
            },
            velocity,
            ConvertBodies(barricadePoints, barricadeBoundaryLines));
    }



    // ===============================================================

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
    public Point GetVelocity(Point velocity)
    {
        var calculatedVelocity = GetVelocity(_target, velocity, _barricades);
        _target = null;
        _barricades = null;
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