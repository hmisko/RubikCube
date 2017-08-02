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

    public class CubeEngineEngine : ICubeEngine
    {
        private readonly Sticker[,,] stickers;

        private readonly int innerSize;

        public CubeEngineEngine(int size, IDictionary<Orientation, Face> faces)
        {
            this.innerSize = size + 2;
            this.stickers = new Sticker[this.innerSize, this.innerSize, this.innerSize];
            this.Create3DMatrix(size, faces);
        }

        private void Create3DMatrix(int size, IDictionary<Orientation, Face> faces)
        {
            foreach (var face in faces)
            {
                if (face.Key == Orientation.Front)
                {
                    for (var x = 0; x < size; x++)
                    {
                        for (var y = 0; y < size; y++)
                        {
                            this.stickers[x, y, 0] = face.Value.Stickers[x, y];
                        }
                    }
                }
                else if (face.Key == Orientation.Back)
                {
                    for (var x = 0; x < size; x++)
                    {
                        for (var y = 0; y < size; y++)
                        {
                            this.stickers[x, y, this.innerSize] = face.Value.Stickers[x, y];
                        }
                    }
                }
                else if (face.Key == Orientation.Left)
                {
                    for (var y = 0; y < size; y++)
                    {
                        for (var z = 0; z < size; z++)
                        {
                            this.stickers[0, y, z] = face.Value.Stickers[z, y];
                        }
                    }
                }
                else if (face.Key == Orientation.Right)
                {
                    for (var y = 0; y < size; y++)
                    {
                        for (var z = 0; z < size; z++)
                        {
                            this.stickers[this.innerSize, y, z] = face.Value.Stickers[z, y];
                        }
                    }
                }
                else if (face.Key == Orientation.Top)
                {
                    for (var x = 0; x < size; x++)
                    {
                        for (var z = 0; z < size; z++)
                        {
                            this.stickers[x, 0, z] = face.Value.Stickers[x, size - z];
                        }
                    }
                }
                else if (face.Key == Orientation.Bottom)
                {
                    for (var x = 0; x < size; x++)
                    {
                        for (var z = 0; z < size; z++)
                        {
                            this.stickers[x, this.innerSize, z] = face.Value.Stickers[x, size - z];
                        }
                    }
                }
            }
        }

        //public IDictionary<Orientation, Face> GetFaces()
        //{
        //    throw new NotImplementedException();
        //}

        //public Face GetFace(Orientation orientation)
        //{
        //    throw new NotImplementedException();
        //}

        //public Face GetFrontFace()
        //{
        //    throw new NotImplementedException();
        //}
    }

    /// <summary>
    /// Value object that represent cube with faces with stickers
    /// </summary>
    public class Cube
    {
        public Cube(
            Sticker[] frontFace, 
            Sticker[] backFace, 
            Sticker[] leftFace, 
            Sticker[] rightFace, 
            Sticker[] topFace, 
            Sticker[] bottomFace)
        {
            
        }
    }

    public interface ICubeEngine
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
        private readonly IDictionary<Orientation, Colour> orientationsColours = new Dictionary<Orientation, Colour>();
        private int size;

        public ICubeBuilder SetSize(int newSize)
        {
            this.size = newSize;
            return this;
        }

        public ICubeBuilder AddFace(Colour colour, Orientation orientation)
        {
            this.orientationsColours.Add(orientation, colour);
            return this;
        }

        public ICubeEngine Build()
        {
            this.ValidateConfiguration();

            var faces = new Dictionary<Orientation, Face>();

            foreach (var orientationColour in this.orientationsColours)
            {
                var stickers = new Sticker[this.size, this.size];
                for (var x = 0; x < this.size; x++)
                {
                    for (var y = 0; y < this.size; y++)
                    {
                        stickers[x, y] = new Sticker(orientationColour.Value);
                    }
                }

                faces.Add(orientationColour.Key, new Face(stickers));
            }

            return new CubeEngineEngine(this.size, faces);
        }

        private void ValidateConfiguration()
        {
            if (this.size == 0)
            {
                throw new Exception("Size must be set");
            }

            if (this.orientationsColours.Count != 6)
            {
                throw new Exception("All six facesColours must be set");
            }
        }
    }

    public enum Orientation
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

        ICubeBuilder AddFace(Colour colour, Orientation orientation);

        ICubeEngine Build();
    }

    public class ClassicCubeBuilder
    {
        private readonly ICubeBuilder cubeBuilder;

        public ClassicCubeBuilder(ICubeBuilder cubeBuilder)
        {
            this.cubeBuilder = cubeBuilder;
        }

        public ICubeEngine Build()
        {
            return this.cubeBuilder.SetSize(3)
                .AddFace(Colour.White, Orientation.Bottom)
                .AddFace(Colour.Yellow, Orientation.Top)
                .AddFace(Colour.Blue, Orientation.Front)
                .AddFace(Colour.Red, Orientation.Right)
                .AddFace(Colour.Orange, Orientation.Left)
                .AddFace(Colour.Green, Orientation.Back)
                .Build();
        }
    }
}
