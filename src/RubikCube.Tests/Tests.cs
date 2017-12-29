namespace RubikCube.Tests
{
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestCubeBuilder()
        {
            var cube = new Cube();

            // cube has faces (like sides)
            // face is a grid (3x3) of stickers 
            // sticker has a color

            /*

                stickers are in matrix, like...

            
                FRONT FACE
                    
                  (Y)
                  ^
                  |
                  |  (1,3,0) (2,3,0) (3,3,0)
                  |
                  |  (1,2,0) (2,2,0) (3,2,0) 
                  |  
                  |  (1,1,0) (2,1,0) (3,1,0)
                  |___________________________> (X)  
                  
            
                BACK FACE
                    
                  (Y)
                  ^
                  |
                  |  (1,3,5) (2,3,5) (3,3,5)
                  |                     
                  |  (1,2,5) (2,2,5) (3,2,5) 
                  |       
                  |  (1,1,5) (2,1,5) (3,1,5)
                  |___________________________> (X)  

            
                BOTTOM FACE
                    
                  (Z)
                  ^
                  |
                  |  (1,0,3) (2,0,3) (3,0,3)
                  |     
                  |  (1,0,2) (2,0,2) (3,0,2) 
                  |     
                  |  (1,0,1) (2,0,1) (3,0,1)
                  |___________________________> (X)  
                  

                TOP FACE
                    
                  (Z)
                  ^
                  |
                  |  (1,5,3) (2,5,3) (3,5,3)
                  |     
                  |  (1,5,2) (2,5,2) (3,5,2) 
                  |     
                  |  (1,5,1) (2,5,1) (3,5,1)
                  |___________________________> (X)  
                  

            // todo

                LEFT FACE
                    
                  (Y)
                  ^
                  |
                  |  (1,5,3) (2,5,3) (3,5,3)
                  |     
                  |  (1,5,2) (2,5,2) (3,5,2) 
                  |     
                  |  (1,5,1) (2,5,1) (3,5,1)
                  |___________________________> (Z)  


                RIGHT FACE
                    
                  (Y)
                  ^
                  |
                  |  (1,5,3) (2,5,3) (3,5,3)
                  |     
                  |  (1,5,2) (2,5,2) (3,5,2) 
                  |     
                  |  (1,5,1) (2,5,1) (3,5,1)
                  |___________________________> (Z)  
                   
                    
                

             
             
             */



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
    }

    public class Cube
    {
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
}