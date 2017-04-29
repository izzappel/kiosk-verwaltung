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
    /// Interaktionslogik für DayStatisticView.xaml
    /// </summary>
    public partial class DayStatisticView : UserControl
    {
        public DayStatisticViewModel ViewModel
        {
            get { return viewModel; }
        }
        private DayStatisticViewModel viewModel;

        public DayStatisticView()
        {
            viewModel = new DayStatisticViewModel();
            this.DataContext = viewModel;

            InitializeComponent();
        }
    }
}
