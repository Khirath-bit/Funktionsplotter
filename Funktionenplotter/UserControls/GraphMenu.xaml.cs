﻿using System;
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

namespace Funktionenplotter.UserControls
{
    /// <summary>
    /// Interaction logic for GraphMenu.xaml
    /// </summary>
    public partial class GraphMenu : UserControl
    {
        public GraphMenu()
        {
            InitializeComponent();
        }

        private void CalculateIntegral_Click(object sender, RoutedEventArgs e)
        {
            new IntegralSettings().ShowDialog();
        }
    }
}
