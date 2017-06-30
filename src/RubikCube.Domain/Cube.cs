namespace RubikCube.Domain
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    /* In a classic Rubik's Cube, 
     * each of the six faces is covered by nine stickers, 
     * each of one of six solid colours: white, red, blue, orange, green, and yellow. 
     * 
     * In currently sold models, white is opposite yellow, blue is opposite green, 
     * and orange is opposite red, and the red, white and blue are arranged in that order in a clockwise arrangement 
     */

    public class Cube : ICube
    {
        public Cube(IDictionary<FaceOrientation, Face> faces)
        {

            // todo create 3D matrix of stickers
        }

        public IDictionary<FaceOrientation, Face> GetFaces()
        {
            throw new NotImplementedException();
        }

        public Face GetFace(FaceOrientation orientation)
        {
            throw new NotImplementedException();
        }

        public Face GetFrontFace()
        {
            throw new NotImplementedException();
        }
    }

    public interface ICube
    {
    }

    public class Face
    {
        public Face(Sticker[,] stickers)
        {
            this.Stickers = stickers;
        }

        public Sticker[,] Stickers { get; }
    }

    public class Sticker
    {
        public Sticker(Colour colour)
        {
            this.Colour = colour;
        }

        public Colour Colour { get; }
    }

    public enum Colour
    {
        White,
        Red,
        Blue,
        Orange,
        Green,
        Yellow
    }

    public class CubeBuilder : ICubeBuilder
    {
        private readonly IDictionary<FaceOrientation, Colour> facesColours = new Dictionary<FaceOrientation, Colour>();
        private int size;

        public ICubeBuilder SetSize(int newSize)
        {
            this.size = newSize;
            return this;
        }

        public ICubeBuilder AddFace(Colour colour, FaceOrientation orientation)
        {
            this.facesColours.Add(orientation, colour);
            return this;
        }

        public ICube Build()
        {
            this.ValidateConfiguration();

            var faces = new List<Face>();

            foreach (var faceColour in this.facesColours)
            {
                var stickers = new Sticker[this.size, this.size];
                for (var x = 0; x < this.size; x++)
                {
                    for (var y = 0; y < this.size; y++)
                    {
                        stickers[x, y] = new Sticker(faceColour.Value);
                    }
                }

                faces.Add(new Face(faceColour.Key, stickers));
            }

            return new Cube(faces.ToArray());
        }

        private void ValidateConfiguration()
        {
            if (this.size == 0)
            {
                throw new Exception("Size must be set");
            }

            if (this.facesColours.Count != 6)
            {
                throw new Exception("All six facesColours must be set");
            }
        }
    }

    public enum FaceOrientation
    {
        Front,
        Back,
        Left,
        Right,
        Top,
        Bottom
    }

    public interface ICubeBuilder
    {
        ICubeBuilder SetSize(int newSize);

        ICubeBuilder AddFace(Colour colour, FaceOrientation orientation);

        ICube Build();
    }

    public class ClassicCubeBuilder
    {
        private readonly ICubeBuilder cubeBuilder;

        public ClassicCubeBuilder(ICubeBuilder cubeBuilder)
        {
            this.cubeBuilder = cubeBuilder;
        }

        public ICube Build()
        {
            return this.cubeBuilder.SetSize(3)
                .AddFace(Colour.White, FaceOrientation.Bottom)
                .AddFace(Colour.Yellow, FaceOrientation.Top)
                .AddFace(Colour.Blue, FaceOrientation.Front)
                .AddFace(Colour.Red, FaceOrientation.Right)
                .AddFace(Colour.Orange, FaceOrientation.Left)
                .AddFace(Colour.Green, FaceOrientation.Back)
                .Build();
        }
    }
}
