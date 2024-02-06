using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;
using Xi.Metagame.Ui;

namespace Xi.TestCase
{
    public class Test_UiMgr : MonoBehaviour
    {
        private ClassBuildWindowController _contrller;

        private async void Start()
        {
            await GameSceneManager.Instance.InitAsync();
            await AssetManager.Instance.InitAsync(GameSceneManager.Instance);
            await UiManager.Instance.InitAsync(Bootstrap.GetTypesFromAssembly(), AssetManager.Instance);

            _contrller = UiManager.Instance.GetController<ClassBuildWindowController>(UiEnum.Metagame_ClassBuild);
            print("=======================");
            await _contrller.OpenAsync(new ClassBuildWindowController.InitParams
            {

            });
            print("=======================");
            await UniTask.Delay(TimeSpan.FromSeconds(5));
            print("=======================");
            await _contrller.CloseAsync();
            print("=======================");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UiManager.Instance.ForceReleaseAllWindow();
                print($" {_contrller.CurrentWindowState} ");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                print($" {_contrller.CurrentWindowState} ");
            }
        }
    }
}
