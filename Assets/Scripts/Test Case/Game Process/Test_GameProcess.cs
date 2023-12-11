using UnityEngine;
using Xi.Gameplay;

namespace Xi.TestCase
{
    public class MyGameProcess : GameProcess
    {
        public bool IsAbleToStart { get; set; }
        public bool IsAbleToOver { get; set; }
        public bool IsAbleRestart { get; set; }
        protected override bool HandleStageAndState((GameStage stage, StageState state) currentGameStatus)
        {
            return currentGameStatus switch
            {
                _ => true,
            };
        }
        protected override bool IsGameAbleToStart() => IsAbleToStart;
        protected override bool IsGameAbleToOver() => IsAbleToOver;
        protected override bool IsGameAbleRestart() => IsAbleRestart;
        protected override void OnGameStageChange(GameStage oldStage, GameStage newStage)
            => Debug.Log($"OldStage: {oldStage} ==> NewStage: {newStage}");
        protected override void OnStageStateChange(GameStage currentStage, StageState oldState, StageState newState)
            => Debug.Log($"GameStage: {currentStage}, OldState: {oldState} ==> NewState: {newState}");
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
                Debug.Log($"---------- {_myGameProcess.CurrentGameStatus.stage}, {_myGameProcess.CurrentGameStatus.state} -------------");
                _myGameProcess.OnUpdate();
                _lastTimeSecond = (int)Time.time;
            }

        }
    }
}
