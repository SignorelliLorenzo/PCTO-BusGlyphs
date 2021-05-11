using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using AForgeFunctions;
using System.Drawing;

namespace Test_Base
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IsImagePathCorrect()
        {
            //Arrange
            int result = 0;
            string test = $@"05.jpg";

            //Act
            result = Funzioni.FindG(test);

            //Assert
            Assert.AreNotEqual(-1, result);
        }
        [TestMethod]
        public void IsGlyphImageStringEmpty()
        {
            //Arrange
            int result = 0;
            string test = $@"black.jpg";

            //Act
            result = Funzioni.FindG(test);

            //Assert
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void IsGlyphImageStringNotEmpty()
        {
            //Arrange
            int result = 0;
            string test = $@"05.jpg";

            //Act
            result = Funzioni.FindG(test);

            //Assert
            Assert.AreNotEqual(0, result);
            Assert.AreNotEqual(-1, result);

        }

        [TestMethod]
        public void IsGlyphImageBitmapTrue()
        {
            //Arrange
            bool check = false;
            Bitmap Imagetest = new Bitmap(Directory.GetCurrentDirectory() + $@"\05.jpg", true);

            //Act
            check = Funzioni.FindG(Imagetest);

            //Assert
            Assert.IsTrue(check);
        }
        [TestMethod]
        public void IsGlyphImageBitmapFalse()
        {
            //Arrange
            bool check = false;
            Bitmap Imagetest = new Bitmap(Directory.GetCurrentDirectory() + $@"\03.jpg", true);

            //Act
            check = Funzioni.FindG(Imagetest);

            //Assert
            Assert.IsFalse(check);
        }

        [TestMethod]
        public void IsGlyphImageApplied()
        {
            //Arrange
            bool check = false;
            Bitmap Imagetest = new Bitmap(Directory.GetCurrentDirectory() + $@"\05.jpg", true);

            //Act
            check = Funzioni.FindG(Imagetest);

            //Assert
            Assert.IsTrue(check);

        }


        [TestMethod]
        public void IsGlyphNameFound()
        {
            //Arrange
            string Name;
            Bitmap Imagetest = new Bitmap(Directory.GetCurrentDirectory() + $@"\05.jpg", true);

            //Act

            Name = Funzioni.FindGlyphName(Imagetest);

            //Assert
            Assert.IsNotNull(Name);



        }

       
    }
}
