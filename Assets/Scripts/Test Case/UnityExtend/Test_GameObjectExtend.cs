using UnityEngine;
using Xi.Extend.UnityExtend;

namespace Xi.TestCase
{
    public class Test_GameObjectExtend : MonoBehaviour
    {
        public SphereCollider _sphereCollider;
        public GameObject _testObject;
        public GameObject _childA;
        public GameObject _parentA;

        private void Start()
        {
            this.SetSelfActive(false);
            gameObject.SetSelfActive(true);

            this.SetSelfEnable(true);
            var collider = this.GetOrAddComponent<BoxCollider>();
            collider.isTrigger = true;
            var rig = gameObject.GetOrAddComponent<Rigidbody>();
            rig.useGravity = false;

            print(gameObject.PrintGameObjectTreePath());

            print(_sphereCollider);
            this.DestroyObjectAndReleaseReference(ref _sphereCollider);
            print(_sphereCollider);

            print(_testObject);
            this.DestroyObjectAndReleaseReference(ref _testObject);
            print(_testObject);

            gameObject.SetLayerRecursively(4);

            print(_childA.IsChildOf(_parentA));
            print(_parentA.HasChild(_childA));

            print(this.GetOrAddComponentAsChild<BoxCollider>("New BoxCollider"));
            print(this.GetOrAddComponentAsChild<SpriteRenderer>(includeInactive: true));
            print(this.GetOrAddComponentAsChild<MeshRenderer>(includeInactive: false));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                this.DestroySelfGameObject();
            }
        }
    }
}
