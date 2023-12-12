using UnityEngine;
using Xi.Gameplay;

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
    }

    public class Test_GameProcess : MonoBehaviour
    {
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
        }
    }
}
