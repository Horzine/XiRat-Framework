using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_UIWindow : UiBaseWindow { }

    public class Test_UiController_A : UiBaseController<Test_UIWindow>
    {
        public override UiEnum UiEnum => UiEnum.TestCase_A;
        public override void BeforeClose() { }
        public void Init_A() => Debug.Log(nameof(Test_UiController_A));
    }
}
