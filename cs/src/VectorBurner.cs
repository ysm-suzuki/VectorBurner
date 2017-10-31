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
        if (velocity.GetPower() == 0)
            return target.point;


        var velocityLength = (float)System.Math.Sqrt(velocity.GetPower());

        var collision = GetCollision(target, velocity, barricades);
        Vector calculatedVelocity;

        if (collision == null)
            calculatedVelocity = velocity;
        else
            calculatedVelocity = collision.velocity;

        var calculatedVelocityLength = (float)System.Math.Sqrt(calculatedVelocity.GetPower());
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

        var newVelocity = Vector.Create(0, 0);
        
        if (slip)
        {
            newVelocity = VectorBurnerCalculation.Math.GetLineVector(
                            calculatedVelocity,
                            collision.lineSegment)
                            * (velocityLength - calculatedVelocityLength);
        }

        return GetDestination(
            newTarget,
            newVelocity,
            barricades);
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
        Collision minDistanceCollision = null;

        for (int i = 0; i < barricades.Count; i++)
        {
            var barricade = barricades[i];

            var collision = target.Collision(
                velocity,
                barricade);

            if (collision == null)
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
        get { return "0.0.7"; }
    }
}