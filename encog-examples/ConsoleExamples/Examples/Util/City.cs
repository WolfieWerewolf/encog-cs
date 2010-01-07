using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encog.Examples.Util
{
    /// <summary>
    /// Holds the x and y location for a city in the traveling salesman problem.
    /// </summary>
    public class City
    {
        /// <summary>
        /// The city's x position.
        /// </summary>
        int xpos;

        /// <summary>
        /// The city's y position.
        /// </summary>
        int ypos;

        /// <summary>
        /// The city's x position.
        /// </summary>
        int X
        {
            get
            {
                return this.xpos;
            }
        }

        /// <summary>
        /// The city's y position.
        /// </summary>
        int Y
        {
            get
            {
                return this.ypos;
            }
        }

        /// <summary>
        /// Construct a city.
        /// </summary>
        /// <param name="x">The city's x location.</param>
        /// <param name="y">The city's y location.</param>
        public City(int x, int y)
        {
            this.xpos = x;
            this.ypos = y;
        }

        /// <summary>
        /// Returns how close the city is to another city.
        /// </summary>
        /// <param name="cother">The other city.</param>
        /// <returns>A distance.</returns>
        public int Proximity(City cother)
        {
            return Proximity(cother.X, cother.Y);
        }

        /// <summary>
        /// Returns how far this city is from a a specific point. This method uses
        /// the pythagorean theorum to calculate the distance.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <returns>The distance.</returns>
        int Proximity(int x, int y)
        {
            int xdiff = this.xpos - x;
            int ydiff = this.ypos - y;
            return (int)Math.Sqrt(xdiff * xdiff + ydiff * ydiff);
        }
    }
}
