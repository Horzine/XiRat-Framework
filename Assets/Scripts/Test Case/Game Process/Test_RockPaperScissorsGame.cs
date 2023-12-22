using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Gameplay.Process;

namespace Xi.TestCase
{
    public class Test_RockPaperScissorsGame : GameProcess
    {
        private const int kMaxRound = 3;
        private int _playerChoice;
        private int _computerChoice;
        private int _currentRound;
        private List<int> _results = new();
        private bool _skipResultDisplay;

        // Define choices for Rock, Paper, Scissors
        private enum Choice
        {
            Rock,
            Paper,
            Scissors
        }

        protected override UniTask<bool> HandleGameStage_Idle(StageState state)
        {
            switch (state)
            {
                case StageState.Before:
                    Debug.Log("Welcome to Rock-Paper-Scissors Game!");
                    break;
                case StageState.Progressing:
                case StageState.After:
                    break;
            }

            return UniTask.FromResult(true);
        }

        protected override UniTask<bool> HandleGameStage_Initialize(StageState state)
        {
            switch (state)
            {
                case StageState.Before:
                    _currentRound = 0;
                    break;
                case StageState.Progressing:
                case StageState.After:
                    break;
            }

            return UniTask.FromResult(true);
        }

        protected override UniTask<bool> HandleGameStage_RoundBegin(StageState state)
        {
            switch (state)
            {
                case StageState.Before:
                    Debug.Log("Round starting...");
                    _playerChoice = -1;
                    _currentRound++;
                    Debug.Log("Choose your move: (0) Rock, (1) Paper, (2) Scissors");
                    break;
                case StageState.Progressing:
                    // Let the player make a choice (0 for Rock, 1 for Paper, 2 for Scissors)
                    //Debug.Log("Choose your move: (0) Rock, (1) Paper, (2) Scissors");
                    if (Input.GetKeyDown(KeyCode.Alpha0))
                    {
                        _playerChoice = 0;
                        break;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        _playerChoice = 1;
                        break;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        _playerChoice = 2;
                        break;
                    }

                    return UniTask.FromResult(false);
                case StageState.After:
                    break;
            }

            return UniTask.FromResult(true);
        }

        protected override UniTask<bool> HandleGameStage_OnAction(StageState state)
        {
            switch (state)
            {
                case StageState.Before:
                    // Computer makes a random choice
                    _computerChoice = Random.Range(0, 3);
                    Debug.Log($"Computer chose: {(Choice)_computerChoice}");
                    break;
                case StageState.Progressing:
                case StageState.After:
                    break;

            }

            return UniTask.FromResult(true);
        }

        protected override UniTask<bool> HandleGameStage_OnResolution(StageState state)
        {
            switch (state)
            {
                case StageState.Before:
                    // Determine the winner
                    DetermineWinner();
                    break;
                case StageState.Progressing:
                case StageState.After:
                    break;
            }

            return UniTask.FromResult(true);
        }

        protected override async UniTask<bool> HandleGameStage_RoundEnd(StageState state)
        {
            switch (state)
            {
                case StageState.Before:
                    // display result 
                    Debug.Log($"Round {_currentRound}: Player: {_playerChoice}, AI: {_computerChoice}");
                    break;
                case StageState.Progressing:
                    await UniTask.WhenAny(UniTask.Delay(5000), UniTask.WaitUntil(() => _skipResultDisplay));
                    break;
                case StageState.After:
                    _playerChoice = _computerChoice = -1;
                    break;
            }

            return true;
        }

        protected override UniTask<bool> HandleGameStage_Over(StageState state)
        {
            switch (state)
            {
                case StageState.Before:
                    Debug.Log($"result: {string.Join(", ", _results)}");
                    break;
                case StageState.Progressing:
                case StageState.After:
                    break;
            }

            return UniTask.FromResult(true);
        }

        protected override UniTask<bool> HandleGameStage_Restart(StageState state)
        {
            switch (state)
            {
                case StageState.Before:
                    _results.Clear();
                    break;
                case StageState.Progressing:
                case StageState.After:
                    break;
            }

            return UniTask.FromResult(true);
        }

        protected override bool IsGameAbleToNextRoundLogic() => _currentRound < kMaxRound;

        protected override bool IsGameAbleToOverLogic() => _currentRound >= kMaxRound;

        private void DetermineWinner()
        {
            int result = (_playerChoice - _computerChoice + 3) % 3;
            _results.Add(result);
        }

        protected override void OnGameStageChange(GameStage oldStage, GameStage newStage)
        {
            // Additional logic when transitioning between stages if needed
        }

        protected override void OnStageStateChange(GameStage currentStage, StageState oldState, StageState newState)
        {
            // Additional logic when the state of a stage changes if needed
        }
    }
}
