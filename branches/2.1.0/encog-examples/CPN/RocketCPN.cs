using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using Encog.Neural.NeuralData;
using Encog.Neural.Data;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Activation;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks.Training.CPN;
using Encog.Neural.Data.Basic;
using Encog.Neural.Networks.Pattern;

namespace CPN
{
    public class RocketCPN
    {

        public const int WIDTH = 11;
        public const int HEIGHT = 11;

        public String[][] PATTERN1 =  { new String[WIDTH]  { 
		"           ",
        "           ",
        "     O     ",
        "     O     ",
        "    OOO    ",
        "    OOO    ",
        "    OOO    ",
        "   OOOOO   ",
        "   OOOOO   ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "        O  ",
        "       O   ",
        "     OOO   ",
        "    OOO    ",
        "   OOO     ",
        " OOOOO     ",
        "OOOOO      ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "           ",
        "  OO       ",
        "  OOOOO    ",
        "  OOOOOOO  ",
        "  OOOOO    ",
        "  OO       ",
        "           ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "OOOOO      ",
        " OOOOO     ",
        "   OOO     ",
        "    OOO    ",
        "     OOO   ",
        "       O   ",
        "        O  ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "   OOOOO   ",
        "   OOOOO   ",
        "    OOO    ",
        "    OOO    ",
        "    OOO    ",
        "     O     ",
        "     O     ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "      OOOOO",
        "     OOOOO ",
        "     OOO   ",
        "    OOO    ",
        "   OOO     ",
        "   O       ",
        "  O        ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "           ",
        "       OO  ",
        "    OOOOO  ",
        "  OOOOOOO  ",
        "    OOOOO  ",
        "       OO  ",
        "           ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "  O        ",
        "   O       ",
        "   OOO     ",
        "    OOO    ",
        "     OOO   ",
        "     OOOOO ",
        "      OOOOO",
        "           ",
        "           "  } };

        String[][] PATTERN2 = { 
                          new String[WIDTH]  { 
		"           ",
        "           ",
        "     O     ",
        "     O     ",
        "     O     ",
        "    OOO    ",
        "    OOO    ",
        "    OOO    ",
        "   OOOOO   ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "     O     ",
        "     O     ",
        "    O O    ",
        "    O O    ",
        "    O O    ",
        "   O   O   ",
        "   O   O   ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "           ",
        "     O     ",
        "    OOO    ",
        "    OOO    ",
        "    OOO    ",
        "   OOOOO   ",
        "           ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "           ",
        "           ",
        "     O     ",
        "     O     ",
        "     O     ",
        "    OOO    ",
        "           ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "  O        ",
        "     O     ",
        "     O     ",
        "    OOO    ",
        "    OO     ",
        "    OOO   O",
        "    OOOO   ",
        "   OOOOO   ",
        "           ",
        "       O   "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "     O     ",
        "     O     ",
        "    OOO    ",
        "    OOO    ",
        "    OOO    ",
        "   OOOOO   ",
        "   OOOOO   ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "       O   ",
        "      O    ",
        "    OOO    ",
        "    OOO    ",
        "   OOO     ",
        "  OOOOO    ",
        " OOOOO     ",
        "           ",
        "           "  },

      new String[WIDTH]  { 
        "           ",
        "           ",
        "        O  ",
        "       O   ",
        "     OOO   ",
        "    OOO    ",
        "   OOO     ",
        " OOOOO     ",
        "OOOOO      ",
        "           ",
        "           "  } };

        public const double HI = 1;
        public const double LO = 0;

        private double[][] input1;
        private double[][] input2;
        private double[][] ideal1;

        private int inputNeurons;
        private int instarNeurons;
        private int outstarNeurons;

