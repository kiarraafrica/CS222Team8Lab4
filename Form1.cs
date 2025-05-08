using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorLab4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void AppendToCalculationString(object sender, EventArgs e)
        {
            Button invokedBtn = sender as Button;
            if (invokedBtn != null)
            {
                txtInput.Text += invokedBtn.Text;
            }
        }

        private void Clear(object sender, EventArgs e)
        {
            txtInput.Text = string.Empty;
        }

        private void ClearEntry(object sender, EventArgs e)
        {
            if (txtInput.Text.Length > 0)
            {
                txtInput.Text = txtInput.Text.Substring(0, txtInput.Text.Length - 1);
            }
        }

        private void EvaluateCalculation(object sender, EventArgs e)
        {
            string expression = txtInput.Text;
            var result = new DataTable();
            try
            {
                double evaluatedResult = Convert.ToDouble(result.Compute(expression, null));
                if (double.IsInfinity(evaluatedResult) || double.IsNaN(evaluatedResult))
                {
                    MessageBox.Show("Expression was evaluated to be undefined.", "Evaluation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtInput.Text = string.Empty;
                    return;
                }

                // Format the result to always show decimals
                txtInput.Text = evaluatedResult.ToString("G"); // "G" ensures significant digits are preserved
            }
            catch (System.Data.SyntaxErrorException)
            {
                MessageBox.Show("Expression was not valid.", "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtInput.Text = string.Empty;
            }
        }

    }
}
