using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;

namespace encog_test.Neural.Networks
{
    public class CreateNetwork
    {
        public static BasicNetwork createXORNetworkUntrained()
        {
            // random matrix data.  However, it provides a constant starting point 
            // for the unit tests.
            double[,] matrixData1 = 
		{
			{-0.8026145065833352, 0.48730020258365925, -0.29670931365567577 },
			{0.07689650585681851, -0.513969748944711, 0.11858304184009771},
			{-0.4485719795825909, 0.15435275595196507, 0.17655902338449336} };

            double[,] matrixData2 = 
		{
			{0.024694322443027827},
			{-0.0447166248226063},
			{0.9000418882323729},
			{0.38999333206070275} };

            Encog.Matrix.Matrix matrix1 = new Encog.Matrix.Matrix(matrixData1);
            Encog.Matrix.Matrix matrix2 = new Encog.Matrix.Matrix(matrixData2);

            FeedforwardLayer layer1, layer2;
            BasicNetwork network = new BasicNetwork();
            network.AddLayer(layer1 = new FeedforwardLayer(2));
            network.AddLayer(layer2 = new FeedforwardLayer(3));
            network.AddLayer(new FeedforwardLayer(1));

            layer1.WeightMatrix = matrix1;
            layer2.WeightMatrix = matrix2;

            return network;
        }
    }
}
