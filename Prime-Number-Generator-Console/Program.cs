using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VariableBase.Mathematics;
using VariableBase.Mathematics.Interfaces;
using VariableBase.Mathematics.Models;
using VariableBase.Mathematics.Algorithms;
using VariableBase.Mathematics.Operators;

namespace Prime_Number_Generator_Console
{
    class Program
    {
        private static Int32 ThreadCount = 1;
        private static Int32 SaveFrequency;
        private static Number NextNumber;
        private static Boolean AutoContinue = false;
        private static IMathEnvironment CurrentMathEnvironment = new CharMathEnvironment(true, true, true, true, true, true, false, false, false, false);

        private static String PrimesSeedFile;
        private static String PrimesSupplementalFile;

        private static String BaseFolderPath = "../../../";

        public static SieveOfEratosthenePrimeAlgorithm PrimeOperator = new SieveOfEratosthenePrimeAlgorithm((NumberSegments foundPrime) =>
        {

        });

        public static IBasicMathAlgorithm GetBasicMath()
        {
            return ((NumberOperator)Number.Operator).BasicMath;
        }

        static void Main(string[] args)
        {
            if (!AutoContinue)
            {
                Console.WriteLine("Welcome to the Variable Base Pime Number Console");


                Console.WriteLine("Please Specify Environment");
                Console.WriteLine("Enter base such as \"0123456789\" or just press enter for P63404");

                var commandMathEnvironmentCommand = Console.ReadLine();
                if (commandMathEnvironmentCommand != "")
                {
                    CurrentMathEnvironment = new CharMathEnvironment(commandMathEnvironmentCommand);
                }

                Console.WriteLine("Environment Base {0}", CurrentMathEnvironment.Base);


                Console.WriteLine("Please Specify Primes Seed File");
                Console.WriteLine("     Press ENTER to use Primes.p{0}", CurrentMathEnvironment.Base);

                IList<String> SeedPrimes = new List<String>();

                var primesSeedFileCommand = Console.ReadLine();
                if (primesSeedFileCommand == "")
                {
                    PrimesSeedFile = String.Format("{0}Primes.p{1}", BaseFolderPath, CurrentMathEnvironment.Base);
                }
                else
                {
                    PrimesSeedFile = String.Format("{0}{1}", BaseFolderPath, primesSeedFileCommand);
                }

                Console.WriteLine("LOADING: Primes Seed File {0}", CurrentMathEnvironment.Base);

                using (FileStream fs = File.Open(PrimesSeedFile, FileMode.OpenOrCreate))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (sr.Peek() >= 0)
                        {
                            SeedPrimes.Add(sr.ReadLine());
                        }
                    }
                }


                Console.WriteLine("LOADED {0} Seed Primes ", SeedPrimes.Count);
                Console.WriteLine("Re-Check Seed Primes?");
                Console.WriteLine("     Press ENTER to Skip");

                String doCheckSeedsCommand = Console.ReadLine().ToUpper();

                if (doCheckSeedsCommand.StartsWith("Y") || doCheckSeedsCommand.StartsWith("S"))
                {
                    var iterativePrimeAlgorithm = new IterativePrimeAlgorithm(CurrentMathEnvironment);
                    var basicMath = new BasicMathAlgorithm();


                    var badPrimes = new List<String>();
                    Parallel.ForEach(SeedPrimes, new ParallelOptions() { MaxDegreeOfParallelism = -1 }, (String uncheckedPrime) =>
                    {
                        if (!iterativePrimeAlgorithm.IsPrime(CurrentMathEnvironment, basicMath, CurrentMathEnvironment.ParseNumberSegments(uncheckedPrime)))
                        {
                            lock (badPrimes)
                            {

                                Console.Write(String.Format("Bad Seed Prime {0}", badPrimes.Count));
                                badPrimes.Add(uncheckedPrime);
                            }
                        }
                        else
                        {
                            Console.WriteLine(uncheckedPrime);
                        }
                    });

                    Console.WriteLine(String.Format("{0} Bad Seed Primes Found", badPrimes.Count));
                }

