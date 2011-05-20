//
// Encog(tm) Core v3.0 - .Net Version
// http://www.heatonresearch.com/encog/
//
// Copyright 2008-2011 Heaton Research, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//   
// For more information on Heaton Research copyrights, licenses 
// and trademarks visit:
// http://www.heatonresearch.com/copyright
//
using System;
using System.IO;
using System.Text;
using Encog.Engine.Network.Activation;
using Encog.MathUtil.Error;
using Encog.ML;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Data.Buffer;
using Encog.ML.Data.Buffer.CODEC;
using Encog.ML.Data.Specific;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training.Propagation;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Neural.Pattern;
using Encog.Util.CSV;
#if !SILVERLIGHT

#endif

namespace Encog.Util.Simple
{
    /// <summary>
    /// General utility class for Encog.  Provides for some common Encog procedures.
    /// </summary>
    public class EncogUtility
    {
        /// <summary>
        /// Private constructor.
        /// </summary>
        private EncogUtility()
        {
        }

        /// <summary>
        /// Convert a CSV file to a binary training file.
        /// </summary>
        /// <param name="csvFile">The CSV file.</param>
        /// <param name="format">The format.</param>
        /// <param name="binFile">The binary file.</param>
        /// <param name="inputCount">The number of input values.</param>
        /// <param name="outputCount">The number of output values.</param>
        /// <param name="headers">True, if there are headers on the3 CSV.</param>
        public static void ConvertCSV2Binary(String csvFile, CSVFormat format,
                                             String binFile, int inputCount, int outputCount,
                                             bool headers)
        {
            Directory.Delete(binFile);

            var csv = new CSVMLDataSet(csvFile,
                                       inputCount, outputCount, false, format);
            var buffer = new BufferedMLDataSet(binFile);
            buffer.BeginLoad(inputCount, outputCount);
            foreach (MLDataPair pair in csv)
            {
                buffer.Add(pair);
            }
            buffer.EndLoad();
        }

        /// <summary>
        /// Convert a CSV file to binary.
        /// </summary>
        /// <param name="csvFile">The CSV file to convert.</param>
        /// <param name="format">The format.</param>
        /// <param name="binFile">The binary file.</param>
        /// <param name="input">The input.</param>
        /// <param name="ideal">The ideal.</param>
        /// <param name="headers">True, if headers are present.</param>
        public static void ConvertCSV2Binary(FileInfo csvFile, CSVFormat format,
                                             FileInfo binFile, int[] input, int[] ideal, bool headers)
        {
            binFile.Delete();
            var csv = new ReadCSV(csvFile.ToString(), headers, format);

            var buffer = new BufferedMLDataSet(binFile.ToString());
            buffer.BeginLoad(input.Length, ideal.Length);
            while (csv.Next())
            {
                var inputData = new BasicMLData(input.Length);
                var idealData = new BasicMLData(ideal.Length);

                // handle input data
                for (int i = 0; i < input.Length; i++)
                {
                    inputData[i] = csv.GetDouble(input[i]);
                }

                // handle input data
                for (int i_0 = 0; i_0 < ideal.Length; i_0++)
                {
                    idealData[i_0] = csv.GetDouble(ideal[i_0]);
                }

                // add to dataset

                buffer.Add(inputData, idealData);
            }
            buffer.EndLoad();
        }

        /// <summary>
        /// Load CSV to memory.
        /// </summary>
        /// <param name="filename">The CSV file to load.</param>
        /// <param name="input">The input count.</param>
        /// <param name="ideal">The ideal count.</param>
        /// <param name="headers">True, if headers are present.</param>
        /// <param name="format">The loaded dataset.</param>
        /// <returns></returns>
        public static MLDataSet LoadCSV2Memory(String filename, int input, int ideal, bool headers, CSVFormat format)
        {
            IDataSetCODEC codec = new CSVDataCODEC(filename, format, headers, input, ideal);
            var load = new MemoryDataLoader(codec);
            MLDataSet dataset = load.External2Memory();
            return dataset;
        }

        /// <summary>
        /// Evaluate the network and display (to the console) the output for every
        /// value in the training set. Displays ideal and actual.
        /// </summary>
        /// <param name="network">The network to evaluate.</param>
        /// <param name="training">The training set to evaluate.</param>
        public static void Evaluate(MLRegression network,
                                    MLDataSet training)
        {
            foreach (MLDataPair pair in training)
            {
                MLData output = network.Compute(pair.Input);
                Console.WriteLine("Input="
                                  + FormatNeuralData(pair.Input)
                                  + ", Actual=" + FormatNeuralData(output)
                                  + ", Ideal="
                                  + FormatNeuralData(pair.Ideal));
            }
        }

