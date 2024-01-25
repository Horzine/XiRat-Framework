//
// This code is Generated. Do not modify !
//
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Xi.Config
{
    [ConfigDataType]
    public class Template : IConfigData
    {
        string IConfigData.Key => Key;
        public string Key { get; private set; }
        public int Type { get; private set; }
        public bool Value { get; private set; }
        public List<string> Description { get; private set; }
        public JObject Json { get; private set; }
    }

    public partial class ConfigCollection
    {
        public Dictionary<string, Template> AllTemplate => _template;
        private Dictionary<string, Template> _template;
    }
}