                Console.WriteLine("Converting... {0} Seed Primes ", SeedPrimes.Count);
                PrimeOperator.LoadSeedPrimes(SeedPrimes.Select(x => CurrentMathEnvironment.ParseNumberSegments(x)).ToList(), CurrentMathEnvironment, GetBasicMath());
                Console.WriteLine("Converted {0} Seed Primes ", SeedPrimes.Count);

                Console.WriteLine("Please Specify Primes Supplemental File");
                Console.WriteLine("     Press ENTER to use Supplemental.p{0}", CurrentMathEnvironment.Base);

                var primesSupplementalFileCommand = Console.ReadLine();
                if (primesSupplementalFileCommand == "")
                {
                    PrimesSupplementalFile = String.Format("{0}Supplemental.p{1}", BaseFolderPath, CurrentMathEnvironment.Base);
                }
                else
                {
                    PrimesSupplementalFile = String.Format("{0}{1}", BaseFolderPath, primesSupplementalFileCommand);
                }

                Console.WriteLine("LOADING: Supplemental Seed File {0}", CurrentMathEnvironment.Base);

                IList<String> SupplementalPrimes = new List<String>();
                using (FileStream fs = File.Open(PrimesSupplementalFile, FileMode.OpenOrCreate))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        while (sr.Peek() >= 0)
                        {
                            SupplementalPrimes.Add(sr.ReadLine());
                        }
                    }
                }

                Console.WriteLine("LOADED {0} Supplemental Primes ", SupplementalPrimes.Count);
                PrimeOperator.LoadSupplementalPrimes(SupplementalPrimes.Select(x => CurrentMathEnvironment.ParseNumberSegments(x)).ToList());
                Console.WriteLine("Converted {0} Supplemental Primes ", SupplementalPrimes.Count);

                Console.WriteLine("How many do you want to try?");
                Console.WriteLine("    Press ENTER for {0}", CurrentMathEnvironment.Base);

                SaveFrequency = (Int32)CurrentMathEnvironment.Base;
                String saveCommand = Console.ReadLine();
                if (saveCommand != "")
                {
                    SaveFrequency = Int32.Parse(saveCommand);
                }

                Console.WriteLine("Auto Continue?");
                Console.WriteLine("    Press ENTER for Yes");

                String autoContinueCommand = Console.ReadLine().ToUpper();
                if (autoContinueCommand == "" || autoContinueCommand.StartsWith("Y"))
                {
                    AutoContinue = true;
                }

                Console.WriteLine("Number of Threads:");
                Console.WriteLine("     Press ENTER to use 1", CurrentMathEnvironment.Base);

                String threadCommand = Console.ReadLine();
                if (!Int32.TryParse(threadCommand, out ThreadCount))
                {
                    ThreadCount = 1;
                }

                Console.WriteLine("Start With Number");
                Console.WriteLine("     Press ENTER to use highest seed prime", CurrentMathEnvironment.Base);
                String startWithCommand = Console.ReadLine();

                Number firstNumber;
                if (startWithCommand == "")
                {
                    if (SeedPrimes.Count() == 0)
                    {
                        firstNumber = CurrentMathEnvironment.SecondNumber;
                    }
                    else
                    {
                        firstNumber = CurrentMathEnvironment.GetNumber(SeedPrimes[SeedPrimes.Count - 1]);
                    }
                }
                else
                {
                    firstNumber = CurrentMathEnvironment.GetNumber(startWithCommand);
                }

                Console.WriteLine("*************************************");
                Console.WriteLine("Starting with: {0} ({1})", firstNumber, firstNumber.GetDecimalArray());
                Console.WriteLine("At:{0}", DateTime.Now);

                if (PrimeOperator.IsPrime(firstNumber.Environment, GetBasicMath(), firstNumber.Segments))
                {
                    Console.WriteLine("Prime:{0}", DateTime.Now);
                }
                else
                {
                    Console.WriteLine("Not Prime:{0}", DateTime.Now);
                }
                Console.WriteLine("*************** GO ******************");
                NextNumber = firstNumber + CurrentMathEnvironment.KeyNumber[1];
            }

            Boolean running = true;
            Int64 checkCount = 0;
            Action<Number> f = (testNumber) =>
            {
                DateTime startTime = DateTime.Now;
                if (PrimeOperator.IsPrime(CurrentMathEnvironment, GetBasicMath(), testNumber.Segments))
                {
                    Console.WriteLine("{0} ({1}) Th:{2} T:{3}", testNumber.GetCharArray(), testNumber.GetDecimalArray(), Thread.CurrentThread.ManagedThreadId, DateTime.Now - startTime);
                }
                checkCount += 1;
                if (checkCount % SaveFrequency == 0)
                {
                    running = false;
                }
            };

            List<Task> tasks = new List<Task>();
            Task t = Task.Run(() =>
            {
                for (var i = 0; i < ThreadCount; i++)
                {
                    Task tItem = Task.Run(() =>
                    {
                        while (running)
                        {
                            NextNumber = NextNumber + CurrentMathEnvironment.KeyNumber[1];
                            f(NextNumber);
                        }
                    });
                    tasks.Add(tItem);
                }

                while (running)
                {
                    Thread.Sleep(10000);
                    foreach (var task in tasks)
                    {
                        if (task.Exception != default(Exception))
                        {

                            using (FileStream fs = File.Open(String.Format("{0}{1}", BaseFolderPath, "Exceptions.txt"), FileMode.Append))
                            {
                                using (StreamWriter sw = new StreamWriter(fs))
                                {
                                    sw.AutoFlush = true;
                                    sw.WriteLine(task.Exception);
                                }
                                Console.WriteLine("**********************************************************");
                                Console.WriteLine("**********************Exception***************************");
                                Console.WriteLine(task.Exception);
                                Console.WriteLine("**********************Exception***************************");
                                Console.WriteLine("**********************************************************");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Thread {0} Status:{1}", task.Id, task.Status);
                        }
                    }
                }
                Console.WriteLine(String.Format("Completed With:{0}", NextNumber));
                Console.WriteLine("*************** STOP ******************");

                Boolean doSave = true;
                if (!AutoContinue)
                {
                    if (Console.ReadLine() == "")
                    {
                        running = false;
                    }


                    Console.WriteLine("Save?");
                    String doSaveCommand = Console.ReadLine().ToUpper();
                    if (doSaveCommand.StartsWith("Y") || doSaveCommand.StartsWith("S"))
                    {
                        doSave = true;
                    }
                    else
                    {
                        doSave = false;
                    }

                    if (doSave)
                    {
                        var definitionFileName = String.Format("{0}/Definition.p{1}", BaseFolderPath, CurrentMathEnvironment.Base);
                        using (FileStream fs = File.Open(definitionFileName, FileMode.Create))
                        {
                            using (StreamWriter sw = new StreamWriter(fs))
                            {
                                sw.WriteLine(CurrentMathEnvironment.GetDefinition());
                            }
                        }

                        Console.WriteLine("Saved as {0}", definitionFileName);

                    }

                   
                }

                if (doSave)
                {
                    var fileName = String.Format("{0}{1}.p{2}", BaseFolderPath, DateTime.Now.Ticks, CurrentMathEnvironment.Base);
                    using (FileStream fs = File.Open(fileName, FileMode.Create))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            foreach (NumberSegments prime in PrimeOperator.PrimeNumbers)
                            {
                                sw.WriteLine(CurrentMathEnvironment.ConvertToString(prime));
                            }
                        }
                    }

                    Console.WriteLine("Saved as {0}", fileName);
                }

                Main(args);
            });

            Console.ReadLine();
        }
    }
}