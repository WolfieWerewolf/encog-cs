using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Neural.NeuralData;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Backpropagation;
using Encog.Neural.Networks.Layers;

namespace encog_test.Neural.Networks.Training
{
    [TestFixture]
    public class TestBackpropagation
    {
        [Test]
        public void TestTrainBackpropagation()
        {
            INeuralDataSet trainingData = new BasicNeuralDataSet(XOR.XOR_INPUT, XOR.XOR_IDEAL);

            BasicNetwork network = CreateNetwork.createXORNetworkUntrained();
            ITrain train = new Backpropagation(network, trainingData, 0.7, 0.9);

            train.Iteration();
            double error1 = train.Error;
            train.Iteration();
            network = (BasicNetwork)train.TrainedNetwork;
            double error2 = train.Error;

            double improve = (error1 - error2) / error1;

            Assert.IsTrue(improve > 0.01);

        }

        [Test]
        public void TestToString()
        {
            BasicNetwork network = CreateNetwork.createXORNetworkUntrained();
            network.InputLayer.ToString();
        }

        public void testCounts()
        {
            BasicNetwork network = CreateNetwork.createXORNetworkUntrained();
            network.InputLayer.ToString();
            Assert.AreEqual(1, network.HiddenLayerCount);
            Assert.AreEqual(6, network.CalculateNeuronCount());
        }

        [Test]
        public void TestPrune()
        {
            BasicNetwork network = CreateNetwork.createXORNetworkUntrained();
            IEnumerator<ILayer> itr = network.HiddenLayers.GetEnumerator();
            itr.MoveNext();
            FeedforwardLayer hidden = (FeedforwardLayer)itr.Current;

            Assert.AreEqual(3, hidden.NeuronCount);
            Assert.AreEqual(4, hidden.MatrixSize);
            Assert.AreEqual(9, network.InputLayer.MatrixSize);

            hidden.Prune(1);

            Assert.AreEqual(2, hidden.NeuronCount);
            Assert.AreEqual(3, hidden.MatrixSize);
            Assert.AreEqual(6, network.InputLayer.MatrixSize);
        }
    }
}
