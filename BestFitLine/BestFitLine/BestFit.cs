using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace BestFitLine
{
    static class BestFit
    { 
        /// <summary>
        /// The main method. Runs getBestFit and times it.
        /// </summary>
        /// <param name="meh"> Console input </param>
        static void Main(String[] meh)
        {
            String filePath = getPath();
            int order = getorder();
            Stopwatch s = new Stopwatch();
            s.Start();
            getBestFit(filePath, order);
            s.Stop();
            Console.WriteLine("\nTime: "+s.ElapsedMilliseconds);
            Console.ReadKey();
        }

        public static int getorder()
        {
            while (true)
            {
                Console.Write("Order of trendline: ");
                String input = Console.ReadLine();
                try
                {
                    int order = int.Parse(input);
                    if (order>4||order<1)
                    {
                        throw new ArgumentException();
                    }
                    return order;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please enter a valid integer between 1 and 4");
                }
            }
        }

        public static string getPath()
        {
            while (true)
            {
                Console.Write("Filepath to Operate On: ");
                String input = Console.ReadLine();
                if (File.Exists(input))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("File does not exist. Please Try Again.");
                }
            }
        }

        /// <summary>
        /// Gets and prints the best fit coefficients from file input
        /// Basically a main method
        /// </summary>
        public static void getBestFit(String filePath, int order)
        {
            List<double> xValues = new List<double>();
            List<double> yValues = new List<double>();

            //file I/O stuff
            String[] lines = File.ReadAllLines(filePath);
            foreach (String line in lines)
            {
                String[] vals = line.Split('\t');
                for(int ii=0; ii<vals.Length; ii++)
                {
                    if (vals[ii].Equals(""))
                    {

                    }
                    else if (ii%2==0)
                    {
                        xValues.Add(double.Parse(vals[ii]));
                    }
                    else
                    {
                        yValues.Add(double.Parse(vals[ii]));
                    }
                }
            }

            //easier and faster to access
            double[] xVals = xValues.ToArray();
            double[] yVals = yValues.ToArray();

            //sets the square matrix
            double[][] origMatrix = new double[order+1][];
            for (int ii=0; ii<origMatrix.Length; ii++)
            {
                origMatrix[ii] = new double[order+1];
                for (int jj = 0; jj < origMatrix[ii].Length; jj++)
                {
                    origMatrix[ii][jj] = mean(xVals, order + ii - jj);
                }
            }

            origMatrix =Matrix.invertWithRowReduction(origMatrix);

            double[][] yMatrix = new double[order+1][];

            //sets the ymatrix
            for (int ii=0; ii<yMatrix.Length; ii++)
            {
                yMatrix[ii] = new double[1];
                yMatrix[ii][0] = mean(yVals, xVals, 1, ii);
            }
            Console.WriteLine();

            //multiplies the inverse across
            double[][] result=Matrix.multiply(origMatrix, yMatrix);
            double[] coefficients = new double[result.Length];
            for (int ii=0; ii<coefficients.Length; ii++)
            {
                coefficients[ii] = result[result.Length-ii-1][0];
            }
            //prints
            Console.WriteLine("Values:");
            for (int ii=coefficients.Length-1; ii>=0; ii--)
            {
                Console.WriteLine(coefficients[ii] + " x^"+(ii));
            }
            Console.WriteLine();
            double RSQ = calculateRSquared(xVals, yVals, coefficients);
            Console.WriteLine("R squared="+RSQ);
        }
        
        /// <summary>
        /// Takes the mean of a set of numbers to a given power
        /// </summary>
        /// <param name="nums"> The number set to be exponentiated and summed</param>
        /// <param name="power"> The power</param>
        /// <returns> The mean </returns>
        public static double mean(double[] nums, int power)
        {
            double total = 0;
            foreach(double num in nums)
            {
                total += Math.Pow(num, power);
            }
            return total / nums.Length;
        }

        /// <summary>
        /// Takes the mean of combinations of numbers to powers
        /// </summary>
        /// <param name="nums1"> The first set of numbers </param>
        /// <param name="nums2"> The second set of numbers</param>
        /// <param name="power1"> The power for nums1 </param>
        /// <param name="power2"> The power for nums2 </param>
        /// <returns>The mean</returns>
        public static double mean(double[] nums1, double[] nums2, int power1, int power2)
        {
            if (nums1.Length!=nums2.Length)
            {
                throw new ArgumentException("Lists not same size.");
            }
            double total = 0;
            for (int ii=0; ii<nums1.Length; ii++) 
            {
                total += (Math.Pow(nums1[ii], power1) * Math.Pow(nums2[ii], power2));
            }
            return total / nums1.Length;
        }

        /// <summary>
        /// Calculates the R Squared Value for a polynomial trendline
        /// </summary>
        /// <param name="xVals"> The X Values of all points in the set</param>
        /// <param name="yVals"> The Y Values of all points in the set, in the same order as the x values</param>
        /// <param name="coefficients"> The coefficients of the polynomial trendline, from highest to lowest order</param>
        /// <returns>An R squared value for the line</returns>
        public static double calculateRSquared(double[] xVals, double[] yVals, double[] coefficients)
        {
            if (xVals.Length!=yVals.Length)
            {
                throw new ArgumentException("Failure in R Squared.");
            }
            double ymean = mean(yVals, 1);
            double totalSum = 0;
            for (int ii=0; ii<xVals.Length; ii++)
            {
                totalSum += Math.Pow(yVals[ii] - ymean, 2);
            }
            double resSum = 0;
            for (int ii=0; ii<xVals.Length; ii++)
            {
                double estVal = 0;
                for (int jj=0; jj<coefficients.Length; jj++)
                {
                    estVal += coefficients[jj]*Math.Pow(xVals[ii], jj);
                }
                resSum += Math.Pow(yVals[ii] - estVal, 2);
            }
            return 1 - (resSum / totalSum);
        }
    }
}
