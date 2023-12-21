namespace Xi.Gameplay.Process
{
    public enum GameStage
    {
        Game_Idle,
        Game_Start,
        Game_InitializeOnce,
        Game_RoundBegin,
        Game_PreDecision,
        Game_Decision,
        Game_PostDecision,
        Game_PreAction,
        Game_Action,
        Game_PostAction,
        Game_PreResolution,
        Game_Resolution,
        Game_PostResolution,
        Game_RoundEnd,
        Game_Over,
        Game_Restart,
    }

    public enum StageState
    {
        Before,
        Progressing,
        After,
    }

    public abstract class GameProcess
    {
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

        private bool ShouldGoToNextStageOrState() => HandleStageAndState(CurrentGameStatus);

        private void UpdateStageAndState()
        {
            if (!ShouldGoToNextStageOrState())
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
        }

        private GameStage GetNextStage(GameStage currentStage)
        {
            return currentStage switch
            {
                GameStage.Game_Idle => IsGameAbleToStartLogic() ? GameStage.Game_Start : currentStage,
                GameStage.Game_RoundEnd => IsGameAbleToOverLogic() ? GameStage.Game_Over :
                                           IsGameAbleToNextRoundLogic() ? GameStage.Game_RoundBegin : currentStage,
                GameStage.Game_Over => IsGameAbleToRestartLogic() ? GameStage.Game_Restart : currentStage,
                GameStage.Game_Restart => GameStage.Game_Start,
                _ => currentStage + 1,
            };
        }

        private void JumpToGame_Start() => CurrentGameStatus = (GameStage.Game_Start, StageState.Before);
        private void JumpToGame_Over() => CurrentGameStatus = (GameStage.Game_Over, StageState.Before);
        private void JumpToGame_Restart() => CurrentGameStatus = (GameStage.Game_Restart, StageState.Before);

        private bool HandleStageAndState((GameStage stage, StageState state) currentGameStatus)
        {
            return currentGameStatus.stage switch
            {
                GameStage.Game_Idle => HandleGameStage_Idle(currentGameStatus.state),
                GameStage.Game_Start => HandleGameStage_Start(currentGameStatus.state),
                GameStage.Game_InitializeOnce => HandleGameStage_InitializeOnce(currentGameStatus.state),
                GameStage.Game_RoundBegin => HandleGameStage_RoundBegin(currentGameStatus.state),
                GameStage.Game_PreDecision => HandleGameStage_PreDecision(currentGameStatus.state),
                GameStage.Game_Decision => HandleGameStage_Decision(currentGameStatus.state),
                GameStage.Game_PostDecision => HandleGameStage_PostDecision(currentGameStatus.state),
                GameStage.Game_PreAction => HandleGameStage_PreAction(currentGameStatus.state),
                GameStage.Game_Action => HandleGameStage_Action(currentGameStatus.state),
                GameStage.Game_PostAction => HandleGameStage_PostAction(currentGameStatus.state),
                GameStage.Game_PreResolution => HandleGameStage_PreResolution(currentGameStatus.state),
                GameStage.Game_Resolution => HandleGameStage_Resolution(currentGameStatus.state),
                GameStage.Game_PostResolution => HandleGameStage_PostResolution(currentGameStatus.state),
                GameStage.Game_RoundEnd => HandleGameStage_RoundEnd(currentGameStatus.state),
                GameStage.Game_Over => HandleGameStage_Over(currentGameStatus.state),
                GameStage.Game_Restart => HandleGameStage_Restart(currentGameStatus.state),
                _ => true,
            };
        }

        protected abstract bool HandleGameStage_Idle(StageState state);
        protected abstract bool HandleGameStage_Start(StageState state);
        protected abstract bool HandleGameStage_InitializeOnce(StageState state);
        protected abstract bool HandleGameStage_RoundBegin(StageState state);
        protected abstract bool HandleGameStage_PreDecision(StageState state);
        protected abstract bool HandleGameStage_Decision(StageState state);
        protected abstract bool HandleGameStage_PostDecision(StageState state);
        protected abstract bool HandleGameStage_PreAction(StageState state);
        protected abstract bool HandleGameStage_Action(StageState state);
        protected abstract bool HandleGameStage_PostAction(StageState state);
        protected abstract bool HandleGameStage_PreResolution(StageState state);
        protected abstract bool HandleGameStage_Resolution(StageState state);
        protected abstract bool HandleGameStage_PostResolution(StageState state);
        protected abstract bool HandleGameStage_RoundEnd(StageState state);
        protected abstract bool HandleGameStage_Over(StageState state);
        protected abstract bool HandleGameStage_Restart(StageState state);

        protected abstract void OnGameStageChange(GameStage oldStage, GameStage newStage);
        protected abstract void OnStageStateChange(GameStage currentStage, StageState oldState, StageState newState);
        protected abstract bool IsGameAbleToStartLogic();
        protected abstract bool IsGameAbleToOverLogic();
        protected abstract bool IsGameAbleToRestartLogic();
        protected abstract bool IsGameAbleToNextRoundLogic();

        public GameStage GetCurrentGameStage() => CurrentGameStatus.stage;

        public StageState GetCurrentStageState() => CurrentGameStatus.state;

        public void OnUpdate() => UpdateStageAndState();

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
            if (force || CurrentGameStatus == (GameStage.Game_RoundEnd, StageState.After))
            {
                JumpToGame_Over();
            }
        }
    }
}