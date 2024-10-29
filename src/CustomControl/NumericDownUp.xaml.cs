using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SeqMaster.CustomControl
{
    public partial class NumericUpDown : UserControl
    {

        public NumericUpDown()
        {
            InitializeComponent();
        }

  
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(NumericUpDown),
                new PropertyMetadata(0));

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }


        public void IncreaseValue(object sender, RoutedEventArgs e)
        {
            Value++;
        }

        public void DecreaseValue(object sender, RoutedEventArgs e)
        {
            Value--;
        }
    }
}
