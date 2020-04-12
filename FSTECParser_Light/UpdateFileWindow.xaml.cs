using FSTECParser_Light.Properties;
using System.Windows;
using System.Windows.Controls;

namespace FSTECParser_Light
{
    /// <summary>
    /// Логика взаимодействия для UpdateFileWindow.xaml
    /// </summary>
    public partial class UpdateFileWindow : Window
    {
        internal UpdateFileWindow()
        {
            InitializeComponent();
            this.Width = Settings.Default.Width;
            this.Height = Settings.Default.Height;
            this.WindowState = Settings.Default.WindowState;
        }

        private void DataGrid_AutoGeneratingColumn(object sender, System.Windows.Controls.DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.Column.Header.ToString())
            {
                case "Id":
                    e.Column.Header = "Идентификатор УБИ";
                    e.Column.Width = DataGridLength.SizeToHeader;
                    break;
                case "Param":
                    e.Column.Header = "Наименование параметра";
                    e.Column.MaxWidth = 200;
                    new DataGridLength(1, DataGridLengthUnitType.Star);
                    break;
                case "LastValue":
                    e.Column.Header = "Старое значение параметра";
                    e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                    break;
                case "NewValue":
                    e.Column.Header = "Новое значение параметра";
                    e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
            DataGrid dataGrid = (DataGrid)sender;
            DataGridTextColumn column = e.Column as DataGridTextColumn;
            if (column != null)
                column.ElementStyle = dataGrid.Resources["ElementStyle"] as Style;
        }
    }
}
