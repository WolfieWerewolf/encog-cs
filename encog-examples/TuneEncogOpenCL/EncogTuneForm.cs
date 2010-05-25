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

                Encog.Encog.Instance.InitCL();

                this.textCLThreadCount.Text = ""+Encog.Encog.Instance.CL.CLThreads;
                this.textCLRatio.Text = "" + Encog.Encog.Instance.CL.EnforcedCLRatio;
                this.textWorkgroupSize.Text = "" + Encog.Encog.Instance.CL.CLWorkloadSize;

                foreach (EncogCLDevice device in Encog.Encog.Instance.CL.Devices)
                {
                    ListViewItem item = new ListViewItem(new String[] { (device.IsCPU?"CPU":"GPU"), 
                        device.Vender, device.Name, ""+device.MaxComputeUnits, ""+device.MaxClockFrequency,
                        Format.FormatMemory(device.LocalMemorySize), 
                        Format.FormatMemory(device.GlobalMemorySize) });
                    listGPU.Items.Add(item);
                }


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

        }

        private void btnBenchmark_Click(object sender, EventArgs e)
        {

        }
    }
}
