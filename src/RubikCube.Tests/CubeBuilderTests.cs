namespace RubikCube.Tests
{
    using NUnit.Framework;

    using RubikCube.Domain;

    [TestFixture]
    public class CubeBuilderTests
    {
        [Test]
        public void TestCubeBuilder()
        {
            // arrange 
            ICubeBuilder cubeBuilder = new CubeBuilder();

            // act 
            var cube = cubeBuilder
                .SetSize(3)
                   .Build();
        }
    }
}