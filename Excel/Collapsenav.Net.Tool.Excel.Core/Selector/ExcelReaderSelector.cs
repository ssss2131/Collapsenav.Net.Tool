using System.Collections.Concurrent;

namespace Collapsenav.Net.Tool.Excel;

public class ExcelReaderSelector
{
    private static ConcurrentDictionary<string, Func<Stream, IExcelReader>> StreamSelectorDict = null;
    private static ConcurrentDictionary<string, Func<object, IExcelReader>> ObjSelectorDict = null;

    public static void Add(ExcelType excelType, Func<object, IExcelReader> func)
    {
        ObjSelectorDict ??= new();
        ObjSelectorDict.AddOrUpdate(excelType.ToString(), func);
    }
    public static void Add(ExcelType excelType, Func<Stream, IExcelReader> func)
    {
        StreamSelectorDict ??= new();
        StreamSelectorDict.AddOrUpdate(excelType.ToString(), func);
    }
    public static void Add(string excelType, Func<object, IExcelReader> func)
    {
        ObjSelectorDict ??= new();
        ObjSelectorDict.AddOrUpdate(excelType, func);
    }
    public static void Add(string excelType, Func<Stream, IExcelReader> func)
    {
        StreamSelectorDict ??= new();
        StreamSelectorDict.AddOrUpdate(excelType, func);
    }

    public static IExcelReader GetExcelReader(object obj)
    {
        return GetExcelReader(obj, null);
    }
    public static IExcelReader GetExcelReader(object obj, string excelType)
    {
        if (ObjSelectorDict.IsEmpty())
            throw new NoRegisterExcelReaderException();
        excelType = ExcelTypeSelector.GetExcelType(obj, excelType);
        if (excelType.NotWhite() && !ObjSelectorDict.ContainsKey(excelType))
            throw new Exception($"未注册 {excelType} 的 IExcelReader 实现");
        else if (excelType.IsWhite())
            throw new NoRegisterExcelReaderException();
        return ObjSelectorDict[excelType](obj);
    }
    public static IExcelReader GetExcelReader(Stream stream, string excelType)
    {
        if (StreamSelectorDict.IsEmpty())
            throw new NoRegisterExcelReaderException();
        excelType = ExcelTypeSelector.GetExcelType(stream, excelType);
        if (excelType.NotWhite() && !StreamSelectorDict.ContainsKey(excelType))
            throw new Exception("未注册指定类型的 IExcelReader 实现");
        else if (excelType.IsWhite())
            throw new NoRegisterExcelReaderException();
        return StreamSelectorDict[excelType](stream);
    }
    public static IExcelReader GetExcelReader(Stream stream)
    {
        return GetExcelReader(stream, null);
    }
}