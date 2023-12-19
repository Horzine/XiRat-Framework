using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Xi.Framework;

namespace Xi.Metagame.Ui
{
    public class MainMenuWindow : UiBaseWindow
    {
        [field: SerializeField] public Button SelectMapBtn { get; private set; }
        [field: SerializeField] public Button ClassBuildBtn { get; private set; }

        public void AddCallback(UnityAction selectMapBtnCallback, UnityAction classBuildBtnCallback)
        {
            SelectMapBtn.onClick.AddListener(selectMapBtnCallback);
            ClassBuildBtn.onClick.AddListener(classBuildBtnCallback);
        }

        public void CleanCallback()
        {
            SelectMapBtn.onClick.RemoveAllListeners();
            ClassBuildBtn.onClick.RemoveAllListeners();
        }
    }
}
