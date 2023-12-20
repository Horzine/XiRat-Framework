using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;
using Xi.Gameplay.Main;
using Xi.Metagame.Main;

namespace Xi.TestCase
{
    public class Test_GameInstance : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
            Bootstrap.InitAllManager().Forget();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameMain.Instance.ChangeSceneToMetagameScene().Forget();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                GameMain.Instance.ChangeSceneToGameplayScene(SceneNameConst.kMap_1).Forget();
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                var metagameInstance = GameMain.Instance.MetagameInstance();
                var gameplayInstance = GameMain.Instance.GameplayInstance();
                print($"{metagameInstance}, {gameplayInstance}");
            }
        }
    }
}
