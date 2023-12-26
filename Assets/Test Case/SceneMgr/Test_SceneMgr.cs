using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_SceneMgr : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(this);

        private void OnGUI()
        {
            int i = 0;
            if (GuiButton("Test_1"))
            {
                Test_1().Forget();
            }

            if (GuiButton("Test_2"))
            {
                Test_2().Forget();
            }

            bool GuiButton(string text) => GUI.Button(new Rect(100, 10 + (60 * i++), 250, 50), text);
        }

        private async UniTaskVoid Test_1()
        {
            var op = await GameSceneManager.Instance.LoadActiveSceneAsync("Map_1");
            await UniTask.Delay(1000);
            print($"op active,");
            await op.Result.ActivateAsync();

            await UniTask.Delay(2000);

            print($"op:{op.IsValid()},");
            var op2 = await GameSceneManager.Instance.LoadActiveSceneAsync("Map_2");
            await UniTask.Delay(1000);
            print($"op:{op.IsValid()}, op2{op2.IsValid()}");
            print($"op2 active,");
            await op2.Result.ActivateAsync();
            print($"op:{op.IsValid()}, op2{op2.IsValid()}");

            await UniTask.Delay(2000);
            print($"op:{op.IsValid()}, op2{op2.IsValid()}");
        }

        private async UniTaskVoid Test_2()
        {
            var op = await GameSceneManager.Instance.LoadActiveSceneAsync("Map_1");
            await UniTask.Delay(1000);
            print($"op active,");
            await op.Result.ActivateAsync();

            await UniTask.Delay(2000);

            print($"op:{op.IsValid()},");
            print(string.Join(",", GameSceneManager.Instance.SubScenes.Keys));

            var op2 = await GameSceneManager.Instance.LoadSubSceneAsync("Map_2");
            await UniTask.Delay(1000);

            print($"op:{op.IsValid()}, op2{op2.IsValid()}");
            print($"op2 active,");
            await op2.Result.ActivateAsync();

            await UniTask.Delay(1000);
            print(string.Join(",", GameSceneManager.Instance.SubScenes.Keys));

            print("Set current Scene op2");
            GameSceneManager.Instance.SetActiveScene(op2);
            await UniTask.Delay(1000);
            print($"op:{op.IsValid()}, op2{op2.IsValid()}");
            print(string.Join(",", GameSceneManager.Instance.SubScenes.Keys));

            await UniTask.Delay(1000);
            print("Unload op1");
            await GameSceneManager.Instance.UnloadSubSceneAsync(op);
            print($"op:{op.IsValid()}, op2{op2.IsValid()}");

            print(string.Join(",", GameSceneManager.Instance.SubScenes.Keys));
            await UniTask.Delay(2000);
            var op3 = await GameSceneManager.Instance.LoadSubSceneAsync("Map_3");
            await op3.Result.ActivateAsync();

            print(string.Join(",", GameSceneManager.Instance.SubScenes.Keys));
            await UniTask.Delay(2000);
            print($"Op2:{op2.IsValid()}, Op3:{op3.IsValid()}");
            var op4 = await GameSceneManager.Instance.LoadActiveSceneAsync("Map_4");
            await op4.Result.ActivateAsync();
            print(string.Join(",", GameSceneManager.Instance.SubScenes.Keys));

            print($"Op2:{op2.IsValid()}, Op3:{op3.IsValid()}");
            print($"Op4:{op4.IsValid()}");
        }
    }
}
