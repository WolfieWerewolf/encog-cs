using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Core;
using Encog.Matrix;


namespace encog_test.Matrix
{
    [TestFixture]
    public class TestMatrix
    {
        [Test]
        public void RowsAndCols()
        {
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(6, 3);
            Assert.AreEqual(matrix.Rows, 6);
            Assert.AreEqual(matrix.Cols, 3);

            matrix[1, 2] = 1.5;
            Assert.AreEqual(matrix[1, 2], 1.5);
        }

        [Test]
        public void RowAndColRangeUnder()
        {
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(6, 3);

            // make sure set registers error on under-bound row
            try
            {
                matrix[-1, 0] = 1;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure set registers error on under-bound col
            try
            {
                matrix[0, -1] = 1;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure get registers error on under-bound row
            try
            {
                double d = matrix[-1, 0];
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure set registers error on under-bound col
            try
            {
                double d = matrix[0, -1];
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }
        }

        [Test]
        public void RowAndColRangeOver()
        {
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(6, 3);

            // make sure set registers error on under-bound row
            try
            {
                matrix[6, 0] = 1;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure set registers error on under-bound col
            try
            {
                matrix[0, 3] = 1;
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure get registers error on under-bound row
            try
            {
                double d = matrix[6, 0];
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }

            // make sure set registers error on under-bound col
            try
            {
                double d = matrix[0, 3];
                Assert.IsTrue(false); // should have thrown an exception
            }
            catch (MatrixError)
            {
            }
        }

        [Test]
        public void MatrixConstruct()
        {
            double[,] m = {
				{1,2,3,4},
				{5,6,7,8},
				{9,10,11,12},
				{13,14,15,16} };
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(m);
            Assert.AreEqual(matrix.Rows, 4);
            Assert.AreEqual(matrix.Cols, 4);
        }

        [Test]
        public void MatrixEquals()
        {
            double[,] m1 = {
				{1,2},
				{3,4} };

            double[,] m2 = {
				{0,2},
				{3,4} };

            Encog.Matrix.Matrix matrix1 = new Encog.Matrix.Matrix(m1);
            Encog.Matrix.Matrix matrix2 = new Encog.Matrix.Matrix(m1);

            Assert.IsTrue(matrix1.Equals(matrix2));

            matrix2 = new Encog.Matrix.Matrix(m2);

            Assert.IsFalse(matrix1.Equals(matrix2));
        }

        [Test]
        public void MatrixEqualsPrecision()
        {
            double[,] m1 = {
				{1.1234,2.123},
				{3.123,4.123} };

            double[,] m2 = {
				{1.123,2.123},
				{3.123,4.123} };

            Encog.Matrix.Matrix matrix1 = new Encog.Matrix.Matrix(m1);
            Encog.Matrix.Matrix matrix2 = new Encog.Matrix.Matrix(m2);

            Assert.IsTrue(matrix1.equals(matrix2, 3));
            Assert.IsFalse(matrix1.equals(matrix2, 4));

            double[,] m3 = {
				{1.1,2.1},
				{3.1,4.1} };

            double[,] m4 = {
				{1.2,2.1},
				{3.1,4.1} };

            Encog.Matrix.Matrix matrix3 = new Encog.Matrix.Matrix(m3);
            Encog.Matrix.Matrix matrix4 = new Encog.Matrix.Matrix(m4);
            Assert.IsTrue(matrix3.equals(matrix4, 0));
            Assert.IsFalse(matrix3.equals(matrix4, 1));

            try
            {
                matrix3.equals(matrix4, -1);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {

            }

            try
            {
                matrix3.equals(matrix4, 19);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {

            }

        }

        [Test]
        public void MatrixMultiply()
        {
            double[,] a = {
				{1,0,2},
				{-1,3,1}
		};

            double[,] b = {
				{3,1},
				{2,1},
				{1,0}
		};

            double[,] c = {
				{5,1},
				{4,2}
		};

            Encog.Matrix.Matrix matrixA = new Encog.Matrix.Matrix(a);
            Encog.Matrix.Matrix matrixB = new Encog.Matrix.Matrix(b);
            Encog.Matrix.Matrix matrixC = new Encog.Matrix.Matrix(c);

            Encog.Matrix.Matrix result = matrixA.Clone();
            result = MatrixMath.Multiply(matrixA, matrixB);

            Assert.IsTrue(result.Equals(matrixC));

            double[,] a2 = {
				{1,2,3,4},
				{5,6,7,8}
		};

            double[,] b2 = {
				{1,2,3},
				{4,5,6},
				{7,8,9},
				{10,11,12}
		};

            double[,] c2 = {
				{70,80,90},
				{158,184,210}
		};

            matrixA = new Encog.Matrix.Matrix(a2);
            matrixB = new Encog.Matrix.Matrix(b2);
            matrixC = new Encog.Matrix.Matrix(c2);

            result = MatrixMath.Multiply(matrixA, matrixB);
            Assert.IsTrue(result.Equals(matrixC));

            result = matrixB.Clone();
            try
            {
                MatrixMath.Multiply(matrixB, matrixA);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {

            }
        }

        [Test]
        public void Boolean()
        {
            bool[,] matrixDataBoolean = { 
				{true,false},
				{false,true}
		};

            double[,] matrixDataDouble = {
				{1.0,-1.0},
				{-1.0,1.0},
		};

            Encog.Matrix.Matrix matrixBoolean = new Encog.Matrix.Matrix(matrixDataBoolean);
            Encog.Matrix.Matrix matrixDouble = new Encog.Matrix.Matrix(matrixDataDouble);

            Assert.IsTrue(matrixBoolean.Equals(matrixDouble));
        }

        [Test]
        public void GetRow()
        {
            double[,] matrixData1 = {
				{1.0,2.0},
				{3.0,4.0}
		};
            double[,] matrixData2 = {
				{3.0,4.0}
		};

            Encog.Matrix.Matrix matrix1 = new Encog.Matrix.Matrix(matrixData1);
            Encog.Matrix.Matrix matrix2 = new Encog.Matrix.Matrix(matrixData2);

            Encog.Matrix.Matrix matrixRow = matrix1.GetRow(1);
            Assert.IsTrue(matrixRow.Equals(matrix2));

            try
            {
                matrix1.GetRow(3);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void GetCol()
        {
            double[,] matrixData1 = {
				{1.0,2.0},
				{3.0,4.0}
		};
            double[,] matrixData2 = {
				{2.0},
				{4.0}
		};

            Encog.Matrix.Matrix matrix1 = new Encog.Matrix.Matrix(matrixData1);
            Encog.Matrix.Matrix matrix2 = new Encog.Matrix.Matrix(matrixData2);

            Encog.Matrix.Matrix matrixCol = matrix1.GetCol(1);
            Assert.IsTrue(matrixCol.Equals(matrix2));

            try
            {
                matrix1.GetCol(3);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {
                Assert.IsTrue(true);
            }
        }

        [Test]
        public void Zero()
        {
            double[,] doubleData = { { 0, 0 }, { 0, 0 } };
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(doubleData);
            Assert.IsTrue(matrix.IsZero());
        }

        [Test]
        public void Sum()
        {
            double[,] doubleData = { { 1, 2 }, { 3, 4 } };
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(doubleData);
            Assert.AreEqual((int)matrix.Sum(), 1 + 2 + 3 + 4);
        }

        [Test]
        public void RowMatrix()
        {
            double[] matrixData = { 1.0, 2.0, 3.0, 4.0 };
            Encog.Matrix.Matrix matrix = Encog.Matrix.Matrix.CreateRowMatrix(matrixData);
            Assert.AreEqual(matrix[0, 0], 1.0);
            Assert.AreEqual(matrix[0, 1], 2.0);
            Assert.AreEqual(matrix[0, 2], 3.0);
            Assert.AreEqual(matrix[0, 3], 4.0);
        }

        [Test]
        public void ColumnMatrix()
        {
            double[] matrixData = { 1.0, 2.0, 3.0, 4.0 };
            Encog.Matrix.Matrix matrix = Encog.Matrix.Matrix.CreateColumnMatrix(matrixData);
            Assert.AreEqual(matrix[0, 0], 1.0);
            Assert.AreEqual(matrix[1, 0], 2.0);
            Assert.AreEqual(matrix[2, 0], 3.0);
            Assert.AreEqual(matrix[3, 0], 4.0);
        }

        [Test]
        public void Add()
        {
            double[] matrixData = { 1.0, 2.0, 3.0, 4.0 };
            Encog.Matrix.Matrix matrix = Encog.Matrix.Matrix.CreateColumnMatrix(matrixData);
            matrix.Add(0, 0, 1);
            Assert.AreEqual(matrix[0, 0], 2.0);
        }

        [Test]
        public void Clear()
        {
            double[] matrixData = { 1.0, 2.0, 3.0, 4.0 };
            Encog.Matrix.Matrix matrix = Encog.Matrix.Matrix.CreateColumnMatrix(matrixData);
            matrix.Clear();
            Assert.AreEqual(matrix[0, 0], 0.0);
            Assert.AreEqual(matrix[1, 0], 0.0);
            Assert.AreEqual(matrix[2, 0], 0.0);
            Assert.AreEqual(matrix[3, 0], 0.0);
        }

        [Test]
        public void IsVector()
        {
            double[] matrixData = { 1.0, 2.0, 3.0, 4.0 };
            Encog.Matrix.Matrix matrixCol = Encog.Matrix.Matrix.CreateColumnMatrix(matrixData);
            Encog.Matrix.Matrix matrixRow = Encog.Matrix.Matrix.CreateRowMatrix(matrixData);
            Assert.IsTrue(matrixCol.IsVector());
            Assert.IsTrue(matrixRow.IsVector());
            double[,] matrixData2 = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(matrixData2);
            Assert.IsFalse(matrix.IsVector());
        }

        [Test]
        public void IsZero()
        {
            double[] matrixData = { 1.0, 2.0, 3.0, 4.0 };
            Encog.Matrix.Matrix matrix = Encog.Matrix.Matrix.CreateColumnMatrix(matrixData);
            Assert.IsFalse(matrix.IsZero());
            double[] matrixData2 = { 0.0, 0.0, 0.0, 0.0 };
            Encog.Matrix.Matrix matrix2 = Encog.Matrix.Matrix.CreateColumnMatrix(matrixData2);
            Assert.IsTrue(matrix2.IsZero());

        }

        [Test]
        public void PackedArray()
        {
            double[,] matrixData = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(matrixData);
            Double[] matrixData2 = matrix.ToPackedArray();
            Assert.AreEqual(4, matrixData2.Length);
            Assert.AreEqual(1.0, matrix[0, 0]);
            Assert.AreEqual(2.0, matrix[0, 1]);
            Assert.AreEqual(3.0, matrix[1, 0]);
            Assert.AreEqual(4.0, matrix[1, 1]);

            Encog.Matrix.Matrix matrix2 = new Encog.Matrix.Matrix(2, 2);
            matrix2.FromPackedArray(matrixData2, 0);
            Assert.IsTrue(matrix.Equals(matrix2));
        }

        [Test]
        public void PackedArray2()
        {
            Double[] data = { 1.0, 2.0, 3.0, 4.0 };
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(1, 4);
            matrix.FromPackedArray(data, 0);
            Assert.AreEqual(1.0, matrix[0, 0]);
            Assert.AreEqual(2.0, matrix[0, 1]);
            Assert.AreEqual(3.0, matrix[0, 2]);
        }

        [Test]
        public void Size()
        {
            double[,] data = { { 1.0, 2.0 }, { 3.0, 4.0 } };
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(data);
            Assert.AreEqual(4, matrix.Size);
        }

        [Test]
        public void Randomize()
        {
            const double MIN = 1.0;
            const double MAX = 10.0;
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(10, 10);
            matrix.Ramdomize(MIN, MAX);
            Double[] array = matrix.ToPackedArray();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] < MIN || array[i] > MAX)
                    Assert.IsFalse(true);
            }
        }

        [Test]
        public void VectorLength()
        {
            double[] vectorData = { 1.0, 2.0, 3.0, 4.0 };
            Encog.Matrix.Matrix vector = Encog.Matrix.Matrix.CreateRowMatrix(vectorData);
            Assert.AreEqual(5, (int)MatrixMath.VectorLength(vector));

            Encog.Matrix.Matrix nonVector = new Encog.Matrix.Matrix(2, 2);
            try
            {
                MatrixMath.VectorLength(nonVector);
                Assert.IsTrue(false);
            }
            catch (MatrixError)
            {

            }
        }
    }
}
