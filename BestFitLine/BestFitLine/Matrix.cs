using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestFitLine
{
    static class Matrix
    {
        /// <summary>
        /// Takes the inverse of a matrix without changing the underlying matrix using the matrix of minors
        /// </summary>
        /// <param name="mat">The matrix to be inverted</param>
        /// <returns>The inverted matrix</returns>
        public static double[][] invertUsingMinors (double[][] mat) //Terrible one it gets big.
        {
            if (mat.Length!=mat[0].Length)
            {
                throw new ArgumentException();
            }
            double det = 1.0/determinant(mat);
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
            mat=transpose(mat);
            adjunct(mat);
            multiply(mat, det);
            return mat;
        }

        /// <summary>
        /// Takes the determinant of a square matrix
        /// </summary>
        /// <param name="mat">The matrix</param>
        /// <returns>The determinant</returns>
        public static double determinant (double[][] mat) //if mat dimensions get 8-9ish, this gets reallll sloowwwwww
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
                for (int ii=0; ii<mat[0].Length; ii++) //this recurses too much if determinant is 8-9ish
                {
                    double val = Math.Pow(-1, ii) * mat[0][ii]*determinant(mat, 0, ii);
                    total += val;
                }
                return total;
            }
        }

        /// <summary>
        /// Takes the determinant of a matrix around a specific value of the matrix
        /// </summary>
        /// <param name="mat">The matrix</param>
        /// <param name="row">The row of the value</param>
        /// <param name="col">The column of the value</param>
        /// <returns>The determinant</returns>
        public static double determinant (double[][] mat, double row, double col)
        {
            return determinant(copyAround(mat, row, col));
        }

        /// <summary>
        /// Transposes a matrix without changing the input matrix
        /// </summary>
        /// <param name="mat"> The matrix to be transposed</param>
        /// <returns> The transposed matrix </returns>
        public static double[][] transpose (double[][] mat) //this is shite
        {
            double[][] newMat = new double[mat[0].Length][];
            for (int ii=0; ii<newMat.Length; ii++)
            {
                newMat[ii] = new double[mat.Length];
                for (int jj=0; jj<newMat[ii].Length; jj++)
                {
                    newMat[ii][jj] = mat[jj][ii];
                }
            }
            return newMat;
        }

        /// <summary>
        /// Checkboards a matrix IN-PLACE
        /// </summary>
        /// <param name="mat"> </param>
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

        /// <summary>
        /// Multiplies a matrix by a scalar
        /// </summary>
        /// <param name="mat">The matrix</param>
        /// <param name="value">The scalar</param>
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

        /// <summary>
        /// Creates a new matrix consisting of the original matrix without a row and a column
        /// </summary>
        /// <param name="mat"> The old matrix</param>
        /// <param name="row"> The row to be eliminated </param>
        /// <param name="col"> The column to be eliminated </param>
        /// <returns> The new matrix </returns>
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

        /// <summary>
        /// Prints a representation of a matrix to the console 
        /// </summary>
        /// <param name="mat"> The matrix to be printed </param>
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

        /// <summary>
        /// Multiplies a matrix by a matrix 
        /// </summary>
        /// <param name="mat1">The first matrix</param>
        /// <param name="mat2">The second matrix</param>
        /// <returns>The resulting matrix</returns>
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
        
        /// <summary>
        /// Uses row reduction to invert a matrix
        /// </summary>
        /// <param name="mat"></param>
        /// <returns></returns>
        public static double[][] invertWithRowReduction (double[][] mat) //ALWAYS FASTER THAN CRAMERS RULE
        {
            double[][] bigMat = new double[mat.Length][];
            for (int ii=0; ii<mat.Length; ii++)
            {
                if (mat[ii].Length!=mat.Length)
                {
                    throw new ArgumentException("Cannot invert non-square matrix");
                }
                bigMat[ii] = new double[2*mat[ii].Length];
                for (int jj=0; jj<mat[ii].Length; jj++)
                {
                    bigMat[ii][jj] = mat[ii][jj];
                    if (ii==jj)
                    {
                        bigMat[ii][mat[ii].Length+jj] = 1;
                    }
                }
            }
            //this cascades the left side of bigMat to eliminate the bottom-left half of the orig matrix.
            for (int ii=0; ii<bigMat[0].Length/2-1; ii++) //ii is column to eliminate
            {
                for (int jj=1+ii; jj<bigMat.Length; jj++) //jj is row to eliminate
                {
                    reduceRow(bigMat[jj], bigMat[ii], ii);
                }
            } 
            //this then cascades everything back up, working column by column all the way up
            //leaving only the diagonals
            for (int ii=bigMat[0].Length/2-1; ii>0; ii--)//value to come back to
            {
                for (int jj=ii-1; jj>-1; jj--) //row being operated on
                {
                    reduceRow(bigMat[jj], bigMat[ii], ii);
                }
                //Console.WriteLine(ii);
            }
            //this then gets the diagonal to be all ones
            for (int ii=0; ii<bigMat.Length; ii++)
            {
                divideRow(bigMat[ii], ii);
            }

            double[][] outMat = new double[mat.Length][];

            for(int ii=0; ii<outMat.Length; ii++)
            {
                outMat[ii] = new double[mat[ii].Length];
                for (int jj=0; jj<outMat[ii].Length; jj++)
                {
                    outMat[ii][jj] = bigMat[ii][jj + outMat[ii].Length];
                }
            }

            double[][] checkInv = multiply(multiply(outMat, mat), mat);
            //print(checkInv);
            //print(mat);
            if (checkInv.Length!=mat.Length)
            {
                throw new ArgumentException("Check Inv length does not match original matrix dimensions");
            }
            for(int ii=0; ii<checkInv.Length; ii++)
            {
                if (checkInv[ii].Length != mat[ii].Length)
                {
                    throw new ArgumentException("Check Inv length does not match original matrix dimensions");
                }
                for (int jj=0; jj<checkInv.Length; jj++)
                {
                    //if (Math.Abs(checkInv[ii][jj]/mat[ii][jj]))
                    {
                        //throw new ArgumentException("Error inverting matrix");
                    }
                }
            }

            return outMat;
        }

        /// <summary>
        /// Adds an array to another to make a specified value of the array equal to zero.
        /// </summary>
        /// <param name="row">The row being reduced </param>
        /// <param name="addor"> The row doing the reducing </param>
        /// <param name="elimValue"> The index of the row array being set to zero </param>
        public static void reduceRow (double[] row, double[] addor, int elimValue)
        {
            if (addor[elimValue]==0)
            {
                throw new ArithmeticException("Addor at elimValue: " + elimValue + " equals zero.");
            }
            double multiplier = row[elimValue] / addor[elimValue];
            for (int ii=0; ii<row.Length; ii++)
            {
                row[ii] = row[ii] - multiplier * addor[ii];
            }
        }

        /// <summary>
        /// Divides all values in an array by a value
        /// </summary>
        /// <param name="row"> The array being divided</param>
        /// <param name="value"> The divisor </param>
        public static void divideRow(double[] row, int value)
        {
            double multiplier = row[value];
            for (int ii=0; ii<row.Length; ii++)
            {
                row[ii] /= multiplier;
            }
        }



    }
}
