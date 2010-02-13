using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleExamples.Examples;
using Encog.Examples;

namespace ConsoleExamples
{
    public class ConsoleExamples
    {
        private List<ExampleInfo> examples = new List<ExampleInfo>();

        public ConsoleExamples()
        {
            examples.Add(Encog.Examples.Adaline.AdalineDigits.Info);
            examples.Add(Encog.Examples.AnnealTSP.SolveTSP.Info);
            examples.Add(Encog.Examples.Art.ART1.ClassifyART1.Info);
            examples.Add(Encog.Examples.BAM.BidirectionalAssociativeMemory.Info);
            examples.Add(Encog.Examples.Benchmark.EncogBenchmarkExample.Info);
            examples.Add(Encog.Examples.Boltzmann.BoltzTSP.Info);
            examples.Add(Encog.Examples.CPN.RocketCPN.Info);
            examples.Add(Encog.Examples.ElmanNetwork.ElmanExample.Info);
            examples.Add(Encog.Examples.GeneticTSP.GeneticSolveTSP.Info);
            examples.Add(Encog.Examples.Hopfield.Simple.HopfieldSimple.Info);
            examples.Add(Encog.Examples.Hopfield.Associate.HopfieldAssociate.Info);
            examples.Add(Encog.Examples.JordanNetwork.JordanExample.Info);
            examples.Add(Encog.Examples.Market.Market.Info);
            examples.Add(Encog.Examples.MultiBench.MultiThreadBenchmark.Info);
            examples.Add(Encog.Examples.XOR.Anneal.XorAnneal.Info);
            examples.Add(Encog.Examples.XOR.Backprop.XorBackprop.Info);
            examples.Add(Encog.Examples.XOR.Gaussian.XorGaussian.Info);
            examples.Add(Encog.Examples.XOR.Genetic.XorGenetic.Info);
            examples.Add(Encog.Examples.XOR.Manhattan.XORManhattan.Info);
            examples.Add(Encog.Examples.XOR.Radial.XorRadial.Info);
            examples.Add(Encog.Examples.XOR.Resilient.XORResilient.Info);
            examples.Add(Encog.Examples.XOR.SCG.XORSCG.Info);
            examples.Add(Encog.Examples.XOR.Thresholdless.XorThresholdless.Info);
            examples.Add(Encog.Examples.Forest.ForestCover.Info);
            examples.Add(Encog.Examples.Lunar.LunarLander.Info);
            examples.Add(Encog.Examples.Image.ImageNeuralNetwork.Info);
            examples.Add(Encog.Examples.Persist.PersistEncog.Info);
            examples.Add(Encog.Examples.Persist.PersistSerial.Info);
            examples.Add(Encog.Examples.Sunspots.Sunspots.Info);
        }

        public void ListCommands()
        {
            Console.WriteLine("The following commands are available:");
            foreach (ExampleInfo info in examples)
            {
                Console.WriteLine(info.Command);
            }

        }

        public void Execute(String[] args)
        {
            int index = 0;
            bool pause = false;
            bool success = false;

            // process any options

            while (index < args.Length && args[index][0] == '-')
            {
                String option = args[index].Substring(1).ToLower();
                if ("pause".Equals(option))
                    pause = true;
                index++;
            }

            if (index >= args.Length )
            {
                Console.WriteLine("Must specify the example to run as the first argument");
                ListCommands();
                if (pause)
                {
                    Pause();
                }
                return;
            }

            String command = args[index++];

            // get any arguments
            String[] pargs = new String[args.Length - index];
            for (int i = 0; i < pargs.Length; i++)
            {
                pargs[i] = args[index + i];
            }

            foreach(ExampleInfo info in examples)
            {
                if (String.Compare(command, info.Command, true) == 0)
                {
                    IExample example = info.CreateInstance();
                    example.Execute(new ConsoleInterface(pargs));
                    success = true;
                    break;
                }
            }

            if (!success)
            {
                Console.WriteLine("Unknown command: " + command);
                ListCommands();
            }

            if (pause)
            {
                Pause();
            }
        }

        public void Pause()
        {
            Console.Write("\n\nPress ENTER to continue.");
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            ConsoleExamples app = new ConsoleExamples();
            app.Execute(args);
        }
    }
}
