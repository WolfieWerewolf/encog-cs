using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Util.CSV;
using System.IO;

namespace Encog.App.Quant
{
    public class EncogQuant
    {
        public CSVFormat Format { get; set; }
        public IList<Field> Data { get { return data; } }
        public int MaxHistory { get; set; }
        
        private IList<Field> data = new List<Field>();
        private History history;


        public EncogQuant()
        {
            Format = CSVFormat.ENGLISH;
            MaxHistory = 200;
            history = new History(this, MaxHistory);
        }

        public void Process(String input, String output)
        {
            ReadCSV inCSV = new ReadCSV(input, true, Format);
            TextWriter tw = new StreamWriter(output);
            double[] historyBar = new double[data.Count];

            history.Init();

            while (inCSV.Next())
            {
                StringBuilder line = new StringBuilder();

                int itemIndex;

                // read from file
                itemIndex = 0;
                foreach (Field item in data)
                {
                    if (item is FileData)
                    {
                        historyBar[itemIndex] = inCSV.GetDouble(item.Index);
                    }
                    itemIndex++;
                }

                // calculate indicators
                itemIndex = 0;
                foreach (Field item in data)
                {
                    if (item is MovingAverage)
                    {
                        MovingAverage ind = (MovingAverage)item;
                        historyBar[itemIndex] = ind.Calculate(history);
                    }
                    itemIndex++;
                }

                history.Add(historyBar);

               
                // output
                itemIndex = 0;
                foreach (Field item in Data)
                {
                    if (item.Output)
                    {
                        double d = historyBar[itemIndex];
                        if (line.Length > 0)
                            line.Append(',');
                        line.Append(d);
                    }
                    itemIndex++;
                }
                
                tw.WriteLine(line.ToString());
            }

            tw.Close();
        }
    }
}
