﻿// Encog Artificial Intelligence Framework v2.x
// DotNet Version
// http://www.heatonresearch.com/encog/
// http://code.google.com/p/encog-cs/
// 
// Copyright 2009, Heaton Research Inc., and individual contributors.
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
using Encog.Neural.NeuralData;
using Encog.Neural.Networks.Training.Strategy;

namespace Encog.Neural.Networks.Training.Propagation.Back
{
    /// <summary>
    /// This class implements a backpropagation training algorithm for feed forward
    /// neural networks. It is used in the same manner as any other training class
    /// that implements the Train interface.
    /// 
    /// Backpropagation is a common neural network training algorithm. It works by
    /// analyzing the error of the output of the neural network. Each neuron in the
    /// output layer's contribution, according to weight, to this error is
    /// determined. These weights are then adjusted to minimize this error. This
    /// process continues working its way backwards through the layers of the neural
    /// network.
    /// 
    /// This implementation of the backpropagation algorithm uses both momentum and a
    /// learning rate. The learning rate specifies the degree to which the weight
    /// matrixes will be modified through each iteration. The momentum specifies how
    /// much the previous learning iteration affects the current. To use no momentum
    /// at all specify zero.
    /// 
    /// One primary problem with backpropagation is that the magnitude of the partial
    /// derivative is often detrimental to the training of the neural network. The
    /// other propagation methods of Manhatten and Resilient address this issue in
    /// different ways. In general, it is suggested that you use the resilient
    /// propagation technique for most Encog training tasks over back propagation.
    /// </summary>
    public class Backpropagation : Propagation, IMomentum,
            ILearningRate
    {

        /**
         * The momentum, this is the degree to which the previous training cycle
         * affects the current one.
         */
        private double momentum;

        /// <summary>
        /// The learning rate, this is value is essentially a percent. It is the
        /// degree to which the gradients are applied to the weight matrix to allow
        /// learning.
        /// </summary>
        private double learningRate;

        /// <summary>
        /// Create a class to train using backpropagation.
        /// </summary>
        /// <param name="network">The network that is to be trained.</param>
        /// <param name="training">The training data to be used for backpropagation.</param>
        public Backpropagation(BasicNetwork network,
                 INeuralDataSet training)
            : this(network, training, 0, 0)
        {
            AddStrategy(new SmartLearningRate());
            AddStrategy(new SmartMomentum());
        }

        /// <summary>
        /// The network that is to be trained
        /// </summary>
        /// <param name="network">The network to use.</param>
        /// <param name="training">The training set.</param>
        /// <param name="learnRate">The rate at which the weight matrix will be adjusted based on
        /// learning.</param>
        /// <param name="momentum">The influence that previous iteration's training deltas will
        /// have on the current iteration.</param>
        public Backpropagation(BasicNetwork network,
                 INeuralDataSet training, double learnRate,
                 double momentum)
            : base(network, new BackpropagationMethod(learnRate,momentum), training)
        {

            this.momentum = momentum;
            this.learningRate = learnRate;
        }

        /// <summary>
        /// The learning rate, this is value is essentially a percent. It is
        /// the degree to which the gradients are applied to the weight
        /// matrix to allow learning.
        /// </summary>
        public double LearningRate
        {
            get
            {
                return this.learningRate;
            }
            set
            {
                this.learningRate = value;
            }
        }

        /// <summary>
        /// The momentum for training.  This is the degree to which changes
        /// from which the previous training iteration will affect this training
        /// iteration.  This can be useful to overcome local minima.
        /// </summary>
        public double Momentum
        {
            get
            {
                return this.momentum;
            }
            set
            {
                this.momentum = value;
            }
        }
    }

}
