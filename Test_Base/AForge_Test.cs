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
        public void IsImagePathCorrect()
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
        public void IsGlyphImage()
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
