using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Xi.EditorExtend
{
    public class PreBuildProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (PlayerSettings.GetScriptingBackend(BuildTargetGroup.Standalone) == ScriptingImplementation.IL2CPP)
            {
                // Add macro definition in IL2CPP mode
                AddMacroDefinition("IL2CPP_BUILD");
            }
            else
            {
                // If it is not IL2CPP mode, delete the macro definition
                RemoveMacroDefinition("IL2CPP_BUILD");
            }
        }

        private void AddMacroDefinition(string macro)
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            if (!symbols.Contains(macro))
            {
                symbols += ";" + macro;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
            }
        }

        private void RemoveMacroDefinition(string macro)
        {
            string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            symbols = symbols.Replace(macro, "").Replace(";;", ";").Trim(';');
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbols);
        }
    }
}
