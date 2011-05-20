//
// Encog(tm) Core v3.0 - .Net Version
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
using Encog.Neural.Networks.Training;
using Encog.Util.Logging;

namespace Encog.ML.Train.Strategy
{
    /// <summary>
    /// A simple greedy strategy. If the last iteration did not improve training,
    /// then discard it. Care must be taken with this strategy, as sometimes a
    /// training algorithm may need to temporarily decrease the error level before
    /// improving it.
    /// </summary>
    ///
    public class Greedy : IStrategy
    {
        /// <summary>
        /// The error rate from the previous iteration.
        /// </summary>
        ///
        private double lastError;

        /// <summary>
        /// The last state of the network, so that we can restore to this
        /// state if needed.
        /// </summary>
        ///
        private double[] lastNetwork;

        private MLEncodable method;

        /// <summary>
        /// Has one iteration passed, and we are now ready to start 
        /// evaluation.
        /// </summary>
        ///
        private bool ready;

        /// <summary>
        /// The training algorithm that is using this strategy.
        /// </summary>
        ///
        private MLTrain train;

        #region IStrategy Members

        /// <summary>
        /// Initialize this strategy.
        /// </summary>
        ///
        /// <param name="train_0">The training algorithm.</param>
        public virtual void Init(MLTrain train_0)
        {
            train = train_0;
            ready = false;

            if (!(train_0.Method is MLEncodable))
            {
                throw new TrainingError(
                    "To make use of the Greedy strategy the machine learning method must support MLEncodable.");
            }

            method = ((MLEncodable) train_0.Method);
            lastNetwork = new double[method.EncodedArrayLength()];
        }

        /// <summary>
        /// Called just after a training iteration.
        /// </summary>
        ///
        public virtual void PostIteration()
        {
            if (ready)
            {
                if (train.Error > lastError)
                {
                    EncogLogging.Log(EncogLogging.LEVEL_DEBUG,
                                     "Greedy strategy dropped last iteration.");
                    train.Error = lastError;
                    method.DecodeFromArray(lastNetwork);
                }
            }
            else
            {
                ready = true;
            }
        }

        /// <summary>
        /// Called just before a training iteration.
        /// </summary>
        ///
        public virtual void PreIteration()
        {
            if (method != null)
            {
                lastError = train.Error;
                method.EncodeToArray(lastNetwork);
                train.Error = lastError;
            }
        }

        #endregion
    }
}
