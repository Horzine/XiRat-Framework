using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Xi.TestCase
{
    public class MyGameProcessConfig
    {
        public int totalRoundCount = 3;
        public int maxPlayerHp = 100;
        public int playerAtk = 20;
        public int maxEnemyHp = 100;
        public int enemyAtk = 20;
    }

    public class Test_GameProcess : MonoBehaviour
    {
        public TextMeshProUGUI currentStageTxt;
        private readonly Test_RockPaperScissorsGame _myGameProcess = new();
        private CancellationToken _cancellationToken;

        private void Start()
        {
            _cancellationToken = this.GetCancellationTokenOnDestroy();
            RunUpdateLoop().Forget();
        }

        private async UniTaskVoid RunUpdateLoop()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                //await UniTask.SwitchToThreadPool();
                await _myGameProcess.OnUpdate();
                //await UniTask.SwitchToMainThread();
                await UniTask.Yield();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _myGameProcess.IsAbleToStart = true;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _myGameProcess.IsAbleToNextRound = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _myGameProcess.IsAbleToOver = true;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _myGameProcess.IsAbleToRestart = true;
            }

            currentStageTxt.text = $"{_myGameProcess.GetCurrentGameStage()}, {_myGameProcess.GetCurrentStageState()}";
        }

        public void StartGame() => _myGameProcess.StartGame();

        public void OverGameForce() => _myGameProcess.OverGame(true);

        public void OverGame() => _myGameProcess.OverGame(false);

        public void RestartGameForce() => _myGameProcess.RestartGame(true);

        public void RestartGame() => _myGameProcess.RestartGame(false);
    }
}
