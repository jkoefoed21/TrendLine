using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestFitLine
{
    static class Matrix
    {
        public static double[][] invert (double[][] mat) //Major problem here
        {
            //print(mat);
            //print(copyAround(mat, 1, 2));
            //Console.WriteLine(determinant(copyAround(mat, 1, 2)));
            double det = 1.0/determinant(mat);
            //Console.WriteLine(det);
            double[][] oldmat = mat;
            mat = new double[oldmat.Length][];
            for (int ii = 0; ii < mat.Length; ii++)
            {
                mat[ii] = new double[oldmat[ii].Length];
                for (int jj = 0; jj < mat[ii].Length; jj++)
                {
                    mat[ii][jj] = determinant(copyAround(oldmat, ii, jj));
                }
            }
            //print(mat);
            transpose(mat);
            //print(mat);
            adjunct(mat);
            //print(mat);
            multiply(mat, det);
            Console.WriteLine("Orig 1");
            //print(mat);
            return mat;
        }


        public static double determinant (double[][] mat)
        {
            if (mat.Length!=mat[0].Length) //not safe but is a compromise between safety and speed
            {
                throw new ArgumentException();
            }
            if (mat.Length==1)
            {
                return mat[0][ 0];
            }
            if (mat.Length==2)
            {
                return mat[0][0] * mat[1][1] - mat[0][1] * mat[1][0];
            }
            else
            {
                double total = 0;
                for (int ii=0; ii<mat[0].Length; ii++)
                {
                    double val = Math.Pow(-1, ii) * mat[0][ii]*determinant(copyAround(mat, 0, ii));
                    total += val;
                }
                return total;
            }
        }

        public static double determinant (double[][] mat, double row, double col)
        {
            return determinant(copyAround(mat, row, col));
        }

        public static void transpose (double[][] mat)
        {
            double[][] newMat = new double[mat.Length][];
            for (int ii=0; ii<mat.Length; ii++)
            {
                newMat[ii] = new double[mat[ii].Length];
                for (int jj=0; jj<mat[ii].Length; jj++)
                {
                    newMat[ii][jj] = mat[jj][ii];
                }
            }
            for (int ii = 0; ii < mat.Length; ii++)
            {
                for (int jj = 0; jj < mat[ii].Length; jj++)
                {
                    mat[ii][ jj] = newMat[ii][ jj];
                }
            }
        }

        public static void adjunct (double [][] mat)
        {
            for (int ii = 0; ii < mat.Length; ii++)
            {
                for (int jj = 0; jj < mat[ii].Length; jj++)
                {
                    mat[ii][jj] = Math.Pow(-1, (ii+ jj)%2)*mat[ii][jj];
                }
            }
        }

        public static void multiply (double[][] mat, double value)
        {
            for (int ii = 0; ii < mat.Length; ii++)
            {
                for (int jj = 0; jj < mat[ii].Length; jj++)
                {
                    mat[ii][jj] = mat[ii][jj]*value;
                }
            }
        }

        public static double[][] copyAround (double[][] mat, double row, double col) //working
        {
            double[][] newMat = new double[mat.Length-1][];

            for (int ii=0; ii<newMat.Length; ii++)
            {
                newMat[ii] = new double[mat[ii].Length - 1];
                for (int jj=0; jj<newMat[ii].Length; jj++)
                {
                    newMat[ii][jj] = mat[ii < row ? ii : ii + 1][jj<col ? jj :jj+1];
                }
            }
            return newMat;
        }
        public static void print(double[][] mat)
        {
            for (int ii = 0; ii < mat.Length; ii++)
            {
                for (int jj = 0; jj < mat[ii].Length; jj++)
                {
                    Console.Write(mat[ii][jj] + " ");
                }
                Console.WriteLine();
            }
        }

        public static double[][] multiply(double[][] mat1, double[][] mat2)
        {
            if (mat1[0].Length!=mat2.Length)
            {
                Console.WriteLine(mat1.Length + " " + mat2[0].Length);
                throw new ArgumentException();
            }
            double[][] result = new double[mat1.Length][];
            for (int ii=0; ii<mat1.Length; ii++)
            {
                result[ii] = new double[mat2[ii].Length];
                for (int jj=0; jj<mat2[ii].Length; jj++)
                {
                    double total = 0;
                    for (int kk=0; kk<mat1[ii].Length; kk++)
                    {
                        total += mat1[ii][kk] * mat2[kk][jj];
                    }
                    result[ii][ jj] = total;
                }
            }
            return result;
        }
    }
}
