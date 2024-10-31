using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NCalc;
using OxyPlot.Wpf;

namespace SeqMaster.AnalysisFolder
{
    internal class AnalysisSequence
    {
        private string formula;
        private TextBlock showFormula;
        private TextBlock showMonotocity;
        private string initialTerm;
        private bool isExplicit;
        public AnalysisSequence(string formula,TextBlock showFormula,TextBlock showMonotony,string initialeTerm, bool isExplicit) 
        {
            this.formula = formula;
            this.showFormula = showFormula;
            this.showMonotocity = showMonotony;
            this.initialTerm = initialeTerm;
            this.isExplicit = isExplicit;
        }

    

        public void AnalysisSeq() 
        {
            
            try
            {
                bool decreasing = true;
                bool increasing = true;


                double[] terms = new double[20];
                double lastValue = Convert.ToDouble(this.initialTerm);

                NCalc.Expression expression = new NCalc.Expression(formula);
                // Recursive Sequence

                for (int i = 0; i < 20; i++)
                {

                    if (isExplicit)
                    {
                        expression.Parameters["n"] = i;

                    }
                    else
                    {
                        expression.Parameters["Un"] = lastValue;
                        expression.Parameters["n"] = i;
                    }

                    double result = Convert.ToDouble(expression.Evaluate());

                    if (double.IsNaN(result))
                    {
                        expression.Parameters["n"] = (int)i;
                        expression.Parameters["Un"] = lastValue;

                        result = Convert.ToDouble(expression.Evaluate());
                    }

                    lastValue = result;
                    terms[i] = result;
                }
                
                    
                
                // Calculate the monotonicity
                for (int i = 0; i < terms.Length - 1; i++)
                {
                    if (terms[i] < terms[i + 1])
                    {
                        decreasing = false;
                    }

                    else if (terms[i] > terms[i + 1])
                    {
                        increasing = false;
                    }
                    

                }

                ShowAnalyses(increasing, decreasing);



            }
            catch (Exception) { }
        }


       

        private void ShowAnalyses(bool increasing,bool decreasing)
        {
            // Show Information (Formula, Monotonicity)
            this.showFormula.Text = "Formula: " + formula;

            if(increasing && !decreasing)
            { 
                this.showMonotocity.Text = "Monotonicity: Increasing Sequence";
            }
            else if (decreasing && !increasing)
            {
                this.showMonotocity.Text = "Monotonicity: Decreasing Sequence";
            }
            else if (!increasing && !decreasing)
            {
                this.showMonotocity.Text = "Monotonicity: Neither Increasing nor decreasing Sequence";
            }
            else
            {
                this.showMonotocity.Text = "Monotonicity: Constant Sequence";
            }
        }

    }
}
