using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.NeuralData;
using Encog.Neural.Data;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Activation;
using Encog.Neural.Networks;
using Encog.Util.Randomize;
using Encog.Neural.Networks.Training.Simple;
using Encog.Neural.Networks.Training;

namespace Adaline
{
    public class AdalineDigits
    {

        public const int CHAR_WIDTH = 5;
        public const int CHAR_HEIGHT = 7;

        public static String[][] DIGITS = { 
      new String[CHAR_HEIGHT] { 
        " OOO ",
        "O   O",
        "O   O",
        "O   O",
        "O   O",
        "O   O",
        " OOO "  },

      new String[CHAR_HEIGHT] {           
        "  O  ",
        " OO  ",
        "O O  ",
        "  O  ",
        "  O  ",
        "  O  ",
        "  O  "  },

      new String[CHAR_HEIGHT] { 
        " OOO ",
        "O   O",
        "    O",
        "   O ",
        "  O  ",
        " O   ",
        "OOOOO"  },

      new String[CHAR_HEIGHT] { 
        " OOO ",
        "O   O",
        "    O",
        " OOO ",
        "    O",
        "O   O",
        " OOO "  },

      new String[CHAR_HEIGHT] { 
        "   O ",
        "  OO ",
        " O O ",
        "O  O ",
        "OOOOO",
        "   O ",
        "   O "  },

      new String[CHAR_HEIGHT] { 
        "OOOOO",
        "O    ",
        "O    ",
        "OOOO ",
        "    O",
        "O   O",
        " OOO "  },

      new String[CHAR_HEIGHT] { 
        " OOO ",
        "O   O",
        "O    ",
        "OOOO ",
        "O   O",
        "O   O",
        " OOO "  },

      new String[CHAR_HEIGHT] {
        "OOOOO",
        "    O",
        "    O",
        "   O ",
        "  O  ",
        " O   ",
        "O    "  },

      new String[CHAR_HEIGHT] { 
        " OOO ",
        "O   O",
        "O   O",
        " OOO ",
        "O   O",
        "O   O",
        " OOO "  },

      new String[CHAR_HEIGHT] { 
        " OOO ",
        "O   O",
        "O   O",
        " OOOO",
        "    O",
        "O   O",
        " OOO "  } };

        public static INeuralDataSet GenerateTraining()
        {
            INeuralDataSet result = new BasicNeuralDataSet();
            for (int i = 0; i < DIGITS.Length; i++)
            {
                BasicNeuralData ideal = new BasicNeuralData(DIGITS.Length);

                // setup input
                INeuralData input = Image2data(DIGITS[i]);

                // setup ideal
                for (int j = 0; j < DIGITS.Length; j++)
                {
                    if (j == i)
                        ideal[j] = 1;
                    else
                        ideal[j] = -1;
                }

                // add training element
                result.Add(input, ideal);
            }
            return result;
        }

        public static INeuralData Image2data(String[] image)
        {
            INeuralData result = new BasicNeuralData(CHAR_WIDTH * CHAR_HEIGHT);

            for (int row = 0; row < CHAR_HEIGHT; row++)
            {
                for (int col = 0; col < CHAR_WIDTH; col++)
                {
                    int index = (row * CHAR_WIDTH) + col;
                    char ch = image[row][col];
                    result[index] = (ch == 'O' ? 1 : -1);
                }
            }

            return result;
        }

        static void Main(string[] args)
        {
            int inputNeurons = CHAR_WIDTH * CHAR_HEIGHT;
            int outputNeurons = DIGITS.Length;

            BasicNetwork network = new BasicNetwork();

            ILayer inputLayer = new BasicLayer(new ActivationLinear(), false, inputNeurons);
            ILayer outputLayer = new BasicLayer(new ActivationLinear(), true, outputNeurons);

            network.AddLayer(inputLayer);
            network.AddLayer(outputLayer);
            network.Structure.FinalizeStructure();

            (new RangeRandomizer(-0.5, 0.5)).Randomize(network);

            // train it
            INeuralDataSet training = GenerateTraining();
            ITrain train = new TrainAdaline(network, training, 0.01);

            int epoch = 1;
            do
            {
                train.Iteration();
                Console.WriteLine("Epoch #" + epoch + " Error:" + train.Error);
                epoch++;
            } while (train.Error > 0.01);

            //
            Console.WriteLine("Error:" + network.CalculateError(training));

            // test it
            for (int i = 0; i < DIGITS.Length; i++)
            {
                int output = network.Winner(Image2data(DIGITS[i]));

                for (int j = 0; j < CHAR_HEIGHT; j++)
                {
                    if (j == CHAR_HEIGHT - 1)
                        Console.WriteLine(DIGITS[i][j] + " -> " + output);
                    else
                        Console.WriteLine(DIGITS[i][j]);

                }

                Console.WriteLine();
            }
        }
    }

}
