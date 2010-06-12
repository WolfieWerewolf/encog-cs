using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using Encog.Neural.NeuralData;
using Encog.Neural.NeuralData.Bipolar;
using Encog.Neural.Networks.Training.Hopfield;
using Encog.Neural.Networks.Training;
using NUnit.Framework;
using Encog.Neural.Networks.Layers;
using Encog.Neural;
using Encog.Neural.Data;

namespace encog_test.Neural.Networks.Layers
{
    [TestFixture]
    public class TestHopfield
    {
        [Test]
        public void Hopfield()
        {
            bool[] input = { true, false, true, false };

            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new HopfieldLayer(4));

            INeuralData data = new BiPolarNeuralData(input);
            ITrain train = new TrainHopfield(data, network);
            train.Iteration();
            BiPolarNeuralData output = (BiPolarNeuralData)network.Compute(new BiPolarNeuralData(input));
            Assert.IsTrue(output.GetBoolean(0));
            Assert.IsFalse(output.GetBoolean(1));
            Assert.IsTrue(output.GetBoolean(2));
            Assert.IsFalse(output.GetBoolean(3));
        }

        [Test]
        public void InvalidTrain()
        {
            try
            {
                bool[] input = { true, false, true };
                INeuralData data = new BiPolarNeuralData(input);
                BasicNetwork network = new BasicNetwork();
                network.AddLayer(new HopfieldLayer(4));
                ITrain train = new TrainHopfield(data, network);
                train.Iteration();
                Assert.IsTrue(false);
            }
            catch (NeuralNetworkError)
            {

            }

        }
    }
}
