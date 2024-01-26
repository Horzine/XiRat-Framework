using System;
using Xi.Config;

namespace Xi.Gameplay.Character.InputHandler
{
    [Serializable]
    public abstract class PlayerInputHandler
    {
        protected InputActionConfig.PlayerActions _player;

        public PlayerInputHandler(InputActionConfig.PlayerActions player) => _player = player;

        public abstract void OnUpdate(float deltaTime);

        public abstract void AddInputListner();

        public abstract void RemoveInputListner();
    }
}
