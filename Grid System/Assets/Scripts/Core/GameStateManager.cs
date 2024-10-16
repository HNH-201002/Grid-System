using UnityEngine;

namespace GridSystem.Core
{
    public class GameStateManager : MonoBehaviour
    {
        public static GameState CurrentGameState;

        private void Start()
        {
            SetGameState(GameState.None);
        }

        public static void SetGameState(GameState newGameState)
        {
            CurrentGameState = newGameState;
        }
    }
}