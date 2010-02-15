// Encog(tm) Artificial Intelligence Framework v2.3: C# Examples
// .Net Version
// http://www.heatonresearch.com/encog/
// http://code.google.com/p/encog-java/
// 
// Copyright 2008-2010 by Heaton Research Inc.
// 
// Released under the LGPL.
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
// 
// Encog and Heaton Research are Trademarks of Heaton Research, Inc.
// For information on Heaton Research trademarks, visit:
// 
// http://www.heatonresearch.com/copyright.html

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

