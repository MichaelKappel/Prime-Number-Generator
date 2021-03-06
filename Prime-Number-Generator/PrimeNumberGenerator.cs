﻿using System;
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
            var env = new CharMathEnvironment("0123456789");

            Number testNumber = env.GetNumber(minSize);

            while (!testNumber.IsPrime())
            {
                testNumber += env.KeyNumber[1];
            }

            return testNumber;
        }

        public Number MillionPlaces()
        {
            var prime = this.Go(1000000000);

            File.WriteAllText("../../../../prime1000000000.txt", prime.ToString());
            
            return prime;
        }

        public Number HundredThousandPlaces()
        {
            Number prime = this.Go(100000000);

            File.WriteAllText("../../../../prime100000000.txt", prime.ToString());

            return prime;
        }

        public Number HundredPlaces()
        {
            Number prime = this.Go(100);

            File.WriteAllText("../../../../prime100.txt", prime.ToString());

            return prime;
        }
    }
}
