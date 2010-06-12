using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.NeuralData;
using Encog.Neural.Data.Basic;

namespace ElmanNetwork
{
    public class TemporalXOR
    {

        /*
         * 1 xor 0 = 1
         * 0 xor 0 = 0
         * 0 xor 1 = 1
         * 1 xor 1 = 0
         */
        public double[] SEQUENCE = { 1.0,0.0,1.0,
		0.0,0.0,0.0,
		0.0,1.0,1.0,
		1.0,1.0,0.0 };

        private double[][] input;
        private double[][] ideal;

        public INeuralDataSet Generate(int count)
        {
            this.input = new double[count][];
            this.ideal = new double[count][];

            for (int i = 0; i < this.input.Length; i++)
            {
                this.input[i] = new double[1];
                this.ideal[i] = new double[1];
                this.input[i][0] = SEQUENCE[i % SEQUENCE.Length];
                this.ideal[i][0] = SEQUENCE[(i + 1) % SEQUENCE.Length];
            }

            return new BasicNeuralDataSet(this.input, this.ideal);
        }
    }
}
