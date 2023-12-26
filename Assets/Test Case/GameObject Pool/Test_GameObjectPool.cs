using UnityEngine;
using Xi.Framework;

namespace Xi.TestCase
{
    public class Test_GameObjectPool : MonoBehaviour
    {
        [SerializeField] private Test_GameObjectPool_Cube _cubeTemplate;
        private GameObjectPool<Test_GameObjectPool_Cube> _cubePool;
        private float _nextGenerateTimer;

        private void Awake()
            => typeof(GameObjectPoolManager).GetMethod("OnSceneEnter", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(GameObjectPoolManager.Instance, new object[] { string.Empty });

        private void Start()
            => _cubePool = GameObjectPoolManager.Instance.CreateGameObjectPool(_cubeTemplate, actionAfterCreate: OnCreateCubeCallback);

        private void Update()
        {
            if (Time.time > _nextGenerateTimer)
            {
                var cube = _cubePool.GetEntry();
                cube.transform.position = Random.insideUnitCircle * 5;
                _nextGenerateTimer = Time.time + 0.1f;
            }
        }

        private void OnCreateCubeCallback(Test_GameObjectPool_Cube cube) => cube.Init(OnCubeDelayTimeCallback);

        private void OnCubeDelayTimeCallback(Test_GameObjectPool_Cube cube) => _cubePool.ReleaseEntry(cube);

        private void OnDestroy()
        {
            if (GameObjectPoolManager.Instance)
            {
                GameObjectPoolManager.Instance.ReleaseGameObjectPool(_cubePool);
            }

            _cubePool = null;
        }
    }
}
