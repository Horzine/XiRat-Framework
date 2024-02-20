using System;

namespace Xi.Config
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigDataTypeAttribute : Attribute
    {
    }
    public interface IConfigData
    {
        internal string Key { get; }
    }
}