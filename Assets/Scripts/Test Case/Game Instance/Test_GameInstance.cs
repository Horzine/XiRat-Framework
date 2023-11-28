using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;
using Xi.Gameplay;
using Xi.Metagame;

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
                var metagameInstance = GameMain.Instance.GetMetagameGameInstance();
                var gameplayInstance = GameMain.Instance.GetGameplayGameInstance();
                print($"{metagameInstance}, {gameplayInstance}");
            }
        }
    }
}
