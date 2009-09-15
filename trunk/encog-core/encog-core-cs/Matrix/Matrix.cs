﻿// Encog Artificial Intelligence Framework v2.x
// DotNet Version
// http://www.heatonresearch.com/encog/
// http://code.google.com/p/encog-cs/
// 
// Copyright 2009, Heaton Research Inc., and individual contributors.
// See the copyright.txt in the distribution for a full listing of 
// individual contributors.
//
// This is free software; you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation; either version 2.1 of
// the License, or (at your option) any later version.
//
// This software is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this software; if not, write to the Free
// Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301 USA, or see the FSF site: http://www.fsf.org.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Persist;
using System.Runtime.Serialization;
using Encog.Util.MathUtil;
#if logging
using log4net;
#endif
namespace Encog.Matrix
{
    /// <summary>
    /// Matrix: This class implements a mathematical matrix.  Matrix
    /// math is very important to neural network processing.  Many
    /// of the classes developed in this book will make use of the
    /// matrix classes in this package.
    /// </summary>
    [Serializable]
    public class Matrix : IEncogPersistedObject
    {
        /// <summary>
        /// The description of this object.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The name of this object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Allows index access to the elements of the matrix.
        /// Warning: This can be a somewhat slow way to access the matrix.  
        /// Do not put this in performance critical loops.  Make sure to use
        /// the Data property and access the matrix array directly.
        /// </summary>
        /// <param name="row">The row to access.</param>
        /// <param name="col">The column to access.</param>
        /// <returns>The element at the specified position in the matrix.</returns>
        public double this[int row, int col]
        {
            get
            {
                Validate(row, col);
                return this.matrix[row][col];
            }
            set
            {
                Validate(row, col);
                if (double.IsInfinity(value) || double.IsNaN(value))
                {
                    throw new MatrixError("Trying to assign invalud number to matrix: "
                            + value);
                }
                this.matrix[row][col] = value;
            }
        }

        /// <summary>
        /// Create a matrix that is a single column.
        /// </summary>
        /// <param name="input">A 1D array to make the matrix from.</param>
        /// <returns>A matrix that contains a single column.</returns>
        public static Matrix CreateColumnMatrix(double[] input)
        {
            double[][] d = new double[input.Length][];
            for (int row = 0; row < d.Length; row++)
            {
                d[row] = new double[1];
                d[row][0] = input[row];
            }
            return new Matrix(d);
        }

        /// <summary>
        /// Create a matrix that is a single row.
        /// </summary>
        /// <param name="input">A 1D array to make the matrix from.</param>
        /// <returns>A matrix that contans a single row.</returns>
        public static Matrix CreateRowMatrix(double[] input)
        {
            double[][] d = new double[1][];

            d[0] = new double[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                d[0][i] = input[i];
            }

            return new Matrix(d);
        }

        /// <summary>
        /// The matrix data, stored as a 2D array.
        /// </summary>
        double[][] matrix;

        /// <summary>
        /// Construct a matrix from a 2D boolean array.  Translate true to 1, false to -1.
        /// </summary>
        /// <param name="sourceMatrix">A 2D array to construcat the matrix from.</param>
        public Matrix(bool[][] sourceMatrix)
        {

            this.matrix = new double[sourceMatrix.Length][];
            for (int r = 0; r < this.Rows; r++)
            {
                this.matrix[r] = new double[sourceMatrix[r].Length];
                for (int c = 0; c < this.Cols; c++)
                {
                    if (sourceMatrix[r][c])
                    {
                        this.matrix[r][c] = 1;
                    }
                    else
                    {
                        this.matrix[r][c] = -1;
                    }
                }
            }
        }

        /// <summary>
        /// Construct a matrix from a 2D double array.
        /// </summary>
        /// <param name="sourceMatrix">A 2D double array.</param>
        public Matrix(double[][] sourceMatrix)
        {
            this.matrix = new double[sourceMatrix.Length][];
            for (int r = 0; r < this.Rows; r++)
            {
                this.matrix[r] = new double[sourceMatrix[r].Length];
                for (int c = 0; c < this.Cols; c++)
                {
                    this.matrix[r][c] = sourceMatrix[r][c];
                }
            }
        }

