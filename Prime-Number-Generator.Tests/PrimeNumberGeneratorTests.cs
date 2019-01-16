using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Prime_Number_Generator.Tests
{
    [TestClass]
    public class PrimeNumberGeneratorTests
    {
        [TestMethod]
        public void MillionPlaces_Test()
        {

            var g = new PrimeNumberGenerator();
            g.MillionPlaces();
        }


        [TestMethod]
        public void HundredThousandPlaces_Test()
        {

            var g = new PrimeNumberGenerator();
            g.HundredThousandPlaces();
        }
    }
}
