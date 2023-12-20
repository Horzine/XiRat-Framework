using Cysharp.Threading.Tasks;
using Xi.Config;

namespace Xi.Framework
{
    public class ConfigManager : MonoSingleton<ConfigManager>, ISingleton
    {
        public ConfigCollection AllConfig { get; private set; }
        void ISingleton.OnCreate() => AllConfig = new ConfigCollection();

        public async UniTask InitAsync()
        {
            AllConfig.Init();
            await UniTask.Yield();
        }
    }
}
