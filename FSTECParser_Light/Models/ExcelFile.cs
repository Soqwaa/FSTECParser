using FSTECParser_Light.Properties;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace FSTECParser_Light
{
    class ExcelFile
    {
        private static List<Threat> _threats;
        public static List<Threat> Threats
        {
            get
            {
                if(_threats == null)
                {
                    try { _threats = ParseFile(); }
                    catch { _threats = new List<Threat>(); }
                }
                return _threats;
            }
        }
        /// <summary>
        /// Определяет, существует ли файл по пути, указанному в настройках программы
        /// </summary>
        /// <returns>Если файл существует, то метод возвращает true, иначе - false</returns>
        internal static bool CheckFileExist()
        {
            return File.Exists(Settings.Default.LocalFilePath);
        }
        /// <summary>
        /// Проверяет наличие интернет соединения
        /// </summary>
        /// <returns>Если соединение существует, метод возвращает true, иначе - false</returns>
        internal static bool CheckInternetConnection()
        {
            try { return new Ping().Send("google.com").Status == IPStatus.Success ? true : false;  }
            catch { return false; }
        }
        /// <summary>
        /// Загружает файл по URL из настроек программы в каталог с названием, указанным в в настройках программы
        /// </summary>
        internal static void DownloadFile()
        {
            var webClient = new WebClient();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.DefaultConnectionLimit = 9999;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
            webClient.DownloadFile(Settings.Default.WebFilePath, Settings.Default.LocalFilePath);
        }
        /// <summary>
        /// Возвращает коллекцию экземпляров класса Threat полученную из файла, расположенному по пути, указанному в настройках программы
        /// </summary>
        /// <param name="sheetName">Название листа Excel для парсинга</param>
        /// <param name="headerLine">Строка в файле Excel, содержащая заголовки параметров</param>
        /// <returns>Возвращает список экземпляров класса Threat </returns>
        internal static List<Threat> ParseFile(string sheetName = "Sheet", int headerLine = 2)
        {
            List<Threat> threats = new List<Threat>();
            if (CheckFileExist())
                using (var package = new ExcelPackage(new FileInfo(Settings.Default.LocalFilePath), true))
                {
                    var worksheet = package.Workbook.Worksheets.Where(sheet => sheet.Name == sheetName).First();
                    for (int row = worksheet.Dimension.Start.Row + headerLine; row <= worksheet.Dimension.End.Row; row++)
                    {
                        Dictionary<string, string> values = new Dictionary<string, string>();
                        for (int column = worksheet.Dimension.Start.Column; column <= worksheet.Dimension.End.Column; column++)
                        {
                            var param = worksheet.Cells[headerLine, column].Value;
                            var value = worksheet.Cells[row, column].Value;
                            values.Add(param == null ? "" : param.ToString(),
                                       value == null ? "" : value.ToString()
                                       );
                        }
                        threats.Add(new Threat(values));
                    }
                }
            _threats = threats;
            return threats;
        }

        internal static bool SaveFile(string savePath)
        {
            if (CheckFileExist())
            {
                File.Copy(Settings.Default.LocalFilePath, savePath, true);
                return true;
            }
            return false;
        }
    }
}
