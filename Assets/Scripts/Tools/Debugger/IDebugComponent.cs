namespace Xi.Tools
{
    internal interface IDebugComponent
    {
        object Target { set; }

        void OnSceneWindow();
    }
}