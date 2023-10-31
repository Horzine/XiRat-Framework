//
// This code is Generated. Do not modify !
//
using System.Collections.Generic;
using System.IO;

namespace Xi.Config
{
    public class ConfigCollection
    {
        private const string kLoadFloder = ConfigUtils.kRuntimLoadFolder;
        public Dictionary<string, Template> AllTemplate { get; } = new();
        public Dictionary<string, Unit> AllUnit { get; } = new();

        public void Init()
        {
            ConfigUtils.ParseConfigData(File.ReadAllLines(Path.Combine(kLoadFloder, "Template.cfg")), AllTemplate);
            ConfigUtils.ParseConfigData(File.ReadAllLines(Path.Combine(kLoadFloder, "Unit.cfg")), AllUnit);

        }
    }
}
