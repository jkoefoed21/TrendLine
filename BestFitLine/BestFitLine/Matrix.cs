using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestFitLine
{
    static class Matrix
    {
        public static double[,] invert (double[,] mat) //Major problem here
        {
            print(mat);
            //print(copyAround(mat, 1, 2));
            //Console.WriteLine(determinant(copyAround(mat, 1, 2)));
            double det = 1.0/determinant(mat);
            Console.WriteLine(det);
            double[,] oldmat = mat;
            mat = new double[oldmat.GetLength(0), oldmat.GetLength(1)];
            for (int ii = 0; ii < mat.GetLength(0); ii++)
            {
                for (int jj = 0; jj < mat.GetLength(1); jj++)
                {
                    mat[ii,jj] = determinant(copyAround(oldmat, ii, jj));
                }
            }
            print(mat);
            transpose(mat);
            print(mat);
            adjunct(mat);
            print(mat);
            multiply(mat, det);
            Console.WriteLine("Orig 1");
            print(mat);
            return mat;
        }


        public static double determinant (double[,] mat)
        {
            if (mat.GetLength(0)!=mat.GetLength(1))
            {
                throw new ArgumentException();
            }
            if (mat.GetLength(0)==1)
            {
                return mat[0, 0];
            }
            if (mat.GetLength(0)==2)
            {
                return mat[0,0] * mat[1,1] - mat[0,1] * mat[1,0];
            }
            else
            {
                double total = 0;
                for (int ii=0; ii<mat.GetLength(1); ii++)
                {
                    double val = Math.Pow(-1, ii) * mat[0,ii]*determinant(copyAround(mat, 0, ii));
                    total += val;
                }
                return total;
            }
        }

        public static double determinant (double[,] mat, double row, double col)
        {
            return determinant(copyAround(mat, row, col));
        }

        public static void transpose (double[,] mat)
        {
            double[,] newMat = new double[mat.GetLength(0),mat.GetLength(1)];
            for (int ii=0; ii<mat.GetLength(0); ii++)
            {
                for (int jj=0; jj<mat.GetLength(1); jj++)
                {
                    newMat[ii,jj] = mat[jj,ii];
                }
            }
            for (int ii = 0; ii < mat.GetLength(0); ii++)
            {
                for (int jj = 0; jj < mat.GetLength(1); jj++)
                {
                    mat[ii, jj] = newMat[ii, jj];
                }
            }
        }

        public static void adjunct (double [,] mat)
        {
            for (int ii = 0; ii < mat.GetLength(0); ii++)
            {
                for (int jj = 0; jj < mat.GetLength(1); jj++)
                {
                    mat[ii,jj] = Math.Pow(-1, (ii+ jj)%2)*mat[ii,jj];
                }
            }
        }

        public static void multiply (double[,] mat, double value)
        {
            for (int ii = 0; ii < mat.GetLength(0); ii++)
            {
                for (int jj = 0; jj < mat.GetLength(1); jj++)
                {
                    mat[ii,jj] = mat[ii,jj]*value;
                }
            }
        }

        public static double[,] copyAround (double[,] mat, double row, double col) //working
        {
            /*
            for (int ii=0; ii<mat.GetLength(); ii++)
            {
                for (int jj=0; jj<mat.GetLength(); jj++)
                {
                    Console.Write(mat[ii,jj]+" ");
                }
                Console.WriteLine();
            }*/
            double[,] newMat = new double[mat.GetLength(0)-1, mat.GetLength(1)-1];

            for (int ii=0; ii<newMat.GetLength(0); ii++)
            {
                for (int jj=0; jj<newMat.GetLength(1); jj++)
                {
                    newMat[ii,jj] = mat[ii < row ? ii : ii + 1,jj<col ? jj :jj+1];
                }
            }
            /*
            for (int ii = 0; ii < newMat.GetLength(); ii++)
            {
                for (int jj = 0; jj < newMat.GetLength(); jj++)
                {
                    Console.Write(newMat[ii,jj] + " ");
                }
                Console.WriteLine();
            }*/
            return newMat;
        }
        public static void print(double[,] mat)
        {
            for (int ii = 0; ii < mat.GetLength(0); ii++)
            {
                for (int jj = 0; jj < mat.GetLength(1); jj++)
                {
                    Console.Write(mat[ii,jj] + " ");
                }
                Console.WriteLine();
            }
        }

        public static double[,] multiply(double[,] mat1, double[,] mat2)
        {
            if (mat1.GetLength(1)!=mat2.GetLength(0))
            {
                Console.WriteLine(mat1.GetLength(0) + " " + mat2.GetLength(1));
                throw new ArgumentException();
            }
            double[,] result = new double[mat1.GetLength(0), mat2.GetLength(1)];
            for (int ii=0; ii<mat1.GetLength(0); ii++)
            {
                for (int jj=0; jj<mat2.GetLength(1); jj++)
                {
                    double total = 0;
                    for (int kk=0; kk<mat1.GetLength(1); kk++)
                    {
                        total += mat1[ii, kk] * mat2[kk, jj];
                    }
                    result[ii, jj] = total;
                }
            }
            return result;
        }
    }
}
