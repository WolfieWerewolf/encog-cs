using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Neural.Networks;
using NUnit.Framework;

namespace encog_test.Neural.Networks.Layers
{
    [TestFixture]
    public class TestClone
    {
        [Test]
        public void Clone()
        {
            BasicNetwork source = XOR.CreateThreeLayerNet();
            source.Reset();

            BasicNetwork target = (BasicNetwork)source.Clone();

            Assert.IsTrue(target.Equals(source));
        }
    }
}
