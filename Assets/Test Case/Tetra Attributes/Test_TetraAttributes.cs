using System.Collections.Generic;
using TetraCreations.Attributes;
using UnityEngine;

namespace Xi.TestCase
{
    public class Test_TetraAttributes : MonoBehaviour
    {
#pragma warning disable 0414
        /// DrawIf
        [Title("Nice title attribute", titleColor: CustomColor.Yellow, lineColor: CustomColor.Orange, lineHeight: 2f, spacing: 20f)]
        public bool DrawOtherFields = false;

        [DrawIf(nameof(DrawOtherFields), true)]
        public string Name;

        [DrawIf(nameof(DrawOtherFields), true)]
        public int Quantity;

        [DrawIf(nameof(DrawOtherFields), true)]
        public float Speed;

        [DrawIf(nameof(DrawOtherFields), true)]
        public int[] Array;

        [DrawIf(nameof(DrawOtherFields), true)]
        public GameObject Go;

        [DrawIf(nameof(DrawOtherFields), true)]
        public List<GameObject> GoList;

        /// ReadOnly & DrawIf : DisablingType.ReadOnly
        [Space(20f)]
        [Title("Read Only Variables", lineHeight: 2f, spacing: 20f)]
        [ReadOnly]
        public float ReadOnlyFloat;

        [DrawIf(nameof(ToggleToEdit), true, disablingType: DisablingType.ReadOnly)]
        public int DisabledByDefault;

        /// HelpBox
        [Space(20f)]
        [Title("Title on the left", alignTitleLeft: true)]
        [HelpBox("HelpBox attribute is useful to describe the usage of a field directly on the inspector window.", HelpBoxMessageType.Warning)]
        public bool ToggleToEdit = false;

        /// MinMaxSlider
        [Space(20f)]
        [Title("Vector2 MinMaxSlider")]
        [MinMaxSlider(min: 0, max: 100)]
        public Vector2 MinMaxSliderAttribute;

        /// SnappedSlider
        [Space(20f)]
        [Title("Snap float value")]
        [SnappedSlider(step: 0.25f, min: 1f, max: 10f)]
        public float SnappedFloat;

        /// Path
        [Space(20f)]
        [Title("Path")]
        public PathReference PathReferencec = new();

        ///Required
        [Space(20f)]
        [Title("Required")]
        [Required]
        public Collider Collider;

        /// Sprite Preview
        [Space(20f)]
        [Title("Sprite Preview")]
        [SpritePreview]
        public Sprite Sprite;
#pragma warning restore 0414

        private void Start() => Debug.Log("GUID : " + PathReferencec);
        [Button(methodName: nameof(ButtonCallback), label: "Click on me !", row: "first", space: 20)]
        public void ButtonCallback() => Debug.Log("You clicked on a button, congrats.");

        [Button(methodName: nameof(Test), label: "Another button", row: "first", space: 20)]
        public void Test() => Debug.Log("This method is incredibly useful.");

        [Button(methodName: nameof(Test_2), label: "Test_2", row: "first", space: 20)]
        public void Test_2() { }
        [Button(methodName: nameof(Test_3), label: "Test_3", row: "xxx", space: 20)]
        public void Test_3() { }
        [Button(methodName: nameof(Test_4), label: "Test_4", space: 20)]
        public void Test_4() { }
        [Button(methodName: nameof(Test_5), label: "Test_5", row: "xxx", space: 20)]
        public void Test_5() { }
    }
}
