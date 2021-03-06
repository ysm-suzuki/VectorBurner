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
                Console.WriteLine("------- " + testCase.title + " ---------");

                var vectorBurner = new VectorBurner();

                var target = testCase.target;
                var velocity = testCase.velocity;
                var barricades = testCase.barricades;
                var bounce = testCase.bounce;
                var expected = testCase.expected;
                
                var result = vectorBurner
                    .SetTarget(target)
                    .SetBarricades(barricades)
                    .SetBounce(bounce)
                    .GetDestination(velocity);

                if (expected.FuzzyEquals(result, 0.01f))
                    Console.WriteLine("test passed");
                else
                {
                    Console.WriteLine("test failed");
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
            public Vector velocity;
            public List<Body> barricades;
            public float bounce;

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
                    vertices = new List<Point>
                        {
                            Point.Create(-1, 1),
                            Point.Create(1, 1),
                            Point.Create(1, -1),
                            Point.Create(-1, -1)
                        }
                },
                velocity = Vector.Create(0, 20),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(10, 10),
                        vertices = new List<Point>
                        {
                            Point.Create(-20, 1),
                            Point.Create(20, 1),
                            Point.Create(20, -1),
                            Point.Create(-20, -1)
                        }
                    }
                },
                bounce = 0,
                expected = Point.Create(3, 8)
            },
            // ---------------------------------------
            new TestCase
            {
                title = "Do not collide with (0.165677f, 0) velocity",
                target = new Body
                {
                    point = Point.Create(0, 0),
                    vertices = new List<Point>
                        {
                            Point.Create(-10, 10),
                            Point.Create(10, 10),
                            Point.Create(10, -10),
                            Point.Create(-10, -10)
                        }
                },
                velocity = Vector.Create(0.165677f, 0),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(60, 0),
                        vertices = new List<Point>
                        {
                            Point.Create(-10, 50),
                            Point.Create(10, 50),
                            Point.Create(10, -50),
                            Point.Create(-10, -50),
                        }
                    }
                },
                bounce = 0,
                expected = Point.Create(0.165677f, 0)
            },
            // ---------------------------------------
            new TestCase
            {
                title = "Collide after collided",
                target = new Body
                {
                    point = Point.Create(40, 0),
                    vertices = new List<Point>
                        {
                            Point.Create(-10, 10),
                            Point.Create(10, 10),
                            Point.Create(10, -10),
                            Point.Create(-10, -10)
                        }
                },
                velocity = Vector.Create(165.677f, 0),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(60, 0),
                        vertices = new List<Point>
                        {
                            Point.Create(-10, 50),
                            Point.Create(10, 50),
                            Point.Create(10, -50),
                            Point.Create(-10, -50),
                        }
                    }
                },
                bounce = 0,
                expected = Point.Create(40, 0)
            },
            // ---------------------------------------
            new TestCase
            {
                title = "Non-slipped collision with Angled boundaries.",
                target = new Body
                {
                    point = Point.Create(0, 0),
                    vertices = new List<Point>
                        {
                            Point.Create(-10, 10),
                            Point.Create(10, 10),
                            Point.Create(10, -10),
                            Point.Create(-10, -10)
                        }
                },
                velocity = Vector.Create(165.677f, 0),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(60, 0),
                        vertices = new List<Point>
                        {
                            Point.Create(-10, 50),
                            Point.Create(0, 50),
                            Point.Create(10, -50),
                            Point.Create(0, -50),
                        }
                    }
                },
                bounce = 0,
                expected = Point.Create(44, 0)
            },
            // ---------------------------------------
            new TestCase
            {
                title = "circle test 1.",
                target = new Body
                {
                    point = Point.Create(0, 0),
                    radius = 10.0f
                },
                velocity = Vector.Create(5.0f, 0),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(23, 0),
                        radius = 10.0f
                    }
                },
                bounce = 0,
                expected = Point.Create(3, 0)
            },
            // ---------------------------------------
            new TestCase
            {
                title = "circle test 2.",
                target = new Body
                {
                    point = Point.Create(0, 0),
                    radius = 10.0f
                },
                velocity = Vector.Create(5.0f, 0),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(23, 0),
                        radius = 10.0f
                    }
                },
                bounce = 1,
                expected = Point.Create(2, 0)
            },
            // ---------------------------------------
            new TestCase
            {
                title = "circle test 3.",
                target = new Body
                {
                    point = Point.Create(0, 0),
                    radius = 10.0f
                },
                velocity = Vector.Create(5.0f, 0),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(50, 0),
                        radius = 10.0f
                    }
                },
                bounce = 1,
                expected = Point.Create(5, 0)
            },
            // ---------------------------------------
            new TestCase
            {
                title = "circle test 4.",
                target = new Body
                {
                    point = Point.Create(0, 10.0f),
                    radius = 10.0f
                },
                velocity = Vector.Create(20.0f, 0),
                barricades = new List<Body>
                {
                    new Body
                    {
                        point = Point.Create(20, 0),
                        radius = 10.0f
                    }
                },
                bounce = 0,
                expected = Point.Create(10, 10)
            },
        };
    }
}