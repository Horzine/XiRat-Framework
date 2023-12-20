//
// This code is Generated. Do not modify !
//
using System.Collections.Generic;
using System.IO;

namespace Xi.Config
{
    public class ConfigCollection
    {
        public Dictionary<string, Template> AllTemplate { get; } = new();
        public Dictionary<string, Unit> AllUnit { get; } = new();

        public void Init()
        {
#if UNITY_EDITOR
            ConfigUtils.ParseConfigData(File.ReadAllLines(Path.Combine(ConfigUtils.kTxtOriginFolder, "Template.txt")), AllTemplate);
            ConfigUtils.ParseConfigData(File.ReadAllLines(Path.Combine(ConfigUtils.kTxtOriginFolder, "Unit.txt")), AllUnit);
#else
            ConfigUtils.ParseConfigData(ConfigUtils.DeserializeFromFile(Path.Combine(ConfigUtils.kRuntimeLoadPath, "Template"), ConfigUtils.kKey), AllTemplate);
            ConfigUtils.ParseConfigData(ConfigUtils.DeserializeFromFile(Path.Combine(ConfigUtils.kRuntimeLoadPath, "Unit"), ConfigUtils.kKey), AllUnit);
#endif

        }
    }
}
