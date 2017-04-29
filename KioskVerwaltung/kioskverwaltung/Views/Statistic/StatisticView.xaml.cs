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
using Microsoft.Win32;
using System.Security;
using System.Xml;
using KioskVerwaltung.Printing;
using System.Printing;
using System.Windows.Xps;

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

            viewModel.SalesUpdated += MonthView.ViewModel.UpdateSales;
            viewModel.SalesUpdated += DayView.ViewModel.UpdateSales;

            viewModel.Udpate();
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

        private void OpenSaleFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Dateien|*.xml;*.XML";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    viewModel.LoadSaleData(openFileDialog.FileName);
                }
                catch (SecurityException exception)
                {
                    MessageBox.Show("Die Datei konnte nicht geöffnet werden. Bitte überprüfen Sie Ihre Rechte.\n\nFolgende Angaben an Support weiterleiten:\n" + exception.Message + "\n" + exception.StackTrace, "Fehler bei Öffnen der Datei");
                }
                catch (XmlException xmlException)
                {
                    MessageBox.Show("Die Datei enthält Fehler oder hat nicht das korrekte Format.\n\nFolgende Angaben an Support weiterleiten:\n" + xmlException.Message + "\n" + xmlException.StackTrace, "Fehler bei Öffnen der Datei");
                }
            }
        }
        private void PrintStatistic(object sender, RoutedEventArgs e)
        {
            if (MonthView.Visibility == System.Windows.Visibility.Visible) {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    PrintQueue printQueue = printDialog.PrintQueue;
                    XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);
                    IDocumentPaginatorSource document = new MonthPrintFlowDocument(MonthView.ViewModel);
                    xpsDocumentWriter.Write(document.DocumentPaginator);
                }

            }
            if (DayView.Visibility == System.Windows.Visibility.Visible)
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {
                    PrintQueue printQueue = printDialog.PrintQueue;
                    XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);
                    IDocumentPaginatorSource document = new DayPrintFlowDocument(DayView.ViewModel);
                    xpsDocumentWriter.Write(document.DocumentPaginator);
                }
            }
        }

    }
}
