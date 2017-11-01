using System;
using System.Collections.Generic;

using Atagoal.Core;

using VectorBurnerCalculation;

public class VectorBurner
{
    public Point GetDestination(
        Body target,
        Vector velocity,
        List<Body> barricades)
    {
        return GetDestination(target, velocity, barricades, true);
    }

    public Point GetDestination(
        Body target,
        Vector velocity,
        List<Body> barricades,
        bool slip)
    {
        return GetDestination(target, velocity, barricades, slip, 0);
    }

    public Point GetDestination(
        Body target,
        Vector velocity,
        List<Body> barricades,
        bool slip,
        int recursiveCount)
    {
        if (velocity == null
            || velocity.GetPower() == 0)
            return target.point;
        if (barricades == null
            || barricades.Count == 0)
            return target.point + velocity;
        if (recursiveCount >= 8)
        {
            System.Diagnostics.Debug.Assert(false, "GetDestination recurse more than 8 times.");
            return target.point;
        }

        var collision = GetCollision(target, velocity, barricades);

        if (collision.hasNoCollision)
            return target.point + velocity;

        var calculatedVelocity = collision.velocity;
        var calculatedVelocityLength = (float)System.Math.Sqrt(calculatedVelocity.GetPower());

        var velocityLength = (float)System.Math.Sqrt(velocity.GetPower());

        var newPoint = target.point + calculatedVelocity;

        if (!slip)
            return newPoint;
        
        if (VectorBurnerCalculation.Math.Inner(collision.velocity, collision.lineVector) == 0)
            return newPoint;

        var remainingVerocityLength = velocityLength - calculatedVelocityLength;
        if (remainingVerocityLength == 0)
            return newPoint;

        var remainingVerocity = velocity.GetUnit() * remainingVerocityLength;
        var temporaryPoint = newPoint + remainingVerocity;
        var projectionPoint = VectorBurnerCalculation.Math.GetProjectionPoint(temporaryPoint, collision.lineSegment);

        var newBarricades = new List<Body>();
        foreach (var barricade in barricades)
            if (barricade != collision.target)
                newBarricades.Add(barricade);

        return GetDestination(
                new Body
                {
                    point = newPoint,
                    boundaryLines = target.boundaryLines,
                },
                Vector.Create(newPoint, projectionPoint),
                newBarricades,
                slip,
                recursiveCount + 1
            );
    }
    

    // ------------------------------------------------------

    private Body _target = null;
    private List<Body> _barricades = null;

    public VectorBurner SetTarget(Body target)
    {
        _target = target;

        return this;
    }

    public VectorBurner SetBarricades(List<Body> barricades)
    {
        _barricades = barricades;

        return this;
    }
    public VectorBurner AddBarricade(Body barricade)
    {
        if (_barricades == null)
            _barricades = new List<Body>();
        _barricades.Add(barricade);

        return this;
    }
    public Point GetDestination(Vector velocity)
    {
        return GetDestination(velocity, true);
    }
    public Point GetDestination(Vector velocity, bool slip)
    {
        var destination = GetDestination(_target, velocity, _barricades, slip);
        _target = null;
        _barricades = null;
        return destination;
    }


    // ===============================================================

    private Collision GetCollision(
        Body target,
        Vector velocity,
        List<Body> barricades)
    {
        float minDistance = float.MaxValue;
        Collision minDistanceCollision = Collision.Non;

        for (int i = 0; i < barricades.Count; i++)
        {
            var barricade = barricades[i];

            var collision = target.GetCollision(
                velocity,
                barricade);

            if (collision.hasNoCollision
                || barricade.IsVertex(collision.point))
                continue;
            
            var temporaryVelocityLength = (float)System.Math.Sqrt(collision.velocity.GetPower());
            if (temporaryVelocityLength < minDistance)
            {
                minDistance = temporaryVelocityLength;
                minDistanceCollision = collision;
            }
        }

        return minDistanceCollision;
    }

    // ====================================================

    public string Version
    {
        get { return "0.0.8"; }
    }
}