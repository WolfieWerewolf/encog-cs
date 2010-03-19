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

namespace Encog.Normalize.Segregate
{
    /// <summary>
    /// Segregators are used to exclude certain rows. You may want to exclude rows to
    /// create training and validation sets. You may also simply wish to exclude some
    /// rows because they do not apply to what you are currently training for.
    /// </summary>
    public interface ISegregator
    {
        /// <summary>
        /// The normalization object that is being used with this segregator.
        /// </summary>
        DataNormalization Owner { get; }

        /// <summary>
        /// Setup this object to use the specified normalization object.
        /// </summary>
        /// <param name="normalization">The normalization object to use.</param>
        void Init(DataNormalization normalization);

        /// <summary>
        /// Should this row be included, according to this segregator.
        /// </summary>
        /// <returns>True if this row should be included.</returns>
        bool ShouldInclude();

        /// <summary>
        /// Init for a pass.
        /// </summary>
        void PassInit();

    }
}