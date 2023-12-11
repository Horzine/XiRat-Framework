namespace Xi.Gameplay
{
    public enum GameStage
    {
        Game_Idle,
        Game_Start,
        Decision,
        Action,
        Resolution,
        Game_Over,
        Game_Restart,
    }

    public enum StageState
    {
        Before,
        In_Progress,
        After,
    }

    public abstract class GameProcess
    {
        public (GameStage stage, StageState state) CurrentGameStatus
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

        public void OnUpdate() => UpdateStageAndState();

        private bool HasNextStageState(StageState state) => state < StageState.After;

        protected void UpdateStageAndState()
        {
            bool willGoNext = HandleStageAndState(_currentGameStatus);
            if (!willGoNext)
            {
                return;
            }

            if (HasNextStageState(CurrentGameStatus.state))
            {
                CurrentGameStatus = (CurrentGameStatus.stage, CurrentGameStatus.state + 1);
                return;
            }

            SwitchToNextStage();
        }

        private void SwitchToNextStage()
        {
            switch (CurrentGameStatus.stage)
            {
                case GameStage.Game_Idle:
                    if (IsGameAbleToStart())
                    {
                        CurrentGameStatus = (CurrentGameStatus.stage + 1, StageState.Before);
                    }
                    break;

                case GameStage.Game_Start:
                case GameStage.Decision:
                case GameStage.Action:
                    CurrentGameStatus = (CurrentGameStatus.stage + 1, StageState.Before);
                    break;

                case GameStage.Resolution:
                    CurrentGameStatus = IsGameAbleToOver()
                        ? (CurrentGameStatus.stage + 1, StageState.Before)
                        : (GameStage.Decision, StageState.Before);
                    break;

                case GameStage.Game_Over:
                    CurrentGameStatus = IsGameAbleRestart() ? (GameStage.Game_Restart, StageState.Before) : (GameStage.Game_Over, StageState.After);
                    break;

                case GameStage.Game_Restart:
                    CurrentGameStatus = (GameStage.Game_Idle, StageState.Before);
                    break;

                default:
                    break;
            }
        }

        protected abstract void OnGameStageChange(GameStage oldStage, GameStage newStage);

        protected abstract void OnStageStateChange(GameStage currentStage, StageState oldState, StageState newState);

        protected abstract bool HandleStageAndState((GameStage stage, StageState state) currentGameStatus);

        protected abstract bool IsGameAbleToStart();

        protected abstract bool IsGameAbleToOver();

        protected abstract bool IsGameAbleRestart();

        public void RestartGame()
        {
            CurrentGameStatus = (GameStage.Game_Idle, StageState.Before);
        }
    }
}