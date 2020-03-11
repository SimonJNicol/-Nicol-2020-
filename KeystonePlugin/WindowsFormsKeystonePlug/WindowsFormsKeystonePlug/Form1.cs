using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeystonePlugin
{
    public partial class BeamDesigner : Form
    {
        public BeamDesigner()
        {
            InitializeComponent();
        }

        private void BeamDesigner_Load(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            StringBuilder woodQuality = new StringBuilder("", 11);
            StringBuilder woodSpecies = new StringBuilder("", 25);
            string cutType = string.Empty;
            string output = string.Empty;
            char minResults = '0';
            double beamLength = -1;
            double liveLoad = -1;
            double deadLoad = -1;

            string temp = textBox1.Text;

            if (temp.Equals(""))
            {
                MessageBox.Show("Beam Length Forgotten... Cannot Calculate... Aborting...");
                System.Windows.Forms.Application.Exit();
            }

            else
            {
                beamLength = double.Parse(temp);
            }

            temp = textBox2.Text;

            if (temp.Equals(""))
            {
                MessageBox.Show("Live Load Forgotten... Cannot Calculate... Aborting...");
                System.Windows.Forms.Application.Exit();
            }

            else
            {
                liveLoad = double.Parse(temp);
            }

            temp = textBox3.Text;

            if (temp.Equals(""))
            {
                MessageBox.Show("Beam Length Forgotten... Cannot Calculate... Aborting...");
                System.Windows.Forms.Application.Exit();
            }

            else
            {
                deadLoad = double.Parse(temp);
            }

            if (checkBox1.Checked)
            {
                minResults = '1';
            }

            temp = comboBox1.SelectedItem.ToString();
            
            switch (temp)
            {
                case "Dimensional Lumber":
                    cutType = "1000";
                    break;
                case "Timbers":
                    cutType = "0100";
                    break;
                case "Beams and Stringers":
                    cutType = "0010";
                    break;
                case "Glulams":
                    cutType = "0001";
                    break;

                default:
                    MessageBox.Show("#ERROR SELECTING CUT TYPE#\n#ABORTING...");
                    cutType = "0000";
                    break;
            }
            
            while (i < 11)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    woodQuality.Append("1");
                }
                else
                {
                    woodQuality.Append("0");
                }
                i++;
            }
            
//            MessageBox.Show("" + i);
            i = 0;
            
            while (i < 25)
            {
                if (checkedListBox2.GetItemChecked(i))
                {
                    woodSpecies.Append("1");
                }
                else
                {
                    woodSpecies.Append("0");
                }
                i++;
            }
            //            MessageBox.Show("" + i);
            output = woodQuality + "," + woodSpecies + "," + cutType + "," + minResults + "," 
                + beamLength + "," + liveLoad + "," + deadLoad;
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\" + "output.txt", output);
            MessageBox.Show("Complete.");
            System.Windows.Forms.Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
