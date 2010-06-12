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

namespace GeneticTSP
{
    class SolveTSP
    {
        public const int CITIES = 50;
        public const int POPULATION_SIZE = 1000;
        public const double MUTATION_PERCENT = 0.1;
        public const double PERCENT_TO_MATE = 0.24;
        public const double MATING_POPULATION_PERCENT = 0.5;
        public const int CUT_LENGTH = CITIES / 5;
        public const int MAP_SIZE = 256;
        public const int MAX_SAME_SOLUTION = 25;

        /// <summary>
        /// The genetic algorithm to use.
        /// </summary>
        private TSPGeneticAlgorithm genetic;

        /// <summary>
        /// The cities to use.
        /// </summary>
        private City[] cities;

        /// <summary>
        /// Place the cities in random locations.
        /// </summary>
        private void InitCities()
        {
            Random rand = new Random();

            cities = new City[CITIES];
            for (int i = 0; i < cities.Length; i++)
            {
                int xPos = (int)(rand.NextDouble() * MAP_SIZE);
                int yPos = (int)(rand.NextDouble() * MAP_SIZE);

                cities[i] = new City(xPos, yPos);
            }
        }

        /// <summary>
        /// Create an initial path of cities.
        /// </summary>
        private void InitPath()
        {
            Random rand = new Random();
            bool[] taken = new bool[this.cities.Length];
            int[] path = new int[this.cities.Length];

            for (int i = 0; i < path.Length; i++)
            {
                taken[i] = false;
            }
            for (int i = 0; i < path.Length - 1; i++)
            {
                int icandidate;
                do
                {
                    icandidate = (int)(rand.NextDouble() * path.Length);
                } while (taken[icandidate]);
                path[i] = icandidate;
                taken[icandidate] = true;
                if (i == path.Length - 2)
                {
                    icandidate = 0;
                    while (taken[icandidate])
                    {
                        icandidate++;
                    }
                    path[i + 1] = icandidate;
                }
            }
        }

        /// <summary>
        /// Display the cities in the final path.
        /// </summary>
        public void DisplaySolution()
        {
            int[] path = genetic.Chromosomes[0].Genes;
            for (int i = 0; i < path.Length; i++)
            {
                if (i != 0)
                {
                    Console.Write(">");
                }
                Console.Write("" + path[i]);
            }
            Console.WriteLine("");
        }

        /// <summary>
        /// Setup and solve the TSP.
        /// </summary>
        public void Solve()
        {
            StringBuilder builder = new StringBuilder();

            InitCities();

            genetic = new TSPGeneticAlgorithm(
                    cities,
                    POPULATION_SIZE,
                    MUTATION_PERCENT,
                    PERCENT_TO_MATE,
                    MATING_POPULATION_PERCENT,
                    CUT_LENGTH);



            InitPath();

            int sameSolutionCount = 0;
            int iteration = 1;
            double lastSolution = Double.MaxValue;

            while (sameSolutionCount < MAX_SAME_SOLUTION)
            {
                genetic.Iteration();

                double thisSolution = genetic.Chromosomes[0].Cost;

                builder.Length = 0;
                builder.Append("Iteration: ");
                builder.Append(iteration++);
                builder.Append(", Best Path Length = ");
                builder.Append(thisSolution);

                Console.WriteLine(builder.ToString());

                if (Math.Abs(lastSolution - thisSolution) < 1.0)
                {
                    sameSolutionCount++;
                }
                else
                {
                    sameSolutionCount = 0;
                }

                lastSolution = thisSolution;
            }

            Console.WriteLine("Good solution found:");
            DisplaySolution();

        }

        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Not used.</param>
        static void Main(string[] args)
        {
            SolveTSP solve = new SolveTSP();
            solve.Solve();
        }

    }
}
