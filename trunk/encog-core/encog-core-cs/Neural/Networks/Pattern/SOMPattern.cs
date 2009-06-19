﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Encog.Neural.Activation;
using Encog.Neural.Networks.Layers;

namespace Encog.Neural.Networks.Pattern
{
    /// <summary>
    /// A self organizing map is a neural network pattern with an input
    /// and output layer.  There is no hidden layer.  The winning neuron,
    /// which is that neuron with the higest output is the winner, this
    /// winning neuron is often used to classify the input into a group.
    /// </summary>
    public class SOMPattern : INeuralNetworkPattern
    {
        /// <summary>
        /// The number of input neurons.
        /// </summary>
        private int inputNeurons;

        /// <summary>
        /// The number of output neurons.
        /// </summary>
        private int outputNeurons;

        /// <summary>
        /// The logging object.
        /// </summary>
        private readonly ILog logger = LogManager.GetLogger(typeof(SOMPattern));

        /// <summary>
        /// Add a hidden layer. SOM networks do not have hidden layers, so this will
        /// throw an error.
        /// </summary>
        /// <param name="count">The number of hidden neurons.</param>
        public void AddHiddenLayer(int count)
        {
            String str = "A SOM network does not have hidden layers.";
            if (this.logger.IsErrorEnabled)
            {
                this.logger.Error(str);
            }
            throw new PatternError(str);

        }

        /// <summary>
        /// Generate the RSOM network.
        /// </summary>
        /// <returns>The neural network.</returns>
        public BasicNetwork Generate()
        {
            ILayer input = new BasicLayer(new ActivationLinear(), false,
                    this.inputNeurons);
            ILayer output = new BasicLayer(new ActivationLinear(), false,
                    this.outputNeurons);
            int y = PatternConst.START_Y;
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(input);
            network.AddLayer(output);
            input.X = PatternConst.START_X;
            output.X = PatternConst.START_X;
            input.Y = y;
            y += PatternConst.INC_Y;
            output.Y = y;
            network.Structure.FinalizeStructure();
            network.Reset();
            return network;
        }

        /// <summary>
        /// Set the activation function.  A SOM uses a linear activation
        /// function, so this method throws an error.
        /// </summary>
        public IActivationFunction ActivationFunction
        {
            set
            {
                String str = "A SOM network can't define an activation function.";
                if (this.logger.IsErrorEnabled)
                {
                    this.logger.Error(str);
                }
                throw new PatternError(str);
            }
            get
            {
                return null;
            }

        }


        /// <summary>
        /// Set the number of output neurons.
        /// </summary>
        public int OutputNeurons
        {
            get
            {
                return this.outputNeurons;
            }
            set
            {
                this.outputNeurons = value;
            }

        }

        /// <summary>
        /// The number of input neurons.
        /// </summary>
        public int InputNeurons
        {
            get
            {
                return this.inputNeurons;
            }
            set
            {
                this.inputNeurons = value;
            }

        }

        /// <summary>
        /// Does nothing, no optinal hidden layers to clear, only the ONE 
        /// predefined hidden layer.
        /// </summary>
        public void Clear()
        {
        }
    }

}
