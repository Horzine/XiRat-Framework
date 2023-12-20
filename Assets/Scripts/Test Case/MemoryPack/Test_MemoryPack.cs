using MemoryPack;
using UnityEngine;

namespace Xi.TestCase
{
    [MemoryPackable]
    public partial class MP_A
    {
        public MP_A(string _str2, string str1)
        {
            this._str2 = _str2;
            Str1 = str1;
        }
        public string str0;
        public string Str1 { get; private set; }
        [MemoryPackInclude] private readonly string _str2;
        [MemoryPackIgnore] public string str3;

        public string Str2 => _str2;
    }

    public class Test_MemoryPack : MonoBehaviour
    {
        private void Start()
        {
            var mp = new MP_A("2", "1")
            {
                str0 = "0",
                str3 = "3",
            };

            byte[] result = MemoryPackSerializer.Serialize(mp);
            var newObj = MemoryPackSerializer.Deserialize<MP_A>(result);
            print(newObj.str0);
            print(newObj.Str1);
            print(newObj.Str2);
            print(newObj.str3);
        }
    }
}
