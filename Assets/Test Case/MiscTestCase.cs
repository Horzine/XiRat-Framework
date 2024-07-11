using UnityEngine;
using Xi.Extend;

namespace Xi.TestCase
{
    public class MiscTestCase : MonoBehaviour
    {
        private void Start() => Test_Enum();

        private enum MyEnum
        {
            Value0 = -3,
            Value1 = 1,
            Value2 = 4,
            Value3 = 7,
            Value4 = 10
        }

        private void Test_Enum()
        {
            var current = MyEnum.Value0;
            // 使用扩展方法进行枚举步进
            current = current.StepEnum(1);
            Debug.Log(current.ToString()); // 输出 Value1

            current = current.StepEnum(-1);
            Debug.Log(current.ToString()); // 输出 Value0

            current = current.StepEnum(3);
            Debug.Log(current.ToString()); // 输出 Value3

            current = current.StepEnum(-2);
            Debug.Log(current.ToString()); // 输出 Value1

            current = current.StepEnum(-24);
            Debug.Log(current.ToString()); // 输出 Value2
        }
    }
}
