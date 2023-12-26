using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Gameplay.Process;
using static Xi.TestCase.Test_RockPaperScissorsGame;

namespace Xi.TestCase
{
    public class Test_RockPaperScissorsGame : GameProcess<ProcessData, RoundData>
    {
        public class ProcessData : GameProcessData<RoundData>
        {
            public string ResultStr => string.Join(", ", _cachedRounds.Select(item => (RoundResult)item.Result));
        }
        public class RoundData : GameProcessRoundData
        {
            public int PlayerChoice { get; set; }
            public int ComputerChoice { get; set; }
            public int Result { get; set; }

            public override void OnRoundBegin() => PlayerChoice = ComputerChoice = Result = -1;
            public override void OnRoundEnd() => Result = (PlayerChoice - ComputerChoice + 3) % 3;
        }

        private const int kMaxRound = 3;
        protected override int MaxRoundCount => kMaxRound;

        private enum Choice
        {
            Rock,
            Paper,
            Scissors
        }

        private enum RoundResult
        {
            TieRound,
            WinRound,
            LoseRound,
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

        protected override UniTask<bool> HandleGameStage_RoundBegin(StageState state)
        {
            switch (state)
            {
                case StageState.Before:
                    Debug.Log("Round starting...");
                    Debug.Log("Choose your move: (0) Rock, (1) Paper, (2) Scissors");
                    break;
                case StageState.Progressing:
                    // Let the player make a choice (0 for Rock, 1 for Paper, 2 for Scissors)
                    //Debug.Log("Choose your move: (0) Rock, (1) Paper, (2) Scissors");
                    if (Input.GetKeyDown(KeyCode.Alpha0))
                    {
                        CurrentRound.PlayerChoice = 0;
                        break;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        CurrentRound.PlayerChoice = 1;
                        break;
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        CurrentRound.PlayerChoice = 2;
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
                    CurrentRound.ComputerChoice = Random.Range(0, 3);
                    Debug.Log($"Computer chose: {(Choice)CurrentRound.ComputerChoice}");
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
                    Debug.Log($"Round {CurrentRound.RoundNum}: Player: {(Choice)CurrentRound.PlayerChoice}, AI: {(Choice)CurrentRound.ComputerChoice}, Result: {(RoundResult)CurrentRound.Result}");
                    //await UniTask.WhenAny(UniTask.Delay(5000), UniTask.WaitUntil(() => _skipResultDisplay));
                    if (HasNextRound)
                    {
                        await UniTask.Delay(3000);
                        IsAbleToNextRound = true;
                    }
                    else
                    {
                        IsAbleToOver = true;
                    }

                    break;
                case StageState.Progressing:
                case StageState.After:
                    break;
            }

            return true;
        }

        protected override UniTask<bool> HandleGameStage_Over(StageState state)
        {
            switch (state)
            {
                case StageState.Before:
                    Debug.Log($"result: {Data.ResultStr}");
                    break;
                case StageState.Progressing:
                case StageState.After:
                    break;
            }

            return UniTask.FromResult(true);
        }
    }
}
