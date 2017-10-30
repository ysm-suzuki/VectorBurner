using System;
using System.Collections.Generic;

using Atagoal.Core;

namespace UnitTest
{
    public class VectorBurnerTest
    {
        public static void Main(string[] args)
        {
            new VectorBurnerTest().Run();
        }

        public void Run()
        {
            var vectorBurner = new VectorBurner();

            var target = new VectorBurner.Body
            {
                point = Point.Create(0, 0),
                boundaryLines = new List<Point>
                    {
                        Point.Create(-1, 1),
                        Point.Create(1, 1),
                        Point.Create(1, -1),
                        Point.Create(-1, -1)
                    }
            };

            var velocity = Point.Create(0, 0);

            var barricades = new List<VectorBurner.Body>
            {
                new VectorBurner.Body
                {
                    point = Point.Create(10, 10),
                    boundaryLines = new List<Point>
                    {
                        Point.Create(-1, 1),
                        Point.Create(1, 1),
                        Point.Create(1, -1),
                        Point.Create(-1, -1)
                    }
                }
            };

            vectorBurner.GetVelocity(
                target,
                velocity,
                barricades);

            vectorBurner
                .SetTarget(target)
                .SetBarricades(barricades)
                .GetVelocity(velocity);
        }
    }
}