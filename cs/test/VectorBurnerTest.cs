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

            foreach(var testCase in _testCases)
            {
                var vectorBurner = new VectorBurner();

                var target = testCase.target;
                var velocity = testCase.velocity;
                var barricades = testCase.barricades;
                var expected = testCase.expected;
                
                var result = vectorBurner
                    .SetTarget(target)
                    .SetBarricades(barricades)
                    .GetDestination(velocity, testCase.slip);

                if (expected.x == result.x
                    && expected.y == result.y)
                    Console.WriteLine("the test passed at " + testCase.title);
                else
                {
                    Console.WriteLine("the test failed at " + testCase.title);
                    Console.WriteLine("expected.x : " + expected.x);
                    Console.WriteLine("expected.y : " + expected.y);
                    Console.WriteLine("but");
                    Console.WriteLine("result.x : " + result.x);
                    Console.WriteLine("result.y : " + result.y);
                }
            }
        }


        class TestCase
        {
            public string title;

            public Body target;
            public Point velocity;
            public List<Body> barricades;
            public bool slip = true;

            public Point expected;
        }

        private List<TestCase> _testCases = new List<TestCase>
        {
            new TestCase
            {
                title = "Collide with (0, 20) velocity",
                target = new Body
                {
                    point = Point.Create(3, 3),
                    boundaryLines = new List<Point>
                        {
                            Point.Create(-1, 1),
                            Point.Create(1, 1),
                            Point.Create(1, -1),
                            Point.Create(-1, -1)
                        }
                },
                velocity = Point.Create(0, 20),
                barricades = new List<Body>
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
                },
                expected = Point.Create(3, 8)
            },
            // ---------------------------------------
            new TestCase
            {
                title = "Don not collide with (0.165677f, 0) velocity",
                target = new Body
                {
                    point = Point.Create(0, 0),
                    boundaryLines = new List<Point>
                        {
                            Point.Create(-10, 10),
                            Point.Create(10, 10),
                            Point.Create(10, -10),
                            Point.Create(-10, -10)
                        }
                },
                velocity = Point.Create(0.165677f, 0),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(60, 0),
                        boundaryLines = new List<Point>
                        {
                            Point.Create(-10, 50),
                            Point.Create(10, 50),
                            Point.Create(10, -50),
                            Point.Create(-10, -50),
                        }
                    }
                },
                expected = Point.Create(0.165677f, 0)
            },
            // ---------------------------------------
            new TestCase
            {
                title = "Collide after collided",
                target = new Body
                {
                    point = Point.Create(40, 0),
                    boundaryLines = new List<Point>
                        {
                            Point.Create(-10, 10),
                            Point.Create(10, 10),
                            Point.Create(10, -10),
                            Point.Create(-10, -10)
                        }
                },
                velocity = Point.Create(165.677f, 0),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(60, 0),
                        boundaryLines = new List<Point>
                        {
                            Point.Create(-10, 50),
                            Point.Create(10, 50),
                            Point.Create(10, -50),
                            Point.Create(-10, -50),
                        }
                    }
                },
                expected = Point.Create(40, 0)
            },
            // ---------------------------------------
            new TestCase
            {
                title = "Non-slipped collision with Angled boundaries.",
                target = new Body
                {
                    point = Point.Create(0, 0),
                    boundaryLines = new List<Point>
                        {
                            Point.Create(-10, 10),
                            Point.Create(10, 10),
                            Point.Create(10, -10),
                            Point.Create(-10, -10)
                        }
                },
                velocity = Point.Create(165.677f, 0),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(60, 0),
                        boundaryLines = new List<Point>
                        {
                            Point.Create(-10, 50),
                            Point.Create(0, -50),
                            Point.Create(10, -50),
                            Point.Create(0, 50),
                        }
                    }
                },
                slip = false,
                expected = Point.Create(44, 0)
            },
        };
    }
}