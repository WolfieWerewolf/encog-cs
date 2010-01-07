using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Solve.Genetic;
using Encog.Examples.Util;

namespace Encog.Examples.GeneticTSP
{
    /// <summary>
    /// A chromosome for the traveling salesman problem.  A chromosome is one path
    /// through the cities.
    /// </summary>
    class TSPChromosome : Chromosome<int>
    {
        /// <summary>
        /// The cities to use.
        /// </summary>
        protected City[] cities;

        /// <summary>
        /// Construct a chromosome.
        /// </summary>
        /// <param name="owner">The genetic algorithm.</param>
        /// <param name="cities">The cities to use.</param>
        public TSPChromosome(TSPGeneticAlgorithm owner, City[] cities)
        {
            Random rand = new Random();
            this.GeneticAlgorithm = owner;

            this.cities = cities;

            int[] genes = new int[this.cities.Length];
            bool[] taken = new bool[cities.Length];

            for (int i = 0; i < genes.Length; i++)
            {
                taken[i] = false;
            }
            for (int i = 0; i < genes.Length - 1; i++)
            {
                int icandidate;
                do
                {
                    icandidate = (int)(rand.NextDouble() * genes.Length);
                } while (taken[icandidate]);
                genes[i] = icandidate;
                taken[icandidate] = true;
                if (i == genes.Length - 2)
                {
                    icandidate = 0;
                    while (taken[icandidate])
                    {
                        icandidate++;
                    }
                    genes[i + 1] = icandidate;
                }
            }
            this.Genes = genes;
            CalculateScore();

        }

        /// <summary>
        /// Calculate the cost of this chromosome.  This is the distance covered
        /// by this path through the cities.
        /// </summary>
        public override void CalculateScore()
        {
            double cost = 0.0;
            for (int i = 0; i < this.cities.Length - 1; i++)
            {
                double dist = this.cities[this.Genes[i]]
                       .Proximity(this.cities[this.Genes[i + 1]]);
                cost += dist;
            }
            this.Score = cost;
        }

        /// <summary>
        /// Mutate the current chromosome.  Swap two random cities.
        /// </summary>
        public override void Mutate()
        {
            Random rand = new Random();
            int length = this.Genes.Length;
            int iswap1 = (int)(rand.NextDouble() * length);
            int iswap2 = (int)(rand.NextDouble() * length);
            int temp = this.Genes[iswap1];
            this.Genes[iswap1] = this.Genes[iswap2];
            this.Genes[iswap2] = temp;
        }

    }
}
