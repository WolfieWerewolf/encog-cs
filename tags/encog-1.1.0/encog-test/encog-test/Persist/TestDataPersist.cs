using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Encog.Neural.Data.Basic;
using encog_test.Neural.Networks;
using Encog.Neural.Persist;
using System.IO;

namespace encog_test.Persist
{


    [TestFixture]
    public class TestDataPersist
    {
        public const string FILENAME = "encogdata.eg";

        [Test]
        public void TestCSVData()
        {

            BasicNeuralDataSet trainingData = new BasicNeuralDataSet(XOR.XOR_INPUT, XOR.XOR_IDEAL);

            EncogPersistedCollection encog = new EncogPersistedCollection();
            encog.Add(trainingData);
            encog.Save(TestDataPersist.FILENAME);

            EncogPersistedCollection encog2 = new EncogPersistedCollection();
            encog2.Load(TestDataPersist.FILENAME);

            BasicNeuralDataSet set = (BasicNeuralDataSet)encog2.List[0];

            XOR.TestXORDataSet(set);
            File.Delete(TestDataPersist.FILENAME);
        }
    }
}
