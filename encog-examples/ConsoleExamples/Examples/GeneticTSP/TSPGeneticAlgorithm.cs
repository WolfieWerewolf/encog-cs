using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Solve.Genetic;
using Encog.Examples.Util;

namespace Encog.Examples.GeneticTSP
{
    /// <summary>
    /// Implement a genetic algorithm for the TSP.
    /// </summary>
    class TSPGeneticAlgorithm : GeneticAlgorithm<int>
    {
        /// <summary>
        /// A genetic algorithm for the TSP.
        /// </summary>
        /// <param name="cities">The cities to use.</param>
        /// <param name="populationSize">The population size for the genetic algorithm.</param>
        /// <param name="mutationPercent">What percent to mutate per iteration.</param>
        /// <param name="percentToMate">What percent to use for mating.</param>
        /// <param name="matingPopulationPercent">The percent of the population allowed to mate.</param>
        /// <param name="cutLength">The cut length for genetic samples.</param>
        public TSPGeneticAlgorithm(City[] cities, int populationSize,
         double mutationPercent, double percentToMate,
         double matingPopulationPercent, int cutLength)
        {
            this.MutationPercent = mutationPercent;
            this.MatingPopulation = matingPopulationPercent;
            this.PopulationSize = populationSize;
            this.PercentToMate = percentToMate;
            this.CutLength = cutLength;
            this.PreventRepeat = true;

            this.Chromosomes = new TSPChromosome[this.PopulationSize];
            for (int i = 0; i < this.Chromosomes.Length; i++)
            {

                TSPChromosome c = new TSPChromosome(this, cities);
                this.Chromosomes[i] = c;
            }
            SortChromosomes();
        }
    }
}

