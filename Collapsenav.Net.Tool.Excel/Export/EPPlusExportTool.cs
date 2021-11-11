using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Collapsenav.Net.Tool.Excel
{
    /// <summary>
    /// 针对表格导出的一些操作(基于EPPlus)
    /// </summary>
    public class EPPlusExportTool
    {
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="option">导出配置</param>
        /// <param name="data">指定数据</param>
        /// <param name="exportType">导出类型</param>
        public static async Task<Stream> ExportAsync<T>(string filePath, ExportConfig<T> option, IEnumerable<T> data = null, ExportType exportType = ExportType.All)
        {
            using var fs = new FileStream(filePath, FileMode.Create);
            return await ExportAsync(fs, option, data, exportType);
        }
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="option">导出配置</param>
        /// <param name="data">指定数据</param>
        /// <param name="exportType">导出类型</param>
        public static async Task<Stream> ExportAsync<T>(Stream stream, ExportConfig<T> option, IEnumerable<T> data = null, ExportType exportType = ExportType.All)
        {
            using var pack = new ExcelPackage();
            ExcelWorksheet sheet = pack.Workbook.Worksheets.Add($@"sheet{pack.Workbook.Worksheets.Count}");
            await Task.Run(() =>
            {
                sheet.Cells[1, 1].LoadFromArrays(
                    exportType switch
                    {
                        ExportType.All => data == null ? option.ExportData : option.GetExportData(data),
                        ExportType.Header => option.ConvertHeader,
                        ExportType.Data => data == null ? option.ConvertData : option.GetConvertData(data),
                        _ => data == null ? option.ExportData : option.GetExportData(data)
                    }
                );
                pack.SaveAs(stream);
            });
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// 导出表头
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="option">导出配置</param>
        public static async Task<Stream> ExportHeaderAsync<T>(string filePath, ExportConfig<T> option)
        {
            using var fs = new FileStream(filePath, FileMode.Create);
            return await ExportHeaderAsync(fs, option);
        }
        /// <summary>
        /// 导出表头
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="option">导出配置</param>
        public static async Task<Stream> ExportHeaderAsync<T>(Stream stream, ExportConfig<T> option)
        {
            using var pack = new ExcelPackage();
            ExcelWorksheet sheet = pack.Workbook.Worksheets.Add($@"sheet{pack.Workbook.Worksheets.Count}");
            await Task.Run(() =>
            {
                sheet.Cells[1, 1].LoadFromArrays(option.ConvertHeader);
                pack.SaveAs(stream);
            });
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="data">指定数据</param>
        /// <param name="option">导出配置</param>
        public static async Task<Stream> ExportDataAsync<T>(string filePath, ExportConfig<T> option, IEnumerable<T> data = null)
        {
            using var fs = new FileStream(filePath, FileMode.Create);
            return await ExportDataAsync(fs, option, data);
        }
        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="data">指定数据</param>
        /// <param name="option">导出配置</param>
        public static async Task<Stream> ExportDataAsync<T>(Stream stream, ExportConfig<T> option, IEnumerable<T> data = null)
        {
            using var pack = new ExcelPackage();
            ExcelWorksheet sheet = pack.Workbook.Worksheets.Add($@"sheet{pack.Workbook.Worksheets.Count}");
            await Task.Run(() =>
            {
                sheet.Cells[1, 1].LoadFromArrays(data == null ? option.ConvertData : option.GetConvertData(data));
                pack.SaveAs(stream);
            });
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

    }
}