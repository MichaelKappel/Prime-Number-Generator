using System;
using System.Collections.Generic;
using System.IO;
using VariableBase.Mathematics;

namespace Prime_Number_Generator_Console
{
    class Program
    {
        static void Main(string[] args)
        {
          
            Console.WriteLine("1: find next prime");
            Console.WriteLine("2: find all primes");
            Console.WriteLine("3: Fibonacci");
            Console.WriteLine("4: Fibonacci primes");
            Console.WriteLine("Other: EXIT");

            var env = new CharMathEnvironment("0123456789");

            var command = Console.ReadLine();
            DateTime startTime;
            if (command == "1")
            {
                var rawNumberOfZeros = Console.ReadLine();

                startTime = DateTime.Now;

                var env2 = new CharMathEnvironment();
                Number testNumber2;
                using (Number testNumber = env.GetNumber(Int32.Parse(rawNumberOfZeros)))
                {
                    testNumber2 = testNumber.Convert(env2);
                }

                Boolean stop = false;
                while (!stop)
                {
                    if (testNumber2.IsPrime())
                    {
                        stop = true;
                    }
                    else
                    {
                        testNumber2 += env2.KeyNumber[1];
                    }
                }
                Number prime = testNumber2.Convert(env);
                Console.WriteLine("\nFirst Prime:" + prime);

                TimeSpan span = DateTime.Now - startTime;
                Console.WriteLine("Time:{0}", span);
            }
            else if (command == "2")
            {
                var charEnvironment = new CharMathEnvironment();

                IList<String> primes = new List<String>();
                using (FileStream fs = File.Open("../../../../primes.txt", FileMode.OpenOrCreate))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (sr.Peek() >= 0)
                        {
                            primes.Add(sr.ReadLine());
                        }
                    }
                }

                //using (FileStream fs = File.Open("../../../../primes.txt", FileMode.Append))
                //{

                    Number testNumber2 = charEnvironment.KeyNumber[1];
                    //using (Number testNumber = (primes.Count > 0) ? env.GetNumber(primes[primes.Count - 1]) : env.KeyNumber[0])
                    //{
                    //    testNumber2 = testNumber; //.Convert(env2);
                    //    testNumber2 += charEnvironment.KeyNumber[1];
                    //}

                    //primes.Clear();

                    //using (StreamWriter sw = new StreamWriter(fs))
                    //{
                    //    sw.AutoFlush = true;
                        Boolean stop = false;
                        while (!stop)
                        {
                            startTime = DateTime.Now;
                            if (testNumber2.IsPrime())
                            {
                                //String prime = testNumber2.ToString(env);
                                //sw.WriteLine(prime);

                                TimeSpan span = DateTime.Now - startTime;
                                //Console.WriteLine(prime);
                                Console.WriteLine("Time:{0}", span);
                            }
                            testNumber2 += charEnvironment.KeyNumber[1];
                        }
                    //}
                //}
            }
            else if (command == "3")
            {
                IList<String> fibonaccis = new List<String>();
                using (FileStream fs = File.Open("../../../../Fibonacci.txt", FileMode.OpenOrCreate))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (sr.Peek() >= 0)
                        {
                            fibonaccis.Add(sr.ReadLine());
                        }
                    }
                }

                using (FileStream fs = File.Open("../../../../Fibonacci.txt", FileMode.Append))
                {
                    var env2 = new CharMathEnvironment();
                    
                    if (fibonaccis.Count == 0)
                    {
                        fibonaccis.Add("1");
                        fibonaccis.Add("2");
                    }

                    Number fibonacci1 = env.GetNumber(fibonaccis[fibonaccis.Count - 2]).Convert(env2);
                    Number fibonacci2 = env.GetNumber(fibonaccis[fibonaccis.Count - 1]).Convert(env2);

                    fibonaccis.Clear();

                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.AutoFlush = true;
                        Boolean stop = false;
                        while (!stop)
                        {
                            startTime = DateTime.Now;
                            Number fibonacciNext = fibonacci1 + fibonacci2;
            
                            String fibonacci = fibonacciNext.ToString(env);
                            sw.WriteLine(fibonacci);

                            TimeSpan span = DateTime.Now - startTime;
                            Console.WriteLine(fibonacci);
                            Console.WriteLine("Time:{0}", span);

                            fibonacci1 = fibonacci2;
                            fibonacci2 = fibonacciNext;
                        }
                    }
                }
            }
            else if (command == "4")
            {
                var env2 = new CharMathEnvironment();

                IList<String> fibonaccis = new List<String>();
                using (FileStream fs = File.Open("../../../../FibonacciPrime.txt", FileMode.OpenOrCreate))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (sr.Peek() >= 0)
                        {
                            fibonaccis.Add(sr.ReadLine());
                        }
                    }
                }

                using (FileStream fs = File.Open("../../../../FibonacciPrime.txt", FileMode.Append))
                {
                    Number fibonacci2;
                    Number fibonacci1;
                    if (fibonaccis.Count == 0)
                    {
                        fibonacci1 = env2.KeyNumber[1];
                        fibonacci2 = env2.SecondNumber;
                    }
                    else
                    {
                        String fibonaccisRaw = fibonaccis[fibonaccis.Count - 1];
                        String[] fibonaccisRawArray = fibonaccisRaw.Split(' ');

                        Number fibonacci0 = env.GetNumber(fibonaccisRawArray[0]).Convert(env2);
                        fibonacci1 = env.GetNumber(fibonaccisRawArray[0]).Convert(env2);
                        fibonacci2 = env.GetNumber(fibonaccisRawArray[1]).Convert(env2);

#if DEBUG
                        if (fibonacci0 +  fibonacci1 != fibonacci2)
                        {
                            throw new Exception("fibonacci addition Error");
                        }
                        else if (fibonacci2 - fibonacci1 != fibonacci0)
                        {
                            throw new Exception("fibonacci addition Error");
                        }
#endif
                    }

                    fibonaccis.Clear();

                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.AutoFlush = true;
                        Boolean stop = false;
                        while (!stop)
                        {
                            startTime = DateTime.Now;
                            Number fibonacciNext = fibonacci1 + fibonacci2;
#if DEBUG
                            if (fibonacciNext - fibonacci1 != fibonacci2)
                            {
                                throw new Exception("fibonacci addition Error");
                            }
#endif
                            if (fibonacciNext.IsPrime())
                            {
                                String fibonacci = String.Format("{0} {1} {2}", fibonacci1.ToString(env), fibonacci2.ToString(env), fibonacciNext.ToString(env));
                                sw.WriteLine(fibonacci);
                                Console.WriteLine(fibonacci);

                                TimeSpan span = DateTime.Now - startTime;
                                Console.WriteLine("Time:{0}", span);
                            }
                            fibonacci1 = fibonacci2;
                            fibonacci2 = fibonacciNext;


                        }
                    }
                }
            }
            else
            {
                return;
            }

           
            Main(args);
        }
    }
}
