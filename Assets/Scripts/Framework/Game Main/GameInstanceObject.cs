using UnityEngine;

namespace Xi.Framework
{
    public abstract class GameInstanceObject : MonoBehaviour
    {
        protected GameInstance _gameInstance;
        public void Init(GameInstance gameInstance) => _gameInstance = gameInstance;
    }
}
