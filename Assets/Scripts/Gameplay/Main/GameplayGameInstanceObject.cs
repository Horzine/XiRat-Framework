using Xi.Framework;

namespace Xi.Gameplay
{
    public class GameplayGameInstanceObject : GameInstanceObject
    {
        public GameplayGameInstance GameplayGameInstance => (GameplayGameInstance)_gameInstance;
    }
}
