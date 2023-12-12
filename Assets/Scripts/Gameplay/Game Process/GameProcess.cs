namespace Xi.Gameplay
{
    public enum GameStage
    {
        Game_Idle,
        Game_Start,
        Game_Decision,
        Game_Action,
        Game_Resolution,
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
        protected bool IsGameAbleToStartValue { get; set; }
        protected bool IsGameAbleToOverValue { get; set; }
        protected bool IsGameAbleToRestartValue { get; set; }
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

        private void UpdateStageAndState()
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
                    if (IsGameAbleToStartValue && IsGameAbleToStartLogic())
                    {
                        CurrentGameStatus = (GameStage.Game_Start, StageState.Before);
                    }

                    break;

                case GameStage.Game_Start:
                case GameStage.Game_Decision:
                case GameStage.Game_Action:
                    CurrentGameStatus = (CurrentGameStatus.stage + 1, StageState.Before);
                    break;

                case GameStage.Game_Resolution:
                    CurrentGameStatus = IsGameAbleToOverValue && IsGameAbleToOverLogic()
                        ? (GameStage.Game_Over, StageState.Before)
                        : (GameStage.Game_Decision, StageState.Before);
                    break;

                case GameStage.Game_Over:
                    CurrentGameStatus = IsGameAbleToRestartValue && IsGameAbleToRestartLogic()
                        ? (GameStage.Game_Restart, StageState.Before)
                        : (GameStage.Game_Over, StageState.After);
                    break;

                case GameStage.Game_Restart:
                    CurrentGameStatus = (GameStage.Game_Idle, StageState.Before);
                    break;

                default:
                    break;
            }
        }

        private bool HandleStageAndState((GameStage stage, StageState state) currentGameStatus)
        {
            return currentGameStatus.stage switch
            {
                GameStage.Game_Idle => HandleGameStage_Idle(currentGameStatus.state),
                GameStage.Game_Start => HandleGameStage_Start(currentGameStatus.state),
                GameStage.Game_Decision => HandleGameStage_Decision(currentGameStatus.state),
                GameStage.Game_Action => HandleGameStage_Action(currentGameStatus.state),
                GameStage.Game_Resolution => HandleGameStage_Resolution(currentGameStatus.state),
                GameStage.Game_Over => HandleGameStage_Over(currentGameStatus.state),
                GameStage.Game_Restart => HandleGameStage_Restart(currentGameStatus.state),
                _ => true,
            };
        }

        public void OnUpdate() => UpdateStageAndState();

        protected abstract void OnGameStageChange(GameStage oldStage, GameStage newStage);
        protected abstract void OnStageStateChange(GameStage currentStage, StageState oldState, StageState newState);
        protected abstract bool IsGameAbleToStartLogic();
        protected abstract bool IsGameAbleToOverLogic();
        protected abstract bool IsGameAbleToRestartLogic();
        protected abstract bool HandleGameStage_Idle(StageState state);
        protected abstract bool HandleGameStage_Start(StageState state);
        protected abstract bool HandleGameStage_Decision(StageState state);
        protected abstract bool HandleGameStage_Action(StageState state);
        protected abstract bool HandleGameStage_Resolution(StageState state);
        protected abstract bool HandleGameStage_Over(StageState state);
        protected abstract bool HandleGameStage_Restart(StageState state);

        public GameStage GetCurrentGameStage() => CurrentGameStatus.stage;
        public StageState GetCurrentStageState() => CurrentGameStatus.state;
    }
}