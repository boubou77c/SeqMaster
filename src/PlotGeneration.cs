using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SeqMaster
{
    class PlotGeneration
    {
        public PlotModel PlotModel { get; private set; }

        private LineSeries lineSeries;

        public PlotView plotView;
        
        private string formula;

        private bool isExplicite;

        private Label r;
        // For Recursive formula 
        private double initialTerm;

        private bool isInfinite;

        private OxyColor colorLine = OxyColor.FromRgb(90,176,140);
        private double Thinkness = 4;
        private string TitleGraph = "New Graph";

        private int maxRange;
        private int minRange;

        private string XTitle = "X";
        private string YTitle = "Y";

        public PlotGeneration(PlotView plotView)
        {
            this.plotView = plotView;
            InitializePlot("New Graph", "X", "Y");
        }

        public void CustomGraph(string title,double thinkness,OxyColor color,string yTitle,string xTitle)
        {
            this.Thinkness = thinkness;
            this.TitleGraph = title;
            this.colorLine = color;
            this.XTitle = xTitle;
            this.YTitle = yTitle;

            InitializePlot(this.formula, this.isExplicite, this.initialTerm, this.r, this.isInfinite, minRange, maxRange);
            
        }
        

        
        public void InitializePlot(string graphTitle, string xTitle, string yTitle)
        {
            PlotModel = new PlotModel
            {
                Title = graphTitle,
                Background = OxyColor.FromRgb(255, 255, 255),

        };
            
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = xTitle,
                MaximumRange = 1000,



                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MinorGridlineColor = OxyColors.LightGray
            };

            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = yTitle,
                MaximumRange = 1000,

                   

            };

            PlotModel.Axes.Add(yAxis);
            PlotModel.Axes.Add(xAxis);

            plotView.Model = PlotModel;
        }

        public void InitializePlot(string formula,bool expliciteSeq,double initialTermRec,Label result,bool SeqInfinite,int minRange, int maxRange)
        {
            PlotModel = new PlotModel { Title = this.TitleGraph};
            PlotModel.Background = OxyColor.FromRgb(255,255,255);

            this.formula = formula;
            this.isExplicite = expliciteSeq;
            this.initialTerm = initialTermRec;
            this.r = result;
            this.isInfinite = SeqInfinite;

            this.minRange = minRange;
            this.maxRange = maxRange;

            lineSeries = new LineSeries {
                Title = "f(n) = " + formula,
               
                LineStyle = LineStyle.Solid,    
                StrokeThickness = this.Thinkness,

                MarkerType = MarkerType.Circle,  
                MarkerSize = this.Thinkness,                
                MarkerFill = this.colorLine,
            };
            

            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = this.XTitle,
                 
                MajorGridlineStyle = LineStyle.Solid,    
                MinorGridlineStyle = LineStyle.Dot,      
                MinorGridlineColor = OxyColors.LightGray,
                MaximumRange = 1000,
                MinimumRange = 1,
                Minimum = 0,
                Maximum = 20,
                MajorStep = 1,
                MinorStep = 1,
            };

            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = this.YTitle,
                MaximumRange = 1000,
                MinimumRange = 1,
                Minimum = 0,
                Maximum = 20,
                MajorStep = 1,
                MinorStep = 1,
                

            };

            PlotModel.Axes.Add(yAxis);
            PlotModel.Axes.Add(xAxis);

            plotView.Model = PlotModel;

            PlotModel.Series.Add(lineSeries);

            xAxis.AxisChanged += (s, e) => AdjustAxisSteps(xAxis);
            yAxis.AxisChanged += (s, e) => AdjustAxisSteps(yAxis);

            var GetXaxis = PlotModel.Axes[1];

            if (isInfinite)
            {
                if (formula.Length > 0)
                {
                    GetXaxis.AxisChanged += Axis_Changed;
                }
                
            }
            else
            {
                if (isExplicite)
                {
                    ExpliciteSeq(minRange, maxRange);
                }
                else
                {
                    RecursiveSeq(minRange, maxRange);
                }
            }
        }

        // Customing zoom
        void AdjustAxisSteps(LinearAxis axis)
        {
            double range = axis.ActualMaximum - axis.ActualMinimum;
            
            if (range <= 20)
            {
                axis.MajorStep = 1;
                axis.MinorStep = 1;
            }
            else if (range <= 100)
            {
                axis.MajorStep = 10;
                axis.MinorStep = 5;
            }
            else
            {
                axis.MajorStep = 100;
                axis.MinorStep = 50;
            }
            plotView.InvalidatePlot(true);
        }

        public List<DataPoint> GetPoints()
        {
             List<DataPoint> points = new List<DataPoint>();


            foreach (var point in lineSeries.Points)
            {
                points.Add(point);
            }
            return points;
        }


        private void Axis_Changed(object sender, EventArgs e)
        {
            var xAxis = sender as LinearAxis;

            if (xAxis != null)
            {
                double minX = xAxis.ActualMinimum;
                double maxX = xAxis.ActualMaximum;


                if (isExplicite)
                {
                    ExpliciteSeq(minX, maxX);
                }
                else { RecursiveSeq(minX, maxX); }
            }
        }

        
        
        private void ExpliciteSeq(double minX, double maxX)
        {
            lineSeries.Points.Clear();

            try
            {
                NCalc.Expression expression = new NCalc.Expression(formula);


                double rangeMin = minX;
                double rangeMax = maxX;

                double step = 1; 

                if (isInfinite)
                {
                    rangeMin = Math.Max(0, minX - (maxX - minX) * 0.1);
                    rangeMax = maxX + (maxX - minX) * 0.1;

                    double visibleRange = maxX - minX;

                    step = Math.Max(1, visibleRange / 500);

                }

                for (double term = rangeMin; term <= rangeMax; term += step)
                {
                    
                    expression.Parameters["n"] = term;
                    double result = Convert.ToDouble(expression.Evaluate());

                    if(double.IsNaN(result))
                    {
                        expression.Parameters["n"] = (int)term;
                        result = Convert.ToDouble(expression.Evaluate());
                    }
                    //MessageBox.Show(Convert.ToString(result));
                    //this.r.Content = ($"term: {term}, result: {result}\n minX {minX} maxX: {maxX}");

                    this.r.Content = ($"Minimum X {minX} \nMaximum X: {maxX}");

                    lineSeries.Points.Add(new DataPoint(term, result));
                    //MessageBox.Show(Convert.ToString(result));
                }

            }
            catch (Exception) { }

            //Pow(-1, n) * (1.0 / n)
            plotView.InvalidatePlot(true);

        }
       

        private void RecursiveSeq(double minX, double maxX)
        {
            lineSeries.Points.Clear();

            try
            {
                NCalc.Expression expression = new NCalc.Expression(formula);

                double lastValue = this.initialTerm;

                double rangeMax = maxX;
                double step = 1;

                if (isInfinite)
                {
                    rangeMax = maxX + (maxX - minX) * 0.1;

                    double visibleRange = maxX - minX;

                    step = Math.Max(1, visibleRange / 500);
                }

                lineSeries.Points.Add(new DataPoint(0, this.initialTerm));

                for (double i = 1; i < rangeMax; i+=step)
                {
                    i = Math.Round(i, 1);

                    expression.Parameters["Un"] = lastValue;
                    expression.Parameters["n"] = i;

                    double result = Convert.ToDouble(expression.Evaluate());

                    if (double.IsNaN(result))
                    {
                        expression.Parameters["n"] = (int)i;
                        result = Convert.ToDouble(expression.Evaluate());
                    }

                    this.r.Content = ($"Minimum X {minX} \nMaximum X: {maxX}");

                    lineSeries.Points.Add(new DataPoint(i, result));

                    lastValue = result;
                }

                plotView.InvalidatePlot(true);

            }
            catch(Exception) { }

        }

        
        public void ZoomIn()
        {
            foreach (var axis in plotView.Model.Axes)
            {

                if (axis is LinearAxis linearAxis)
                {
                    double newMin = axis.ActualMinimum + 10;
                    double newMax = axis.ActualMaximum - 10;
                
                    linearAxis.Zoom(newMin, newMax);                  
                }


            }
            plotView.InvalidatePlot(true);
        }


        public void ZoomOut()
        {
            foreach (var axis in plotView.Model.Axes)
            {
                if (axis is LinearAxis linearAxis)
                {
                   
                    linearAxis.Zoom(axis.ActualMinimum * 1.1, axis.ActualMaximum * 1.1);
                }
            }
            plotView.InvalidatePlot(true);
        }

        // Reset Zoom 
        public void ResetZoom()
        {
            plotView.ResetAllAxes();
            plotView.InvalidatePlot(true);
        }

    }
}
