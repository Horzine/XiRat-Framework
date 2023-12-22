using Cysharp.Threading.Tasks;

namespace Xi.Gameplay.Process
{
    public enum GameStage
    {
        Game_Idle,// Staying or go Game_Start
        Game_Start,
        Game_Initialize,
        Game_RoundBegin,
        Game_PreDecision,
        Game_OnDecision,
        Game_PostDecision,
        Game_PreAction,
        Game_OnAction,
        Game_PostAction,
        Game_PreResolution,
        Game_OnResolution,
        Game_PostResolution,
        Game_RoundEnd,// go Game_RoundBegin or Game_Over
        Game_Over,// Staying or go Game_Restart
        Game_Restart,// go Game_Start
    }

    public enum StageState
    {
        Before,
        Progressing,
        After,
    }

    public abstract class GameProcess
    {
        public bool IsAbleToStart { get; set; }
        public bool IsAbleToOver { get; set; }
        public bool IsAbleToRestart { get; set; }
        public bool IsAbleToNextRound { get; set; }
        protected (GameStage stage, StageState state) CurrentGameStatus
        {
            get => _currentGameStatus;
            private set
            {
                var oldStage = _currentGameStatus.stage;
                var oldState = _currentGameStatus.state;
                _currentGameStatus = value;
                if (value.stage != oldStage)
                {
                    OnGameStageChange(oldStage, value.stage);
                }
                else if (value.state != oldState)
                {
                    OnStageStateChange(value.stage, oldState, value.state);
                }
            }
        }

        private (GameStage stage, StageState state) _currentGameStatus = (GameStage.Game_Idle, StageState.Before);

        private bool HasNextStageState(StageState state) => state < StageState.After;

        private async UniTask<bool> ShouldGoToNextStatus() => await HandleStageAndState(CurrentGameStatus);

        private async UniTask UpdateStageAndState()
        {
            if (!await ShouldGoToNextStatus())
            {
                return;
            }

            if (HasNextStageState(CurrentGameStatus.state))
            {
                CurrentGameStatus = (CurrentGameStatus.stage, CurrentGameStatus.state + 1);
            }
            else
            {
                SwitchToNextStage();
            }
        }

        private void SwitchToNextStage()
        {
            var nextStage = GetNextStage(CurrentGameStatus.stage);
            if (CurrentGameStatus.stage != nextStage)
            {
                CurrentGameStatus = (nextStage, StageState.Before);
            }

            CleanAllMutex();
        }

        private void CleanAllMutex()
        {
            IsAbleToStart = false;
            IsAbleToOver = false;
            IsAbleToRestart = false;
            IsAbleToNextRound = false;
        }

        private GameStage GetNextStage(GameStage currentStage)
        {
            return currentStage switch
            {
                GameStage.Game_Idle => IsGameAbleToStartLogic() ? GameStage.Game_Start : currentStage,
                GameStage.Game_RoundEnd => IsGameAbleToOverLogic() ? GameStage.Game_Over : IsGameAbleToNextRoundLogic() ? GameStage.Game_RoundBegin : currentStage,
                GameStage.Game_Over => IsGameAbleToRestartLogic() ? GameStage.Game_Restart : currentStage,
                GameStage.Game_Restart => GameStage.Game_Start,
                _ => currentStage + 1,
            };
        }

        private void JumpToGame_Start() => CurrentGameStatus = (GameStage.Game_Start, StageState.Before);

        private void JumpToGame_Over() => CurrentGameStatus = (GameStage.Game_Over, StageState.Before);

        private void JumpToGame_Restart() => CurrentGameStatus = (GameStage.Game_Restart, StageState.Before);

        private async UniTask<bool> HandleStageAndState((GameStage stage, StageState state) currentGameStatus)
        {
            return currentGameStatus.stage switch
            {
                GameStage.Game_Idle => await HandleGameStage_Idle(currentGameStatus.state),
                GameStage.Game_Start => await HandleGameStage_Start(currentGameStatus.state),
                GameStage.Game_Initialize => await HandleGameStage_Initialize(currentGameStatus.state),
                GameStage.Game_RoundBegin => await HandleGameStage_RoundBegin(currentGameStatus.state),
                GameStage.Game_PreDecision => await HandleGameStage_PreDecision(currentGameStatus.state),
                GameStage.Game_OnDecision => await HandleGameStage_OnDecision(currentGameStatus.state),
                GameStage.Game_PostDecision => await HandleGameStage_PostDecision(currentGameStatus.state),
                GameStage.Game_PreAction => await HandleGameStage_PreAction(currentGameStatus.state),
                GameStage.Game_OnAction => await HandleGameStage_OnAction(currentGameStatus.state),
                GameStage.Game_PostAction => await HandleGameStage_PostAction(currentGameStatus.state),
                GameStage.Game_PreResolution => await HandleGameStage_PreResolution(currentGameStatus.state),
                GameStage.Game_OnResolution => await HandleGameStage_OnResolution(currentGameStatus.state),
                GameStage.Game_PostResolution => await HandleGameStage_PostResolution(currentGameStatus.state),
                GameStage.Game_RoundEnd => await HandleGameStage_RoundEnd(currentGameStatus.state),
                GameStage.Game_Over => await HandleGameStage_Over(currentGameStatus.state),
                GameStage.Game_Restart => await HandleGameStage_Restart(currentGameStatus.state),
                _ => true,
            };
        }

        protected virtual UniTask<bool> HandleGameStage_Idle(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_Start(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_Initialize(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_RoundBegin(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_PreDecision(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_OnDecision(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_PostDecision(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_PreAction(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_OnAction(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_PostAction(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_PreResolution(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_OnResolution(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_PostResolution(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_RoundEnd(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_Over(StageState state) => UniTask.FromResult(true);
        protected virtual UniTask<bool> HandleGameStage_Restart(StageState state) => UniTask.FromResult(true);

        protected abstract void OnGameStageChange(GameStage oldStage, GameStage newStage);
        protected abstract void OnStageStateChange(GameStage currentStage, StageState oldState, StageState newState);
        protected virtual bool IsGameAbleToStartLogic() => IsAbleToStart;
        protected virtual bool IsGameAbleToOverLogic() => IsAbleToOver;
        protected virtual bool IsGameAbleToRestartLogic() => IsAbleToRestart;
        protected virtual bool IsGameAbleToNextRoundLogic() => IsAbleToNextRound;

        public GameStage GetCurrentGameStage() => CurrentGameStatus.stage;
        public StageState GetCurrentStageState() => CurrentGameStatus.state;
        public async UniTask OnUpdate() => await UpdateStageAndState();
        public void StartGame()
        {
            if (CurrentGameStatus == (GameStage.Game_Idle, StageState.After))
            {
                JumpToGame_Start();
            }
        }
        public void RestartGame(bool force)
        {
            if (force || CurrentGameStatus == (GameStage.Game_Over, StageState.After))
            {
                JumpToGame_Restart();
            }
        }
        public void OverGame(bool force)
        {
            if (force || CurrentGameStatus == (GameStage.Game_OnResolution, StageState.After))
            {
                JumpToGame_Over();
            }
        }
    }
}
