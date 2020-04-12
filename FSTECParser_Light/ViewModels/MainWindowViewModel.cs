using FSTECParser_Light.Models;
using FSTECParser_Light.Properties;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FSTECParser_Light.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private static ObservableCollection<ShortThreat> threats = ShortThreat.AllShortThreats;
        private static List<ShortThreat> pageThreats;
        private static bool fileIsBroken = false;
        private static int page = 1;
        public int Page
        {
            get
            {
                return page;
            }
            set
            {
                page = value;
                OnPropertyChanged("page");
            }
        }
        public List<ShortThreat> PageThreats
        {
            get
            {
                if (pageThreats == null)
                    return GetShortThreatsByPage();
                return pageThreats;
            }
            set
            {
                pageThreats = value;
                OnPropertyChanged("pageThreats");
            }
        }
        static MainWindowViewModel()
        {
            Initialize();
        }

        #region Commands Region

        #region NextPage Command

        RelayCommand nextPageCommand;
        public ICommand NextPage
        {
            get
            {
                if (nextPageCommand == null)
                    nextPageCommand = new RelayCommand(ExecuteNextPageCommand, CanExecuteNextPageCommand);
                return nextPageCommand;
            }
        }

        public void ExecuteNextPageCommand(object parameter)
        {
            Page++;
            PageThreats = GetShortThreatsByPage();
        }

        public bool CanExecuteNextPageCommand(object parameter)
        {
            if (Page == threats.Count / Settings.Default.ThreatsOnPage + 1)
                return false;
            return true;
        }
        #endregion

        #region PreviousPageCommand

        RelayCommand previousPageCommand;
        public ICommand PreviousPage
        {
            get
            {
                if (previousPageCommand == null)
                    previousPageCommand = new RelayCommand(ExecutePreviousPageCommand, CanExecutePreviousPageCommand);
                return previousPageCommand;
            }
        }

        public void ExecutePreviousPageCommand(object parameter)
        {
            Page--;
            PageThreats = GetShortThreatsByPage();
        }

        public bool CanExecutePreviousPageCommand(object parameter)
        {
            if (Page == 1)
                return false;
            return true;
        }
        #endregion

        #region FirstPageCommand

        RelayCommand firstPageCommand;
        public ICommand FirstPage
        {
            get
            {
                if (firstPageCommand == null)
                    firstPageCommand = new RelayCommand(ExecuteFirstPageCommand, CanExecuteFirstPageCommand);
                return firstPageCommand;
            }
        }

        public void ExecuteFirstPageCommand(object parameter)
        {
            Page = 1;
            PageThreats = GetShortThreatsByPage();
        }

        public bool CanExecuteFirstPageCommand(object parameter)
        {
            if (Page == 1)
                return false;
            return true;
        }
        #endregion

        #region LastPageCommand

        RelayCommand lastPageCommand;
        public ICommand LastPage
        {
            get
            {
                if (lastPageCommand == null)
                    lastPageCommand = new RelayCommand(ExecuteLastPageCommand, CanExecuteLastPageCommand);
                return lastPageCommand;
            }
        }

        public void ExecuteLastPageCommand(object parameter)
        {
            Page = threats.Count / Settings.Default.ThreatsOnPage + 1;
            PageThreats = GetShortThreatsByPage();
        }

        public bool CanExecuteLastPageCommand(object parameter)
        {
            if (Page == threats.Count / Settings.Default.ThreatsOnPage + 1)
                return false;
            return true;
        }
        #endregion

        #region SaveCommand

        RelayCommand saveCommand;
        public ICommand Save
        {
            get
            {
                if (saveCommand == null)
                    saveCommand = new RelayCommand(ExecuteSaveCommand, CanExecuteSaveCommand);
                return saveCommand;
            }
        }

        public void ExecuteSaveCommand(object parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "thrlist.xlsx";
            saveFileDialog.Filter = "Книга Excel (*.xlsx)|*.xlsx";
            // TODO: Избавиться от класса MessageBox
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    if (!ExcelFile.SaveFile(saveFileDialog.FileName))
                        MessageBox.Show("Не удалось сохранить файл", "Не удалось сохранить файл", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    else
                        MessageBox.Show("Файл был успешно сохранён!", "Файл сохранён", MessageBoxButton.OK, MessageBoxImage.None, MessageBoxResult.OK);
                }
                catch
                {
                    MessageBox.Show("Не удалось сохранить файл", "Не удалось сохранить файл", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
        }

        public bool CanExecuteSaveCommand(object parameter)
        {
            return ExcelFile.CheckFileExist() && !fileIsBroken;
        }
        #endregion

        #region ExitCommand
        RelayCommand exitCommand;
        public ICommand Exit
        {
            get
            {
                if (exitCommand == null)
                    exitCommand = new RelayCommand(ExecuteExitCommand, CanExecuteExitCommand);
                return exitCommand;
            }
        }

        public void ExecuteExitCommand(object parameter)
        {
            Environment.Exit(0);
        }

        public bool CanExecuteExitCommand(object parameter)
        {
            return true;
        }
        #endregion.

        #region UpdateFileCommand

        RelayCommand updateFileCommand;
        public ICommand UpdateFile
        {
            get
            {
                if (updateFileCommand == null)
                    updateFileCommand = new RelayCommand(ExecuteUpdateFileCommand, CanExecuteUpdateFileCommand);
                return updateFileCommand;
            }
        }

        public void ExecuteUpdateFileCommand(object parameter)
        {
            if (ExcelFile.CheckFileExist() && fileIsBroken == false)
            {
                Dictionary<Threat, Threat> differentThreats = new Dictionary<Threat, Threat>();
                var lastThreats = ExcelFile.Threats;
                if (TryToDownloadFile())
                {
                    new UpdateFileWindowViewModel(Change.GetChanges(lastThreats, ExcelFile.Threats));
                    Page = 1;
                    PageThreats = GetShortThreatsByPage();
                }
            }
            else
            {
                Initialize();
                PageThreats = GetShortThreatsByPage();
            }
        }

        public bool CanExecuteUpdateFileCommand(object parameter)
        {
            return true;
        }
        #endregion

        #endregion

        private static void Initialize()
        {
            if (!ExcelFile.CheckFileExist())
            {
                string message = "Не удалось найти файл локальной базы данных\n" +
                                 "Желаете произвести первичную загрузку данных?";
                MessageBoxResult boxResult = MessageBox.Show(message, "Не удалось найти файл", MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes);
                if (boxResult == MessageBoxResult.Yes)
                    TryToDownloadFile();
            }
            else
            {
                try { threats = ShortThreat.GetShortThreatsFromThreats(ExcelFile.ParseFile()); }
                catch { MessageBox.Show("Не удалось распарсить файл!\nФайл локальной базы данных повреждён или формат не совпадает\nУдалите файл local в папке с программой и выполните обновления файла в пункте Правка", "Файл повреждён", MessageBoxButton.OK, MessageBoxImage.Error); fileIsBroken = true; }
            }
        }
        private List<ShortThreat> GetShortThreatsByPage()
        {
            int threatsOnPage = Settings.Default.ThreatsOnPage;
            return new List<ShortThreat>(threats.Skip((page - 1) * threatsOnPage)
                                                .Take(threatsOnPage));
        }
        private static bool TryToDownloadFile()
        {
            if (ExcelFile.CheckInternetConnection())
            {
                try
                {
                    ExcelFile.DownloadFile();
                    MessageBox.Show("Файл успешно загружен!", "Успех", MessageBoxButton.OK, MessageBoxImage.None);
                    threats = ShortThreat.GetShortThreatsFromThreats(ExcelFile.ParseFile());
                    page = 1;
                    return true;
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "Ошибка загрузки файла", MessageBoxButton.OK, MessageBoxImage.Error); return false; }
            }
            else
            {
                string message = "Отсутствует подключение к сети Интернет или указан неверный URL адрес.\n" +
                                 "Проверьте ваше интернет соединение и корректность URL адреса для загрузки файла в настройках программы";
                MessageBox.Show(message, "Ошибка загрузки файла", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false;
            }
        }
    }
}
