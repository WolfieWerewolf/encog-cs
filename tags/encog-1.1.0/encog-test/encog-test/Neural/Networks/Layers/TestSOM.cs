using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Neural.NeuralData;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training.SOM;
using Encog.Util;
using Encog.Neural.Data;

namespace encog_test.Neural.Networks.Layers
{
    [TestFixture]
    public class TestSOM
    {
        public const double MAX_ERROR = 0.05;

        public double[,] untrainedData = 
	{{0.6882551270881396, -0.9291471917702279, 0.9631574105879768, -0.6847023846227012, 0.6445001219615334},
	{0.2596121773799609, 0.20903647997830488, 0.44901788840545387, -0.8991254913211779, -0.4440569188207164}};

        public double[,] trainedData = {{0.48746847854732106, 0.5010119462167667, 0.5167202966276256, 0.4943294925693857, 0.0},
		{-0.49856939758003643, -0.5027761724685629, -0.5061504393638588, -0.49239862807111295, 0.0}};

        [Test]
        public void TrainSOM()
        {
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(this.untrainedData);
            double[] pattern1 = { -0.5, -0.5, -0.5, -0.5 };
            double[] pattern2 = { 0.5, 0.5, 0.5, 0.5 };

            INeuralData data1 = new BasicNeuralData(pattern1);
            INeuralData data2 = new BasicNeuralData(pattern2);

            SOMLayer layer;
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(layer = new SOMLayer(4, NormalizationType.MULTIPLICATIVE));
            network.AddLayer(new BasicLayer(2));

            layer.WeightMatrix = matrix;

            INeuralDataSet trainingSet = new BasicNeuralDataSet();
            trainingSet.Add(data1);
            trainingSet.Add(data2);

            TrainSelfOrganizingMap train = new TrainSelfOrganizingMap(
                    network, trainingSet, Encog.Neural.Networks.Training.SOM.TrainSelfOrganizingMap.LearningMethod.SUBTRACTIVE, 0.5);


            train.Iteration();
            double error1 = train.TotalError;
            train.Iteration();
            double error2 = train.TotalError;

            double diff = (error2 - error1) / error1;

            Assert.IsTrue(diff <= 0.0);
        }

        [Test]
        public void RunSOM()
        {
            Encog.Matrix.Matrix matrix = new Encog.Matrix.Matrix(this.trainedData);

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

            int data1Neuron = network.Winner(data1);
            int data2Neuron = network.Winner(data2);

            Assert.IsTrue(data1Neuron != data2Neuron);

            int data3Neuron = network.Winner(data3);
            int data4Neuron = network.Winner(data4);

            Assert.IsTrue(data3Neuron == data1Neuron);
            Assert.IsTrue(data4Neuron == data2Neuron);
        }
    }
}
