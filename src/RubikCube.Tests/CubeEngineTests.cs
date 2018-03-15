namespace RubikCube.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using NUnit.Framework;

    [TestFixture]
    public class CubeEngineTests
    {
        [Test]
        public void CubeEngineShouldMapCubeToMatrix()
        {
            // arrange 
            var cube = BuildeSampleCube();
            var cubeEngine = new TestableCubeEngine();

            // act
            var matrixOfStickers = cubeEngine.TestMapCubeTo3DMatrix_NEW(cube);

            // assert
            void AssertStickerPoints(params (int x, int y, int z, string value)[] points)
            {
                foreach (var point in points)
                {
                    Assert.That(matrixOfStickers.Contains(new Point<Sticker>(point.x, point.y, point.z, Sticker.FromColor(point.value))),
                                $"Sticker \"{point.value}\" ({point.x},{point.y},{point.z}) not found.");
                }
            }

            AssertStickerPoints(
                (1, 1, 0, "f1"), (2, 1, 0, "f2"), (3, 1, 0, "f3"), (1, 2, 0, "f4"), (2, 2, 0, "f5"), (3, 2, 0, "f6"), (1, 3, 0, "f7"), (2, 3, 0, "f8"), (3, 3, 0, "f9"),
                (1, 1, 4, "b1"), (2, 1, 4, "b2"), (3, 1, 4, "b3"), (1, 2, 4, "b4"), (2, 2, 4, "b5"), (3, 2, 4, "b6"), (1, 3, 4, "b7"), (2, 3, 4, "b8"), (3, 3, 4, "b9"),
                (0, 1, 3, "l1"), (0, 1, 2, "l2"), (0, 1, 1, "l3"), (0, 2, 3, "l4"), (0, 2, 2, "l5"), (0, 2, 1, "l6"), (0, 3, 3, "l7"), (0, 3, 2, "l8"), (0, 3, 1, "l9"),
                (4, 1, 3, "r1"), (4, 1, 2, "r2"), (4, 1, 1, "r3"), (4, 2, 3, "r4"), (4, 2, 2, "r5"), (4, 2, 1, "r6"), (4, 3, 3, "r7"), (4, 3, 2, "r8"), (4, 3, 1, "r9"),
                (1, 4, 1, "t1"), (2, 4, 1, "t2"), (3, 4, 1, "t3"), (1, 4, 2, "t4"), (2, 4, 2, "t5"), (3, 4, 2, "t6"), (1, 4, 3, "t7"), (2, 4, 3, "t8"), (3, 4, 3, "t9"),
                (1, 0, 1, "d1"), (2, 0, 1, "d2"), (3, 0, 1, "d3"), (1, 0, 2, "d4"), (2, 0, 2, "d5"), (3, 0, 2, "d6"), (1, 0, 3, "d7"), (2, 0, 3, "d8"), (3, 0, 3, "d9"));
        }

        [Test]
        public void CubeEngineShouldGetCubeFromMatrix()
        {
            // arrange 
            var cubeEngine = new TestableCubeEngine();
            var matrixOfStickers = cubeEngine.TestMapCubeTo3DMatrix_NEW(BuildeSampleCube());

            // act
            var cube = cubeEngine.TestGetCubeFrom3DMatrix_NEW(matrixOfStickers);

            // assert
            void AssertFace(Sticker[,] face, params (int x, int y, string value)[] points)
            {
                foreach (var point in points)
                {
                    Assert.True(face[point.x, point.y] == Sticker.FromColor(point.value),
                                $"FrontFace[{point.x},{point.y}] should have color \"{point.value}\" instead of \"{face[point.x, point.y].Color}\"");
                }
            }

            AssertFace(cube.FrontFace, (0, 0, "f1"), (1, 0, "f2"), (2, 0, "f3"), (0, 1, "f4"), (1, 1, "f5"), (2, 1, "f6"), (0, 2, "f7"), (1, 2, "f8"), (2, 2, "f9"));
            AssertFace(cube.BackFace, (0, 0, "b1"), (1, 0, "b2"), (2, 0, "b3"), (0, 1, "b4"), (1, 1, "b5"), (2, 1, "b6"), (0, 2, "b7"), (1, 2, "b8"), (2, 2, "b9"));
            AssertFace(cube.TopFace, (0, 0, "t1"), (1, 0, "t2"), (2, 0, "t3"), (0, 1, "t4"), (1, 1, "t5"), (2, 1, "t6"), (0, 2, "t7"), (1, 2, "t8"), (2, 2, "t9"));
            AssertFace(cube.BottomFace, (0, 0, "d1"), (1, 0, "d2"), (2, 0, "d3"), (0, 1, "d4"), (1, 1, "d5"), (2, 1, "d6"), (0, 2, "d7"), (1, 2, "d8"), (2, 2, "d9"));
            AssertFace(cube.LeftFace, (0, 0, "l1"), (1, 0, "l2"), (2, 0, "l3"), (0, 1, "l4"), (1, 1, "l5"), (2, 1, "l6"), (0, 2, "l7"), (1, 2, "l8"), (2, 2, "l9"));
            AssertFace(cube.RightFace, (0, 0, "r1"), (1, 0, "r2"), (2, 0, "r3"), (0, 1, "r4"), (1, 1, "r5"), (2, 1, "r6"), (0, 2, "r7"), (1, 2, "r8"), (2, 2, "r9"));
        }

        private static Cube BuildeSampleCube()
        {
            var builder = new RotatedArrayBasedCubeBuilder();
            builder.SetFrontFace(new[,]
                                 {
                                     { "f7", "f8", "f9" },
                                     { "f4", "f5", "f6" },
                                     { "f1", "f2", "f3" }
                                 });
            builder.SetBackFace(new[,]
                                {
                                    { "b7", "b8", "b9" },
                                    { "b4", "b5", "b6" },
                                    { "b1", "b2", "b3" }
                                });
            builder.SetTopFace(new[,]
                               {
                                   { "t7", "t8", "t9" },
                                   { "t4", "t5", "t6" },
                                   { "t1", "t2", "t3" }
                               });
            builder.SetBottomFace(new[,]
                                  {
                                      { "d7", "d8", "d9" },
                                      { "d4", "d5", "d6" },
                                      { "d1", "d2", "d3" }
                                  });
            builder.SetLeftFace(new[,]
                                {
                                    { "l7", "l8", "l9" },
                                    { "l4", "l5", "l6" },
                                    { "l1", "l2", "l3" }
                                });
            builder.SetRightFace(new[,]
                                 {
                                     { "r7", "r8", "r9" },
                                     { "r4", "r5", "r6" },
                                     { "r1", "r2", "r3" }
                                 });

            var cube = builder.Build();
            return cube;
        }


        /********* OLD VERSION *************/


        [Test]
        public void CubeEngineShouldGetCubeFrom3DMatrix()
        {
            // arrange
            var builder = new TestCubeBuilder();
            var cubeEngine = new TestableCubeEngine();

            #region Set 54 sticker points

            var stickerPoints = new Dictionary<Sticker, Point3D>
                                {
                                    { builder.Front1, new Point3D(1, 1, 0) },
                                    { builder.Front2, new Point3D(1, 2, 0) },
                                    { builder.Front3, new Point3D(1, 3, 0) },
                                    { builder.Front4, new Point3D(2, 1, 0) },
                                    { builder.Front5, new Point3D(2, 2, 0) },
                                    { builder.Front6, new Point3D(2, 3, 0) },
                                    { builder.Front7, new Point3D(3, 1, 0) },
                                    { builder.Front8, new Point3D(3, 2, 0) },
                                    { builder.Front9, new Point3D(3, 3, 0) },

                                    { builder.Back1, new Point3D(3, 1, 4) },
                                    { builder.Back2, new Point3D(3, 2, 4) },
                                    { builder.Back3, new Point3D(3, 3, 4) },
                                    { builder.Back4, new Point3D(2, 1, 4) },
                                    { builder.Back5, new Point3D(2, 2, 4) },
                                    { builder.Back6, new Point3D(2, 3, 4) },
                                    { builder.Back7, new Point3D(1, 1, 4) },
                                    { builder.Back8, new Point3D(1, 2, 4) },
                                    { builder.Back9, new Point3D(1, 3, 4) },

                                    { builder.Left1, new Point3D(0, 1, 3) },
                                    { builder.Left2, new Point3D(0, 2, 3) },
                                    { builder.Left3, new Point3D(0, 3, 3) },
                                    { builder.Left4, new Point3D(0, 1, 2) },
                                    { builder.Left5, new Point3D(0, 2, 2) },
                                    { builder.Left6, new Point3D(0, 3, 2) },
                                    { builder.Left7, new Point3D(0, 1, 1) },
                                    { builder.Left8, new Point3D(0, 2, 1) },
                                    { builder.Left9, new Point3D(0, 3, 1) },

                                    { builder.Right1, new Point3D(4, 1, 1) },
                                    { builder.Right2, new Point3D(4, 2, 1) },
                                    { builder.Right3, new Point3D(4, 3, 1) },
                                    { builder.Right4, new Point3D(4, 1, 2) },
                                    { builder.Right5, new Point3D(4, 2, 2) },
                                    { builder.Right6, new Point3D(4, 3, 2) },
                                    { builder.Right7, new Point3D(4, 1, 3) },
                                    { builder.Right8, new Point3D(4, 2, 3) },
                                    { builder.Right9, new Point3D(4, 3, 3) },

                                    { builder.Top1, new Point3D(1, 4, 1) },
                                    { builder.Top2, new Point3D(1, 4, 2) },
                                    { builder.Top3, new Point3D(1, 4, 3) },
                                    { builder.Top4, new Point3D(2, 4, 1) },
                                    { builder.Top5, new Point3D(2, 4, 2) },
                                    { builder.Top6, new Point3D(2, 4, 3) },
                                    { builder.Top7, new Point3D(3, 4, 1) },
                                    { builder.Top8, new Point3D(3, 4, 2) },
                                    { builder.Top9, new Point3D(3, 4, 3) },

                                    { builder.Bottom1, new Point3D(1, 0, 1) },
                                    { builder.Bottom2, new Point3D(1, 0, 2) },
                                    { builder.Bottom3, new Point3D(1, 0, 3) },
                                    { builder.Bottom4, new Point3D(2, 0, 1) },
                                    { builder.Bottom5, new Point3D(2, 0, 2) },
                                    { builder.Bottom6, new Point3D(2, 0, 3) },
                                    { builder.Bottom7, new Point3D(3, 0, 1) },
                                    { builder.Bottom8, new Point3D(3, 0, 2) },
                                    { builder.Bottom9, new Point3D(3, 0, 3) }
                                };

            #endregion

            // act
            var resultCube = cubeEngine.TestGetCubeFrom3DMatrix(stickerPoints);

            #region Assertions of 54 stickers

            void AssertFrontFace(Sticker sticker, int x, int y) =>
                Assert.True(resultCube.FrontFace[x, y] == sticker, $"FrontFace[{x},{y}] should have {sticker.Color} instead of {resultCube.FrontFace[x, y].Color}");

            AssertFrontFace(builder.Front1, 0, 0);
            AssertFrontFace(builder.Front2, 0, 1);
            AssertFrontFace(builder.Front3, 0, 2);
            AssertFrontFace(builder.Front4, 1, 0);
            AssertFrontFace(builder.Front5, 1, 1);
            AssertFrontFace(builder.Front6, 1, 2);
            AssertFrontFace(builder.Front7, 2, 0);
            AssertFrontFace(builder.Front8, 2, 1);
            AssertFrontFace(builder.Front9, 2, 2);

            void AssertBackFace(Sticker sticker, int x, int y) =>
                Assert.True(resultCube.BackFace[x, y] == sticker, $"BackFace[{x},{y}] should have {sticker.Color} instead of {resultCube.BackFace[x, y].Color}");

            AssertBackFace(builder.Back1, 0, 0);
            AssertBackFace(builder.Back2, 0, 1);
            AssertBackFace(builder.Back3, 0, 2);
            AssertBackFace(builder.Back4, 1, 0);
            AssertBackFace(builder.Back5, 1, 1);
            AssertBackFace(builder.Back6, 1, 2);
            AssertBackFace(builder.Back7, 2, 0);
            AssertBackFace(builder.Back8, 2, 1);
            AssertBackFace(builder.Back9, 2, 2);

            void AssertTopFace(Sticker sticker, int x, int y) =>
                Assert.True(resultCube.TopFace[x, y] == sticker, $"TopFace[{x},{y}] should have {sticker.Color} instead of {resultCube.TopFace[x, y].Color}");

            AssertTopFace(builder.Top1, 0, 0);
            AssertTopFace(builder.Top2, 0, 1);
            AssertTopFace(builder.Top3, 0, 2);
            AssertTopFace(builder.Top4, 1, 0);
            AssertTopFace(builder.Top5, 1, 1);
            AssertTopFace(builder.Top6, 1, 2);
            AssertTopFace(builder.Top7, 2, 0);
            AssertTopFace(builder.Top8, 2, 1);
            AssertTopFace(builder.Top9, 2, 2);

            void AssertBottomFace(Sticker sticker, int x, int y) =>
                Assert.True(resultCube.BottomFace[x, y] == sticker, $"BottomFace[{x},{y}] should have {sticker.Color} instead of {resultCube.BottomFace[x, y].Color}");

            AssertBottomFace(builder.Bottom1, 0, 0);
            AssertBottomFace(builder.Bottom2, 0, 1);
            AssertBottomFace(builder.Bottom3, 0, 2);
            AssertBottomFace(builder.Bottom4, 1, 0);
            AssertBottomFace(builder.Bottom5, 1, 1);
            AssertBottomFace(builder.Bottom6, 1, 2);
            AssertBottomFace(builder.Bottom7, 2, 0);
            AssertBottomFace(builder.Bottom8, 2, 1);
            AssertBottomFace(builder.Bottom9, 2, 2);

            void AssertLeftFace(Sticker sticker, int x, int y) =>
                Assert.True(resultCube.LeftFace[x, y] == sticker, $"LeftFace[{x},{y}] should have {sticker.Color} instead of {resultCube.LeftFace[x, y].Color}");

            AssertLeftFace(builder.Left1, 0, 0);
            AssertLeftFace(builder.Left2, 0, 1);
            AssertLeftFace(builder.Left3, 0, 2);
            AssertLeftFace(builder.Left4, 1, 0);
            AssertLeftFace(builder.Left5, 1, 1);
            AssertLeftFace(builder.Left6, 1, 2);
            AssertLeftFace(builder.Left7, 2, 0);
            AssertLeftFace(builder.Left8, 2, 1);
            AssertLeftFace(builder.Left9, 2, 2);

            void AssertRightFace(Sticker sticker, int x, int y) =>
                Assert.True(resultCube.RightFace[x, y] == sticker, $"RightFace[{x},{y}] should have {sticker.Color} instead of {resultCube.RightFace[x, y].Color}");

            AssertRightFace(builder.Right1, 0, 0);
            AssertRightFace(builder.Right2, 0, 1);
            AssertRightFace(builder.Right3, 0, 2);
            AssertRightFace(builder.Right4, 1, 0);
            AssertRightFace(builder.Right5, 1, 1);
            AssertRightFace(builder.Right6, 1, 2);
            AssertRightFace(builder.Right7, 2, 0);
            AssertRightFace(builder.Right8, 2, 1);
            AssertRightFace(builder.Right9, 2, 2);

            #endregion
        }

        [Test]
        public void CubeShouldHaveProperStateAfterFrontFaceCounterClockwiseRotation()
        {
            // arrange
            var cube = BuildTestCube();
            var cubeEngine = new CubeEngine();

            // act
            var resultCube = cubeEngine.RotateCounterclockwise(cube, c => c.FrontFace);

            // assert
            var builder = new StringBasedCubeBuilder();

            builder.SetFrontFace(@"F3 F6 F9
                                   F2 F5 F8
                                   F1 F4 F7");

            builder.SetBackFace(@"B1 B2 B3
                                  B4 B5 B6
                                  B7 B8 B9");

            builder.SetTopFace(@"T1 T2 T3
                                 T4 T5 T6
                                 T7 T8 T9");

            builder.SetBottomFace(@"D1 D2 D3
                                    D4 D5 D6
                                    D7 D8 D9");

            builder.SetLeftFace(@"L1 L2 L3
                                  L4 L5 L6
                                  L7 L8 L9");

            builder.SetRightFace(@"R1 R2 R3
                                   R4 R5 R6
                                   R7 R8 R9");

            var expectedCube = builder.Build();

            AssertCube(resultCube, expectedCube);


            //void AssertFrontFace(Sticker sticker, int x, int y) =>
            //    Assert.True(resultCube.FrontFace[x, y] == sticker, $"FrontFace[{x},{y}] should have {sticker.Color} instead of {resultCube.FrontFace[x, y].Color}");

            // todo prepare assert data in one array, in the same way like it is in testCubeBuilder

            // todo maybe ... resign from testBuilder, use Sticker.White, or TestableSticker.Front1 and put it to standard builder

            //AssertFrontFace(testCubeBuilder.Front3, 0, 0);
            //AssertFrontFace(testCubeBuilder.Front2, 1, 0);
            //AssertFrontFace(testCubeBuilder.Front1, 2, 0);
            //AssertFrontFace(testCubeBuilder.Front6, 0, 1);
            //AssertFrontFace(testCubeBuilder.Front5, 1, 1);
            //AssertFrontFace(testCubeBuilder.Front4, 2, 1);
            //AssertFrontFace(testCubeBuilder.Front9, 0, 2);
            //AssertFrontFace(testCubeBuilder.Front8, 1, 2);
            //AssertFrontFace(testCubeBuilder.Front7, 2, 2);

            // todo asserts for rest of stickers ....
        }

        private static void AssertCube(Cube resultCube, Cube expectedCube)
        {
            void AssertFace(string name, Sticker[,] resultFace, Sticker[,] expectedFace)
            {
                for (var i = 0; i < 3; i++)
                {
                    for (var j = 0; j < 3; j++)
                    {
                        Assert.That(resultFace[i, j], Is.EqualTo(expectedFace[i, j]),
                                    $"{name}[{i},{j}] should have {expectedFace[i, j].Color} instead of {resultFace[i, j].Color}");
                    }
                }
            }

            AssertFace(nameof(resultCube.FrontFace), resultCube.FrontFace, expectedCube.FrontFace);
            AssertFace(nameof(resultCube.BackFace), resultCube.BackFace, expectedCube.BackFace);
            AssertFace(nameof(resultCube.TopFace), resultCube.TopFace, expectedCube.TopFace);
            AssertFace(nameof(resultCube.BottomFace), resultCube.BottomFace, expectedCube.BottomFace);
            AssertFace(nameof(resultCube.LeftFace), resultCube.LeftFace, expectedCube.LeftFace);
            AssertFace(nameof(resultCube.RightFace), resultCube.RightFace, expectedCube.RightFace);
        }

        private static Cube BuildTestCube()
        {
            var builder = new StringBasedCubeBuilder();

            builder.SetFrontFace(@"F1 F2 F3
                                   F4 F5 F6
                                   F7 F8 F9");

            builder.SetBackFace(@"B1 B2 B3
                                  B4 B5 B6
                                  B7 B8 B9");

            builder.SetTopFace(@"T1 T2 T3
                                 T4 T5 T6
                                 T7 T8 T9");

            builder.SetBottomFace(@"D1 D2 D3
                                    D4 D5 D6
                                    D7 D8 D9");

            builder.SetLeftFace(@"L1 L2 L3
                                  L4 L5 L6
                                  L7 L8 L9");

            builder.SetRightFace(@"R1 R2 R3
                                   R4 R5 R6
                                   R7 R8 R9");

            return builder.Build();
        }


        /* In a classic Rubik's Cube, 
         * each of the six faces is covered by nine stickers, 
         * each of one of six solid colours: white, red, blue, orange, green, and yellow. 
         * 
         * In currently sold models, white is opposite yellow, blue is opposite green, 
         * and orange is opposite red, and the red, white and blue are arranged in that order in a clockwise arrangement 
         */

        /*
         - Klasa Cube jako prezentacja 6 ścian z gridem stockerow.Dostęp poprzez metody typu GetFrontFace 
            a nie dictionary.Do tego builder poprawnej kostki
         - Klasa CubeEngine z implementacją mechaniki ruchu, 
            api do obracania poszczególnych ścian.Komunikacja 
                (ustawienie stanu początkowego i sprawdzenie stanu) 
                poprzez klasę Cube.Api pozwala też sprawdzić czy kostka jest już ułożona 
         - CubeUkladacz ;-) klasa która implementuje algorytm układania kostki 
            operując na CubeEngine.Może jakiś dedykowany język skryptowy :-) 
         */


        // cube has faces (like sides)
        // face is a grid (3x3) of stickers 
        // sticker has a color
    }

    public struct Sticker
    {
        public static Sticker White = FromColor("White");
        public static Sticker Red = FromColor("Red");
        public static Sticker Blue = FromColor("Blue");
        public static Sticker Orange = FromColor("Orange");
        public static Sticker Green = FromColor("Green");
        public static Sticker Yellow = FromColor("Yellow");

        private Sticker(string color)
        {
            this.Color = color;
        }

        public static Sticker FromColor(string color)
        {
            return new Sticker(color);
        }

        public string Color { get; }

        public override bool Equals(object obj)
        {
            return obj is Sticker s && this == s;
        }

        public override int GetHashCode()
        {
            return this.Color.GetHashCode();
        }

        public static bool operator ==(Sticker x, Sticker y)
        {
            return x.Color == y.Color;
        }

        public static bool operator !=(Sticker x, Sticker y)
        {
            return !(x == y);
        }
    }

    public class CubeBuilder
    {
        public Cube Build()
        {
            throw new System.NotImplementedException();
        }
    }

    public class TestableCubeEngine : CubeEngine
    {
        public IDictionary<Sticker, Point3D> TestMapCubeTo3DMatrix(Cube cube)
        {
            return this.MapCubeTo3DMatrix(cube);
        }

        public Cube TestGetCubeFrom3DMatrix(IDictionary<Sticker, Point3D> stickers)
        {
            return this.GetCubeFrom3DMatrix(stickers);
        }

        public IList<Point<Sticker>> TestMapCubeTo3DMatrix_NEW(Cube cube)
        {
            return this.MapCubeTo3DMatrix_NEW(cube);
        }

        public Cube TestGetCubeFrom3DMatrix_NEW(IList<Point<Sticker>> matrixOfStickers)
        {
            return this.GetCubeFrom3DMatrix_NEW(matrixOfStickers);
        }
    }

    public class CubeEngine
    {
        // todo add validation against cube size (should be 3x3) 

        private const int Size = 3;

        public Cube RotateClockwise(Cube cube, Expression<Func<Cube, Sticker[,]>> face)
        {
            return this.Rotate(cube, face, (x, y) => RotatePoints(x, y, true));
        }

        public Cube RotateCounterclockwise(Cube cube, Expression<Func<Cube, Sticker[,]>> face)
        {
            return this.Rotate(cube, face, (x, y) => RotatePoints(x, y, false));
        }

        private Cube Rotate(Cube cube,
                            Expression<Func<Cube, Sticker[,]>> face,
                            Func<int, int, (int x, int y)> rotatePoints)
        {
            var faceName = GetFaceName(face);
            var stickerPoints = this.MapCubeTo3DMatrix(cube);

            if (faceName == nameof(cube.FrontFace))
            {
                foreach (var stickerPoint in stickerPoints.ToList())
                {
                    (int oldX, int oldY, int oldZ) = (stickerPoint.Value.X, stickerPoint.Value.Y, stickerPoint.Value.Z);

                    if (oldZ == 0 || oldZ == 1)
                    {
                        (int newX, int newY) = rotatePoints(oldX, oldY);
                        stickerPoints[stickerPoint.Key] = new Point3D(newX, newY, oldZ);
                    }
                }
            }

            return this.GetCubeFrom3DMatrix(stickerPoints);
        }

        private static string GetFaceName(Expression<Func<Cube, Sticker[,]>> face)
        {
            var faceExpression = (MemberExpression) face.Body;
            var faceName = faceExpression.Member.Name;
            return faceName;
        }

        private static (int x, int y) RotatePoints(int x, int y, bool isClockwise)
        {
            // rotating face needs to be moved to the point (0,0) 
            // before ration matrix will be calculated
            const int offset = 2;

            (int x, int y) result = (x - offset, y - offset);

            // todo use external class for this

            if (isClockwise)
            {
                const double angle = -Math.PI * 0.5;
                result.x = (int) Math.Round((x - offset) * Math.Cos(angle) + (y - offset) * Math.Sin(angle)) + offset;
                result.y = (int) Math.Round(-(x - offset) * Math.Sin(angle) + (y - offset) * Math.Cos(angle)) + offset;
            }
            else
            {
                const double angle = Math.PI * 0.5;
                result.x = (int) Math.Round((x - offset) * Math.Cos(angle) - (y - offset) * Math.Sin(angle)) + offset;
                result.y = (int) Math.Round((x - offset) * Math.Sin(angle) + (y - offset) * Math.Cos(angle)) + offset;
            }

            return result;
        }

        protected IList<Point<Sticker>> MapCubeTo3DMatrix_NEW(Cube cube)
        {
            var result = new List<Point<Sticker>>();

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    result.Add(new Point<Sticker>(i + 1, j + 1, 0, cube.FrontFace[i, j]));
                    result.Add(new Point<Sticker>(Size - i, j + 1, Size + 1, cube.BackFace[i, j]));
                    result.Add(new Point<Sticker>(0, j + 1, Size - i, cube.LeftFace[i, j]));
                    result.Add(new Point<Sticker>(Size + 1, j + 1, i + 1, cube.RightFace[i, j]));
                    result.Add(new Point<Sticker>(i + 1, Size + 1, j + 1, cube.TopFace[i, j]));
                    result.Add(new Point<Sticker>(i + 1, 0, j + 1, cube.BottomFace[i, j]));
                }
            }

            return result;
        }

        protected Cube GetCubeFrom3DMatrix_NEW(IList<Point<Sticker>> matrixOfStickers)
        {
            var frontFace = new Sticker[3, 3];
            var backFace = new Sticker[3, 3];
            var leftFace = new Sticker[3, 3];
            var rightFace = new Sticker[3, 3];
            var topFace = new Sticker[3, 3];
            var bottomFace = new Sticker[3, 3];

            foreach (var point in matrixOfStickers)
            {
                if (point.Z == 0)
                {
                    frontFace[point.X - 1, point.Y - 1] = point.Value;
                }
                if (point.Z == Size + 1)
                {
                    backFace[Size - point.X, point.Y - 1] = point.Value;
                }
                if (point.X == 0)
                {
                    leftFace[Size - point.Z, point.Y - 1] = point.Value;
                }
                if (point.X == Size + 1)
                {
                    rightFace[point.Z - 1, point.Y - 1] = point.Value;
                }
                if (point.Y == Size + 1)
                {
                    topFace[point.X - 1, point.Z - 1] = point.Value;
                }
                if (point.Y == 0)
                {
                    bottomFace[point.X - 1, point.Z - 1] = point.Value;
                }
            }

            return new Cube(
                frontFace,
                backFace,
                topFace,
                bottomFace,
                leftFace,
                rightFace);
        }

        protected IDictionary<Sticker, Point3D> MapCubeTo3DMatrix(Cube cube)
        {
            // todo use List<Point>, where Point<Value> { X,Y,Z,Value } 

            var stickerPoints = new Dictionary<Sticker, Point3D>();

            for (var i = 0; i < Size; i++)
            {
                for (var j = 0; j < Size; j++)
                {
                    //stickerPoints.Add(cube.FrontFace[i, j], new Point3D(i + 1, Size - j, 0));
                    //stickerPoints.Add(cube.BackFace[i, j], new Point3D(Size - i, Size - j, Size + 1));
                    //stickerPoints.Add(cube.LeftFace[i, j], new Point3D(0, Size - j, Size - i));
                    //stickerPoints.Add(cube.RightFace[i, j], new Point3D(Size + 1, Size - j, i + 1));
                    //stickerPoints.Add(cube.TopFace[i, j], new Point3D(i + 1, Size + 1, Size - j));
                    //stickerPoints.Add(cube.BottomFace[i, j], new Point3D(i + 1, 0, Size - j));

                    stickerPoints.Add(cube.FrontFace[i, j], new Point3D(i + 1, j + 1, 0));
                    stickerPoints.Add(cube.BackFace[i, j], new Point3D(Size - i, j + 1, Size + 1));
                    stickerPoints.Add(cube.LeftFace[i, j], new Point3D(0, j + 1, Size - i));
                    stickerPoints.Add(cube.RightFace[i, j], new Point3D(Size + 1, j + 1, i + 1));
                    stickerPoints.Add(cube.TopFace[i, j], new Point3D(i + 1, Size + 1, j + 1));
                    stickerPoints.Add(cube.BottomFace[i, j], new Point3D(i + 1, 0, j + 1));
                }
            }

            return stickerPoints;
        }

        protected Cube GetCubeFrom3DMatrix(IDictionary<Sticker, Point3D> stickerPoints)
        {
            var frontFace = new Sticker[3, 3];
            var backFace = new Sticker[3, 3];
            var leftFace = new Sticker[3, 3];
            var rightFace = new Sticker[3, 3];
            var topFace = new Sticker[3, 3];
            var bottomFace = new Sticker[3, 3];

            foreach (var stickerPoint in stickerPoints)
            {
                var sticker = stickerPoint.Key;
                var point = stickerPoint.Value;

                if (point.Z == 0)
                {
                    frontFace[point.X - 1, point.Y - 1] = sticker;
                }
                if (point.Z == Size + 1)
                {
                    backFace[Size - point.X, point.Y - 1] = sticker;
                }
                if (point.X == 0)
                {
                    leftFace[Size - point.Z, point.Y - 1] = sticker;
                }
                if (point.X == Size + 1)
                {
                    rightFace[point.Z - 1, point.Y - 1] = sticker;
                }
                if (point.Y == Size + 1)
                {
                    topFace[point.X - 1, point.Z - 1] = sticker;
                }
                if (point.Y == 0)
                {
                    bottomFace[point.X - 1, point.Z - 1] = sticker;
                }
            }

            return new Cube(
                frontFace,
                backFace,
                topFace,
                bottomFace,
                leftFace,
                rightFace);
        }
    }

    public struct Point<T>
    {
        public Point(int x, int y, int z, T value)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.Value = value;
        }

        public int X { get; }

        public int Y { get; }

        public int Z { get; }
        public T Value { get; }

        public override bool Equals(object obj)
        {
            return obj is Point<T> d && this == d;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        public static bool operator ==(Point<T> p1, Point<T> p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
        }

        public static bool operator !=(Point<T> p1, Point<T> p2)
        {
            return !(p1 == p2);
        }
    }

    [DebuggerDisplay("(X,Y,Z)=({X},{Y},{Z})")]
    public struct Point3D
    {
        public Point3D(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public int X { get; }

        public int Y { get; }

        public int Z { get; }

        public override bool Equals(object obj)
        {
            return obj is Point3D d && this == d;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() ^ this.Z.GetHashCode();
        }

        public static bool operator ==(Point3D p1, Point3D p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z;
        }

        public static bool operator !=(Point3D p1, Point3D p2)
        {
            return !(p1 == p2);
        }
    }

    public class Cube
    {
        public Cube(Sticker[,] frontFace,
                    Sticker[,] backFace,
                    Sticker[,] topFace,
                    Sticker[,] bottomFace,
                    Sticker[,] leftFace,
                    Sticker[,] rightFace)
        {
            this.FrontFace = frontFace;
            this.BackFace = backFace;
            this.TopFace = topFace;
            this.BottomFace = bottomFace;
            this.LeftFace = leftFace;
            this.RightFace = rightFace;
        }

        public Sticker[,] FrontFace { get; }

        public Sticker[,] BackFace { get; }

        public Sticker[,] TopFace { get; }

        public Sticker[,] BottomFace { get; }

        public Sticker[,] LeftFace { get; }

        public Sticker[,] RightFace { get; }
    }

    public enum Color
    {
        White,
        Red,
        Blue,
        Orange,
        Green,
        Yellow
    }


    public class TestCubeBuilder
    {
        public readonly Sticker Front1 = Sticker.FromColor("front1");
        public readonly Sticker Front2 = Sticker.FromColor("front2");
        public readonly Sticker Front3 = Sticker.FromColor("front3");
        public readonly Sticker Front4 = Sticker.FromColor("front4");
        public readonly Sticker Front5 = Sticker.FromColor("front5");
        public readonly Sticker Front6 = Sticker.FromColor("front6");
        public readonly Sticker Front7 = Sticker.FromColor("front7");
        public readonly Sticker Front8 = Sticker.FromColor("front8");
        public readonly Sticker Front9 = Sticker.FromColor("front9");

        public readonly Sticker Back1 = Sticker.FromColor("back1");
        public readonly Sticker Back2 = Sticker.FromColor("back2");
        public readonly Sticker Back3 = Sticker.FromColor("back3");
        public readonly Sticker Back4 = Sticker.FromColor("back4");
        public readonly Sticker Back5 = Sticker.FromColor("back5");
        public readonly Sticker Back6 = Sticker.FromColor("back6");
        public readonly Sticker Back7 = Sticker.FromColor("back7");
        public readonly Sticker Back8 = Sticker.FromColor("back8");
        public readonly Sticker Back9 = Sticker.FromColor("back9");

        public readonly Sticker Left1 = Sticker.FromColor("Left1");
        public readonly Sticker Left2 = Sticker.FromColor("Left2");
        public readonly Sticker Left3 = Sticker.FromColor("Left3");
        public readonly Sticker Left4 = Sticker.FromColor("Left4");
        public readonly Sticker Left5 = Sticker.FromColor("Left5");
        public readonly Sticker Left6 = Sticker.FromColor("Left6");
        public readonly Sticker Left7 = Sticker.FromColor("Left7");
        public readonly Sticker Left8 = Sticker.FromColor("Left8");
        public readonly Sticker Left9 = Sticker.FromColor("Left9");

        public readonly Sticker Right1 = Sticker.FromColor("right1");
        public readonly Sticker Right2 = Sticker.FromColor("right2");
        public readonly Sticker Right3 = Sticker.FromColor("right3");
        public readonly Sticker Right4 = Sticker.FromColor("right4");
        public readonly Sticker Right5 = Sticker.FromColor("right5");
        public readonly Sticker Right6 = Sticker.FromColor("right6");
        public readonly Sticker Right7 = Sticker.FromColor("right7");
        public readonly Sticker Right8 = Sticker.FromColor("right8");
        public readonly Sticker Right9 = Sticker.FromColor("right9");

        public readonly Sticker Top1 = Sticker.FromColor("top1");
        public readonly Sticker Top2 = Sticker.FromColor("top2");
        public readonly Sticker Top3 = Sticker.FromColor("top3");
        public readonly Sticker Top4 = Sticker.FromColor("top4");
        public readonly Sticker Top5 = Sticker.FromColor("top5");
        public readonly Sticker Top6 = Sticker.FromColor("top6");
        public readonly Sticker Top7 = Sticker.FromColor("top7");
        public readonly Sticker Top8 = Sticker.FromColor("top8");
        public readonly Sticker Top9 = Sticker.FromColor("top9");

        public readonly Sticker Bottom1 = Sticker.FromColor("bottom1");
        public readonly Sticker Bottom2 = Sticker.FromColor("bottom2");
        public readonly Sticker Bottom3 = Sticker.FromColor("bottom3");
        public readonly Sticker Bottom4 = Sticker.FromColor("bottom4");
        public readonly Sticker Bottom5 = Sticker.FromColor("bottom5");
        public readonly Sticker Bottom6 = Sticker.FromColor("bottom6");
        public readonly Sticker Bottom7 = Sticker.FromColor("bottom7");
        public readonly Sticker Bottom8 = Sticker.FromColor("bottom8");
        public readonly Sticker Bottom9 = Sticker.FromColor("bottom9");

        public Cube Build()
        {
            return new Cube(
                new[,]
                {
                    { this.Front1, this.Front2, this.Front3 },
                    { this.Front4, this.Front5, this.Front6 },
                    { this.Front7, this.Front8, this.Front9 }
                },
                new[,]
                {
                    { this.Back1, this.Back2, this.Back3 },
                    { this.Back4, this.Back5, this.Back6 },
                    { this.Back7, this.Back8, this.Back9 }
                },
                new[,]
                {
                    { this.Top1, this.Top2, this.Top3 },
                    { this.Top4, this.Top5, this.Top6 },
                    { this.Top7, this.Top8, this.Top9 }
                },
                new[,]
                {
                    { this.Bottom1, this.Bottom2, this.Bottom3 },
                    { this.Bottom4, this.Bottom5, this.Bottom6 },
                    { this.Bottom7, this.Bottom8, this.Bottom9 }
                },
                new[,]
                {
                    { this.Left1, this.Left2, this.Left3 },
                    { this.Left4, this.Left5, this.Left6 },
                    { this.Left7, this.Left8, this.Left9 }
                },
                new[,]
                {
                    { this.Right1, this.Right2, this.Right3 },
                    { this.Right4, this.Right5, this.Right6 },
                    { this.Right7, this.Right8, this.Right9 }
                });
        }
    }


    /*
            [Test]
        public void CubeEngineShouldMapCubeTo3DMatrix()
        {
            // arrange
            var builder = new TestCubeBuilder();
            var cube = builder.Build();
            var cubeEngine = new TestableCubeEngine();

            // act
            var resultMatrix = cubeEngine.TestMapCubeTo3DMatrix(cube);

            // assert

            #region Assertions of 54 stickers

            void AssertStickerCoordinates(Sticker s, Point3D p) =>
                Assert.True(resultMatrix[s] == p, $"Sticker \"{s.Color}\" has coordinates ({resultMatrix[s].X},{resultMatrix[s].Y},{resultMatrix[s].Z}) instead of ({p.X},{p.Y},{p.Z})");

            AssertStickerCoordinates(builder.Front1, new Point3D(1, 1, 0));
            AssertStickerCoordinates(builder.Front2, new Point3D(1, 2, 0));
            AssertStickerCoordinates(builder.Front3, new Point3D(1, 3, 0));
            AssertStickerCoordinates(builder.Front4, new Point3D(2, 1, 0));
            AssertStickerCoordinates(builder.Front5, new Point3D(2, 2, 0));
            AssertStickerCoordinates(builder.Front6, new Point3D(2, 3, 0));
            AssertStickerCoordinates(builder.Front7, new Point3D(3, 1, 0));
            AssertStickerCoordinates(builder.Front8, new Point3D(3, 2, 0));
            AssertStickerCoordinates(builder.Front9, new Point3D(3, 3, 0));

            AssertStickerCoordinates(builder.Back1, new Point3D(3, 1, 4));
            AssertStickerCoordinates(builder.Back2, new Point3D(3, 2, 4));
            AssertStickerCoordinates(builder.Back3, new Point3D(3, 3, 4));
            AssertStickerCoordinates(builder.Back4, new Point3D(2, 1, 4));
            AssertStickerCoordinates(builder.Back5, new Point3D(2, 2, 4));
            AssertStickerCoordinates(builder.Back6, new Point3D(2, 3, 4));
            AssertStickerCoordinates(builder.Back7, new Point3D(1, 1, 4));
            AssertStickerCoordinates(builder.Back8, new Point3D(1, 2, 4));
            AssertStickerCoordinates(builder.Back9, new Point3D(1, 3, 4));

            AssertStickerCoordinates(builder.Left1, new Point3D(0, 1, 3));
            AssertStickerCoordinates(builder.Left2, new Point3D(0, 2, 3));
            AssertStickerCoordinates(builder.Left3, new Point3D(0, 3, 3));
            AssertStickerCoordinates(builder.Left4, new Point3D(0, 1, 2));
            AssertStickerCoordinates(builder.Left5, new Point3D(0, 2, 2));
            AssertStickerCoordinates(builder.Left6, new Point3D(0, 3, 2));
            AssertStickerCoordinates(builder.Left7, new Point3D(0, 1, 1));
            AssertStickerCoordinates(builder.Left8, new Point3D(0, 2, 1));
            AssertStickerCoordinates(builder.Left9, new Point3D(0, 3, 1));

            AssertStickerCoordinates(builder.Right1, new Point3D(4, 1, 1));
            AssertStickerCoordinates(builder.Right2, new Point3D(4, 2, 1));
            AssertStickerCoordinates(builder.Right3, new Point3D(4, 3, 1));
            AssertStickerCoordinates(builder.Right4, new Point3D(4, 1, 2));
            AssertStickerCoordinates(builder.Right5, new Point3D(4, 2, 2));
            AssertStickerCoordinates(builder.Right6, new Point3D(4, 3, 2));
            AssertStickerCoordinates(builder.Right7, new Point3D(4, 1, 3));
            AssertStickerCoordinates(builder.Right8, new Point3D(4, 2, 3));
            AssertStickerCoordinates(builder.Right9, new Point3D(4, 3, 3));

            AssertStickerCoordinates(builder.Top1, new Point3D(1, 4, 1));
            AssertStickerCoordinates(builder.Top2, new Point3D(1, 4, 2));
            AssertStickerCoordinates(builder.Top3, new Point3D(1, 4, 3));
            AssertStickerCoordinates(builder.Top4, new Point3D(2, 4, 1));
            AssertStickerCoordinates(builder.Top5, new Point3D(2, 4, 2));
            AssertStickerCoordinates(builder.Top6, new Point3D(2, 4, 3));
            AssertStickerCoordinates(builder.Top7, new Point3D(3, 4, 1));
            AssertStickerCoordinates(builder.Top8, new Point3D(3, 4, 2));
            AssertStickerCoordinates(builder.Top9, new Point3D(3, 4, 3));

            AssertStickerCoordinates(builder.Bottom1, new Point3D(1, 0, 1));
            AssertStickerCoordinates(builder.Bottom2, new Point3D(1, 0, 2));
            AssertStickerCoordinates(builder.Bottom3, new Point3D(1, 0, 3));
            AssertStickerCoordinates(builder.Bottom4, new Point3D(2, 0, 1));
            AssertStickerCoordinates(builder.Bottom5, new Point3D(2, 0, 2));
            AssertStickerCoordinates(builder.Bottom6, new Point3D(2, 0, 3));
            AssertStickerCoordinates(builder.Bottom7, new Point3D(3, 0, 1));
            AssertStickerCoordinates(builder.Bottom8, new Point3D(3, 0, 2));
            AssertStickerCoordinates(builder.Bottom9, new Point3D(3, 0, 3));

            #endregion
        }



     */
}