using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VariableBase.Mathematics;

namespace Prime_Number_Generator
{
    public class PrimeNumberGenerator
    {
        public Number Go(Int32 minSize)
        {
            var env = new MathEnvironment("0123456789");

            var numberSegments = new Char[minSize];
            for (var i = 0; i < minSize - 1; i++)
            {
                numberSegments[i] = env.Bottom;
            }
            numberSegments[numberSegments.Length - 1] = env.First;

            var testNumber = env.GetNumber(numberSegments.ToList());

            while (!testNumber.IsPrime())
            {
                testNumber += testNumber;
            }

            return testNumber;
        }

        public void MillionPlaces()
        {
            var worldsLargestPrime = this.Go(1000000000);

            File.WriteAllText("prime.txt", worldsLargestPrime.ToString());
        }

        public void HundredThousandPlaces()
        {
            var worldsLargestPrime = this.Go(100000000);

            File.WriteAllText("prime.txt", worldsLargestPrime.ToString());
        }

    }
}
