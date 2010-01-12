using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using Encog.Normalize;
using Encog.Normalize.Input;
using Encog.Normalize.Output;
using Encog.Neural.Data;

namespace Encog.Examples.Lunar
{
    public class NeuralPilot
    {
        private BasicNetwork network;
        private DataNormalization norm;
        private bool track;
        private IExampleInterface app;

        public NeuralPilot(IExampleInterface app, BasicNetwork network, bool track)
        {
            IInputField fuelIN;
            IInputField altitudeIN;
            IInputField velocityIN;

            this.track = track;
            this.network = network;
            this.app = app;

            norm = new DataNormalization();
            norm.AddInputField(fuelIN = new BasicInputField());
            norm.AddInputField(altitudeIN = new BasicInputField());
            norm.AddInputField(velocityIN = new BasicInputField());
            norm.AddOutputField(new OutputFieldRangeMapped(fuelIN, -0.9, 0.9));
            norm.AddOutputField(new OutputFieldRangeMapped(altitudeIN, -0.9, 0.9));
            norm.AddOutputField(new OutputFieldRangeMapped(velocityIN, -0.9, 0.9));
            fuelIN.Max = 200;
            fuelIN.Min = 0;
            altitudeIN.Max = 10000;
            altitudeIN.Min = 0;
            velocityIN.Min = -LanderSimulator.TERMINAL_VELOCITY;
            velocityIN.Max = LanderSimulator.TERMINAL_VELOCITY;

        }

        public int ScorePilot()
        {
            LanderSimulator sim = new LanderSimulator();
            while (sim.Flying)
            {
                double[] data = new double[3];
                data[0] = sim.Fuel;
                data[1] = sim.Altitude;
                data[2] = sim.Velocity;

                INeuralData input = this.norm.BuildForNetworkInput(data);
                INeuralData output = this.network.Compute(input);
                double value = output.Data[0];

                bool thrust;

                if (value > 0)
                {
                    thrust = true;
                    if (track)
                        app.WriteLine("THRUST");
                }
                else
                    thrust = false;

                sim.Turn(thrust);
                if (track)
                    app.WriteLine(sim.Telemetry());
            }
            return (sim.Score());
        }
    }
}
