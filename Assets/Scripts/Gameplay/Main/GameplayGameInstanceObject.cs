using Xi.Framework;

namespace Xi.Gameplay.Main
{
    public class GameplayGameInstanceObject : GameInstanceObject
    {
        public GameplayGameInstance GameplayGameInstance => (GameplayGameInstance)_gameInstance;
    }
}
