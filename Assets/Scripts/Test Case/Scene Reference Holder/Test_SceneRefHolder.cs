using UnityEngine;
using Xi.Framework;
using Xi.Metagame;

namespace Xi.TestCase
{
    public class Test_SceneRefHolder : MonoBehaviour
    {
        private void Awake()
        {
            var holderGo = FindObjectOfType<SceneObjectReferenceHolderGameObject>();
            var holder = holderGo.GetComponent<ISceneObjectReferenceHolder>() as MetagameSceneObjRefHolder;
            print(holder.name);
            //var t2 = holder.GetSceneObjectReference<Rigidbody>(MetagameSceneObjectEnum.Test_2);
            //print(t2);
            //var t1 = holder.GetSceneObjectReference<Rigidbody>(MetagameSceneObjectEnum.Test_1);
            //print(t1);
            //var me = holder.GetSceneObjectReference<Test_SceneRefHolder>(MetagameSceneObjectEnum.Test_1);
            //print(me.name);
        }
    }
}
