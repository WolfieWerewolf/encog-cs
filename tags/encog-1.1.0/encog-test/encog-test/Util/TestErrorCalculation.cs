using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Util;

namespace encog_test.Util
{
    [TestFixture]
    public class TestErrorCalculation
    {
        [Test]
        public void ErrorCalculation()
        {
            double[][] ideal = {
				new double[4] {1,2,3,4},
				new double[4] {5,6,7,8},
				new double[4] {9,10,11,12},
				new double[4] {13,14,15,16} };

            double[][] actual_good = {
				new double[4] {1,2,3,4},
				new double[4] {5,6,7,8},
				new double[4] {9,10,11,12},
				new double[4] {13,14,15,16} };

            double[][] actual_bad = {
				new double[4] {1,2,3,5},
				new double[4] {5,6,7,8},
				new double[4] {9,10,11,12},
				new double[4] {13,14,15,16} };

            ErrorCalculation error = new ErrorCalculation();

            for (int i = 0; i < ideal.Length; i++)
            {
                error.UpdateError(actual_good[i], ideal[i]);
            }
            Assert.AreEqual(0.0, error.CalculateRMS());

            error.Reset();

            for (int i = 0; i < ideal.Length; i++)
            {
                error.UpdateError(actual_bad[i], ideal[i]);
            }
            Assert.AreEqual(125, (int)(error.CalculateRMS() * 1000));

        }
    }
}
