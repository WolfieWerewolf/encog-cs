﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Util.Logging;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Synapse;
using Encog.Neural.Networks;
using Encog.Neural.NeuralData;
using Encog.Neural.Networks.Training.Competitive;
using Encog.Neural.Networks.Training.Competitive.Neighborhood;
using Encog.Neural.Data.Basic;
using Encog.Neural.Activation;
using Encog.Neural.Data;

namespace encog_test.Neural.Networks.Training
{
    [TestFixture]
    public class TestCompetitive
    {

        public static double[][] SOM_INPUT = { 
        new double[4] { 0.0, 0.0, 1.0, 1.0 },
		new double[4] { 1.0, 1.0, 0.0, 0.0 } };

        // Just a random starting matrix, but it gives us a constant starting point
        public static double[,] MATRIX_ARRAY = {
			{0.9950675732277183, -0.09315692732658198}, 
			{0.9840257865083011, 0.5032129897356723}, 
			{-0.8738960119753589, -0.48043680531294997}, 
			{-0.9455207768842442, -0.8612565984447569}
			};

        private ISynapse findSynapse(BasicNetwork network)
        {
            ILayer input = network.InputLayer;
            return input.Next[0];
        }

        [Test]
        public void TestSOM()
        {
            Logging.stopConsoleLogging();

            // create the training set
            INeuralDataSet training = new BasicNeuralDataSet(
                   TestCompetitive.SOM_INPUT, null);

            // Create the neural network.
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(new ActivationLinear(), false, 4));
            network.AddLayer(new BasicLayer(new ActivationLinear(), false, 2));
            network.Structure.FinalizeStructure();
            network.Reset();

            ISynapse synapse = findSynapse(network);
            synapse.WeightMatrix = new Encog.Matrix.Matrix(MATRIX_ARRAY);

            CompetitiveTraining train = new CompetitiveTraining(network, 0.4,
                   training, new NeighborhoodSingle());

            int iteration = 0;

            for (iteration = 0; iteration <= 100; iteration++)
            {
                train.Iteration();
            }

            INeuralData data1 = new BasicNeuralData(
                   TestCompetitive.SOM_INPUT[0]);
            INeuralData data2 = new BasicNeuralData(
                   TestCompetitive.SOM_INPUT[1]);

            int result1 = network.Winner(data1);
            int result2 = network.Winner(data2);

            Assert.IsTrue(result1 != result2);

        }

    }

}