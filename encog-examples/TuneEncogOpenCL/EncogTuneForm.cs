using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Encog.Util.CL;
using Encog;
using Encog.Util;
using System.Threading;
using Encog.Neural.NeuralData;
using Encog.Util.Banchmark;
using Encog.Neural.Networks;
using Encog.Util.Simple;
using System.Diagnostics;
using Encog.Neural.Networks.Training.Propagation.Resilient;

namespace TuneEncogOpenCL
{
    public partial class EncogTuneForm : Form
    {
        public EncogTuneForm()
        {
            InitializeComponent();
        }

        private void EncogTuneForm_Load(object sender, EventArgs e)
        {
            try
            {
                listGPU.CheckBoxes = true;

                //Encog.Encog.Instance.InitCL();

                //BenchmarkProc();

                //this.textCLThreadCount.Text = ""+Encog.Encog.Instance.CL.CLThreads;
                //this.textCLRatio.Text = "" + Encog.Encog.Instance.CL.EnforcedCLRatio;
                //this.textWorkgroupSize.Text = "" + Encog.Encog.Instance.CL.CLWorkloadSize;
                /*
                foreach (EncogCLDevice device in Encog.Encog.Instance.CL.Devices)
                {
                    ListViewItem item = new ListViewItem(new String[] { (device.IsCPU?"CPU":"GPU"), 
                        device.Vender, device.Name, ""+device.MaxComputeUnits, ""+device.MaxClockFrequency,
                        Format.FormatMemory(device.LocalMemorySize), 
                        Format.FormatMemory(device.GlobalMemorySize) });
                    listGPU.Items.Add(item);
                }*/


            }
            catch (EncogError ex)
            {
                MessageBox.Show(ex.ToString(), "Can't Access OpenCL");
            }
            
        }

        private void ValidateInt(object sender, CancelEventArgs e)
        {
            int i;

            if (sender is TextBox)
            {
                TextBox text = (TextBox)sender;
                if (!int.TryParse(text.Text, out i))
                {
                    e.Cancel = true;
                    MessageBox.Show("Must be integer");
                }
            }
        }

        private void ValidateIntNonZero(object sender, CancelEventArgs e)
        {
            int i;

            if (sender is TextBox)
            {
                TextBox text = (TextBox)sender;
                if (!int.TryParse(text.Text, out i))
                {
                    e.Cancel = true;
                    MessageBox.Show("Must be integer");
                }
                if( i==0 )
                {
                    e.Cancel = true;
                    MessageBox.Show("Can't be zero");
                }
            }
        }

        private void btnAutoTune_Click(object sender, EventArgs e)
        {
            btnAutoTune.Enabled = false;
            btnBenchmark.Enabled = false;
            statusBar.Text = "Running autotune...";
            

        }

        private void btnBenchmark_Click(object sender, EventArgs e)
        {
            btnAutoTune.Enabled = false;
            btnBenchmark.Enabled = false;

            Thread thread = new Thread(new ThreadStart(this.BenchmarkProc));
            thread.Start();
        }

        public void BenchmarkProc()
        {
            int outputSize = 1;
            int inputSize = 10;
            int trainingSize = 10000;

            INeuralDataSet training = RandomTrainingFactory.Generate(
                trainingSize, inputSize, outputSize, -1, 1);
            BasicNetwork network = EncogUtility.SimpleFeedForward(
                training.InputSize, 6, 0, training.IdealSize, true);
            network.Reset();

            Encog.Encog.Instance.InitCL();

            ResilientPropagation train = new ResilientPropagation(network, training);
            //Propagation train = new Backpropagation(network, training, 0.000007, 0.0);
            train.NumThreads = 0;
            train.Iteration();

            long start = Environment.TickCount;
            for (int i = 0; i < 100; i++)
            {
                train.Iteration();
                Console.WriteLine("Train error: " + train.Error);
            }
            long stop = Environment.TickCount;

            Console.WriteLine("GPU Time:" + train.FlatTraining.CLTimePerIteration);
            Console.WriteLine("CPU Time:" + train.FlatTraining.CPUTimePerIteration);
            Console.WriteLine("Ratio:" + train.FlatTraining.CalculatedCLRatio);
            Console.WriteLine("Done:" + (stop - start));
            Console.WriteLine("Stop");
            if (Encog.Encog.Instance.CL != null)
                Console.WriteLine(Encog.Encog.Instance.CL.ToString());


            /*
            //Encog.Encog.Instance.InitCL();
            statusBar.Invoke((MethodInvoker)delegate { statusBarText.Text = "Running benchmark..."; });

            int trainingSize = int.Parse(textTrainingSize.Text);
            int inputSize = int.Parse(textInputNeurons.Text);
            int outputSize = int.Parse(textOutputNeurons.Text);

            INeuralDataSet training = RandomTrainingFactory.Generate(trainingSize, inputSize, outputSize, -1, 1);
            BasicNetwork network = EncogUtility.SimpleFeedForward(
                training.InputSize, 6, 0, training.IdealSize, true);
            network.Reset();
            ResilientPropagation train = new ResilientPropagation(network, training);
            train.Iteration();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < 100; i++)
            {
                train.Iteration();
                statusBar.Invoke((MethodInvoker)delegate { statusBarText.Text = "Running benchmark...Iteration " + i + " of 100."; });
            }
            stopwatch.Stop();*/
        }
    }
}
