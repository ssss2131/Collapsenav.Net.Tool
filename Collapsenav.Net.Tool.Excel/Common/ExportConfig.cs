using System.Data;

namespace Collapsenav.Net.Tool.Excel;
/// <summary>
/// 导出表格设置
/// </summary>
public partial class ExportConfig<T>
{
    /// <summary>
    /// 根据给出的表头筛选options
    /// </summary>
    public void FilterOptionByHeaders(IEnumerable<string> headers)
    {
        FieldOption = FilterOptionByHeaders(FieldOption, headers);
    }

    /// <summary>
    /// 根据给出的表头筛选options
    /// </summary>
    public static IEnumerable<ExportCellOption<T>> FilterOptionByHeaders(IEnumerable<ExportCellOption<T>> options, IEnumerable<string> headers)
    {
        return options.Where(item => headers.Any(head => head == item.ExcelField));
    }

    /// <summary>
    /// 设置需要导出的表格数据
    /// </summary>
    public virtual void SetExcelData(IEnumerable<T> data)
    {
        Data = data;
    }
    /// <summary>
    /// 获取数据
    /// </summary>
    public virtual IEnumerable<object[]> GetConvertData(IEnumerable<T> data)
    {
        return data?.Select(item => FieldOption.Select(option => option.Action(item)).ToArray());
    }
    /// <summary>
    /// 获取表头和数据
    /// </summary>
    public virtual IEnumerable<object[]> GetExportData(IEnumerable<T> data)
    {
        return ConvertHeader.Concat(GetConvertData(data));
    }

    /// <summary>
    /// 添加普通单元格设置
    /// </summary>
    public virtual ExportConfig<T> Add(ExportCellOption<T> option)
    {
        FieldOption = FieldOption.Append(option);
        return this;
    }
    /// <summary>
    /// check条件为True时添加普通单元格设置
    /// </summary>
    public virtual ExportConfig<T> AddIf(bool check, ExportCellOption<T> option)
    {
        if (check)
            return Add(option);
        return this;
    }
    /// <summary>
    /// 添加普通单元格设置
    /// </summary>
    /// <param name="field">表头列</param>
    /// <param name="propName">属性名称</param>
    public virtual ExportConfig<T> Add(string field, string propName)
    {
        return Add(field, item => item.GetValue(propName).ToString());
    }
    /// <summary>
    /// 添加普通单元格设置
    /// </summary>
    /// <param name="check">判断结果</param>
    /// <param name="field">表头列</param>
    /// <param name="propName">属性名称</param>
    public virtual ExportConfig<T> AddIf(bool check, string field, string propName)
    {
        return check ? Add(field, field) : this;
    }
    /// <summary>
    /// 添加普通单元格设置
    /// </summary>
    /// <param name="field">表头列</param>
    /// <param name="propName">属性名称</param>
    /// <param name="format">格式化(暂时只支持DateTime的tostring格式化字符串)</param>
    public virtual ExportConfig<T> Add(string field, string propName, string format)
    {
        var prop = typeof(T).GetProperty(propName);
        if (prop.PropertyType.Name == nameof(DateTime))
            return Add(field, item => ((DateTime)item.GetValue(propName)).ToString(format));
        return Add(field, propName);
    }
    /// <summary>
    /// 添加普通单元格设置
    /// </summary>
    /// <param name="check">判断结果</param>
    /// <param name="field">表头列</param>
    /// <param name="propName">属性名称</param>
    /// <param name="format">格式化(暂时只支持DateTime的tostring格式化字符串)</param>
    public virtual ExportConfig<T> AddIf(bool check, string field, string propName, string format)
    {
        return check ? Add(field, field, format) : this;
    }
    /// <summary>
    /// 添加普通单元格设置
    /// </summary>
    public ExportConfig<T> Add(string field, Func<T, object> action)
    {
        FieldOption = FieldOption.Append(new ExportCellOption<T>
        {
            ExcelField = field,
            Action = action
        });
        return this;
    }
    /// <summary>
    /// 添加普通单元格设置
    /// </summary>
    public ExportConfig<T> AddIf(bool check, string field, Func<T, object> action)
    {
        if (check)
        {
            FieldOption = FieldOption.Append(new ExportCellOption<T>
            {
                ExcelField = field,
                Action = action
            });
        }
        return this;
    }
}
