using Xi.Framework;

namespace Xi.Metagame
{
    public class MetagameGameInstanceObject : GameInstanceObject
    {
        public MetagameGameInstance MetagameGameInstance => (MetagameGameInstance)_gameInstance;
    }
}
