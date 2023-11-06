using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Xi.TestCase
{
    public class Test_UniTask : MonoBehaviour
    {
        //要读取的 AssetReference
        [SerializeField] private AssetReference _target;
        [SerializeField] private Image _image;

        private void Start()
        {
            var token = this.GetCancellationTokenOnDestroy();
            // InitializeAsync(_target, token).Forget();

            TestDOTweenAsync().Forget();

        }

        public async UniTaskVoid InitializeAsync(AssetReference target, CancellationToken token)
        {
            //等待 Addressables.load 与 await 异步加载资产
            var texture = await Addressables.LoadAssetAsync<Texture>(target).WithCancellation(token);
            if (token.IsCancellationRequested)
            {
                target?.ReleaseAsset();
                return;
            }

            print(texture.name);
        }

        public async UniTaskVoid TestDOTweenAsync()
        {
            // 序列
            await transform.DOMoveX(2, 10);
            await transform.DOMoveZ(5, 20);

            //并行Cancellation
            var ct = this.GetCancellationTokenOnDestroy();

            await UniTask.WhenAll(
                transform.DOMoveX(10, 3).WithCancellation(ct),
                transform.DOScale(10, 3).WithCancellation(ct));
        }

        public async UniTaskVoid TestTextMeshPro()
        {
            var tmp = GetComponent<TMP_InputField>();
            await tmp.OnValueChangedAsync();
        }
    }
}
