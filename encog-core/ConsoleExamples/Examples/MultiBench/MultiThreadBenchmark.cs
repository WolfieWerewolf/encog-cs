//
// Encog(tm) Console Examples v3.0 - .Net Version
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
using ConsoleExamples.Examples;
using Encog.ML.Data;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Util.Banchmark;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using Encog.Util.Logging;

namespace Encog.Examples.MultiBench
{
    public class MultiThreadBenchmark:IExample
    {
        public const int INPUT_COUNT = 40;
        public const int HIDDEN_COUNT = 60;
        public const int OUTPUT_COUNT = 20;

        private IExampleInterface app;

        public static ExampleInfo Info
        {
            get
            {
                ExampleInfo info = new ExampleInfo(
                    typeof(MultiThreadBenchmark),
                    "multibench",
                    "Multithreading Benchmark",
                    "See the effects that multithreading has on performance.");
                return info;
            }
        }


        public BasicNetwork generateNetwork()
        {
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(MultiThreadBenchmark.INPUT_COUNT));
            network.AddLayer(new BasicLayer(MultiThreadBenchmark.HIDDEN_COUNT));
            network.AddLayer(new BasicLayer(MultiThreadBenchmark.OUTPUT_COUNT));
            network.Structure.FinalizeStructure();
            network.Reset();
            return network;
        }

        public IMLDataSet generateTraining()
        {
            IMLDataSet training = RandomTrainingFactory.Generate(1000,50000,
                    INPUT_COUNT, OUTPUT_COUNT, -1, 1);
            return training;
        }

        public double evaluateRPROP(BasicNetwork network, IMLDataSet data)
        {

            ResilientPropagation train = new ResilientPropagation(network, data);
            long start = DateTime.Now.Ticks;
            Console.WriteLine(@"Training 20 Iterations with RPROP");
            for (int i = 1; i <= 1; i++)
            {
                train.Iteration();
                Console.WriteLine("Iteration #" + i + " Error:" + train.Error);
            }
            //train.FinishTraining();
            long stop = DateTime.Now.Ticks;
            double diff = new TimeSpan(stop - start).Seconds;
            Console.WriteLine("RPROP Result:" + diff + " seconds.");
            Console.WriteLine("Final RPROP error: " + network.CalculateError(data));
            return diff;
        }

        public double evaluateMPROP(BasicNetwork network, IMLDataSet data)
        {

            ResilientPropagation train = new ResilientPropagation(network, data);
            long start = DateTime.Now.Ticks;
            Console.WriteLine(@"Training 20 Iterations with MPROP");
            for (int i = 1; i <= 20; i++)
            {
                train.Iteration();
                Console.WriteLine("Iteration #" + i + " Error:" + train.Error);
            }
            //train.finishTraining();
            long stop = DateTime.Now.Ticks;
            double diff = new TimeSpan(stop - start).Seconds;
            Console.WriteLine("MPROP Result:" + diff + " seconds.");
            Console.WriteLine("Final MPROP error: " + network.CalculateError(data));
            return diff;
        }
        public void Execute(IExampleInterface app)
        {
            this.app = app;

            BasicNetwork network = generateNetwork();
            IMLDataSet data = generateTraining();

            double rprop = evaluateRPROP(network, data);
            double mprop = evaluateMPROP(network, data);
            double factor = rprop / mprop;
            Console.WriteLine("Factor improvement:" + factor);
        }
    }
}
