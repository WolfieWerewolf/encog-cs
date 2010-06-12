// Encog Neural Network and Bot Library for DotNet v0.5
// http://www.heatonresearch.com/encog/
// http://code.google.com/p/encog-cs/
// 
// Copyright 2008, Heaton Research Inc., and individual contributors.
// See the copyright.txt in the distribution for a full listing of 
// individual contributors.
//
// This is free software; you can redistribute it and/or modify it
// under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation; either version 2.1 of
// the License, or (at your option) any later version.
//
// This software is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this software; if not, write to the Free
// Software Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301 USA, or see the FSF site: http://www.fsf.org.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Encog.Neural.Genetic;

namespace GeneticTSP
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
            this.GA = owner;

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
            CalculateCost();

        }

        /// <summary>
        /// Calculate the cost of this chromosome.  This is the distance covered
        /// by this path through the cities.
        /// </summary>
        public override void CalculateCost()
        {
            double cost = 0.0;
            for (int i = 0; i < this.cities.Length - 1; i++)
            {
                double dist = this.cities[this.GetGene(i)]
                       .Proximity(this.cities[GetGene(i + 1)]);
                cost += dist;
            }
            this.Cost = cost;
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
            int temp = GetGene(iswap1);
            SetGene(iswap1, GetGene(iswap2));
            SetGene(iswap2, temp);
        }

    }
}
