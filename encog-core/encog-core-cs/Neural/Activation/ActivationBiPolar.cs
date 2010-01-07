﻿// Encog Artificial Intelligence Framework v2.x
// DotNet Version
// http://www.heatonresearch.com/encog/
// http://code.google.com/p/encog-cs/
// 
// Copyright 2009-2010, Heaton Research Inc., and individual contributors.
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
using Encog.Persist;
using Encog.Persist.Persistors;

namespace Encog.Neural.Activation
{
    /// <summary>
    /// BiPolar activation function. This will scale the neural data into the bipolar
    /// range. Greater than zero becomes 1, less than or equal to zero becomes -1.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class ActivationBiPolar : BasicActivationFunction
    {
        /// <summary>
        /// Implements the activation function.  The array is modified according
        /// to the activation function being used.  See the class description
        /// for more specific information on this type of activation function.
        /// </summary>
        /// <param name="d">The input array to the activation function.</param>
        public override void ActivationFunction(double[] d)
        {
            for (int i = 0; i < d.Length; i++)
            {
                if (d[i] > 0)
                {
                    d[i] = 1;
                }
                else
                {
                    d[i] = -1;
                }
            }

        }

        /// <summary>
        /// The object cloned.
        /// </summary>
        /// <returns>The cloned object.</returns>
        public override Object Clone()
        {
            return new ActivationBiPolar();
        }

        /// <summary>
        /// Create a Persistor for this activation function.
        /// </summary>
        /// <returns>The persistor.</returns>
        public override IPersistor CreatePersistor()
        {
            return new ActivationBiPolarPersistor();
        }

        /// <summary>
        /// Implements the activation function derivative.  The array is modified 
        /// according derivative of the activation function being used.  See the 
        /// class description for more specific information on this type of 
        /// activation function. Propagation training requires the derivative. 
        /// Some activation functions do not support a derivative and will throw
        /// an error.
        /// </summary>
        /// <param name="d">The input array to the activation function.</param>
        public override void DerivativeFunction(double[] d)
        {
            throw new NeuralNetworkError(
                    "Can't use the bipolar activation function "
                            + "where a derivative is required.");

        }

        /// <summary>
        /// Return false, bipolar has no derivative.
        /// </summary>
        public override bool HasDerivative
        {
            get
            {
                return false;
            }
        }
    }

}