using System;
using TMPro;
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
        [field: SerializeField] public TextMeshProUGUI TestIntTxt { get; private set; }
        [field: SerializeField] public Button RandomTestIntBtn { get; private set; }
        private Func<string> _claimUserTestIntStrCallback;

        public void AddCallback(UnityAction selectMapBtnCallback,
            UnityAction classBuildBtnCallback,
            Func<string> claimUserTestIntStrCallback,
            UnityAction randomTestIntCallback)
        {
            SelectMapBtn.onClick.AddListener(selectMapBtnCallback);
            ClassBuildBtn.onClick.AddListener(classBuildBtnCallback);
            RandomTestIntBtn.onClick.AddListener(randomTestIntCallback);
            _claimUserTestIntStrCallback = claimUserTestIntStrCallback;
        }

        public void CleanCallback()
        {
            SelectMapBtn.onClick.RemoveAllListeners();
            ClassBuildBtn.onClick.RemoveAllListeners();
        }

        public void Refresh()
        {
            if (_claimUserTestIntStrCallback != null)
            {
                string result = _claimUserTestIntStrCallback.Invoke();
                if (result != TestIntTxt.text)
                {
                    TestIntTxt.SetText(result);
                }
            }
        }
    }
}
