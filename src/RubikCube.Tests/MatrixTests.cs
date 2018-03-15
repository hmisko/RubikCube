namespace RubikCube.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class MatrixTests
    {
        [TestCase(-1, -1, -1, 1)]
        [TestCase(-1, 1, 1, 1)]
        [TestCase(1, 1, 1, -1)]
        [TestCase(1, -1, -1, -1)]
        public void RotateClockwise(int x, int y, int expectedX, int expectedY)
        {
            var result = Matrix.Rotate((x, y), true);
            Assert.That(result.x, Is.EqualTo(expectedX));
            Assert.That(result.y, Is.EqualTo(expectedY));
        }

        [TestCase(-1, -1, 1, -1)]
        [TestCase(1, -1, 1, 1)]
        [TestCase(1, 1, -1, 1)]
        [TestCase(-1, 1, -1, -1)]
        public void RotateCounterClockwise(int x, int y, int expectedX, int expectedY)
        {
            var result = Matrix.Rotate((x, y), false);
            Assert.That(result.x, Is.EqualTo(expectedX));
            Assert.That(result.y, Is.EqualTo(expectedY));
        }

        [Test]
        public void RotateArray()
        {
            // arrange
            var input = new[,]
                        {
                            { 1, 2, 3 },
                            { 4, 5, 6 },
                            { 7, 8, 9 }
                        };

            var expected = new[,]
                           {
                               { 7, 4, 1 },
                               { 8, 5, 2 },
                               { 9, 6, 3 }
                           };

            // act
            var result = Matrix.RotateArray(input, true);

            // assert
            for (var x = 0; x < expected.GetLength(0); x++)
            {
                for (var y = 0; y < expected.GetLength(1); y++)
                {
                    Assert.That(result[x, y], Is.EqualTo(expected[x, y]));
                }
            }
        }
    }

    public class Matrix
    {
        public static (int x, int y) Rotate((int x, int y) point, bool isClockwise)
        {
            var result = point;

            var angle = Math.PI * (isClockwise ? -0.5 : 0.5);
            result.x = (int)Math.Round(point.x * Math.Cos(angle) - point.y * Math.Sin(angle));
            result.y = (int)Math.Round(point.x * Math.Sin(angle) + point.y * Math.Cos(angle));

            return result;
        }

        public static T[,] RotateArray<T>(T[,] input, bool isClockwise)
        {
            var result = new T[input.GetLength(0), input.GetLength(1)];

            for (var x = 0; x < input.GetLength(0); x++)
            {
                for (var y = 0; y < input.GetLength(1); y++)
                {
                    var newPoint = Rotate((x - 1, y - 1), isClockwise);
                    result[newPoint.x + 1, newPoint.y + 1] = input[x, y];
                }
            }

            return result;
        }
    }
}