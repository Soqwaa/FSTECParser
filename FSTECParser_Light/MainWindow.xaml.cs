using System;
using OfficeOpenXml;
using System.Windows;
using System.Windows.Controls;
using FSTECParser_Light.Properties;
using FSTECParser_Light.ViewModels;

namespace FSTECParser_Light
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            InitializeComponent();
            InitializeMainWindowSettings();
        }
        // Инициализация размеров и состояния окна с предыдущей сессии
        private void InitializeMainWindowSettings()
        {
            this.Width = Settings.Default.Width;
            this.Height = Settings.Default.Height;
            this.WindowState = Settings.Default.WindowState;
        }
        private void DataGridMain_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "VisibleId":
                    e.Column.Header = "Идентификатор УБИ";
                    e.Column.Width = DataGridLength.SizeToHeader;
                    break;
                case "Name":
                    e.Column.Header = "Наименование УБИ";
                    e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
        }
        // Изменение размеров окна
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Settings.Default.Height = this.Height;
            Settings.Default.Width = this.Width;
            Settings.Default.Save();
        }
        // Изменение состояния окна (на весь экран / оконный режим)
        private void Window_StateChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Было: " + Settings.Default.WindowState.ToString());
            Settings.Default.WindowState = this.WindowState;
            MessageBox.Show("Стало: " + Settings.Default.WindowState.ToString());
            Settings.Default.Save();
            MessageBox.Show("Save: " + Settings.Default.WindowState.ToString());
        }
        // Анимация открытия и закрытия списка угроз
        private void ListButton_Click(object sender, RoutedEventArgs e)
        {
            this.ListContainter.Visibility = (ListContainter.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            this.ListButton.Content = (ListContainter.Visibility == Visibility.Collapsed) ? "Показать список" : "Скрыть список";
        }
    }
}
