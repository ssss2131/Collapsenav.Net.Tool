using NPOI.SS.UserModel;

namespace Collapsenav.Net.Tool.Excel;
public partial class ReadConfig<T>
{
    /// <summary>
    /// 将表格数据转换为指定的数据实体-NPOI
    /// </summary>
    public async Task<IEnumerable<T>> NPOIExcelToEntityAsync()
    {
        if (ExcelStream == null)
            throw new Exception();
        return await NPOIExcelReadTool.ExcelToEntityAsync(ExcelStream, this);
    }
    /// <summary>
    /// 将表格数据转换为指定的数据实体-NPOI
    /// </summary>
    public async Task<IEnumerable<T>> NPOIExcelToEntityAsync(string filepath)
    {
        return await NPOIExcelReadTool.ExcelToEntityAsync(filepath, this);
    }
    /// <summary>
    /// 将表格数据转换为指定的数据实体-NPOI
    /// </summary>
    public async Task<IEnumerable<T>> NPOIExcelToEntityAsync(Stream stream)
    {
        return await NPOIExcelReadTool.ExcelToEntityAsync(stream, this);
    }
    /// <summary>
    /// 将表格数据转换为指定的数据实体-NPOI
    /// </summary>
    public async Task<IEnumerable<T>> NPOIExcelToEntityAsync(ISheet sheet)
    {
        return await NPOIExcelReadTool.ExcelToEntityAsync(sheet, this);
    }

}