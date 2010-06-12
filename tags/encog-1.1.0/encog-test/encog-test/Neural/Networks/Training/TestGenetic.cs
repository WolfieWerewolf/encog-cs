using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.NeuralData;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training.Genetic;
using NUnit.Framework;

namespace encog_test.Neural.Networks.Training
{
    [TestFixture]
    public class TestGenetic
    {
        [Test]
        public void TestTrainGenetic()
        {
            INeuralDataSet trainingData = new BasicNeuralDataSet(XOR.XOR_INPUT, XOR.XOR_IDEAL);
            BasicNetwork network = XOR.CreateThreeLayerNet();

            TrainingSetNeuralGeneticAlgorithm train = new TrainingSetNeuralGeneticAlgorithm(network, true, trainingData, 500, 0.1, 0.25);

            train.Iteration();
            double error1 = train.Error;
            for(int i=0;i<10;i++)
            train.Iteration();
            network = (BasicNetwork)train.TrainedNetwork;
            double error2 = train.Error;

            double improve = (error1 - error2) / error1;

            Assert.IsTrue(improve > 0.0001);

        }
    }
}
