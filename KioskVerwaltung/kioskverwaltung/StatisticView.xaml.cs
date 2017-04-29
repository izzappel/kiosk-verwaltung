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
    /// Interaktionslogik für StatisticView.xaml
    /// </summary>
    public partial class StatisticView : UserControl
    {
        private StatisticViewModel viewModel;

        public StatisticView()
        {
            viewModel = new StatisticViewModel();
            DataContext = viewModel;

            InitializeComponent();

            MonthView.Visibility = System.Windows.Visibility.Visible;
            WeekView.Visibility = System.Windows.Visibility.Collapsed;
            DayView.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void SetMonthView(object sender, RoutedEventArgs e)
        {
            MonthView.Visibility = System.Windows.Visibility.Visible;
            WeekView.Visibility = System.Windows.Visibility.Collapsed;
            DayView.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void SetWeekView(object sender, RoutedEventArgs e)
        {
            WeekView.Visibility = System.Windows.Visibility.Visible;
            MonthView.Visibility = System.Windows.Visibility.Collapsed;
            DayView.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void SetDayView(object sender, RoutedEventArgs e)
        {
            DayView.Visibility = System.Windows.Visibility.Visible;
            WeekView.Visibility = System.Windows.Visibility.Collapsed;
            MonthView.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}