        /// <summary>
        /// Format neural data as a list of numbers.
        /// </summary>
        /// <param name="data">The neural data to format.</param>
        /// <returns>The formatted neural data.</returns>
        private static String FormatNeuralData(MLData data)
        {
            var result = new StringBuilder();
            for (int i = 0; i < data.Count; i++)
            {
                if (i != 0)
                {
                    result.Append(',');
                }
                result.Append(Format.FormatDouble(data[i], 4));
            }
            return result.ToString();
        }

        /// <summary>
        /// Create a simple feedforward neural network.
        /// </summary>
        /// <param name="input">The number of input neurons.</param>
        /// <param name="hidden1">The number of hidden layer 1 neurons.</param>
        /// <param name="hidden2">The number of hidden layer 2 neurons.</param>
        /// <param name="output">The number of output neurons.</param>
        /// <param name="tanh">True to use hyperbolic tangent activation function, false to
        /// use the sigmoid activation function.</param>
        /// <returns>The neural network.</returns>
        public static BasicNetwork SimpleFeedForward(int input,
                                                     int hidden1, int hidden2, int output,
                                                     bool tanh)
        {
            var pattern = new FeedForwardPattern();
            pattern.InputNeurons = input;
            pattern.OutputNeurons = output;
            if (tanh)
            {
                pattern.ActivationFunction = new ActivationTANH();
            }
            else
            {
                pattern.ActivationFunction = new ActivationSigmoid();
            }

            if (hidden1 > 0)
            {
                pattern.AddHiddenLayer(hidden1);
            }
            if (hidden2 > 0)
            {
                pattern.AddHiddenLayer(hidden2);
            }

            var network = (BasicNetwork) pattern.Generate();
            network.Reset();
            return network;
        }

        /// <summary>
        /// Train the neural network, using SCG training, and output status to the
        /// console.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="minutes">The number of minutes to train for.</param>
        public static void TrainConsole(BasicNetwork network,
                                        MLDataSet trainingSet, int minutes)
        {
            Propagation train = new ResilientPropagation(network,
                                                         trainingSet);
            train.NumThreads = 0;
            TrainConsole(train, network, trainingSet, minutes);
        }


        /// <summary>
        /// Train the network, using the specified training algorithm, and send the
        /// output to the console.
        /// </summary>
        /// <param name="train">The training method to use.</param>
        /// <param name="network">The network to train.</param>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="minutes">The number of minutes to train for.</param>
        public static void TrainConsole(MLTrain train,
                                        BasicNetwork network, MLDataSet trainingSet,
                                        int minutes)
        {
            int epoch = 1;
            long remaining;

            Console.WriteLine("Beginning training...");
            long start = Environment.TickCount;
            do
            {
                train.Iteration();

                long current = Environment.TickCount;
                long elapsed = (current - start)/1000;
                remaining = minutes - elapsed/60;

                Console.WriteLine("Iteration #" + Format.FormatInteger(epoch)
                                  + " Error:" + Format.FormatPercent(train.Error)
                                  + " elapsed time = " + Format.FormatTimeSpan((int) elapsed)
                                  + " time left = "
                                  + Format.FormatTimeSpan((int) remaining*60));
                epoch++;
            } while (remaining > 0 && !train.TrainingDone);
            train.FinishTraining();
        }

#if !SILVERLIGHT
        /// <summary>
        /// Train using RPROP and display progress to a dialog box.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="trainingSet">The training set to use.</param>
        public static void TrainDialog(BasicNetwork network,
                                       MLDataSet trainingSet)
        {
            Propagation train = new ResilientPropagation(network,
                                                         trainingSet);
            train.NumThreads = 0;
            TrainDialog(train, network, trainingSet);
        }
#endif

#if !SILVERLIGHT
        /// <summary>
        /// Train, using the specified training method, display progress to a dialog
        /// box.
        /// </summary>
        /// <param name="train">The training method to use.</param>
        /// <param name="network">The network to train.</param>
        /// <param name="trainingSet">The training set to use.</param>
        public static void TrainDialog(MLTrain train,
                                       BasicNetwork network, MLDataSet trainingSet)
        {
            var dialog = new TrainingDialog();
            dialog.Train = train;
            dialog.ShowDialog();
        }
#endif

