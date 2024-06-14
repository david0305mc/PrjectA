using OfficeOpenXml;
using System.Text;

namespace Excel2CSV
{
    public class ExportToCsv
    {
        private const string srcFolder = "SrcFolder";
        private const string serverFolder = "ServerCsv";
        private const string clientFolder = "../Assets/Resources/Data/";
        //private const string clientFolder = "Client";

        public enum TargetType
        {
            Server,
            Client,
        }
        public ExportToCsv(TargetType targetType)
        {
            string serverFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, serverFolder);
            if (!Directory.Exists(serverFolderPath))
                Directory.CreateDirectory(serverFolderPath);

            string clientFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, clientFolder);
            if (!Directory.Exists(clientFolderPath))
                Directory.CreateDirectory(clientFolderPath);

            string srcFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, srcFolder);
            if (!Directory.Exists(clientFolderPath))
                Directory.CreateDirectory(clientFolderPath);

            //FileInfo srcFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, srcfileName));

            DirectoryInfo di = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, srcFolder));
            foreach (FileInfo srcFile in di.GetFiles())
            {
                if (srcFile.Extension.ToLower().CompareTo(".xlsx") == 0)
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(srcFile))
                    {
                        //excelPackage.ConvertToCsv(Path.Combine(csvFolderPath, "test"));
                        var format = new ExcelOutputTextFormat();
                        format.Encoding = Encoding.UTF8;
                        format.Delimiter = '|';


                        for (int cnt = 0; cnt < excelPackage.Workbook.Worksheets.Count; cnt++)
                        {
                            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[cnt];

                            int totalColumn = worksheet.Dimension.End.Column;
                            for (int i = totalColumn; i > 0; i--)
                            {
                                if (string.IsNullOrEmpty(worksheet.Cells[1, i].Text))
                                {
                                    worksheet.DeleteColumn(i);
                                    continue;
                                }
                                string targetStr = worksheet.Cells[3, i].Text.ToLower();

                                if (targetStr.CompareTo("nodata") == 0)
                                    worksheet.DeleteColumn(i);

                                if (targetType == TargetType.Client && targetStr.CompareTo("server") == 0)
                                    worksheet.DeleteColumn(i);

                                if (targetType == TargetType.Server && targetStr.CompareTo("client") == 0)
                                    worksheet.DeleteColumn(i);

                            }
                            worksheet.DeleteRow(3);

                            int totalRow = worksheet.Dimension.End.Row;
                            for (int i = totalRow; i > 0; i--)
                            {
                                if (string.IsNullOrEmpty(worksheet.Cells[i, 1].Text))
                                {
                                    worksheet.DeleteRow(i);
                                }
                            }
                            //Path.Combine(csvFolderPath, worksheet.Name)

                            FileInfo dstFile = new FileInfo(Path.Combine(targetType == TargetType.Server ? serverFolderPath : clientFolderPath, $"{worksheet.Name}.csv"));
                            worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column].SaveToText(dstFile, format);
                        }
                    }
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExportToCsv exportClient = new ExportToCsv(ExportToCsv.TargetType.Client);
            }
            catch
            {
                Console.ReadLine();
            }
            //ExportToCsv exportServer = new ExportToCsv(ExportToCsv.TargetType.Server);
        }
    }
}