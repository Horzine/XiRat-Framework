using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;
using Xi.Tools;

namespace Xi.Gameplay
{
    public class Bootstrap : MonoBehaviour
    {
        private AdvancedLoggerTool _logger;
        private void Awake() => _logger = new AdvancedLoggerTool();

        private void Start()
        {
            GameSceneManager.Instance.LoadActiveSceneAsync(SceneNameConst.kMainScene, true).Forget();
        }
    }
}
