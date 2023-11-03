using DG.Tweening;
using UnityEngine;

namespace Xi.TestCase
{
    public class Test_DOTween : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start() => transform.DOMove(new Vector3(1, 2, 3), 3);

        // Update is called once per frame
        private void Update()
        {

        }
    }
}