        /// <summary>
        /// Train the network, to a specific error, send the output to the console.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="trainingSet">The training set to use.</param>
        /// <param name="error">The error level to train to.</param>
        public static void TrainToError(BasicNetwork network,
                                        MLDataSet trainingSet, double error)
        {
            Propagation train = new ResilientPropagation(network,
                                                         trainingSet);
            train.NumThreads = 0;
            TrainToError(train, trainingSet, error);
        }

        /// <summary>
        /// Train to a specific error, using the specified training method, send the
        /// output to the console.
        /// </summary>
        /// <param name="train">The training method.</param>
        /// <param name="trainingSet">The training set to use.</param>
        /// <param name="error">The desired error level.</param>
        public static void TrainToError(MLTrain train,
                                        MLDataSet trainingSet,
                                        double error)
        {
            int epoch = 1;

            Console.WriteLine("Beginning training...");

            do
            {
                train.Iteration();

                Console.WriteLine("Iteration #" + Format.FormatInteger(epoch)
                                  + " Error:" + Format.FormatPercent(train.Error)
                                  + " Target Error: " + Format.FormatPercent(error));
                epoch++;
            } while (train.Error > error && !train.TrainingDone);
            train.FinishTraining();
        }

        /// <summary>
        /// Calculate a regression error.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <param name="data">The data to check.</param>
        /// <returns>The error.</returns>
        public static double CalculateRegressionError(MLRegression method,
                                                      MLDataSet data)
        {
            var errorCalculation = new ErrorCalculation();
            if (method is MLContext)
                ((MLContext) method).ClearContext();


            foreach (MLDataPair pair in data)
            {
                MLData actual = method.Compute(pair.Input);
                errorCalculation.UpdateError(actual.Data, pair.Ideal.Data);
            }
            return errorCalculation.Calculate();
        }

        /// <summary>
        /// Save the dataset to a CSV file.
        /// </summary>
        /// <param name="targetFile">The target file.</param>
        /// <param name="format">The format to use.</param>
        /// <param name="set">The data set.</param>
        public static void SaveCSV(FileInfo targetFile, CSVFormat format, MLDataSet set)
        {
            try
            {
                var file = new StreamWriter(targetFile.ToString());

                foreach (MLDataPair data in set)
                {
                    var line = new StringBuilder();

                    for (int i = 0; i < data.Input.Count; i++)
                    {
                        double d = data.Input[i];
                        Encog.App.Analyst.CSV.Basic.BasicFile.AppendSeparator(line, format);
                        line.Append(format.Format(d, EncogFramework.DEFAULT_PRECISION));
                    }

                    for (int i_0 = 0; i_0 < data.Ideal.Count; i_0++)
                    {
                        double d_1 = data.Ideal[i_0];
                        Encog.App.Analyst.CSV.Basic.BasicFile.AppendSeparator(line, format);
                        line.Append(format.Format(d_1, EncogFramework.DEFAULT_PRECISION));
                    }

                    file.WriteLine(line);
                }

                file.Close();
            }
            catch (IOException ex)
            {
                throw new EncogError(ex);
            }
        }

        /// <summary>
        /// Calculate an error for a method that makes use of classification.
        /// </summary>
        /// <param name="method">The method to check.</param>
        /// <param name="data">The data to check.</param>
        /// <returns>The error.</returns>
        public static double CalculateClassificationError(MLClassification method,
                                                          MLDataSet data)
        {
            int total = 0;
            int correct = 0;


            foreach (MLDataPair pair in data)
            {
                var ideal = (int) pair.Ideal[0];
                int actual = method.Classify(pair.Input);
                if (actual == ideal)
                    correct++;
                total++;
            }
            return (total - correct)/(double) total;
        }

        /// <summary>
        /// Load an EGB file to memory.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>A memory data set.</returns>
        public static MLDataSet LoadEGB2Memory(FileInfo filename)
        {
            var buffer = new BufferedMLDataSet(filename.ToString());
            return buffer.LoadToMemory();
        }

        /// <summary>
        /// Train to a specific error, using the specified training method, send the
        /// output to the console.
        /// </summary>
        ///
        /// <param name="train">The training method.</param>
        /// <param name="error">The desired error level.</param>
        public static void TrainToError(MLTrain train, double error)
        {

            int epoch = 1;

            System.Console.Out.WriteLine("Beginning training...");

            do
            {
                train.Iteration();

                System.Console.Out.WriteLine("Iteration #" + Encog.Util.Format.FormatInteger(epoch)
                        + " Error:" + Encog.Util.Format.FormatPercent(train.Error)
                        + " Target Error: " + Encog.Util.Format.FormatPercent(error));
                epoch++;
            } while ((train.Error > error) && !train.TrainingDone);
            train.FinishTraining();
        }
    }
}
