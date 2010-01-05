﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Matrix;

namespace encog_test.TestMatrix
{
    [TestFixture]
    public class TestMatrixMath
    {
        [Test]
        public void Inverse()
        {
            double[,] matrixData1 = { { 1, 2, 3, 4 } };
            double[,] matrixData2 = {{1},
			{2},
			{3},
			{4}
		};
   
            Matrix matrix1 = new Matrix(matrixData1);
            Matrix checkMatrix = new Matrix(matrixData2);

            Matrix matrix2 = MatrixMath.Transpose(matrix1);

            Assert.IsTrue(matrix2.Equals(checkMatrix));
        }

        [Test]
        public void DotProduct()
        {
            double[,] matrixData1 = { { 1, 2, 3, 4 } };
            double[,] matrixData2 = {{5},
			{6},
			{7},
			{8}
		};

            Matrix matrix1 = new Matrix(matrixData1);
            Matrix matrix2 = new Matrix(matrixData2);

            double dotProduct = MatrixMath.DotProduct(matrix1, matrix2);

            Assert.AreEqual(dotProduct, 70.0);

            // test dot product errors
            double[,] nonVectorData = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            double[,] differentLengthData = { { 1.0 } };
            Matrix nonVector = new Matrix(nonVectorData);
            Matrix differentLength = new Matrix(differentLengthData);

            try
            {
                MatrixMath.DotProduct(matrix1, nonVector);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
            }

            try
            {
                MatrixMath.DotProduct(nonVector, matrix2);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
            }

            try
            {
                MatrixMath.DotProduct(matrix1, differentLength);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
            }


        }

        [Test]
        public void Multiply()
        {
            double[,] matrixData1 = {{1,4},
				{2,5},
				{3,6}
			};
            double[,] matrixData2 = {{7,8,9},
				{10,11,12}};


            double[,] matrixData3 = {{47,52,57},
				{64,71,78},
				{81,90,99}
		};

            Matrix matrix1 = new Matrix(matrixData1);
            Matrix matrix2 = new Matrix(matrixData2);

            Matrix matrix3 = new Matrix(matrixData3);

            Matrix result = MatrixMath.Multiply(matrix1, matrix2);

            Assert.IsTrue(result.Equals(matrix3));
        }

        [Test]
        public void VerifySame()
        {
            double[,] dataBase = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            double[,] dataTooManyRows = { { 1.0, 2.0 }, { 3.0, 4.0 }, { 5.0, 6.0 } };
            double[,] dataTooManyCols = { { 1.0, 2.0, 3.0 }, { 4.0, 5.0, 6.0 } };
            Matrix baseMatrix = new Matrix(dataBase);
            Matrix tooManyRows = new Matrix(dataTooManyRows);
            Matrix tooManyCols = new Matrix(dataTooManyCols);
            MatrixMath.Add(baseMatrix, baseMatrix);
            try
            {
                MatrixMath.Add(baseMatrix, tooManyRows);
                Assert.IsFalse(true);
            }
            catch (MatrixError)
            {
            }
            try
            {
                MatrixMath.Add(baseMatrix, tooManyCols);
                Assert.IsFalse(true);
            }
            catch (MatrixError)
            {
            }
        }

        [Test]
        public void Divide()
        {
            double[,] data = { { 2.0, 4.0 }, { 6.0, 8.0 } };
            Matrix matrix = new Matrix(data);
            Matrix result = MatrixMath.Divide(matrix, 2.0);
            Assert.AreEqual(1.0, result[0, 0]);
        }

        [Test]
        public void Identity()
        {
            try
            {
                MatrixMath.Identity(0);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {

            }

            double[,] checkData = { { 1, 0 }, { 0, 1 } };
            Matrix check = new Matrix(checkData);
            Matrix matrix = MatrixMath.Identity(2);
            Assert.IsTrue(check.Equals(matrix));
        }

        [Test]
        public void MultiplyScalar()
        {
            double[,] data = { { 2.0, 4.0 }, { 6.0, 8.0 } };
            Matrix matrix = new Matrix(data);
            Matrix result = MatrixMath.Multiply(matrix, 2.0);
            Assert.AreEqual(4.0, result[0, 0]);
        }

        [Test]
        public void DeleteRow()
        {
            double[,] origData = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            double[,] checkData = { { 3.0, 4.0 } };
            Matrix orig = new Matrix(origData);
            Matrix matrix = MatrixMath.DeleteRow(orig, 0);
            Matrix check = new Matrix(checkData);
            Assert.IsTrue(check.Equals(matrix));

            try
            {
                MatrixMath.DeleteRow(orig, 10);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
            }
        }

        [Test]
        public void DeleteCol()
        {
            double[,] origData = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            double[,] checkData = { { 2.0 }, { 4.0 } };
            Matrix orig = new Matrix(origData);
            Matrix matrix = MatrixMath.DeleteCol(orig, 0);
            Matrix check = new Matrix(checkData);
            Assert.IsTrue(check.Equals(matrix));

            try
            {
                MatrixMath.DeleteCol(orig, 10);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
            }
        }

        [Test]
        public void Copy()
        {
            double[,] data = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            Matrix source = new Matrix(data);
            Matrix target = new Matrix(2, 2);
            MatrixMath.Copy(source, target);
            Assert.IsTrue(source.Equals(target));
        }

    }
}