﻿using Cysharp.Threading.Tasks;
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
            Bootstrap.InitAllManagerAsync().Forget();
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
                var metagameInstance = GameMain.Instance.GetMetagameInstance();
                var gameplayInstance = GameMain.Instance.GetGameplayInstance();
                print($"{metagameInstance}, {gameplayInstance}");
            }
        }
    }
}
