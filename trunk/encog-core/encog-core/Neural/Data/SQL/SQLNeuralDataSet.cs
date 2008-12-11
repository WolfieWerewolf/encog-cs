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
using Encog.Util.DB;
using Encog.Neural.Data.Basic;
using Encog.Neural.Data;

namespace Encog.Neural.NeuralData.SQL
{
    /// <summary>
    /// A dataset based on a SQL query. This is not a memory based dataset, so it can
    /// handle very large datasets without a memory issue. and can handle very large
    /// datasets.
    /// </summary>
    public class SQLNeuralDataSet : INeuralDataSet, IEnumerable<INeuralDataPair>
    {
        /// <summary>
        /// Enumerator for the SQLNeuralDataSet.
        /// </summary>
        public class SQLNeuralEnumerator : IEnumerator<INeuralDataPair>
        {
            private SQLNeuralDataSet owner;


            private INeuralDataPair current;

            /// <summary>
            /// Holds results from the SQL query.
            /// </summary>
            private RepeatableStatement.Results results;


            /// <summary>
            /// Construct the SQLNeuralEnumerator.
            /// </summary>
            /// <param name="owner">The owner of the enumerator.</param>
            public SQLNeuralEnumerator(SQLNeuralDataSet owner)
            {
                this.owner = owner;
                this.results = owner.statement.ExecuteQuery();
            }

            /// <summary>
            /// The current data item.
            /// </summary>
            public INeuralDataPair Current
            {
                get
                {
                    if (this.current == null)
                    {
                        MoveNext();
                    }
                    return this.current;
                }
            }

            /// <summary>
            /// Dispose of this object.
            /// </summary>
            public void Dispose()
            {
                this.results.Close();
            }

            /// <summary>
            /// Obtain the current item.
            /// </summary>
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    if (this.current == null)
                    {
                        MoveNext();
                    }
                    return this.current;
                }
            }

            /// <summary>
            /// Move to the next object.
            /// </summary>
            /// <returns>True if there is a next object.</returns>
            public bool MoveNext()
            {
                if (!this.results.DataReader.NextResult())
                    return false;
                INeuralData input = new BasicNeuralData(owner.inputSize);
                INeuralData ideal = null;

                for (int i = 1; i <= owner.inputSize; i++)
                {
                    input[i - 1] = this.results.DataReader.GetDouble(i);
                }

                if (owner.idealSize > 0)
                {
                    ideal =
                    new BasicNeuralData(owner.idealSize);
                    for (int i = 1; i <= owner.idealSize; i++)
                    {
                        ideal[i - 1] =
                            this.results.DataReader.GetDouble(i + owner.inputSize);
                    }

                }

                this.current = new BasicNeuralDataPair(input, ideal);
                return true;
            }

            /// <summary>
            /// Not supported.
            /// </summary>
            public void Reset()
            {
                throw new NotImplementedException();
            }


        }

        /// <summary>
        /// What is the size of the input data?
        /// </summary>
        private int inputSize;

        /// <summary>
        /// What is the size of the ideal data?
        /// </summary>
        private int idealSize;

        /// <summary>
        /// The database connection.
        /// </summary>
        private RepeatableConnection connection;

        /// <summary>
        /// The SQL statement being used.
        /// </summary>
        private RepeatableStatement statement;

        /// <summary>
        /// Create a SQL neural data set.
        /// </summary>
        /// <param name="sql">The SQL to execute.</param>
        /// <param name="inputSize">The size of the input data being read.</param>
        /// <param name="idealSize">The size of the ideal output data being read.</param>
        /// <param name="connectString">The connection string.</param>
        public SQLNeuralDataSet(String sql, int inputSize,
                 int idealSize, String connectString)
        {
            this.inputSize = inputSize;
            this.idealSize = idealSize;
            this.connection = new RepeatableConnection(connectString);
            this.statement = this.connection.CreateStatement(sql);
            this.connection.Open();
        }


        /// <summary>
        /// The size of the ideal data, zero if unsupervised.
        /// </summary>
        public int IdealSize
        {
            get
            {
                return this.idealSize;
            }
        }

        /// <summary>
        /// The size of the input data.
        /// </summary>
        public int InputSize
        {
            get
            {
                return this.inputSize;
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="data1">Not used.</param>
        public void Add(INeuralData data1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="inputData">Not used.</param>
        /// <param name="idealData">Not used.</param>
        public void Add(INeuralData inputData, INeuralData idealData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not used.
        /// </summary>
        /// <param name="inputData">Not used.</param>
        public void Add(INeuralDataPair inputData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// Get an enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        public IEnumerator<INeuralDataPair> GetEnumerator()
        {
            return new SQLNeuralEnumerator(this);
        }

        /// <summary>
        /// Get an enumerator.
        /// </summary>
        /// <returns>The enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new SQLNeuralEnumerator(this);
        }

    }
}
