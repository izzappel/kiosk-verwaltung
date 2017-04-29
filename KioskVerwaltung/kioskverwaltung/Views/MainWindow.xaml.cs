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
using KioskVerwaltung.BusinessObjects;

namespace KioskVerwaltung
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;
        
        public MainWindow()
        {
            viewModel = new MainViewModel();
            this.DataContext = viewModel;

            InitializeComponent();
            SaleView.Focus();
        }

        private void OpenKioskFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Dateien|*.xml;*.XML";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    viewModel.LoadKioskData(openFileDialog.FileName);
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
        private void SaveKioskFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "XML Dateien|*.xml;*.XML";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    viewModel.SaveKioskData(saveFileDialog.FileName);
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

        private void Close(object sender, RoutedEventArgs e)
        {
            viewModel.Close();
            this.Close();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 1)
            {
                SaleView.Focus();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaleView.CleanForClosing();
        }
    }
}
