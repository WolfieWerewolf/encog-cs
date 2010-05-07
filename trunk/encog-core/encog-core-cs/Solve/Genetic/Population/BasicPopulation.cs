﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Util.Identity;
using Encog.Solve.Genetic.Genome;
using Encog.Solve.Genetic.Innovation;
using Encog.Solve.Genetic.Species;
using Encog.Persist;
using Encog.Persist.Persistors.Generic;
using Encog.Persist.Attributes;

namespace Encog.Solve.Genetic.Population
{
    [EGReferenceable]
    public class BasicPopulation : IPopulation
    {
        /// <summary>
        /// The name of this object.
        /// </summary>
        private String name;

        /// <summary>
        /// The description of this object.
        /// </summary>
        private String description;

        /// <summary>
        /// The innovations for this population.
        /// </summary>
        private IInnovationList innovations;

        /// <summary>
        /// The percent to decrease "old" genom's score by.
        /// </summary>
        private double oldAgePenalty;

        /// <summary>
        /// The age at which to consider a genome "old".
        /// </summary>
        private int oldAgeThreshold;

        /// <summary>
        /// The max population size.
        /// </summary>
        private int populationSize;

        /// <summary>
        /// The survival rate.
        /// </summary>
        private double survivalRate;

        /// <summary>
        /// The age, below which, a genome is considered "young".
        /// </summary>
        private int youngBonusAgeThreshold;

        /// <summary>
        /// The bonus given to "young" genomes.
        /// </summary>
        private double youngScoreBonus;

        /// <summary>
        /// A list of innovations in this population.
        /// </summary>
        public IInnovationList Innovations 
        {
            get
            {
                return this.innovations;
            }
            set
            {
                this.innovations = value;
            }   
        }

        /// <summary>
        /// The percent to decrease "old" genom's score by.
        /// </summary>
        public double OldAgePenalty 
        {
            get
            {
                return this.oldAgePenalty;
            }
            set
            {
                this.oldAgePenalty = value;
            }  
        }

        /// <summary>
        /// The age at which to consider a genome "old".
        /// </summary>
        public int OldAgeThreshold 
        {
            get
            {
                return this.oldAgeThreshold;
            }
            set
            {
                this.oldAgeThreshold = value;
            } 
        }

        /// <summary>
        /// The max population size.
        /// </summary>
        public int PopulationSize 
        {
            get
            {
                return this.populationSize;
            }
            set
            {
                this.populationSize = value;
            } 
        }

        /// <summary>
        /// The survival rate.
        /// </summary>
        public double SurvivalRate 
        {
            get
            {
                return this.survivalRate;
            }
            set
            {
                this.survivalRate = value;
            } 
        }

        /// <summary>
        /// The age, below which, a genome is considered "young".
        /// </summary>
        public int YoungBonusAgeThreshold 
        {
            get
            {
                return this.youngBonusAgeThreshold;
            }
            set
            {
                this.youngBonusAgeThreshold = value;
            }
        }

        /// <summary>
        /// The bonus given to "young" genomes.
        /// </summary>
        public double YoungScoreBonus 
        {
            get
            {
                return this.youngScoreBonus;
            }
            set
            {
                this.youngScoreBonus = value;
            }
        }

        /// <summary>
        /// Generate gene id's.
        /// </summary>
        private IGenerateID geneIDGenerate = new BasicGenerateID();

        /// <summary>
        /// Generate genome id's.
        /// </summary>
        private IGenerateID genomeIDGenerate = new BasicGenerateID();

        /// <summary>
        /// The population.
        /// </summary>
        private List<IGenome> genomes = new List<IGenome>();

        /// <summary>
        /// Generate innovation id's.
        /// </summary>
        private IGenerateID innovationIDGenerate = new BasicGenerateID();

        /// <summary>
        /// The species in this population.
        /// </summary>
        private IList<ISpecies> species = new List<ISpecies>();

        /// <summary>
        /// Generate species id's.
        /// </summary>
        private IGenerateID speciesIDGenerate = new BasicGenerateID();


        /// <summary>
        /// Construct a population.
        /// </summary>
        /// <param name="populationSize">The population size.</param>
        public BasicPopulation(int populationSize)
        {
            this.PopulationSize = populationSize;
        }

        /// <summary>
        /// Add a genome to the population.
        /// </summary>
        /// <param name="genome">The genome to add.</param>
        public void Add(IGenome genome)
        {
            genomes.Add(genome);

        }

        /// <summary>
        /// Add all of the specified members to this population. 
        /// </summary>
        /// <param name="newPop">A list of new genomes to add.</param>
        public void AddAll(IList<IGenome> newPop)
        {
            genomes.Clear();
            foreach (IGenome g in newPop)
            {
                this.genomes.Add(g);
            }
        }


        /// <summary>
        /// Assign a gene id.
        /// </summary>
        /// <returns>The gene id.</returns>
        public long AssignGeneID()
        {
            return geneIDGenerate.Generate();
        }

        /// <summary>
        /// Assign a genome id.
        /// </summary>
        /// <returns>The genome id.</returns>
        public long AssignGenomeID()
        {
            return genomeIDGenerate.Generate();
        }

        /// <summary>
        /// Assign an innovation id.
        /// </summary>
        /// <returns>The innovation id.</returns>
        public long AssignInnovationID()
        {
            return innovationIDGenerate.Generate();
        }

        /// <summary>
        /// Assign a species id.
        /// </summary>
        /// <returns>The species id.</returns>
        public long AssignSpeciesID()
        {
            return speciesIDGenerate.Generate();
        }

        /// <summary>
        /// Clear all genomes from this population.
        /// </summary>
        public void Clear()
        {
            genomes.Clear();
        }

        /// <summary>
        /// The best genome in the population.
        /// </summary>
        /// <returns>The genome.</returns>
        public IGenome GetBest()
        {
            if (genomes.Count == 0)
            {
                return null;
            }
            else
            {
                return genomes[0];
            }
        }

        /// <summary>
        /// The genomes in the population.
        /// </summary>
        public IList<IGenome> Genomes
        {
            get
            {
                return genomes;
            }
        }

        /// <summary>
        /// The species in this population.
        /// </summary>
        public IList<ISpecies> Species
        {
            get
            {
                return this.species;
            }
        }

        /// <summary>
        /// Sort the population.
        /// </summary>
        public void Sort()
        {
            genomes.Sort();
        }
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public IPersistor CreatePersistor()
        {
            return new GenericPersistor(typeof(BasicPopulation));
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
