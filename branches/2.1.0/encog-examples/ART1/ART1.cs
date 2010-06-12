using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks.Pattern;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Logic;
using Encog.Neural.NeuralData.Bipolar;

namespace ART1
{
    public class NeuralART1
    {

        public const int INPUT_NEURONS = 5;
        public const int OUTPUT_NEURONS = 10;

        public String[] PATTERN = { 
			"   O ",
            "  O O",
            "    O",
            "  O O",
            "    O",
            "  O O",
            "    O",
            " OO O",
            " OO  ",
            " OO O",
            " OO  ",
            "OOO  ",
            "OO   ",
            "O    ",
            "OO   ",
            "OOO  ",
            "OOOO ",
            "OOOOO",
            "O    ",
            " O   ",
            "  O  ",
            "   O ",
            "    O",
            "  O O",
            " OO O",
            " OO  ",
            "OOO  ",
            "OO   ",
            "OOOO ",
            "OOOOO"  };

        private bool[][] input;

        public void SetupInput()
        {
            this.input = new bool[PATTERN.Length][];
            for (int n = 0; n < PATTERN.Length; n++)
            {
                this.input[n] = new bool[INPUT_NEURONS];
                for (int i = 0; i < INPUT_NEURONS; i++)
                {
                    this.input[n][i] = (PATTERN[n][i] == 'O');
                }
            }
        }


        public void Run()
        {
            this.SetupInput();
            ART1Pattern pattern = new ART1Pattern();
            pattern.InputNeurons = INPUT_NEURONS;
            pattern.OutputNeurons = OUTPUT_NEURONS;
            BasicNetwork network = pattern.Generate();
            ART1Logic logic = (ART1Logic)network.Logic;


            for (int i = 0; i < PATTERN.Length; i++)
            {
                BiPolarNeuralData dataIn = new BiPolarNeuralData(this.input[i]);
                BiPolarNeuralData dataOut = new BiPolarNeuralData(OUTPUT_NEURONS);
                logic.Compute(dataIn, dataOut);
                if (logic.HasWinner)
                {
                    Console.WriteLine(PATTERN[i] + " - " + logic.Winner);
                }
                else
                {
                    Console.WriteLine(PATTERN[i] + " - new Input and all Classes exhausted");
                }
            }
        }

        static void Main(string[] args)
        {
            NeuralART1 art = new NeuralART1();
            art.Run();
        }
    }

}
