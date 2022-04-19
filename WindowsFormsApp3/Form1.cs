using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form

    {
        public double U, S, T, R, V;
        int Trials, Steps;
        private double[,] getmatrix1(int Trials, int Steps)
        {
            double[,] matrix1 = RandomNumberGenerator.NormalRandomNumbers(Trials, Steps);
            return matrix1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        double[,] matrix;
        
        String C;
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                U = Convert.ToDouble(textBox1.Text);
                S = Convert.ToDouble(textBox2.Text);
                T = Convert.ToDouble(textBox3.Text);
                V = Convert.ToDouble(textBox4.Text);
                R = Convert.ToDouble(textBox5.Text);
                Trials = Convert.ToInt32(textBox6.Text);
                Steps = Convert.ToInt32(textBox7.Text);
                matrix = getmatrix1(Trials, Steps);
            }
            catch
            {
                MessageBox.Show("Please check the input and enter the right type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if(radioButton1.Checked==true || radioButton2.Checked==true)
            {
                if(radioButton1.Checked==true)
                {
                    C = "C";
                }
                if(radioButton2.Checked==true)
                {
                    C = "P";
                }
            }
            else
            {
                MessageBox.Show("Please select either Call or Put Option", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            European_Option first_option = new European_Option
            {
                U = U,
                S = S,
                T = T,
                R = R,
                D = R,
                V = V,
                Trials = Trials,
                Steps = Steps,
                matrix1 = matrix,
                CorP = C
            };
            label15.Text = Convert.ToString(first_option.Option_Price);
            label16.Text = Convert.ToString(first_option.Delta);
            label17.Text = Convert.ToString(first_option.Gamma);
            label18.Text = Convert.ToString(first_option.Theta);
            label19.Text = Convert.ToString(first_option.Vega);
            label20.Text = Convert.ToString(first_option.Rho);
            label21.Text = Convert.ToString(first_option.Standard_Deviation);
        }
    }
}
