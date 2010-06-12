using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Neural.Activation;
using Encog.Persist.Persistors;
using Encog;

namespace encog_test.Encog.Neural.Activation
{
    [TestFixture]
    public class TestActivationLinear
    {
        [Test]
        public void testLinear()
        {
            ActivationLinear activation = new ActivationLinear();
            Assert.IsFalse(activation.HasDerivative);

            ActivationLinear clone = (ActivationLinear)activation.Clone();
            Assert.IsNotNull(clone);

            double[] input = { 1, 2, 3 };

            activation.ActivationFunction(input);

            Assert.AreEqual(1.0, input[0], 0.1);
            Assert.AreEqual(2.0, input[1], 0.1);
            Assert.AreEqual(3.0, input[2], 0.1);

            // this will throw an error if it does not work
            ActivationLinearPersistor p = (ActivationLinearPersistor)activation.CreatePersistor();

            // test derivative, should throw an error
            try
            {
                activation.DerivativeFunction(input);
                Assert.IsTrue(false);// mark an error
            }
            catch (EncogError )
            {
                // good, this should happen
            }

            // test name and description
            // names and descriptions are not stored for these
            activation.Name = "name";
            activation.Description = "name";
            Assert.AreEqual(null, activation.Name);
            Assert.AreEqual(null, activation.Description);
        }
    }
}
