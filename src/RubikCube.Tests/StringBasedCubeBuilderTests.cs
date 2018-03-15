namespace RubikCube.Tests
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using NUnit.Framework;

    [TestFixture]
    public class StringBasedCubeBuilderTests
    {
        [Test]
        public void BuildAllFaces()
        {
            // arrange
            var input =
                @"1 2 3
                  4 5 6
                  7 8 9";
            var builder = new StringBasedCubeBuilder();
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
                Assert.That(face[1, 0].Color, Is.EqualTo("2"));
                Assert.That(face[2, 0].Color, Is.EqualTo("3"));
                Assert.That(face[0, 1].Color, Is.EqualTo("4"));
                Assert.That(face[1, 1].Color, Is.EqualTo("5"));
                Assert.That(face[2, 1].Color, Is.EqualTo("6"));
                Assert.That(face[0, 2].Color, Is.EqualTo("7"));
                Assert.That(face[1, 2].Color, Is.EqualTo("8"));
                Assert.That(face[2, 2].Color, Is.EqualTo("9"));
            }
        }
    }

    public class StringBasedCubeBuilder
    {
        private readonly Sticker[,] frontFace = new Sticker[3, 3];
        private readonly Sticker[,] backFace = new Sticker[3, 3];
        private readonly Sticker[,] topFace = new Sticker[3, 3];
        private readonly Sticker[,] bottomFace = new Sticker[3, 3];
        private readonly Sticker[,] leftFace = new Sticker[3, 3];
        private readonly Sticker[,] rightFace = new Sticker[3, 3];

        public StringBasedCubeBuilder SetFrontFace(string input)
        {
            SetFace(input, this.frontFace);
            return this;
        }

        public StringBasedCubeBuilder SetBackFace(string input)
        {
            SetFace(input, this.backFace);
            return this;
        }

        public StringBasedCubeBuilder SetTopFace(string input)
        {
            SetFace(input, this.topFace);
            return this;
        }

        public StringBasedCubeBuilder SetBottomFace(string input)
        {
            SetFace(input, this.bottomFace);
            return this;
        }

        public StringBasedCubeBuilder SetLeftFace(string input)
        {
            SetFace(input, this.leftFace);
            return this;
        }

        public StringBasedCubeBuilder SetRightFace(string input)
        {
            SetFace(input, this.rightFace);
            return this;
        }

        private static void SetFace(string input, Sticker[,] face)
        {
            var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (lines.Length != 3)
            {
                throw new ArgumentException("Input text should contains exactly 3 lines.");
            }

            for (var i = 0; i < lines.Length; i++)
            {
                var stickers = Regex.Split(lines[i], @"\s{1,}").Where(x => x.Length > 0).ToArray();
                if (stickers.Length != 3)
                {
                    throw new ArgumentException("Every line in input text should contains exactly 3 stickers.");
                }

                for (var j = 0; j < stickers.Length; j++)
                {
                    face[j, i] = Sticker.FromColor(stickers[j]);
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