using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Util;

namespace Encog.Examples.Lunar
{
    public class LanderSimulator
    {
        public const double GRAVITY = 1.62;
        public const double THRUST = 10;
        public const double TERMINAL_VELOCITY = 40;

        public int Fuel { get; set; }
        public int Seconds { get; set; }
        public double Altitude { get; set; }
        public double Velocity { get; set; }

        public LanderSimulator()
        {
            this.Fuel = 200;
            this.Seconds = 0;
            this.Altitude = 10000;
            this.Velocity = 0;
        }

        public void Turn(bool thrust)
        {
            this.Seconds++;
            this.Velocity -= GRAVITY;
            this.Altitude += this.Velocity;

            if (thrust && this.Fuel > 0)
            {
                this.Fuel--;
                this.Velocity += THRUST;
            }

            this.Velocity = Math.Max(-TERMINAL_VELOCITY, this.Velocity);
            this.Velocity = Math.Min(TERMINAL_VELOCITY, this.Velocity);

            if (this.Altitude < 0)
                this.Altitude = 0;
        }

        public String Telemetry()
        {
            StringBuilder result = new StringBuilder();
            result.Append("Elapsed: ");
            result.Append(Seconds);
            result.Append(" s, Fuel: ");
            result.Append(this.Fuel);
            result.Append(" l, Velocity: ");
            result.Append(Format.FormatDouble(Velocity,4));
            result.Append(" m/s, ");
            result.Append((int)Altitude);
            result.Append(" m");
            return result.ToString();
        }

        public int Score()
        {
            return (int)((this.Fuel * 10) + this.Seconds + (this.Velocity * 1000));
        }


        public bool Flying
        {
            get
            {
                return (this.Altitude > 0);
            }
        }
    }
}
