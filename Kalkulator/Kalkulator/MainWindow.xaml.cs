using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace Kalkulator
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Decimal previous = 0;
        private bool writeFromBeginnig = true;
        private bool isSubmitClicked = false;
        private bool isMathematicalOperationOngoing = false;
        private bool isAddingOn = false;
        private bool isSubtractingOn = false;
        private bool isMultiplicationOn = false;
        private bool isDivisionOn = false;
        private bool isSquareRootOn = false;
        private bool errorOcured = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MathematicalOperation(ref bool someBool)
        {
            if ((!writeFromBeginnig || isSubmitClicked) && !errorOcured)
            {
                if (isMathematicalOperationOngoing)
                {
                    Submit();
                }
                previous = Convert.ToDecimal(textBox.Text);
                isMathematicalOperationOngoing = true;
                someBool = true;
                writeFromBeginnig = true;
                isSubmitClicked = false;
            }
        }

        private void Submit()
        {
            if (isAddingOn)
            {
                Decimal current = Convert.ToDecimal(textBox.Text) + previous;
                textBox.Text = Convert.ToString(current);
                isAddingOn = false;
            }
            else if (isSubtractingOn)
            {
                Decimal current = previous - Convert.ToDecimal(textBox.Text);
                textBox.Text = Convert.ToString(current);
                isSubtractingOn = false;
            }
            else if (isMultiplicationOn)
            {
                Decimal current = previous * Convert.ToDecimal(textBox.Text);
                textBox.Text = Convert.ToString(current);
                isMultiplicationOn = false;
            }
            else if (isDivisionOn)
            {
                if (Convert.ToDecimal(textBox.Text) != 0)
                {
                    Decimal current = previous / Convert.ToDecimal(textBox.Text);
                    textBox.Text = Convert.ToString(current);
                }
                else
                {
                    textBox.Text = "Cholero, nie dziel przez zero!";
                    errorOcured = true;
                }
                
                isDivisionOn = false;
            }
            else if (isSquareRootOn)
            {
                Decimal current = Convert.ToDecimal(Math.Sqrt(Convert.ToDouble(textBox.Text)));
                textBox.Text = Convert.ToString(current);
                isSquareRootOn = false;
            }
            isMathematicalOperationOngoing = false;
            writeFromBeginnig = true;
        }

        private void DeleteOneNumber()
        {
            if ((!writeFromBeginnig || isSubmitClicked) && !errorOcured)
            {
                textBox.SelectionStart = textBox.Text.Length;

                if (textBox.Text.Contains("-"))
                {
                    if (textBox.Text.Length == 2)
                    {
                        textBox.Text = "0";
                    }
                    else
                        textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                }
                else
                {
                    if (textBox.Text.Length == 1)
                    {
                        textBox.Text = "0";
                    }
                    else
                        textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                }
            }
        }

        private void Btn_Number_Click(object sender, RoutedEventArgs e)
        {
            if (!errorOcured)
            {
                Button btn = sender as Button;

                if (writeFromBeginnig || textBox.Text == "0")
                {
                    textBox.Text = btn.Content.ToString();
                    writeFromBeginnig = false;
                }
                else
                    textBox.Text += btn.Content.ToString();
            }
        }

        private void Btn_Comma_Click(object sender, RoutedEventArgs e)
        {
            if (((!textBox.Text.Contains(",") || textBox.Text == "0") && !writeFromBeginnig)  && !errorOcured)
            {
                Button btn = sender as Button;
                textBox.Text += btn.Content.ToString();
            }
        }

        private void Btn_ChangeSign_Click(object sender, RoutedEventArgs e)
        {
            if ((!writeFromBeginnig || isSubmitClicked) && !errorOcured)
            {
                textBox.Text = Convert.ToString(-Convert.ToDecimal(textBox.Text));
            }
        }

        private void Btn_Backspace_Click(object sender, RoutedEventArgs e)
        {
            DeleteOneNumber();
        }

        private void Btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = "0";
            previous = 0;
            writeFromBeginnig = true;
            isSubmitClicked = false;
            isMathematicalOperationOngoing = false;
            isAddingOn = false;
            isSubtractingOn = false;
            isMultiplicationOn = false;
            isDivisionOn = false;
            isSquareRootOn = false;
            errorOcured = false;
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            MathematicalOperation(ref isAddingOn);
        }

        private void Btn_Subtract_Click(object sender, RoutedEventArgs e)
        {
            MathematicalOperation(ref isSubtractingOn);
        }

        private void Btn_Multiply_Click(object sender, RoutedEventArgs e)
        {
            MathematicalOperation(ref isMultiplicationOn);
        }

        private void Btn_Division_Click(object sender, RoutedEventArgs e)
        {
            MathematicalOperation(ref isDivisionOn);
        }

        private void Btn_SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            if (!errorOcured)
            {
                isSquareRootOn = true;
                Submit();
                isSubmitClicked = true;
            }
        }
        
        private void Btn_Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!errorOcured)
            {
                Submit();
                isSubmitClicked = true;
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!errorOcured)
            {
                Regex regex = null;
                textBox.SelectionStart = textBox.Text.Length;
                if (textBox.Text.Contains(","))
                {
                    regex = new Regex("[^0-9]+");
                }
                else
                    regex = new Regex("[^0-9,]+");
                if (!regex.IsMatch(e.Text))
                {
                    if (!errorOcured)
                    {
                        if (writeFromBeginnig || textBox.Text == "0")
                        {
                            textBox.Text = e.Text;
                            writeFromBeginnig = false;
                        }
                        else
                            textBox.Text += e.Text;
                    }
                }
                e.Handled = true;
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Add || e.Key == Key.OemPlus)
            {
                MathematicalOperation(ref isAddingOn);
            }
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                MathematicalOperation(ref isSubtractingOn);
            }
            else if (e.Key == Key.Multiply)
            {
                MathematicalOperation(ref isMultiplicationOn);
            }
            else if (e.Key == Key.Divide)
            {
                MathematicalOperation(ref isDivisionOn);
            }
            else if (e.Key == Key.Enter)
            {
                if (!errorOcured)
                {
                    Submit();
                    isSubmitClicked = true;
                }
            }
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                DeleteOneNumber();
            }
            e.Handled = true;
        }
    }
}
