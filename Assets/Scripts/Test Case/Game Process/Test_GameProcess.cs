using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Xi.Gameplay.Process;

namespace Xi.TestCase
{
    public class MyGameProcess : GameProcess
    {
        protected override void OnGameStageChange(GameStage oldStage, GameStage newStage) => Debug.Log($"OldStage: {oldStage} ==> NewStage: {newStage}");
        protected override void OnStageStateChange(GameStage currentStage, StageState oldState, StageState newState) { }
        //=> Debug.Log($"GameStage: {currentStage}, OldState: {oldState} ==> NewState: {newState}");

        protected override async UniTask<bool> HandleGameStage_Idle(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_Start(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_OnDecision(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_OnAction(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_OnResolution(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_Over(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_Restart(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_Initialize(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_RoundBegin(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_PreDecision(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_PostDecision(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_PreAction(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_PostAction(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_PreResolution(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_PostResolution(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }
        protected override async UniTask<bool> HandleGameStage_RoundEnd(StageState state)
        {
            await UniTask.Delay(300);
            return true;
        }

        protected override void OnGameStatusStaying(GameStage stage, StageState state) => Debug.Log($"{nameof(OnGameStatusStaying)}: {stage}, {state}");
    }

    public class Test_GameProcess : MonoBehaviour
    {
        public TextMeshProUGUI currentStageTxt;
        private readonly MyGameProcess _myGameProcess = new();
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
