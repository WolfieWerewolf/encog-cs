// Encog Neural Network and Bot Library for DotNet v0.5
// http://www.heatonresearch.com/encog/
// http://code.google.com/p/encog-cs/
// 
// Copyright 2008, Heaton Research Inc., and individual contributors.
// See the copyright.txt in the distribution for a full listing of 
// individual contributors.
//
// This is free software; you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation; either version 2.1 of
// the License, or (at your option) any later version.
//
// This software is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this software; if not, write to the Free
// Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301 USA, or see the FSF site: http://www.fsf.org.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Encog.Neural.Feedforward;
using Encog.Neural.Feedforward.Train.Anneal;

namespace EncogExamplesCS
{
    /// <summary>
    /// Learn to recognize the XOR pattern using a simulated annealing training algorithm.
    /// </summary>
    class XorAnneal
    {
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

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Not used.</param>
        static void Main(string[] args)
        {
            FeedforwardNetwork network = new FeedforwardNetwork();
            network.AddLayer(new FeedforwardLayer(2));
            network.AddLayer(new FeedforwardLayer(3));
            network.AddLayer(new FeedforwardLayer(1));
            network.Reset();

            // train the neural network
            NeuralSimulatedAnnealing train = new NeuralSimulatedAnnealing(
                    network, XOR_INPUT, XOR_IDEAL, 10, 2, 100);

            int epoch = 1;

            do
            {
                train.Iteration();
                Console.WriteLine("Epoch #" + epoch + " Error:" + train.Error);
                epoch++;
            } while ((epoch < 100) && (train.Error > 0.001));

            // network = train.getNetwork();

            // test the neural network
            Console.WriteLine("Neural Network Results:");
            for (int i = 0; i < XOR_IDEAL.Length; i++)
            {
                double[] actual = network.ComputeOutputs(XOR_INPUT[i]);
                Console.WriteLine(XOR_INPUT[i][0] + "," + XOR_INPUT[i][1]
                        + ", actual=" + actual[0] + ",ideal=" + XOR_IDEAL[i][0]);
            }
        }
    }
}
