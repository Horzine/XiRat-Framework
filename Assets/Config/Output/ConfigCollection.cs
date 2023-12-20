//
// This code is Generated. Do not modify !
//
using System.Collections.Generic;
using System.IO;

namespace Xi.Config
{
    public class ConfigCollection
    {
        private static readonly string kLoadFloder = ConfigUtils.kRuntimeLoadPath;
        public Dictionary<string, Template> AllTemplate { get; } = new();
        public Dictionary<string, Unit> AllUnit { get; } = new();

        public void Init()
        {
            ConfigUtils.ParseConfigData(ConfigUtils.DeserializeFromFile(Path.Combine(kLoadFloder, "Template"), ConfigUtils.kKey), AllTemplate);
            ConfigUtils.ParseConfigData(ConfigUtils.DeserializeFromFile(Path.Combine(kLoadFloder, "Unit"), ConfigUtils.kKey), AllUnit);

        }
    }
}
