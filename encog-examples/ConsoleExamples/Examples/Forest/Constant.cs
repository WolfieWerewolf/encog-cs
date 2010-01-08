using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.Examples.Forest
{
    /// <summary>
    /// Modify these to suit the paths on your computer.
    /// </summary>
    public class Constant
    {
        /// <summary>
        /// The base directory that all of the data for this example is stored in.
        /// </summary>
        public const String BASE_DIRECTORY = "c:\\data\\";

        /// <summary>
        /// The source data file from which all others are built.  This file can
        /// be downloaded from:
        /// 
        /// http://kdd.ics.uci.edu/databases/covertype/covertype.html
        /// </summary>
        public const String COVER_TYPE_FILE = BASE_DIRECTORY + "covtype.data";

        /// <summary>
        /// 75% of the data will be moved into this file to be used as training data.  The 
        /// data is still in "raw form" in this file.
        /// </summary>
        public const String TRAINING_FILE = BASE_DIRECTORY + "training.csv";

        /// <summary>
        /// 25% of the data will be moved into this file to be used as evaluation data.  The 
        /// data is still in "raw form" in this file.
        /// </summary>
        public const String EVALUATE_FILE = BASE_DIRECTORY + "evaluate.csv";

        /// <summary>
        /// We will limit the number of samples per "tree type" to 3000, this causes the data
        /// to be more balanced and will not allow one tree type to over-fit the network.
        /// The training file is narrowed and placed into this file in "raw form".
        /// </summary>
        public const String BALANCE_FILE = BASE_DIRECTORY + "balance.csv";

        /// <summary>
        /// The training file is normalized and placed into this file.
        /// </summary>
        public const String NORMALIZED_FILE = BASE_DIRECTORY + "normalized.csv";

        /// <summary>
        /// CSV files are slow to parse with because the text must be converted into numbers.
        /// The balanced file will be converted to a binary file to be used for training.
        /// </summary>
        public const String BINARY_FILE = BASE_DIRECTORY + "normalized.bin";

        /// <summary>
        /// The trained network and normalizer will be saved into an Encog EG file.
        /// </summary>
        public const String TRAINED_NETWORK_FILE = BASE_DIRECTORY + "forest.eg";

        /// <summary>
        /// The name of the network inside of the EG file.
        /// </summary>
        public const String TRAINED_NETWORK_NAME = "forest-network";

        /// <summary>
        /// The name of the normalization object inside of the EG file.
        /// </summary>
        public const String NORMALIZATION_NAME = "forest-norm";

        /// <summary>
        /// How many minutes to train for (console mode only)
        /// </summary>
        public const int TRAINING_MINUTES = 10;

        /// <summary>
        /// How many hidden neurons to use.
        /// </summary>
        public const int HIDDEN_COUNT = 100;
    }
}
