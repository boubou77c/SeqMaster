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
using System.Windows.Shapes;

namespace SeqMaster.AboutAppWindow
{
    /// <summary>
    /// Interaction logic for AboutAppWin.xaml
    /// </summary>
    public partial class AboutAppWin : Window
    {
        public AboutAppWin()
        {
            InitializeComponent();
            MaxHeight = 530;
            MaxWidth = 416;

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
