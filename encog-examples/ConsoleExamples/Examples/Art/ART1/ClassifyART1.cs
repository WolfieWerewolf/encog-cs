using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleExamples.Examples;
using Encog.Neural.Networks.Pattern;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Logic;
using Encog.Neural.NeuralData.Bipolar;

namespace Encog.Examples.Art.ART1
{
    public class ClassifyART1: IExample
    {
        public static ExampleInfo Info
        {
            get
            {
                ExampleInfo info = new ExampleInfo(
                    typeof(ClassifyART1),
                    "art1-classify",
                    "Classify Patterns with ART1",
                    "Uses an ART1 neural network to classify input patterns into groups.  The ART1 network learns these groups as it is presented with items to classify.");
                return info;
            }
        }

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

        private IExampleInterface app;
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


        public void Execute(IExampleInterface app)
        {
            this.app = app;
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
                    app.WriteLine(PATTERN[i] + " - " + logic.Winner);
                }
                else
                {
                    app.WriteLine(PATTERN[i] + " - new Input and all Classes exhausted");
                }
            }
        }
    }
}
