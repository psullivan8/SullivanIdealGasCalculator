//Patrik Sullivan psullivan8@cnm.edu
//SullivanIdealGasCalculator 05/26/2020

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SullivanIdealGasCalculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Declare variables
            int count;
            double mass = 0;
            double vol = 0;
            double temp = 0;
            double pressure = 0;
            string doAnother = null;
            string gasName = null;
            string[] gasNames = { };
            double[] molecularWeights = { };
            DisplayHeader();
            GetMolecularWeights(ref gasNames, ref molecularWeights, out count);
            int countGases = count;
            DisplayGasNames(gasNames, countGases);
            //do while loop that runs as long as user chooses to continue.
            do
            {
                try
                {
                    //Check for user input and return molecular weight if valid. If gas name is not found
                    //method returns 1 and asks user if they want to try another one.
                    double molecularWeight = GetMolecularWeightFromName(gasName, gasNames, molecularWeights, countGases);
                    if (molecularWeight != 1)
                    {
                        //Ask user for mass, temp, vol, and pass molecularWeight for pressure calculation.
                        Console.Write("Enter the mass in grams: ");
                        mass = double.Parse(Console.ReadLine());
                        Console.Write("Enter the temperature in Celcius: ");
                        temp = double.Parse(Console.ReadLine());
                        Console.Write("Enter the volume in cubic meters: ");
                        vol = double.Parse(Console.ReadLine());
                        //method calculates pressure in Pascals and returns value.
                        pressure = Pressure(mass, vol, temp, molecularWeight);
                        //Displays pressure in Pascals and PSI.
                        DisplayPresure(pressure);
                        //Asks user if they would like to do another. If answer is "y", the loop continues.
                        //If the answer is not "y", we exit the loop and say goodbye.
                        Console.Write("Would you like to do another? y/n ");
                        doAnother = Console.ReadLine();
                        continue;
                    }
                    else
                    {
                        //If gas name is not found in array of gasNames we continue the loop. 
                        doAnother = "y";
                    }
                }
                catch
                {
                    Console.WriteLine("Sorry something went wrong.");
                }
            } while (doAnother == "y");
            //If user answer is not "y" we exit the loop and say goodbye.
            if (doAnother != "y")
            {
                Console.WriteLine("\nThank you for using this program. Goodbye.");
            }
        }
        static void DisplayHeader()
        {
            Console.WriteLine("Patrik Sullivan" +
                "\nIdeal Gas Calculator" +
                "\n\nThis program calculates the pressure exerted by gas in a container.\n");
        }
        public static void GetMolecularWeights(ref string[] gasNames, ref double[] molecularWeights, out int count)
        {
            string line;
            count = 0;

            List<string> names = new List<string>();
            List<double> weights = new List<double>();
            //Opens StreamReader to read the file in.
            StreamReader file = new StreamReader(@"MolecularWeightsGasesAndVapors.csv");
            //Skips the first line of data from the input file.
            string header = file.ReadLine();
            //While loop that iterates over the values until the end of the file is reach.
            while ((line = file.ReadLine()) != null)
                try
                {
                    //Splitting values at each instance of the comma character.
                    String[] gas = line.Split(',');
                    //As the loop iterates, each value is placed in the appropriate array one at a time.
                    names.Add(gas[0]);
                    weights.Add(double.Parse(gas[1]));
                    count++;
                }
                catch (Exception)
                {

                }

            gasNames = names.ToArray();
            molecularWeights = weights.ToArray();
        }

        private static void DisplayGasNames(string[] gasNames, int countGases)
        {
            int count = 0;
            for (int i = 0; i < gasNames.Length; i++)
            {
                Console.Write(String.Format("{0,-25}", gasNames[i]));
                count++;
                if (i % 3 == 2)
                    Console.WriteLine();
                count = 0;
            }
            countGases = gasNames.Length - 1;
        }

        private static double GetMolecularWeightFromName(string gasName, string[] gasNames, double[] molecularWeights, int countGases)
        {
            Console.Write("Enter the gas name: ");
            try
            {
                gasName = Console.ReadLine();
                int gasIndex = Array.FindIndex(gasNames, gas => gas == gasName);
                double gasWeight = molecularWeights[gasIndex];
                return gasWeight;
            }
            catch (Exception)
            {
                Console.WriteLine("That gas could not be found!");
                return 1;
            }

        }

        static double Pressure(double mass, double vol, double temp, double molecularWeight)
        {
            double celcius = temp;
            double kelvin = 0;
            double r = 8.3145;
            double n;
            n = NumberOfMoles(mass, molecularWeight);
            kelvin = CelciusToKelvin(celcius);
            double pascals;
            pascals = ((n * r * kelvin) / vol);

            return pascals;
        }

        static double NumberOfMoles(double mass, double molecularWeight)
        {
            double n;
            n = mass / molecularWeight;
            return n;
        }

        public static double CelciusToKelvin(double celcius)
        {
            double kelvin;
            kelvin = celcius += 273;
            return kelvin;
        }

        static double PaToPSI(double pascals)
        {
            double psiConvert = 0.000145;
            double pressure = pascals * psiConvert;

            return pressure;
        }

        public static void DisplayPresure(double pressure)
        {
            double psiDisplay;
            psiDisplay = PaToPSI(pressure);
            Console.WriteLine("Pressure in Pascals is: " + pressure);
            Console.WriteLine("Pressure in PSI is: " + psiDisplay);
        }
    }
}