using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.NeuralData;
using Encog.Neural.Data.Basic;
using encog_test.Neural.Networks;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.Backpropagation;
using Encog.Neural.Persist;
using System.IO;
using Encog.Neural.Networks.Training.Hopfield;
using Encog.Neural.NeuralData.Bipolar;
using Encog.Util;
using Encog.Neural.Data;

namespace encog_test.Persist
{
    [TestFixture]
    public class TestPersist
    {
        public const double MAX_ERROR = 0.05;

        public static double[,] trainedData = {{0.48746847854732106, 0.5010119462167667, 0.5167202966276256, 0.4943294925693857, 0.0},
		{-0.49856939758003643, -0.5027761724685629, -0.5061504393638588, -0.49239862807111295, 0.0}};


        private BasicNetwork CreateNetwork()
        {
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new FeedforwardLayer(2));
            network.AddLayer(new FeedforwardLayer(3));
            network.AddLayer(new FeedforwardLayer(1));
            network.Reset();
            return network;
        }

        [Test]
        public void TestFeedforwardPersist()
        {
            INeuralDataSet trainingData = new BasicNeuralDataSet(XOR.XOR_INPUT, XOR.XOR_IDEAL);

            BasicNetwork network = CreateNetwork();
            ITrain train = new Backpropagation(network, trainingData, 0.7, 0.9);

            for (int i = 0; i < 5000; i++)
            {
                train.Iteration();
                network = (BasicNetwork)train.TrainedNetwork;
            }

            Assert.IsTrue(train.Error < 0.1);
            Assert.IsTrue(XOR.VerifyXOR(network, 0.1));

            EncogPersistedCollection encog = new EncogPersistedCollection();
            encog.Add(network);
            encog.Save("encogtest.xml");

            EncogPersistedCollection encog2 = new EncogPersistedCollection();
            encog2.Load("encogtest.xml");
            File.Delete("encogtest.xml");

            BasicNetwork n = (BasicNetwork)encog2.List[0];
            Assert.IsTrue(n.CalculateError(trainingData) < 0.1);

        }

        [Test]
        public void TestHopfieldPersist()
        {
            bool[] input = { true, false, true, false };

            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new HopfieldLayer(4));

            INeuralData data = new BiPolarNeuralData(input);
            ITrain train = new TrainHopfield(data, network);
            train.Iteration();

            EncogPersistedCollection encog = new EncogPersistedCollection();
            encog.Add(network);
            encog.Save("encogtest.xml");

            EncogPersistedCollection encog2 = new EncogPersistedCollection();
            encog2.Load("encogtest.xml");
            File.Delete("encogtest.xml");

            BasicNetwork network2 = (BasicNetwork)encog2.List[0];

            BiPolarNeuralData output = (BiPolarNeuralData)network2.Compute(new BiPolarNeuralData(input));
            Assert.IsTrue(output.GetBoolean(0));
            Assert.IsFalse(output.GetBoolean(1));
            Assert.IsTrue(output.GetBoolean(2));
            Assert.IsFalse(output.GetBoolean(3));
        }

        [Test]
        public void TestSOMPersist()
        {
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(TestPersist.trainedData);
            double[] pattern1 = { -0.5, -0.5, -0.5, -0.5 };
            double[] pattern2 = { 0.5, 0.5, 0.5, 0.5 };
            double[] pattern3 = { -0.5, -0.5, -0.5, 0.5 };
            double[] pattern4 = { 0.5, 0.5, 0.5, -0.5 };

            INeuralData data1 = new BasicNeuralData(pattern1);
            INeuralData data2 = new BasicNeuralData(pattern2);
            INeuralData data3 = new BasicNeuralData(pattern3);
            INeuralData data4 = new BasicNeuralData(pattern4);

            SOMLayer layer;
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(layer = new SOMLayer(4, NormalizationType.MULTIPLICATIVE));
            network.AddLayer(new BasicLayer(2));
            layer.WeightMatrix = matrix;

            EncogPersistedCollection encog = new EncogPersistedCollection();
            encog.Add(network);
            encog.Save("encogtest.xml");

            EncogPersistedCollection encog2 = new EncogPersistedCollection();
            encog2.Load("encogtest.xml");
            File.Delete("encogtest.xml");

            BasicNetwork network2 = (BasicNetwork)encog2.List[0];

            int data1Neuron = network2.Winner(data1);
            int data2Neuron = network2.Winner(data2);

            Assert.IsTrue(data1Neuron != data2Neuron);

            int data3Neuron = network2.Winner(data3);
            int data4Neuron = network2.Winner(data4);

            Assert.IsTrue(data3Neuron == data1Neuron);
            Assert.IsTrue(data4Neuron == data2Neuron);
        }
    }
}
