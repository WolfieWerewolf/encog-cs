using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Encog.Util.CSV;
using System.IO;
using Encog.App.Quant.Indicators;

namespace Encog.App.Quant
{
    public class EncogQuant
    {
        public CSVFormat Format { get; set; }
        public IList<Field> Data { get { return data; } }
        public int MaxHistory { get; set; }
        
        private IList<Field> data = new List<Field>();
        private BarMapping mapping;
        private BarBuffer future;
        private ReadCSV inCSV;
        public bool Headers { get; set; }


        public EncogQuant()
        {
            Format = CSVFormat.ENGLISH;
            MaxHistory = 200;
            mapping = new BarMapping(this);
            Headers = true;
            future = new BarBuffer(200);
        }

        private void FillFuture()
        {
            while( !future.Full && inCSV.Next() )
            {
                // create future bar
                double[] futureBar = new double[data.Count];

                // read from file
                int itemIndex = 0;
                foreach (Field item in data)
                {
                    if (item is FileData)
                    {
                        futureBar[itemIndex] = inCSV.GetDouble(item.Index);
                    }
                    itemIndex++;
                }

                // add to future
                this.future.Add(futureBar);
            }
        }

        private double []ReadNextBar()
        {
            FillFuture();
            if (future.Data.Count > 0)
            {
                return future.Pop();
            }
            else
            {
                return null;
            }
        }

        public void Process(String input, String output)
        {
            TextWriter tw = null;

            try
            {
                this.inCSV = new ReadCSV(input, Headers, Format);
                tw = new StreamWriter(output);
                double[] currentBar = null;

                mapping.Init();

                while ((currentBar = ReadNextBar()) != null)
                {
                    StringBuilder line = new StringBuilder();

                    // calculate indicators
                    int itemIndex = 0;
                    foreach (Field item in data)
                    {
                        if (item is IIndicator)
                        {
                            IIndicator ind = (IIndicator)item;
                            currentBar[itemIndex] = ind.Calculate(mapping, currentBar, this.future);
                        }
                        itemIndex++;
                    }


                    // output
                    itemIndex = 0;
                    foreach (Field item in Data)
                    {
                        if (item.Output)
                        {
                            double d = currentBar[itemIndex];
                            if (line.Length > 0)
                                line.Append(',');
                            line.Append(d);
                        }
                        itemIndex++;
                    }

                    tw.WriteLine(line.ToString());
                }
            }
            catch (IOException ex)
            {
                throw new EncogError(ex);
            }
            finally
            {
                if (tw != null)
                {
                    try
                    {
                        tw.Close();
                    }
                    catch (Exception)
                    {
                    }
                }

                if (inCSV != null)
                {
                    try
                    {
                        this.inCSV.Close();
                    }
                    catch (Exception)
                    {
                    }
                    this.inCSV = null;
                }
            }            
        }
    }
}
