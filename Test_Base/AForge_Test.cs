using System;
using Xunit;
using Moq;
using Glyphs;
using FluentAssertions;
using AForge.Vision.GlyphRecognition;


namespace Test_Base
{
    public class AForge_Test
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            int result = 0;
            string test = "05";

            //Act
            result = Funzioni.FindG(test);

            //Assert
            result.Should().NotBe(-1);          
        }
        [Fact]
        public void Test2()
        {
            //Arrange
            int result = 0;
            string test = "black";

            //Act
            result = Funzioni.FindG(test);

            //Assert
            result.Should().Be(0);
        }
    }
}
