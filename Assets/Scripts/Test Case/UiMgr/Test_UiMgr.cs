using Cysharp.Threading.Tasks;
using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_UiMgr : MonoBehaviour
    {
        private async void Start() => await Test_2();

        private async UniTask Test_1()
        {
            await UiManager.Instance.InitAsync();
            await UniTask.Delay(1000);

            UiManager.Instance.GetController<Test_UiController_A>(UiEnum.TestCase_A).Init_A();
            UiManager.Instance.GetController<Test_UiController_B>(UiEnum.TestCase_B).Init_B();
            UiManager.Instance.GetController<Test_UiController_C>(UiEnum.TestCase_C).Init_C();
            UiManager.Instance.GetController<Test_UiController_C>(UiEnum.TestCase_A)?.Init_C();// is null here
            UiManager.Instance.GetController<Test_UiController_C>(UiEnum.TestCase_D)?.Init_C();// is null here

            var controllerA = UiManager.Instance.GetController<Test_UiController_A>(UiEnum.TestCase_A);
            Debug.Log("Open Begin");
            await controllerA.OpenAsync();
            Debug.Log("Open Over");
            await UniTask.Delay(1000);
            Debug.Log("Close Begin");
            await controllerA.CloseAsync();
            Debug.Log("Close Over");

            await controllerA.OpenAsync();
            await UniTask.Delay(1000);
            var controllerC = UiManager.Instance.GetController<Test_UiController_C>(UiEnum.TestCase_C);
            await controllerC.OpenAsync();
        }

        private async UniTask Test_2()
        {
            await UiManager.Instance.InitAsync();

            var ctrlA = UiManager.Instance.GetController<Test_UiController_A>(UiEnum.TestCase_A);
            var ctrlB = UiManager.Instance.GetController<Test_UiController_B>(UiEnum.TestCase_B);
            var ctrlC = UiManager.Instance.GetController<Test_UiController_C>(UiEnum.TestCase_C);
            ctrlA.Init_A();
            ctrlB.Init_B();
            ctrlC.Init_C();

            print($"A: {ctrlA.CurrentWindowState}, B: {ctrlB.CurrentWindowState}, C: {ctrlC.CurrentWindowState}");
            await ctrlA.OpenAsync();
            print($"A: {ctrlA.CurrentWindowState}, B: {ctrlB.CurrentWindowState}, C: {ctrlC.CurrentWindowState}");
            ctrlB.OpenAsync().Forget();
            print($"A: {ctrlA.CurrentWindowState}, B: {ctrlB.CurrentWindowState}, C: {ctrlC.CurrentWindowState}");
            await ctrlC.OpenAsync();
            print($"A: {ctrlA.CurrentWindowState}, B: {ctrlB.CurrentWindowState}, C: {ctrlC.CurrentWindowState}");
            ctrlC.CloseAsync().Forget();
            print($"A: {ctrlA.CurrentWindowState}, B: {ctrlB.CurrentWindowState}, C: {ctrlC.CurrentWindowState}");

        }

        private void Update()
        {
            var ctrlA = UiManager.Instance.GetController<Test_UiController_A>(UiEnum.TestCase_A);
            var ctrlB = UiManager.Instance.GetController<Test_UiController_B>(UiEnum.TestCase_B);
            var ctrlC = UiManager.Instance.GetController<Test_UiController_C>(UiEnum.TestCase_C);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                UiManager.Instance.ForceReleaseAllWindow();
                print($"A: {ctrlA.CurrentWindowState}, B: {ctrlB.CurrentWindowState}, C: {ctrlC.CurrentWindowState}");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                print($"A: {ctrlA?.CurrentWindowState}, B: {ctrlB?.CurrentWindowState}, C: {ctrlC?.CurrentWindowState}");
            }
        }
    }
}
