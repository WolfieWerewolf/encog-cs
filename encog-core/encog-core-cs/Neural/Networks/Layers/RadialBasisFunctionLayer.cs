﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Activation;
using Encog.Util.MathUtil.RBF;
using log4net;
using Encog.Util.MathUtil;
using Encog.Util.Randomize;
using Encog.Neural.Data;
using Encog.Neural.Data.Basic;
using Encog.Persist;
using Encog.Persist.Persistors;

namespace Encog.Neural.Networks.Layers
{
    /// <summary>
    /// This layer type makes use of several radial basis function to scale the
    /// output from this layer. Each RBF can have a different center, peak, and
    /// width. Proper selection of these values will greatly impact the success of
    /// the layer. Currently, Encog provides no automated way of determining these
    /// values. There is one RBF per neuron.
    /// 
    /// Radial basis function layers have neither thresholds nor a regular activation
    /// function. Calling any methods that deal with the activation function or
    /// thresholds will result in an error.
    /// </summary>
    public class RadialBasisFunctionLayer : BasicLayer
    {

        /**
         * The serial id.
         */
        private const long serialVersionUID = 2779781041654829282L;

        /// <summary>
        /// The logging object.
        /// </summary>
        private readonly ILog logger = LogManager.GetLogger(typeof(BasicNetwork));

        /// <summary>
        /// The radial basis functions to use, there should be one for each neuron.
        /// </summary>
        private IRadialBasisFunction[] radialBasisFunction;

        /// <summary>
        /// Default constructor, mainly so the workbench can easily create a default
        /// layer.
        /// </summary>
        public RadialBasisFunctionLayer()
            : this(1)
        {

        }

        /// <summary>
        /// Construct a radial basis function layer.
        /// </summary>
        /// <param name="neuronCount">The neuron count.</param>
        public RadialBasisFunctionLayer(int neuronCount)
            : base(new ActivationLinear(), false, neuronCount)
        {
            this.radialBasisFunction = new IRadialBasisFunction[neuronCount];
        }


        /// <summary>
        /// Compute the values before sending output to the next layer.
        /// This function allows the activation functions to be called.
        /// </summary>
        /// <param name="pattern">The incoming Project.</param>
        /// <returns>The output from this layer.</returns>
        public INeuralData compute(INeuralData pattern)
        {

            INeuralData result = new BasicNeuralData(NeuronCount);

            for (int i = 0; i < NeuronCount; i++)
            {

                if (this.radialBasisFunction[i] == null)
                {
                    String str =
               "Error, must define radial functions for each neuron";
                    if (this.logger.IsErrorEnabled)
                    {
                        this.logger.Error(str);
                    }
                    throw new NeuralNetworkError(str);
                }

                IRadialBasisFunction f = this.radialBasisFunction[i];
                double total = 0;
                for (int j = 0; j < pattern.Count; j++)
                {
                    double value = f.Calculate(pattern[j]);
                    total += value * value;
                }

                result[i] = BoundMath.Sqrt(total);

            }

            return result;
        }

        /// <summary>
        /// Create a persistor for this layer.
        /// </summary>
        /// <returns></returns>
        public override IPersistor CreatePersistor()
        {
            return new RadialBasisFunctionLayerPersistor();
        }

        /// <summary>
        /// The activation function for this layer, in this case
        /// it produces an error because RBF layers do not have an
        /// activation function.
        /// </summary>
        public override IActivationFunction ActivationFunction
        {
            get
            {
                String str =
                   "Should never call getActivationFunction on "
                   + "RadialBasisFunctionLayer, this layer has a compound "
                   + "activation function setup.";
                if (this.logger.IsErrorEnabled)
                {
                    this.logger.Error(str);
                }
                throw new NeuralNetworkError(str);
            }
        }

        /// <summary>
        /// An array of radial basis functions.
        /// </summary>
        public IRadialBasisFunction[] RadialBasisFunction
        {
            get
            {
                return this.radialBasisFunction;
            }
        }

        /// <summary>
        /// Set the gausian components to random values.
        /// </summary>
        /// <param name="min">The minimum value for the centers, widths and peaks.</param>
        /// <param name="max">The maximum value for the centers, widths and peaks.</param>
        public void RandomizeGaussianCentersAndWidths(double min,
                 double max)
        {
            for (int i = 0; i < this.NeuronCount; i++)
            {
                this.radialBasisFunction[i] = new GaussianFunction(RangeRandomizer
                        .Randomize(min, max), RangeRandomizer.Randomize(min, max),
                        RangeRandomizer.Randomize(min, max));
            }
        }

    }

}