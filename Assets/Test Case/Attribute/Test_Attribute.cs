using UnityEngine;
using Xi.Extend.Attribute;

namespace Xi.TestCase
{
    public class Test_Attribute : MonoBehaviour
    {
        [ReadOnly, SerializeField] private int a;
        [ReadOnly] public string s;

        [TagName] public string Tag;
        [Label("Value")] public float v;
    }
}