        /// <summary>
        /// Construct a blank matrix with the specified number of rows and columns.
        /// </summary>
        /// <param name="rows">How many rows.</param>
        /// <param name="cols">How many columns.</param>
        public Matrix(int rows, int cols)
        {
            this.matrix = new double[rows][];
            for(int i=0;i<rows;i++)
            {
                this.matrix[i] = new double[cols];
            }

        }

        /// <summary>
        /// Add the specified value to the specified row and column of the matrix.
        /// </summary>
        /// <param name="row">The row to add to.</param>
        /// <param name="col">The column to add to.</param>
        /// <param name="value">The value to add.</param>
        public void Add(int row, int col, double value)
        {
            Validate(row, col);
            double newValue = this.matrix[row][col] + value;
            this.matrix[row][col] = newValue;
        }

        /// <summary>
        /// Clear the matrix.
        /// </summary>
        public void Clear()
        {
            for (int r = 0; r < this.Rows; r++)
            {
                for (int c = 0; c < this.Cols; c++)
                {
                    this.matrix[r][c] = 0;
                }
            }
        }

        /// <summary>
        /// Clone the matrix.
        /// </summary>
        /// <returns>A cloned copy of the matrix.</returns>
        public Object Clone()
        {
            return new Matrix(this.matrix);
        }

        /// <summary>
        /// Determine if this matrix is equal to another.  Use a precision of 10 decimal places.
        /// </summary>
        /// <param name="matrix">The other matrix to compare.</param>
        /// <returns>True if the two matrixes are equal.</returns>
        public bool Equals(Matrix matrix)
        {
            return equals(matrix, 10);
        }

