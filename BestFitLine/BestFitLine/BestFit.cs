using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace BestFitLine
{
    class BestFit
    {
        public static readonly int ORDER = 3; 

        public static String filePath = "C:\\Users\\Jack Koefoed\\OneDrive\\12\\Multi\\bestfit_dataset_example.txt";

        static void Main(String[] args)
        {
            /*
             double[][] origMat = new double[][] 
             { 
                 new double[] { 1, 3, 1 }, 
                 new double[] { 2, 1, 9 },
                 new double[] { 3, 2, 1 },
                 //new double[] { 3, 5, 2}
             };
             //Matrix.print(origMat);
            Stopwatch s = new Stopwatch();
            s.Start();
            for (int ii = 0; ii < ITERATIONS; ii++)
            {
                origMat = Matrix.invert(origMat);
            }
            Console.WriteLine(s.ElapsedMilliseconds);
            s.Restart();
            //Matrix.print(origMat);
            //Matrix.print(origMat);
            origMat = new double[][]
            {
                 new double[] { 1, 3, 1 },
                 new double[] { 2, 1, 9 },
                 new double[] { 3, 2, 1 },
            };
            for (int ii = 0; ii < ITERATIONS; ii++)
            {
                origMat = Matrix.invertWithRowReduction(origMat);
            }
            Console.WriteLine(s.ElapsedMilliseconds);
            Console.ReadKey();
             */
            Stopwatch s = new Stopwatch();
            s.Start();
            getBestFit();
            s.Stop();
            Console.WriteLine("\nTime: "+s.ElapsedMilliseconds);
            //Console.WriteLine(Matrix.x);
            Console.ReadKey();
        }

        static void getBestFit()
        {
            List<double> xValues = new List<double>();
            List<double> yValues = new List<double>();

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

            double[] xVals = xValues.ToArray();
            double[] yVals = yValues.ToArray();

            double[][] origMatrix = new double[ORDER][];
            for (int ii=0; ii<origMatrix.Length; ii++)
            {
                origMatrix[ii] = new double[ORDER];
                for (int jj = 0; jj < origMatrix[ii].Length; jj++)
                {
                    origMatrix[ii][jj] = mean(xVals, ORDER - 1 + ii - jj);
                }
            }
            //Matrix.print(newMat);
            origMatrix =Matrix.invertWithRowReduction(origMatrix);

            double[][] yMatrix = new double[ORDER][];

            for (int ii=0; ii<yMatrix.Length; ii++)
            {
                yMatrix[ii] = new double[1];
                yMatrix[ii][0] = mean(yVals, xVals, 1, ii);
            }
            //Console.WriteLine("Orig 2");
            //Matrix.print(origMatrix);
            //Matrix.print(newMat);
            //Console.WriteLine("Check Inverse");
            //Matrix.print(Matrix.multiply(origMatrix, newMat));
            Console.WriteLine();

            //Matrix.print(yMatrix);

            double[][] result=Matrix.multiply(origMatrix, yMatrix);

            Console.WriteLine("Values:");
            for (int ii=0; ii<result.Length; ii++)
            {
                Console.WriteLine(result[ii][0]);
            }
            /*
            double[][] lol = { { 1, 3, 1 , 5 }, { 2, 1, 0 , 6 },{ 3, 2, 1 , 7 },{ 3, 5, 2, 7 } };
            double[][] lmao = { { 1 }, { 2 }, { 4 }, { 3 } };
            double[][] rofl = { { 2, 3 }, { 5, 6 } };
            //Matrix.invert(lol);
            Matrix.print(Matrix.multiply(lol, lmao));
            Console.ReadKey();*/
        }
        
        public static double mean(double[] nums, int power)
        {
            double total = 0;
            foreach(double num in nums)
            {
                total += Math.Pow(num, power);
            }
            return total / nums.Length;
        }

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
    }
}
