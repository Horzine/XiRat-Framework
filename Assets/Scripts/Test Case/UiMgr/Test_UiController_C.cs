using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_UiController_C : UiBaseController<Test_UIWindow>
    {
        public override UiEnum UiEnum => UiEnum.TestCase_C;

        public override void ForceReleaseWindow() { }
        public void Init_C() => Debug.Log(nameof(Test_UiController_C));
    }
}
