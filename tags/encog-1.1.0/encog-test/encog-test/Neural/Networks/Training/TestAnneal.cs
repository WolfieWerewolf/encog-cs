using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Neural.NeuralData;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training.Anneal;

namespace encog_test.Neural.Networks.Training
{
    [TestFixture]
    public class TestAnneal
    {
        [Test]
        public void TestTrainAnneal()
        {
            INeuralDataSet trainingData = new BasicNeuralDataSet(XOR.XOR_INPUT, XOR.XOR_IDEAL);
            BasicNetwork network = XOR.CreateThreeLayerNet();
            NeuralSimulatedAnnealing train = new NeuralSimulatedAnnealing(network, trainingData, 10, 2, 100);

            train.Iteration();
            double error1 = train.Error;
            for(int i=0;i<10;i++)
            	train.Iteration();
            network = (BasicNetwork)train.TrainedNetwork;
            double error2 = train.Error;

            double improve = (error1 - error2) / error1;

            Assert.IsTrue(improve > 0.01);
        }
    }
}
