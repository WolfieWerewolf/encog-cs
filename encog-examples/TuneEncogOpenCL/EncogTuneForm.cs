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
using Cloo;

namespace TuneEncogOpenCL
{
    public partial class EncogTuneForm : Form
    {
        IDictionary<ListViewItem, EncogCLDevice> deviceMap = new Dictionary<ListViewItem, EncogCLDevice>(); 

        public EncogTuneForm()
        {
            InitializeComponent();
        }

        private void EncogTuneForm_Load(object sender, EventArgs e)
        {
            try
            {
                listGPU.CheckBoxes = true;

                Encog.Encog.Instance.InitCL();

                this.textCLThreadCount.Text = ""+Encog.Encog.Instance.CL.CLThreads;
                this.textCLRatio.Text = "" + Encog.Encog.Instance.CL.EnforcedCLRatio;
                this.textWorkgroupSize.Text = "" + Encog.Encog.Instance.CL.CLWorkloadSize;

                deviceMap.Clear();
                foreach (EncogCLDevice device in Encog.Encog.Instance.CL.Devices)
                {
                    ListViewItem item = new ListViewItem(new String[] { (device.IsCPU?"CPU":"GPU"), 
                        device.Vender, device.Name, ""+device.MaxComputeUnits, ""+device.MaxClockFrequency,
                        Format.FormatMemory(device.LocalMemorySize), 
                        Format.FormatMemory(device.GlobalMemorySize) });
                    deviceMap[item] = device;
                    listGPU.Items.Add(item);
                }

                SelectDevices();
            }
            catch (EncogError ex)
            {
                MessageBox.Show(ex.ToString(), "Can't Access OpenCL");
            }
            
        }

        private void SelectDevices()
        {
            foreach (ListViewItem item in deviceMap.Keys)
            {
                EncogCLDevice device = deviceMap[item];
                item.Checked = true;
            }
        }

        bool CollectDeviceChecks()
        {
            int totalEnabled = 0;

            foreach (ListViewItem item in deviceMap.Keys)
            {
                if (item.Checked)
                    totalEnabled++;
            }

            if (totalEnabled == 0)
            {
                MessageBox.Show("You must select at least one OpenCL device.");
                return false;
            }

            foreach (ListViewItem item in deviceMap.Keys)
            {
                EncogCLDevice device = deviceMap[item];
                if (item.Checked)
                    device.Enabled = true;
                else
                    device.Enabled = false;
            }

            return true;
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
            if (!CollectDeviceChecks())
                return;

            btnAutoTune.Enabled = false;
            btnBenchmark.Enabled = false;
            statusBar.Text = "Running autotune...";

            Thread thread = new Thread(new ThreadStart(this.AutoTuneProc));
            thread.Start();
        }

        private void btnBenchmark_Click(object sender, EventArgs e)
        {
            if (!CollectDeviceChecks())
                return;

            btnAutoTune.Enabled = false;
            btnBenchmark.Enabled = false;

            Thread thread = new Thread(new ThreadStart(this.BenchmarkProc));
            thread.Start();
        }

