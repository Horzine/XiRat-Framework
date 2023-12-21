using TMPro;
using UnityEngine;
using Xi.Gameplay.Process;

namespace Xi.TestCase
{
    public class MyGameProcess : GameProcess
    {
        public bool IsAbleToStart { get; set; }
        public bool IsAbleToOver { get; set; }
        public bool IsAbleRestart { get; set; }
        protected override bool IsGameAbleToStartLogic() => IsAbleToStart;
        protected override bool IsGameAbleToOverLogic() => IsAbleToOver;
        protected override bool IsGameAbleToRestartLogic() => IsAbleRestart;
        protected override void OnGameStageChange(GameStage oldStage, GameStage newStage)
            => Debug.Log($"OldStage: {oldStage} ==> NewStage: {newStage}");
        protected override void OnStageStateChange(GameStage currentStage, StageState oldState, StageState newState)
            => Debug.Log($"GameStage: {currentStage}, OldState: {oldState} ==> NewState: {newState}");
        protected override bool HandleGameStage_Idle(StageState state) => true;
        protected override bool HandleGameStage_Start(StageState state) => true;
        protected override bool HandleGameStage_Decision(StageState state) => true;
        protected override bool HandleGameStage_Action(StageState state) => true;
        protected override bool HandleGameStage_Resolution(StageState state) => true;
        protected override bool HandleGameStage_Over(StageState state) => true;
        protected override bool HandleGameStage_Restart(StageState state) => true;
        protected override bool HandleGameStage_InitializeOnce(StageState state) => true;
        protected override bool HandleGameStage_RoundBegin(StageState state) => true;
        protected override bool HandleGameStage_PreDecision(StageState state) => true;
        protected override bool HandleGameStage_PostDecision(StageState state) => true;
        protected override bool HandleGameStage_PreAction(StageState state) => true;
        protected override bool HandleGameStage_PostAction(StageState state) => true;
        protected override bool HandleGameStage_PreResolution(StageState state) => true;
        protected override bool HandleGameStage_PostResolution(StageState state) => true;
        protected override bool HandleGameStage_RoundEnd(StageState state) => true;
        protected override bool IsGameAbleToNextRoundLogic() => true;
    }

    public class Test_GameProcess : MonoBehaviour
    {
        public TextMeshProUGUI currentStageTxt;
        private readonly MyGameProcess _myGameProcess = new();
        private int _lastTimeSecond;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _myGameProcess.IsAbleToStart = true;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _myGameProcess.IsAbleToOver = true;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _myGameProcess.IsAbleRestart = true;
            }

            if (_lastTimeSecond != (int)Time.time)
            {
                Debug.Log($"---------- {_myGameProcess.GetCurrentGameStage()}, {_myGameProcess.GetCurrentStageState()} -------------");
                _myGameProcess.OnUpdate();
                _lastTimeSecond = (int)Time.time;
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
