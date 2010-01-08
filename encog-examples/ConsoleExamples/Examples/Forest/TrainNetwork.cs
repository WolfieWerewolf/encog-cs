using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using Encog.Neural.NeuralData;
using Encog.Neural.Activation;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Logic;
using Encog.Persist;
using Encog.Normalize;
using Encog.Neural.Data.Buffer;
using Encog.Util.Simple;
using System.IO;

namespace Encog.Examples.Forest
{
    public class TrainNetwork
    {
        private IExampleInterface app;

        public TrainNetwork(IExampleInterface app)
        {
            this.app = app;
        }

        public BasicNetwork GenerateNetwork(INeuralDataSet trainingSet)
        {
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, trainingSet.InputSize));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, Constant.HIDDEN_COUNT));
            network.AddLayer(new BasicLayer(new ActivationSigmoid(), true, trainingSet.IdealSize));
            network.Logic = new FeedforwardLogic();
            network.Structure.FinalizeStructure();
            network.Reset();
            return network;
        }

        public void Train(bool useGUI)
        {
            app.WriteLine("Converting training file to binary");
            EncogPersistedCollection encog = new EncogPersistedCollection(Constant.TRAINED_NETWORK_FILE, FileMode.Open);
            DataNormalization norm = (DataNormalization)encog.Find(Constant.NORMALIZATION_NAME);

            EncogUtility.convertCSV2Binary(Constant.NORMALIZED_FILE, Constant.BINARY_FILE, norm.GetNetworkInputLayerSize(), norm.GetNetworkOutputLayerSize(), false);
            BufferedNeuralDataSet trainingSet = new BufferedNeuralDataSet(Constant.BINARY_FILE);

            BasicNetwork network = (BasicNetwork)encog.Find(Constant.TRAINED_NETWORK_NAME);
            if (network == null)
                network = EncogUtility.simpleFeedForward(norm.GetNetworkInputLayerSize(), Constant.HIDDEN_COUNT, 0, norm.GetNetworkOutputLayerSize(), false);

            if (useGUI)
            {
                EncogUtility.trainDialog(network, trainingSet);
            }
            else
            {
                EncogUtility.trainConsole(network, trainingSet, Constant.TRAINING_MINUTES);
            }

            app.WriteLine("Training complete, saving network...");
            encog.Add(Constant.TRAINED_NETWORK_NAME, network);
        }

    }
}
