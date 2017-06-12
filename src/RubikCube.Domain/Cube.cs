namespace RubikCube.Domain
{
    using System;
    using System.Linq;

    /* In a classic Rubik's Cube, 
     * each of the six faces is covered by nine stickers, 
     * each of one of six solid colours: white, red, blue, orange, green, and yellow. 
     * 
     * In currently sold models, white is opposite yellow, blue is opposite green, 
     * and orange is opposite red, and the red, white and blue are arranged in that order in a clockwise arrangement 
     */

    public class Cube
    {
        public Cube(Face[] faces)
        {
            this.Faces = faces;
        }

        public Face[] Faces { get; }
    }

    public class Face
    {
        public Face(Sticker[] stickers)
        {
            this.Stickers = stickers;
        }

        public Sticker[] Stickers { get; }
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

    public class ClassicCubeBuilder
    {
        public Cube Build()
        {
            var availableColours = Enum.GetValues(typeof(Colour)).Cast<Colour>().ToArray();
            var faces = new Face[6];

            for (var i = 0; i < 6; i++)
            {
                var colour = availableColours[i];
                var stickers = new Sticker[9];

                for (var j = 0; j < 9; j++)
                {
                    stickers[j] = new Sticker(colour);
                }

                faces[i] = new Face(stickers);
            }

            return new Cube(faces);
        }
    }
}
