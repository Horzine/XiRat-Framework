using System;

namespace XiConfig
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigDataTypeAttribute : Attribute
    {
    }
    public interface IConfigData
    {
        string Key { get; }
    }
}