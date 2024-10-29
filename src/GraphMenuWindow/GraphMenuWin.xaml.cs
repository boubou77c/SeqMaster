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
using System.Drawing;
using OxyPlot;
using System.Windows.Markup;
using SeqMaster.JsonAction;

namespace SeqMaster.GraphMenuWindow
{
    /// <summary>
    /// Interaction logic for GraphMenuWin.xaml
    /// </summary>
    public partial class GraphMenuWin : Window
    {

        private JsonDataAction jsonDataAction;
        private DataSaved dataSave;

        public GraphMenuWin()
        {
            InitializeComponent();
            jsonDataAction = new JsonDataAction();

            DataSaved data = jsonDataAction.LoadSettings();
            MaxHeight = 504;
            MaxWidth = 350;
            if (data != null)
            {
                this.LineThicknessSlider.Value = data.pointThinkness;
                this.XTitle.Text = data.Xtitle;
                this.YTitle.Text = data.Ytitle;
                this.GraphTitleTextBox.Text = data.titleGraph;
            }
        }
        public string GraphTitle { get;  set; }
        public OxyColor LineColor { get;  set; }
        public double LineThickness { get;  set; }
        public bool ShowGrid { get;  set; }
        public string Xtitle { get;  set; }
        public string Ytitle { get;  set; }

        

        private void ChooseColorButton_Click(object sender , EventArgs e)
        {
            // Get the selected item
            if (ColorComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedColor = selectedItem.Content.ToString();

                if (selectedColor == "Blue")
                {
                    LineColor = OxyColor.FromRgb(0, 8, 255);
                }
                else if (selectedColor == "Red")
                {
                    LineColor = OxyColor.FromRgb(255, 0, 0);
                }
                else if (selectedColor == "Green")
                {
                    LineColor = OxyColor.FromRgb(109, 196, 102);
                }

            
        }

    }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            dataSave = new DataSaved();

            GraphTitle = GraphTitleTextBox.Text;
            LineThickness = LineThicknessSlider.Value;
            DialogResult = true;
            Xtitle = XTitle.Text;
            Ytitle = YTitle.Text;

            dataSave.titleGraph = GraphTitle;
            dataSave.Xtitle = Xtitle;
            dataSave.Ytitle = Ytitle;
            dataSave.pointThinkness = LineThickness;
            

            jsonDataAction = new JsonDataAction();
            jsonDataAction.SaveSettings(dataSave);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }


        
    }
}