        public void PrepareInput()
        {
            int n, i, j;

            this.inputNeurons = WIDTH * HEIGHT;
            this.instarNeurons = PATTERN1.Length;
            this.outstarNeurons = 2;

            this.input1 = new double[PATTERN1.Length][];
            this.input2 = new double[PATTERN2.Length][];
            this.ideal1 = new double[PATTERN1.Length][];

            for (n = 0; n < PATTERN1.Length; n++)
            {
                input1[n] = new double[this.inputNeurons];
                input2[n] = new double[this.inputNeurons];
                ideal1[n] = new double[this.instarNeurons];
                for (i = 0; i < HEIGHT; i++)
                {
                    for (j = 0; j < WIDTH; j++)
                    {
                        input1[n][i * WIDTH + j] = (PATTERN1[n][i][j] == 'O') ? HI : LO;
                        input2[n][i * WIDTH + j] = (PATTERN2[n][i][j] == 'O') ? HI : LO;
                    }
                }
            }
            NormalizeInput();
            for (n = 0; n < PATTERN1.Length; n++)
            {
                this.ideal1[n][0] = Math.Sin(n * 0.25 * Math.PI);
                this.ideal1[n][1] = Math.Cos(n * 0.25 * Math.PI);
            }

        }

        public double Sqr(double x)
        {
            return x * x;
        }


        void NormalizeInput()
        {
            int n, i;
            double length1, length2;

            for (n = 0; n < PATTERN1.Length; n++)
            {
                length1 = 0;
                length2 = 0;
                for (i = 0; i < this.inputNeurons; i++)
                {
                    length1 += Sqr(this.input1[n][i]);
                    length2 += Sqr(this.input2[n][i]);
                }
                length1 = Math.Sqrt(length1);
                length2 = Math.Sqrt(length2);

                for (i = 0; i < this.inputNeurons; i++)
                {
                    input1[n][i] /= length1;
                    input2[n][i] /= length2;
                }
            }
        }

        public BasicNetwork CreateNetwork()
        {
            CPNPattern pattern = new CPNPattern();
            pattern.InputNeurons = this.inputNeurons;
            pattern.InstarCount = this.instarNeurons;
            pattern.OutstarCount = this.outstarNeurons;

            BasicNetwork network = pattern.Generate();
            network.Reset();

            return network;
        }

        public void TrainInstar(BasicNetwork network, INeuralDataSet training)
        {
            int epoch = 1;

            ITrain train = new TrainInstar(network, training, 0.1);
            for (int i = 0; i < 1000; i++)
            {
                train.Iteration();
                Console.WriteLine("Training instar, Epoch #" + epoch);
                epoch++;
            }
        }

        public void TrainOutstar(BasicNetwork network, INeuralDataSet training)
        {
            int epoch = 1;

            ITrain train = new TrainOutstar(network, training, 0.1);
            for (int i = 0; i < 1000; i++)
            {
                train.Iteration();
                Console.WriteLine("Training outstar, Epoch #" + epoch);
                epoch++;
            }
        }

        public INeuralDataSet GenerateTraining(double[][] input, double[][] ideal)
        {
            INeuralDataSet result = new BasicNeuralDataSet(input, ideal);
            return result;
        }

        public double DetermineAngle(INeuralData angle)
        {
            double result;

            result = (Math.Atan2(angle[0], angle[1]) / Math.PI) * 180;
            if (result < 0)
                result += 360;

            return result;
        }

        public void Test(BasicNetwork network, String[][] pattern, double[][] input)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                INeuralData inputData = new BasicNeuralData(input[i]);
                INeuralData outputData = network.Compute(inputData);
                double angle = DetermineAngle(outputData);

                // display image
                for (int j = 0; j < HEIGHT; j++)
                {
                    if (j == HEIGHT - 1)
                        Console.WriteLine("[" + pattern[i][j] + "] -> " + ((int)angle) + " deg");
                    else
                        Console.WriteLine("[" + pattern[i][j] + "]");
                }

                Console.WriteLine();
            }
        }

        public void Run()
        {
            PrepareInput();
            NormalizeInput();
            BasicNetwork network = CreateNetwork();
            INeuralDataSet training = GenerateTraining(this.input1, this.ideal1);
            TrainInstar(network, training);
            TrainOutstar(network, training);
            Test(network, PATTERN1, this.input1);
        }



        static void Main(string[] args)
        {
            RocketCPN cpn = new RocketCPN();
            cpn.Run();
        }

    }
}
