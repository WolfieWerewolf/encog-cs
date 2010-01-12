using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Encog.Neural.Networks.Synapse;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Training.Competitive;
using Encog.Neural.Networks.Training.Competitive.Neighborhood;
using Encog.Neural.Networks.Pattern;
using Encog.Neural.Data;
using Encog.Neural.Data.Basic;
using Encog.Util.Randomize;
using Encog.Util.MathUtil;

namespace SOMColors
{
    public partial class SOMColors : Form
    {
        public const int CELL_SIZE = 8;
	    public const int WIDTH = 50;
	    public const int HEIGHT = 50;

        private ISynapse synapse;

        private BasicNetwork network;
        private CompetitiveTraining train;
        private NeighborhoodGaussianMulti gaussian;
        private IList<INeuralData> samples;
        private int iteration;

        public SOMColors()
        {
            InitializeComponent();

            this.network = CreateNetwork();
            this.synapse = network.GetLayer(BasicNetwork.TAG_INPUT).Next[0];
            this.gaussian = new NeighborhoodGaussianMulti(SOMColors.WIDTH, SOMColors.HEIGHT);
            this.train = new CompetitiveTraining(this.network, 0.01, null, gaussian);
            train.ForceWinner = false;

            samples = new List<INeuralData>();
            for (int i = 0; i < 15; i++)
            {
                INeuralData data = new BasicNeuralData(3);
                data.Data[0] = RangeRandomizer.Randomize(-1, 1);
                data.Data[1] = RangeRandomizer.Randomize(-1, 1);
                data.Data[2] = RangeRandomizer.Randomize(-1, 1);
                samples.Add(data);
            }

            this.train.SetAutoDecay(100, 0.8, 0.003, 30, 5);
        }

        private BasicNetwork CreateNetwork()
        {
            BasicNetwork result = new BasicNetwork();
            SOMPattern pattern = new SOMPattern();
            pattern.InputNeurons = 3;
            pattern.OutputNeurons = SOMColors.WIDTH * SOMColors.HEIGHT;
            result = pattern.Generate();
            result.Reset();
            return result;
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            this.iteration++;
            if (this.iteration > 100)
                this.updateTimer.Enabled = false;


            int idx = (int)(ThreadSafeRandom.NextDouble()*samples.Count);
			INeuralData c = samples[idx];
			
			this.train.TrainPattern(c);
			this.train.AutoDecay();
            DrawMap();
			//System.out.println("Iteration " + i + ","+ this.train.toString());
        }

        private int ConvertColor(double d)
        {
            double result = 128 * d;
            result += 128;
            result = Math.Min(result, 255);
            result = Math.Max(result, 0);
            return (int)result;
        }

        private void DrawMap()
        {
            Graphics g = this.CreateGraphics();
            for(int y = 0; y< HEIGHT; y++)
		    {
			    for(int x = 0; x< WIDTH; x++)
			    {
				    int index = (y*WIDTH)+x;
				    int red = ConvertColor(this.synapse.WeightMatrix[0, index]);
                    int green = ConvertColor(this.synapse.WeightMatrix[1, index]);
                    int blue = ConvertColor(this.synapse.WeightMatrix[2, index]);
                    Color c = Color.FromArgb(red, green, blue);
                    Brush brush = new SolidBrush(c);
                    g.FillRectangle(brush, x * CELL_SIZE, y * CELL_SIZE, CELL_SIZE, CELL_SIZE);
    			}
    		}	
            g.Dispose();
        }
    }
}
