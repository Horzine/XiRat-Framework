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

            switch (CurrentGameStatus.stage)
            {
                case GameStage.Game_Idle:
                    if (IsGameAbleToStart())
                    {
                        CurrentGameStatus = (CurrentGameStatus.stage + 1, StageState.Before);
                    }
                    break;
                case GameStage.Game_Start:
                    CurrentGameStatus = (CurrentGameStatus.stage + 1, StageState.Before);
                    break;
                case GameStage.Decision:
                    CurrentGameStatus = (CurrentGameStatus.stage + 1, StageState.Before);
                    break;
                case GameStage.Action:
                    CurrentGameStatus = (CurrentGameStatus.stage + 1, StageState.Before);
                    break;
                case GameStage.Resolution:
                    CurrentGameStatus = IsGameAbleToOver() ? (CurrentGameStatus.stage + 1, StageState.Before) : (GameStage.Decision, StageState.Before);
                    break;
                case GameStage.Game_Over:
                    break;
                default:
                    break;
            }
        }

        protected abstract void OnGameStageChange(GameStage oldStage, GameStage newStage);// 在阶段改变时的逻辑处理，可以根据需要进行实现

        protected abstract void OnStageStateChange(GameStage currentStage, StageState oldState, StageState newState);// 在状态改变时的逻辑处理，可以根据需要进行实现

        protected abstract bool HandleStageAndState((GameStage stage, StageState state) currentGameStatus);

        protected abstract bool IsGameAbleToStart();

        protected abstract bool IsGameAbleToOver();

    }
}
