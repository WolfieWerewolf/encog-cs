using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleExamples.Examples;
using Encog.Neural.NeuralData;
using Encog.Neural.Data.Basic;
using Encog.Util.Simple;
using Encog.Persist;
using Encog.Neural.Networks;
using Encog.Util;

namespace Encog.Examples.Persist
{
    public class PersistSerial : IExample
    {
        private IExampleInterface app;

        public static ExampleInfo Info
        {
            get
            {
                ExampleInfo info = new ExampleInfo(
                    typeof(PersistSerial),
                    "persist-serial",
                    "Persist using .Net Serialization",
                    "Create and persist a neural network using .Net serialization.");
                return info;
            }
        }

        /// <summary>
        /// Input for the XOR function.
        /// </summary>
        public static double[][] XOR_INPUT ={
            new double[2] { 0.0, 0.0 },
            new double[2] { 1.0, 0.0 },
			new double[2] { 0.0, 1.0 },
            new double[2] { 1.0, 1.0 } };

        /// <summary>
        /// Ideal output for the XOR function.
        /// </summary>
        public static double[][] XOR_IDEAL = {                                              
            new double[1] { 0.0 }, 
            new double[1] { 1.0 }, 
            new double[1] { 1.0 }, 
            new double[1] { 0.0 } };

        public void Execute(IExampleInterface app)
        {
            this.app = app;
            this.app = app;
            INeuralDataSet trainingSet = new BasicNeuralDataSet(XOR_INPUT, XOR_IDEAL);
            BasicNetwork network = EncogUtility.SimpleFeedForward(2, 6, 0, 1, false);
            EncogUtility.TrainToError(network, trainingSet, 0.01);
            double error = network.CalculateError(trainingSet);
            SerializeObject.Save("encog.ser", network);
            network = (BasicNetwork)SerializeObject.Load("encog.ser");
            double error2 = network.CalculateError(trainingSet);
            app.WriteLine("Error before save to ser: " + Format.FormatPercent(error));
            app.WriteLine("Error before after to ser: " + Format.FormatPercent(error2));
        }
    }
}
