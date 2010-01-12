using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks.Training;
using Encog.Neural.Networks;

namespace Encog.Examples.Lunar
{
    public class PilotScore : ICalculateScore
    {
        private IExampleInterface app;

        public PilotScore(IExampleInterface app)
        {
            this.app = app;
        }

        public double CalculateScore(BasicNetwork network)
        {
            NeuralPilot pilot = new NeuralPilot(this.app, network, false);
            return pilot.ScorePilot();
        }


        public bool ShouldMinimize
        {
            get
            {
                return false;
            }
        }
    }
}