        /// <summary>
        /// Compare the matrix to another with the specified level of precision.
        /// </summary>
        /// <param name="matrix">The other matrix to compare.</param>
        /// <param name="precision">The number of decimal places of precision to use.</param>
        /// <returns>True if the two matrixes are equal.</returns>
        public bool equals(Matrix matrix, int precision)
        {

            if (precision < 0)
            {
                throw new MatrixError("Precision can't be a negative number.");
            }

            double test = Math.Pow(10.0, precision);
            if (double.IsInfinity(test) || (test > long.MaxValue))
            {
                throw new MatrixError("Precision of " + precision
                        + " decimal places is not supported.");
            }

            precision = (int)Math.Pow(10, precision);

            double[][] otherMatrix = matrix.Data;
            for (int r = 0; r < this.Rows; r++)
            {
                for (int c = 0; c < this.Cols; c++)
                {
                    if ((long)(this.matrix[r][c] * precision) != (long)(otherMatrix[r][c] * precision))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Take the values of thie matrix from a packed array.
        /// </summary>
        /// <param name="array">The packed array to read the matrix from.</param>
        /// <param name="index">The index to begin reading at in the array.</param>
        /// <returns>The new index after this matrix has been read.</returns>
        public int FromPackedArray(double[] array, int index)
        {

            for (int r = 0; r < this.Rows; r++)
            {
                for (int c = 0; c < this.Cols; c++)
                {
                    this.matrix[r][c] = array[index++];
                }
            }

            return index;
        }

        /// <summary>
        /// Get one column from this matrix as a column matrix.
        /// </summary>
        /// <param name="col">The desired column.</param>
        /// <returns>The column matrix.</returns>
        public Matrix GetCol(int col)
        {
            if (col > this.Cols)
            {
                throw new MatrixError("Can't get column #" + col
                        + " because it does not exist.");
            }

            double[][] newMatrix = new double[this.Rows][];

            for (int row = 0; row < this.Rows; row++)
            {
                newMatrix[row] = new double[1];
                newMatrix[row][0] = this.matrix[row][col];
            }

            return new Matrix(newMatrix);
        }

        /// <summary>
        /// Get the number of columns in this matrix
        /// </summary>
        public int Cols
        {
            get
            {
                return this.matrix[0].Length;
            }
        }

        /// <summary>
        /// Get the specified row as a row matrix.
        /// </summary>
        /// <param name="row">The desired row.</param>
        /// <returns>A row matrix.</returns>
        public Matrix GetRow(int row)
        {
            if (row > this.Rows)
            {
                throw new MatrixError("Can't get row #" + row
                        + " because it does not exist.");
            }

            double[][] newMatrix = new double[1][];
            newMatrix[0] = new double[this.Cols];

            for (int col = 0; col < this.Cols; col++)
            {                
                newMatrix[0][col] = this.matrix[row][col];
            }

            return new Matrix(newMatrix);
        }

        /// <summary>
        /// Get the number of rows in this matrix
        /// </summary>
        public int Rows
        {
            get
            {
                return this.matrix.GetUpperBound(0) + 1;
            }
        }


        /// <summary>
        /// Determine if this matrix is a vector.  A vector matrix only has a single row or column.
        /// </summary>
        /// <returns>True if this matrix is a vector.</returns>
        public bool IsVector()
        {
            if (this.Rows == 1)
            {
                return true;
            }
            else
            {
                return this.Cols == 1;
            }
        }

        /// <summary>
        /// Determine if all of the values in the matrix are zero.
        /// </summary>
        /// <returns>True if all of the values in the matrix are zero.</returns>
        public bool IsZero()
        {
            for (int row = 0; row < this.Rows; row++)
            {
                for (int col = 0; col < this.Cols; col++)
                {
                    if (this.matrix[row][col] != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Fill the matrix with random values in the specified range.
        /// </summary>
        /// <param name="min">The minimum value for the random numbers.</param>
        /// <param name="max">The maximum value for the random numbers.</param>
        public void Ramdomize(double min, double max)
        {
            for (int r = 0; r < this.Rows; r++)
            {
                for (int c = 0; c < this.Cols; c++)
                {
                    this.matrix[r][c] = (ThreadSafeRandom.NextDouble() * (max - min)) + min;
                }
            }
        }


        /// <summary>
        /// Get the size fo the matrix.  This is thr rows times the columns.
        /// </summary>
        public int Size
        {
            get
            {
                return this.Rows * this.Cols;
            }
        }

        /// <summary>
        /// Sum all of the values in the matrix.
        /// </summary>
        /// <returns>The sum of all of the values in the matrix.</returns>
        public double Sum()
        {
            double result = 0;
            for (int r = 0; r < this.Rows; r++)
            {
                for (int c = 0; c < this.Cols; c++)
                {
                    result += this.matrix[r][c];
                }
            }
            return result;
        }

        /// <summary>
        /// Convert the matrix to a packed array.
        /// </summary>
        /// <returns>A packed array.</returns>
        public double[] ToPackedArray()
        {
            double[] result = new double[this.Rows * this.Cols];

            int index = 0;
            for (int r = 0; r < this.Rows; r++)
            {
                for (int c = 0; c < this.Cols; c++)
                {
                    result[index++] = this.matrix[r][c];
                }
            }

            return result;
        }

        /// <summary>
        /// Validate that the specified row and column are inside of the range of the matrix.
        /// </summary>
        /// <param name="row">The row to check.</param>
        /// <param name="col">The column to check.</param>
        private void Validate(int row, int col)
        {
            if ((row >= this.Rows) || (row < 0))
            {
                throw new MatrixError("The row:" + row + " is out of range:"
                        + this.Rows);
            }

            if ((col >= this.Cols) || (col < 0))
            {
                throw new MatrixError("The col:" + col + " is out of range:"
                        + this.Cols);
            }
        }


        /// <summary>
        /// Create a persistor for this object.
        /// </summary>
        /// <returns>A persistor for this object.</returns>
        public IPersistor CreatePersistor()
        {
            // Matrixes are not persisted directly.
            return null;
        }

        /// <summary>
        /// Add the specified matrix to this matrix.  This will modify the matrix
        /// to hold the result of the addition.
        /// </summary>
        /// <param name="matrix">The matrix to add.</param>
        public void Add(Matrix matrix)
        {
            for (int row = 0; row < this.Rows; row++)
            {
                for (int col = 0; col < this.Cols; col++)
                {
                    this.Add(row, col, matrix[row, col]);
                }
            }
        }

        /// <summary>
        /// Set every value in the matrix to the specified value.
        /// </summary>
        /// <param name="value">The value to set the matrix to.</param>
        public void Set(double value)
        {
            for (int row = 0; row < this.Rows; row++)
            {
                for (int col = 0; col < this.Cols; col++)
                {
                    this.matrix[row][col] = value;
                }
            }

        }

        /// <summary>
        /// Get the matrix array for this matrix.
        /// </summary>
        public double[][] Data
        {
            get
            {
                return this.matrix;
            }
        }

    }

}
