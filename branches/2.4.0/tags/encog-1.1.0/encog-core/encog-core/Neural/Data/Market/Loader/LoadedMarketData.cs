﻿// Encog Neural Network and Bot Library v1.x (DotNet)
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

namespace Encog.Neural.NeuralData.Market.Loader
{
    /// <summary>
    /// Market data loaded from a source.
    /// </summary>
    public class LoadedMarketData
    {
        /// <summary>
        /// When was this data sample taken.
        /// </summary>
        private DateTime when;

        /// <summary>
        /// What is the ticker symbol for this data sample.
        /// </summary>
        private TickerSymbol ticker;

        /// <summary>
        /// The data that was collection for the sample date.
        /// </summary>
        private IDictionary<MarketDataType, Double> data;

        /// <summary>
        /// Set the specified type of data.
        /// </summary>
        /// <param name="t">The type of data to set.</param>
        /// <param name="d">The value to set.</param>
        public void SetData(MarketDataType t, double d)
        {
            this.data[t] = d;
        }

        /// <summary>
        /// Get the specified data type.
        /// </summary>
        /// <param name="t">The type of data to get.</param>
        /// <returns>The value.</returns>
        public double GetData(MarketDataType t)
        {
            return this.data[t];
        }

        /// <summary>
        /// When is this data from.
        /// </summary>
        public DateTime When
        {
            get
            {
                return this.when;
            }
            set
            {
                this.when = value;
            }
        }

        /// <summary>
        /// The ticker symbol that this data was from.
        /// </summary>
        public TickerSymbol Ticker
        {
            get
            {
                return this.ticker;
            }
        }

        /// <summary>
        /// The data that was downloaded.
        /// </summary>
        public IDictionary<MarketDataType, Double> Data
        {
            get
            {
                return this.data;
            }
        }


        /// <summary>
        /// Construct one sample of market data.
        /// </summary>
        /// <param name="when">When was this sample taken.</param>
        /// <param name="ticker">What is the ticker symbol for this data.</param>
        public LoadedMarketData(DateTime when, TickerSymbol ticker)
        {
            this.when = when;
            this.ticker = ticker;
            this.data = new Dictionary<MarketDataType, Double>();
        }
    }
}
