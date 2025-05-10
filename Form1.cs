using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CalculatorLab4
{
    public partial class Form1 : Form
    {
        private bool isResultDisplayed = false;
        private string lastResult = "0";

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyPress += Form1_KeyPress;
            this.KeyDown += Form1_KeyDown;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string validKeys = "0123456789.+-*/%";
            if (validKeys.Contains(e.KeyChar))
            {
                Button btn = new Button { Text = e.KeyChar.ToString() };
                AppendToCalculationString(btn, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyChar == (char)Keys.Enter || e.KeyChar == '=')
            {
                EvaluateCalculation(sender, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                ClearEntry(sender, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                Clear(sender, EventArgs.Empty);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter)
            {
                EvaluateCalculation(sender, EventArgs.Empty);
                e.Handled = true;
            }
        }

        private void AppendToCalculationString(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            string input = btn.Text;
            string operators = "+-*/%";

            if (isResultDisplayed)
            {
                isResultDisplayed = false;
                if (operators.Contains(input))
                {
                    txtInput.Text = lastResult + input;
                }
                else
                {
                    txtInput.Text = (input == ".") ? "0." : input;
                }
                return;
            }

            if (input == ".")
            {
                string[] parts = txtInput.Text.Split(operators.ToCharArray());
                string lastNumber = parts.Length > 0 ? parts[parts.Length - 1] : "";
                if (lastNumber.Contains("."))
                    return;
            }

            if (txtInput.Text == "0" && input != ".")
            {
                txtInput.Text = input;
                return;
            }

            if (operators.Contains(input) && txtInput.Text.Length > 0 &&
                operators.Contains(txtInput.Text[txtInput.Text.Length - 1].ToString()))
            {
                txtInput.Text = txtInput.Text.Substring(0, txtInput.Text.Length - 1) + input;
                return;
            }

            txtInput.Text += input;
        }

        private void Clear(object sender, EventArgs e)
        {
            txtInput.Text = "0";
        }

        private void ClearEntry(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtInput.Text) && txtInput.Text.Length > 1)
                txtInput.Text = txtInput.Text.Substring(0, txtInput.Text.Length - 1);
            else
                txtInput.Text = "0";
        }

        private void EvaluateCalculation(object sender, EventArgs e)
        {
            if (isResultDisplayed)
                txtHistory.Clear();

            string expression = txtInput.Text;
            if (string.IsNullOrWhiteSpace(expression))
            {
                MessageBox.Show("Please enter an expression.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                expression = expression.Replace("×", "*").Replace("÷", "/");

                string operators = "+-*/%";
                if (operators.Contains(expression[expression.Length - 1].ToString()))
                {
                    MessageBox.Show("Expression cannot end with an operator.", "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var dt = new DataTable();
                object evalResult = dt.Compute(expression, null);

                if (evalResult == null)
                    throw new Exception("Evaluation returned null.");

                double result = Convert.ToDouble(evalResult);

                if (double.IsInfinity(result) || double.IsNaN(result))
                {
                    MessageBox.Show("Expression is undefined.", "Evaluation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtInput.Text = "0";
                    return;
                }

                txtInput.Text = result.ToString("G");
                txtHistory.Text = expression;
                isResultDisplayed = true;
                lastResult = result.ToString("G");
            }
            catch (SyntaxErrorException)
            {
                MessageBox.Show("Expression was invalid.", "Syntax Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtInput.Text = "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid expression.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtInput.Text = "0";
            }
        }
    }
}
