using UnityEngine;

namespace Xi.Data
{
    [CreateAssetMenu(fileName = "Unit_SO_", menuName = "Xi/Data/Unit/New Unit")]
    public class So_Entry_Unit : SoCollectionEntry_SO
    {
        public override string ToTxt_Comment() => "#Id\tAge\tSex\tMisc";
        public override string ToTxt_Entry() => "1\t22\ttrue\t[a,b]";
        public override string ToTxt_Header() => "Key\tType\tValue\tDescription";
        public override string ToTxt_Type() => "string\tint\tbool\tList<string>";
    }
}
