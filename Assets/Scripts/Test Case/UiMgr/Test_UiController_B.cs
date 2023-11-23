using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_UiController_B : UiBaseController<Test_UIWindow>
    {
        public override UiEnum UiEnum => UiEnum.TestCase_B;

        public override void ForceReleaseWindow() { }
        public void Init_B() => Debug.Log(nameof(Test_UiController_B));
    }
}
