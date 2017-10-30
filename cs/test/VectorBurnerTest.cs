using System;
using System.Collections.Generic;

using Atagoal.Core;

using VectorBurnerCalculation;

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

            var target = new Body
            {
                point = Point.Create(3, 3),
                boundaryLines = new List<Point>
                    {
                        Point.Create(-1, 1),
                        Point.Create(1, 1),
                        Point.Create(1, -1),
                        Point.Create(-1, -1)
                    }
            };

            var velocity = Point.Create(0, 20);

            var barricades = new List<Body>
            {
                new Body
                {
                    point = Point.Create(10, 10),
                    boundaryLines = new List<Point>
                    {
                        Point.Create(-20, 1),
                        Point.Create(20, 1),
                        Point.Create(20, -1),
                        Point.Create(-20, -1)
                    }
                }
            };

            var expected = Point.Create(3, 9);

            var result1 = vectorBurner.GetDestination(
                target,
                velocity,
                barricades);

            var result2 = vectorBurner
                .SetTarget(target)
                .SetBarricades(barricades)
                .GetDestination(velocity);

            if (expected.x == result1.x
                && expected.y == result1.y)
                Console.WriteLine("the test passed at result1");
            else
                Console.WriteLine("the test failed at result1");

            if (expected.x == result2.x
                && expected.y == result2.y)
                Console.WriteLine("the test passed at result2");
            else
                Console.WriteLine("the test failed at result2");
        }
    }
}