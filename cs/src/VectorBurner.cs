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
        if (velocity == null
            || velocity.GetPower() == 0)
            return target.point;
        if (barricades == null
            || barricades.Count == 0)
            return target.point + velocity;

        var collision = GetCollision(target, velocity, barricades);

        if (collision.hasNoCollision)
            return target.point + velocity;

        var calculatedVelocity = collision.velocity;
        var calculatedVelocityLength = (float)System.Math.Sqrt(calculatedVelocity.GetPower());

        var velocityLength = (float)System.Math.Sqrt(velocity.GetPower());
        
        return target.point + calculatedVelocity;
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
        var destination = GetDestination(_target, velocity, _barricades);
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
        get { return "0.0.10"; }
    }
}