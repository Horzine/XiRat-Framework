//
// This code is Generated. Do not modify !
//

namespace Xi.Config
{
    public partial class ConfigCollection
    {
        public void Init()
        {
            ConfigUtils.LoadAndParseConfigDictionary("Template", ref _template);
            ConfigUtils.LoadAndParseConfigDictionary("Unit", ref _unit);

        }
    }
}
