using System;
using MemoryPack;
using UnityEngine;

namespace Xi_
{
    [Serializable]
    public abstract class DataClass
    {
        [HideInInspector] public byte[] sonClassInfo;
        public abstract void FromBytes();
        public abstract void ToBytes();
    }

    [MemoryPackable]
    [Serializable]
    public partial class Info_1 : IInfo
    {
        public int info_1_int = 100;
        public string info_1_string;
        public bool info_1_bool;
    }
    [Serializable]
    public class DataClass_1 : DataClass
    {
        public Info_1 info;
        public override void FromBytes() => info = MemoryPackSerializer.Deserialize<Info_1>(sonClassInfo);
        public override void ToBytes() => sonClassInfo = MemoryPackSerializer.Serialize(info);
    }

    [MemoryPackable]
    [Serializable]
    public partial class Info_2 : IInfo
    {
        public int info_2_int = -876;
    }
    public class DataClass_2 : DataClass
    {
        public Info_2 info;
        public override void FromBytes() => info = MemoryPackSerializer.Deserialize<Info_2>(sonClassInfo);
        public override void ToBytes() => sonClassInfo = MemoryPackSerializer.Serialize(info);
    }

    public interface IInfo
    {

    }
}
