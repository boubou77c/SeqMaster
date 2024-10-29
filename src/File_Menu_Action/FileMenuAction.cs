using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SeqMaster.File_Menu_Action
{

    internal class FileMenuAction
    {
        private PlotModel plotModel;
        private PlotView plotView;
        private PlotGeneration plotGeneration;
        public FileMenuAction(PlotGeneration plotGeneration) { this.plotGeneration = plotGeneration; }

        public void SavePlotAsImage()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Plot As";
            saveFileDialog.Filter = "Png Image |*.png |Jpeg Image |*.jpeg";
            saveFileDialog.FileName = "Plot.png";


            if(saveFileDialog.ShowDialog()==true)
            {
                using(var stream = File.Create(saveFileDialog.FileName))
                {
                    var pngExporter = new PngExporter { Width = 900, Height = 600 };

                    pngExporter.Export(this.plotGeneration.PlotModel, stream);
                }
            }

        }


        public void ExportDataToCSV(List<DataPoint> dataPoints)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Title = "CSV Save";
            saveFile.Filter = "CSV |*.csv";

            if(saveFile.ShowDialog()==true)
            {
                string filePath = saveFile.FileName;
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("X,Y");

                foreach (var point in dataPoints)
                {
                    stringBuilder.AppendLine($"{point.X};{point.Y}");
                }

                File.WriteAllText(filePath,stringBuilder.ToString());
            }
        }


        public void OpenCsvFile()
        {
            List<double[]> data = new List<double[]>();


            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files | *.csv",
                Title = "Open CSV file",

            };

            if (openFileDialog.ShowDialog() == true)
            {


                string filePath = openFileDialog.FileName;
                string fileName= openFileDialog.SafeFileName;
                using (var reader = new StreamReader(filePath))
                {
                    //initialize var line
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Separate the values
                        string[] value = line.Split(';');
                        // If value is >= 2 and the conversion successful for the index 0 and 1
                        if(value.Length >= 2 
                            && double.TryParse(value[0], out double x) 
                            && double.TryParse(value[1], out double y))
                        {
                            // add x and y into a double array
                            data.Add(new double[] { x, y });
                        }

                    }

                    var lineSeries = new LineSeries
                    {
                        Title = "Points du CSV",
                        MarkerType = MarkerType.Circle,
                        MarkerSize = 5,
                        StrokeThickness = 2
                    };
                    foreach (var point in data)
                    {
                        lineSeries.Points.Add(new DataPoint(point[0], point[1]));
                    }

                    this.plotGeneration.PlotModel.Title = fileName;
                    this.plotGeneration.PlotModel.Series.Clear();
                    this.plotGeneration.PlotModel.Series.Add(lineSeries);
                    this.plotGeneration.PlotModel.InvalidatePlot(true);
                }

            }

            
        }

    }

}
