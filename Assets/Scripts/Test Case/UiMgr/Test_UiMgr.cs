using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_UiMgr : MonoBehaviour
    {
        private async void Start()
        {
            await UiManager.Instance.InitAsync();
            await UniTask.Delay(1000);

            UiManager.Instance.GetController<Test_UiController_A>(UiEnum.TestCase_A).Init_A();
            UiManager.Instance.GetController<Test_UiController_B>(UiEnum.TestCase_B).Init_B();
            UiManager.Instance.GetController<Test_UiController_C>(UiEnum.TestCase_A)?.Init_C();// is null here
            UiManager.Instance.GetController<Test_UiController_C>(UiEnum.TestCase_D)?.Init_C();// is null here
        }
    }
}
