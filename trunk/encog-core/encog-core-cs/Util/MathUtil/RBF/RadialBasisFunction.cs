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

namespace Encog.Util.MathUtil.RBF
{
    /// <summary>
    /// Provides a generic interface to a radial basis function (RBF). Encog uses
    /// RBF's for a variety of purposes.
    /// </summary>
    public interface IRadialBasisFunction
    {
        /// <summary>
        /// Calculate the RBF result for the specified value.
        /// </summary>
        /// <param name="x">The value to be passed into the RBF.</param>
        /// <returns>The RBF value.</returns>
        double Calculate(double x);

        /// <summary>
        /// Calculate the derivative of the RBF function.
        /// </summary>
        /// <param name="x">The value to calculate for.</param>
        /// <returns>The calculated value.</returns>
        double CalculateDerivative(double x);

        /// <summary>
        /// The center of the RBF.
        /// </summary>
        double Center
        {
            get;
        }

        /// <summary>
        /// The peak of the RBF.
        /// </summary>
        double Peak
        {
            get;
        }

        /// <summary>
        /// The width of the RBF.
        /// </summary>
        double Width
        {
            get;
        }
    }
}