namespace RubikCube.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class ArrayBasedCubeBuilderTests
    {
        [Test]
        public void BuildAllFaces()
        {
            // arrange
            var input = new[,]
                        {
                            { "1", "2", "3" },
                            { "4", "5", "6" },
                            { "7", "8", "9" }
                        };

            var builder = new ArrayBasedCubeBuilder();
            builder.SetFrontFace(input);
            builder.SetBackFace(input);
            builder.SetTopFace(input);
            builder.SetBottomFace(input);
            builder.SetLeftFace(input);
            builder.SetRightFace(input);

            // act
            var cube = builder.Build();

            // assert
            var faces = new[]
                        {
                            cube.FrontFace, cube.BackFace,
                            cube.TopFace, cube.BottomFace,
                            cube.LeftFace, cube.RightFace
                        };

            foreach (var face in faces)
            {
                Assert.That(face[0, 0].Color, Is.EqualTo("1"));
                Assert.That(face[0, 1].Color, Is.EqualTo("2"));
                Assert.That(face[0, 2].Color, Is.EqualTo("3"));
                Assert.That(face[1, 0].Color, Is.EqualTo("4"));
                Assert.That(face[1, 1].Color, Is.EqualTo("5"));
                Assert.That(face[1, 2].Color, Is.EqualTo("6"));
                Assert.That(face[2, 0].Color, Is.EqualTo("7"));
                Assert.That(face[2, 1].Color, Is.EqualTo("8"));
                Assert.That(face[2, 2].Color, Is.EqualTo("9"));
            }
        }
    }


    public class ArrayBasedCubeBuilder
    {
        private readonly Sticker[,] frontFace = new Sticker[3, 3];
        private readonly Sticker[,] backFace = new Sticker[3, 3];
        private readonly Sticker[,] topFace = new Sticker[3, 3];
        private readonly Sticker[,] bottomFace = new Sticker[3, 3];
        private readonly Sticker[,] leftFace = new Sticker[3, 3];
        private readonly Sticker[,] rightFace = new Sticker[3, 3];

        public ArrayBasedCubeBuilder SetFrontFace(string[,] input)
        {
            SetFace(input, this.frontFace);
            return this;
        }

        public ArrayBasedCubeBuilder SetBackFace(string[,] input)
        {
            SetFace(input, this.backFace);
            return this;
        }

        public ArrayBasedCubeBuilder SetTopFace(string[,] input)
        {
            SetFace(input, this.topFace);
            return this;
        }

        public ArrayBasedCubeBuilder SetBottomFace(string[,] input)
        {
            SetFace(input, this.bottomFace);
            return this;
        }

        public ArrayBasedCubeBuilder SetLeftFace(string[,] input)
        {
            SetFace(input, this.leftFace);
            return this;
        }

        public ArrayBasedCubeBuilder SetRightFace(string[,] input)
        {
            SetFace(input, this.rightFace);
            return this;
        }

        private static void SetFace(string[,] input, Sticker[,] face)
        {
            for (var i = 0; i < input.GetLength(0); i++)
            {
                for (var j = 0; j < input.GetLength(1); j++)
                {
                    face[i, j] = new Sticker(input[i, j]);
                }
            }
        }

        public Cube Build()
        {
            return new Cube(
                this.frontFace,
                this.backFace,
                this.topFace,
                this.bottomFace,
                this.leftFace,
                this.rightFace);
        }

    }
}