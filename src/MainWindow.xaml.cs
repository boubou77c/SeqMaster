using OxyPlot;
using SeqMaster.AnalysisFolder;
using SeqMaster.File_Menu_Action;
using SeqMaster.GraphMenuWindow;
using SeqMaster.JsonAction;
using SeqMaster.UpdatesMenu;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;

namespace SeqMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlotModel PlotModel { get; private set; }
        DataSaved dataSave;

        GraphMenuWin graphMenuWin;

        AboutWindow.AboutWindow aboutSequenceWindow;
        AboutAppWindow.AboutAppWin aboutAppWindow;
        PlotGeneration plotGeneration;
        FileMenuAction fileMenuAction;

        private bool expliciteSequence = true;
        private double initialTermRec;

        private bool isInfinite = false;


        public MainWindow()
        {


            dataSave = new DataSaved();

            
            InitializeComponent();
            MaxHeight = 625;
            MaxWidth = 1218;

            // Create a basic Plot when the application starts
            plotGeneration = new PlotGeneration(plotView);
            fileMenuAction = new FileMenuAction(plotGeneration);


            showPointsBtn.Click += ShowResult_Click;
            
            // Looks if user change the sequence mod
            RBexpliciteSeq.Click += typeSeq_Changed;
            RBrecursiveSeq.Click += typeSeq_Changed;

            this.KeyDown += OnKeyDown;
        }

        public void ShowResult_Click(Object sender, EventArgs e)
        {
            // Check if it is an infinite graph
            if (maxRange.Value == 0 && minRange.Value == 0)
            {
                isInfinite = true;
            }
            else { isInfinite = false; }

            // Get the range min and Maxi
            int minRangeSeq = minRange.Value;
            int maxRangeSeq = maxRange.Value;

            // Get the formula
            string formula = seqInput.Text;

            // For Recursive sequence -> Get the first terms
            if (firstTermsRC.Text != string.Empty) 
            {
                try
                {
                    initialTermRec = Convert.ToDouble(firstTermsRC.Text);

                }catch(FormatException ) { }
            }
            else { initialTermRec = 0; }
            
            
            plotGeneration.InitializePlot(formula,expliciteSeq:expliciteSequence,initialTermRec, resultLB,isInfinite,minRangeSeq,maxRangeSeq);

            AnalysisSeq();

        }

        // Method for displaying a specific interface based on the user's choice
        public void typeSeq_Changed(object sender, EventArgs e)
        {
            if(RBexpliciteSeq.IsChecked == true)
            {
                recursiveMode.Visibility = Visibility.Collapsed;
                expliciteSequence = true;
            }
            else
            {
                recursiveMode.Visibility = Visibility.Visible;
                expliciteSequence = false;
            }

        }

        // Method for changing the graph as user wants
        public void CustomGraph_Click(object sender, EventArgs e)
        {
            graphMenuWin = new GraphMenuWin();
            
            if(graphMenuWin.ShowDialog() == true)
            {
                string title = graphMenuWin.GraphTitle;
                double thickness = graphMenuWin.LineThickness;
                bool grid = graphMenuWin.ShowGrid;
                OxyColor color = graphMenuWin.LineColor;
                string xTitle = graphMenuWin.Xtitle;
                string yTitle = graphMenuWin.Ytitle;

                plotGeneration.CustomGraph(title, thickness,color,yTitle,xTitle);
            }
            

        }


        // ShortCut 
        private void OnKeyDown(object sender,KeyEventArgs e)
        {
            base.OnKeyDown(e);
           
            // Check if the key CTRL is pressed
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {

                if (e.Key == Key.O && Keyboard.IsKeyDown(Key.LeftShift))
                {
                    Load_Json();
                    e.Handled = true;
                }
                else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftShift))
                {
                    fileMenuAction.ExportDataToCSV(plotGeneration.GetPoints());
                    e.Handled = true;
                }
                else if (e.Key == Key.A && Keyboard.IsKeyDown(Key.LeftShift)) { AnalysesSeqAction(); }

                else if (e.Key == Key.R)
                {
                    plotGeneration.ResetZoom();
                    e.Handled = true;
                }
                else if (e.Key == Key.OemPlus) 
                {
                    plotGeneration.ZoomIn();
                    e.Handled = true;
                }

                else if (e.Key == Key.OemMinus) 
                {
                    plotGeneration.ZoomOut();
                    e.Handled = true;
                }

                else if (e.Key == Key.N) // New Plot
                {
                    ResetPlot();
                    e.Handled = true;
                }
                else if (e.Key == Key.O) // Open CSV
                {
                    fileMenuAction.OpenCsvFile();
                    e.Handled = true;
                }
                else if (e.Key == Key.S) // Save
                {
                    Save();
                    e.Handled = true;
                }
                else if (e.Key == Key.E) // Export as image
                {
                    fileMenuAction.SavePlotAsImage();
                    e.Handled = true;
                }

            }
            
        }

        private void ZoomIn_Click(object sender,EventArgs e) { plotGeneration.ZoomIn(); }

        private void ResetZoom_Click(object sender,EventArgs args) { plotGeneration.ResetZoom(); }

        private void ZoomOut_Click(object sender,EventArgs e ) {  plotGeneration.ZoomOut(); }

        private void SaveImagePlot_Click(object sender, EventArgs e) { fileMenuAction.SavePlotAsImage(); }

        private void NewFile_Click(object sender ,EventArgs e) {ResetPlot();}

        private void SaveCSV_Click(object sender, EventArgs e) { fileMenuAction.ExportDataToCSV(plotGeneration.GetPoints()); }

        private void OpenCSV_Click(Object sender ,EventArgs e) {fileMenuAction.OpenCsvFile();}

        private void ExitApp_Click(object sender ,EventArgs e) {Close();}

        private void Save_Click(object sender, EventArgs e)  { Save(); }

        private void Load_Click(object sender ,EventArgs e) { Load_Json(); }
        private void AboutSeq_Click(object sender ,EventArgs e)
        {
            aboutSequenceWindow = new AboutWindow.AboutWindow();
            aboutSequenceWindow.ShowDialog();
        }

        private void AboutApp_Click(object sender, EventArgs e)
        {
            aboutAppWindow = new AboutAppWindow.AboutAppWin();
            aboutAppWindow.ShowDialog();
        }

        private void UpdatesWin_Click(object sender, EventArgs e) {  UpdatesWindow updatesWindow = new UpdatesWindow();  updatesWindow.ShowDialog(); }

        private void AnalysisSeq_Click(object sender , EventArgs e) 
        {
            AnalysesSeqAction();
        }
        private void AnalysesSeqAction()
        {
            if (this.infoGrid.Visibility == Visibility.Visible)
            {
                this.InfoSeqRow.Height = new GridLength(0);
                this.infoGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.InfoSeqRow.Height = new GridLength(116);
                this.infoGrid.Visibility = Visibility.Visible;
                bool isExplicit = RBexpliciteSeq.IsChecked ?? false;
                string firstTerm = string.IsNullOrEmpty(this.firstTermsRC.Text) ? "0" : this.firstTermsRC.Text;
                AnalysisSequence analysisSequence = new AnalysisSequence(this.seqInput.Text, formulaTB, monotocityTB, firstTerm, isExplicit);
                analysisSequence.AnalysisSeq();
            }
            AnalysisSeq();
        }
        private void AnalysisSeq()
        {
            if (this.infoGrid.Visibility == Visibility.Visible)
            {
                this.InfoSeqRow.Height = new GridLength(116);
                this.infoGrid.Visibility = Visibility.Visible;
                bool isExplicit = RBexpliciteSeq.IsChecked ?? false;
                string firstTerm = string.IsNullOrEmpty(this.firstTermsRC.Text) ? "0" : this.firstTermsRC.Text;

                AnalysisSequence analysisSequence = new AnalysisSequence(this.seqInput.Text, formulaTB, monotocityTB, firstTerm, isExplicit);
                analysisSequence.AnalysisSeq();
            }
            
        }
        private void ResetPlot()
        {
            seqInput.Text = "";
            minRange.Value = 0;

            maxRange.Value = 0;
            firstTermsRC.Text = "";

            plotGeneration.InitializePlot("New Graph", "X", "Y");
        }
        private void Save()
        {
            JsonDataAction jsonData = new JsonDataAction();

            dataSave.Formula = seqInput?.Text ?? "";
            dataSave.minRange = minRange.Value;
            dataSave.maxRange = maxRange.Value;
            dataSave.titleGraph = graphMenuWin?.GraphTitle ?? "";
            dataSave.Xtitle = graphMenuWin?.Xtitle ?? "";
            dataSave.Ytitle = graphMenuWin?.Ytitle ?? "";
            dataSave.pointThinkness = graphMenuWin?.LineThickness ?? 4;
            dataSave.firstTermRecur = firstTermsRC.Text;

            if (RBrecursiveSeq.IsChecked == true)
            {
                dataSave.isRecursive = true;
            }
            else { dataSave.isRecursive = false; }

            jsonData.SaveData(dataSave);
        }

        private void Load_Json()
        {
            JsonDataAction jsonData = new JsonDataAction();

            DataSaved data = jsonData.LoadData();

            if (data != null)
            {
                seqInput.Text = data.Formula;
                minRange.Value = data.minRange;
                maxRange.Value = data.maxRange;

                RBrecursiveSeq.IsChecked = data.isRecursive;
                if (RBrecursiveSeq.IsChecked == true)
                {
                    recursiveMode.Visibility = Visibility.Visible;
                    firstTermsRC.Text = data.firstTermRecur;
                }


            }
        }
    }
}