        public void BenchmarkProc()
        {
            try
            {
                statusBar.Invoke((MethodInvoker)delegate { statusBarText.Text = "Running benchmark..."; });

                int trainingSize = int.Parse(textTrainingSize.Text);
                int inputSize = int.Parse(textInputNeurons.Text);
                int outputSize = int.Parse(textOutputNeurons.Text);
                int hiddenSize = int.Parse(textHiddenNeurons.Text);

                Encog.Encog.Instance.CL.CLThreads = int.Parse(textCLThreadCount.Text);
                Encog.Encog.Instance.CL.CLWorkloadSize = int.Parse(textWorkgroupSize.Text);
                Encog.Encog.Instance.CL.EnforcedCLRatio = double.Parse(textCLRatio.Text);

                if (Encog.Encog.Instance.CL.CLWorkloadSize > Encog.Encog.Instance.CL.CLThreads )
                {
                    MessageBox.Show("Workload size cannot be larger than the number of threads.");
                    return;
                }

                INeuralDataSet training = RandomTrainingFactory.Generate(trainingSize, inputSize, outputSize, -1, 1);
                BasicNetwork network = EncogUtility.SimpleFeedForward(
                    training.InputSize, hiddenSize, 0, training.IdealSize, true);
                
                //
                // train OpenCL
                //
                network.Reset();
                ResilientPropagation train = new ResilientPropagation(network, training);
                train.NumThreads = -1;
                train.Iteration();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 1; i <= 100; i++)
                {
                    train.Iteration();
                    this.Invoke((MethodInvoker)delegate { statusBarText.Text = "Running benchmark...CL...Iteration " + i + " of 100."; });
                }
                stopwatch.Stop();
                long clTime = stopwatch.ElapsedMilliseconds;
                this.Invoke((MethodInvoker)delegate { this.textCLResult.Text = Format.FormatInteger((int)stopwatch.ElapsedMilliseconds) + "ms"; });
                String ratio;
                if (((int)train.FlatTraining.CLTimePerIteration) == 0)
                {
                    ratio = "n/a (no GPU)";
                }
                else
                {
                    ratio = Format.FormatDouble(train.FlatTraining.CalculatedCLRatio, 2);
                }

                //
                // Non-CL
                //
                EncogCL temp = Encog.Encog.Instance.CL;
                Encog.Encog.Instance.CL = null;
                network.Reset();
                train = new ResilientPropagation(network, training);
                train.NumThreads = 0;
                train.Iteration();
                stopwatch = new Stopwatch();
                stopwatch.Start();
                for (int i = 1; i <= 100; i++)
                {
                    train.Iteration();
                    this.Invoke((MethodInvoker)delegate { statusBarText.Text = "Running benchmark...Non-CL...Iteration " + i + " of 100."; });
                }
                stopwatch.Stop();

                long nonCLTime = stopwatch.ElapsedMilliseconds;

                this.Invoke((MethodInvoker)delegate { this.textNonCLResult.Text = Format.FormatInteger((int)stopwatch.ElapsedMilliseconds) + "ms"; });
                this.Invoke((MethodInvoker)delegate { this.textPerf.Text = Format.FormatPercent(((double)nonCLTime/(double)clTime)); });

                Encog.Encog.Instance.CL = temp;
                //
                // finish up
                //
                this.Invoke((MethodInvoker)delegate { this.textCalcCLRatio.Text = ratio; });
                this.Invoke((MethodInvoker)delegate { statusBarText.Text = "Ready."; });
                this.Invoke((MethodInvoker)delegate { btnAutoTune.Enabled = true; });
                this.Invoke((MethodInvoker)delegate { btnBenchmark.Enabled = true; });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        public void AutoTuneProc()
        {
            statusBar.Invoke((MethodInvoker)delegate { statusBarText.Text = "Running autotune..."; });

            int trainingSize = int.Parse(textTrainingSize.Text);
            int inputSize = int.Parse(textInputNeurons.Text);
            int outputSize = int.Parse(textOutputNeurons.Text);
            int hiddenSize = int.Parse(textHiddenNeurons.Text);

            INeuralDataSet training = RandomTrainingFactory.Generate(trainingSize, inputSize, outputSize, -1, 1);
            BasicNetwork network = EncogUtility.SimpleFeedForward(
                training.InputSize, hiddenSize, 0, training.IdealSize, true);
            network.Reset();

            long bestTicks = long.MaxValue;
            int bestThreads = 0;

            Encog.Encog.Instance.CL.CLThreads = 25;
            Encog.Encog.Instance.CL.CLWorkloadSize = 25;
            ResilientPropagation warmUp = new ResilientPropagation(network, training);
            warmUp.NumThreads = -1;
            for (int i = 1; i <= 10; i++)
            {
                this.Invoke((MethodInvoker)delegate { statusBarText.Text = "Autotune...Warmup..." + i + "/10"; });
                for(int j=0;j<50;j++)
                    warmUp.Iteration();
            }

            int sameCount = 0;

            for (int i = 2; i < 500; i++)
            {
                try
                {
                    Encog.Encog.Instance.CL.CLThreads = i;
                    Encog.Encog.Instance.CL.CLWorkloadSize = i;

                    ResilientPropagation train = new ResilientPropagation(network, training);
                    train.NumThreads = -1;
                    train.Iteration();
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    for (int j = 0; j < 25; j++)
                        train.Iteration();
                    stopwatch.Stop();

                    if (stopwatch.ElapsedTicks < bestTicks)
                    {
                        bestTicks = stopwatch.ElapsedTicks;
                        bestThreads = i;
                        sameCount = 0;
                    }
                    else
                    {
                        sameCount++;
                        if (sameCount >= 50)
                            break;
                    }

                    this.Invoke((MethodInvoker)delegate
                    {
                        statusBarText.Text = "Autotune...Threads " + i + "/500 :  "
                            + Format.FormatInteger((int)stopwatch.ElapsedTicks) + "ticks, best= threads:" + bestThreads +
                            ", ticks:" + Format.FormatInteger((int)bestTicks);
                    });
                }
                catch (Exception)
                {
                    // too many threads
                    break;
                }
            }

            this.Invoke((MethodInvoker)delegate { this.textCLThreadCount.Text = "" + bestThreads; });
            Encog.Encog.Instance.CL.CLThreads = bestThreads;

            //
            // Now find the workgroup size
            //
            int bestWorkgroup = 0;
            sameCount = 0;
            bestTicks = int.MaxValue;

            for (int i = 1; i < bestThreads; i++)
            {
                try
                {
                    Encog.Encog.Instance.CL.CLThreads = bestThreads;
                    Encog.Encog.Instance.CL.CLWorkloadSize = i;

                    ResilientPropagation train = new ResilientPropagation(network, training);
                    train.NumThreads = -1;
                    train.Iteration();
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    for (int j = 0; j < 25; j++)
                        train.Iteration();
                    stopwatch.Stop();

                    if (stopwatch.ElapsedTicks < bestTicks)
                    {
                        bestTicks = stopwatch.ElapsedTicks;
                        bestWorkgroup = i;
                        sameCount = 0;
                    }
                    else
                    {
                        sameCount++;
                        if (sameCount >= 50)
                            break;
                    }

                    this.Invoke((MethodInvoker)delegate
                    {
                        statusBarText.Text = "Autotune...Workgroup " + i + "/" + bestThreads + " :  "
                            + Format.FormatInteger((int)stopwatch.ElapsedTicks) + "ticks, best= workgroup:" + bestWorkgroup +
                            ", ticks:" + Format.FormatInteger((int)bestTicks);
                    });
                }
                catch (Exception)
                {
                    // too many threads
                    break;
                }
            }

            this.Invoke((MethodInvoker)delegate { this.textWorkgroupSize.Text = "" + bestWorkgroup; });


            //this.Invoke((MethodInvoker)delegate { this.textCalcCLRatio.Text = ratio; });
            this.Invoke((MethodInvoker)delegate { statusBarText.Text = "Ready."; });
            this.Invoke((MethodInvoker)delegate { btnAutoTune.Enabled = true; });
            this.Invoke((MethodInvoker)delegate { btnBenchmark.Enabled = true; });
        }

    }
}
