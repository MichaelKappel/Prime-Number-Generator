using Microsoft.VisualStudio.TestTools.UnitTesting;
using VariableBase.Mathematics;

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


        [TestMethod]
        public void HundredPlaces_Test()
        {

            var g = new PrimeNumberGenerator();
            Number divisor = g.HundredPlaces();

        }


    }
}
