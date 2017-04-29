using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KioskVerwaltung
{
    /// <summary>
    /// Interaktionslogik für MonthStatisticView.xaml
    /// </summary>
    public partial class MonthStatisticView : UserControl
    {
        public MonthStatisticViewModel ViewModel
        {
            get { return viewModel; }
        }
        private MonthStatisticViewModel viewModel;

        public MonthStatisticView()
        {
            viewModel = new MonthStatisticViewModel();
            this.DataContext = viewModel;

            InitializeComponent();
        }
    }
}
