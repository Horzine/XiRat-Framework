using System;
using Xi.Config;

namespace Xi.Gameplay.Character.InputHandler
{
    [Serializable]
    public class PlayerInputHandlerCombat : PlayerInputHandler
    {
        public PlayerInputHandlerCombat(InputActionConfig.PlayerActions player) : base(player) { }

        public override void OnUpdate(float deltaTime)
        {

        }

        public override void AddInputListner()
        {
        }

        public override void RemoveInputListner()
        {
        }
    }
}
