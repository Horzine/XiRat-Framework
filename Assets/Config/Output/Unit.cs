//
// This code is Generated. Do not modify !
//
using System.Collections.Generic;

namespace XiConfig
{
    [ConfigDataType]
    public class Unit : IConfigData
    {
        string IConfigData.Key => Key;
        public string Key { get; private set; }
        public int Type { get; private set; }
        public bool Value { get; private set; }
        public List<string> Description { get; private set; }
    }
}
