using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Profiling;
using Object = UnityEngine.Object;

namespace Xi.Tools
{
    [DefaultExecutionOrder(-20)]
    public sealed class DebugManager : MonoBehaviour
    {
        [Serializable]
        public class LogData
        {
            public int count;

            public bool show;

            public string text;

            public Color color;

            public LogData(string text, bool show, Color color)
            {
                this.text = text;
                this.show = show;
                this.color = color;
            }
        }

        private static DebugManager Instance;

        [SerializeField]
        private bool debugEnabled = true;

        private Dictionary<WindowType, Action> windowDict;

        private Dictionary<LogType, LogData> debugDict;

        private Dictionary<Type, Type> typeDict;

        private List<Component> componentList;

        private List<Transform> transformList;

        private List<DebugData> debugList;

        private List<Type> typeList;

        private int debugIndex = -1;

        private int componentIndex = -1;

        private int transformIndex = -1;

        private bool windowMax;

        private bool isAddComponent;

        private long minTotalReservedMemory = 10000L;

        private long maxTotalReservedMemory;

        private long minTotalAllocatedMemory = 10000L;

        private long maxTotalAllocatedMemory;

        private long minTotalUnusedReservedMemory = 10000L;

        private long maxTotalUnusedReservedMemory;

        private long minMonoHeapSize = 10000L;

        private long maxMonoHeapSize;

        private long minMonoUsedSize = 10000L;

        private long maxMonoUsedSize;

        private float frameTime;

        private float frameCount;

        private readonly float transformMemory;

        private string transformFilter = "";

        private string componentFilter = "";

        private Rect windowRect;

        private Color windowColor = Color.white;

        private Vector2 transformView = Vector2.zero;

        private Vector2 inspectorView = Vector2.zero;

        private Vector2 scrollTimeView = Vector2.zero;

        private Vector2 logMessageView = Vector2.zero;

        private Vector2 debugMessageView = Vector2.zero;

        private Vector2 scrollMemoryView = Vector2.zero;

        private Vector2 scrollSystemView = Vector2.zero;

        private Vector2 scrollScreenView = Vector2.zero;

        private Vector2 scrollProjectView = Vector2.zero;

        private WindowType windowType = WindowType.Console;

        private IDebugComponent debugComponent;

        private void ConsoleWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("Clear", new GUILayoutOption[1] { DebugSetting.height }))
            {
                debugIndex = -1;
                debugList.Clear();
                foreach (var value in debugDict.Values)
                {
                    value.count = 0;
                }

                windowColor = Color.white;
            }

            foreach (var value2 in debugDict.Values)
            {
                GUI.contentColor = value2.show ? Color.white : Color.gray;
                if (GUILayout.Button($"{value2.text} [{value2.count}]", new GUILayoutOption[1] { DebugSetting.height }))
                {
                    value2.show = !value2.show;
                }
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            logMessageView = GUILayout.BeginScrollView(logMessageView, new GUIStyle("Box"), new GUILayoutOption[1] { DebugSetting.heightFix });
            for (int i = 0; i < debugList.Count; i++)
            {
                var logData = debugDict[debugList[i].type];
                if (logData.show)
                {
                    GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                    GUI.contentColor = logData.color;
                    if (GUILayout.Toggle(debugIndex == i, debugList[i].showTitle, Array.Empty<GUILayoutOption>()))
                    {
                        debugIndex = i;
                    }

                    GUI.contentColor = Color.white;
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();
            debugMessageView = GUILayout.BeginScrollView(debugMessageView, new GUIStyle("Box"));
            if (debugIndex != -1)
            {
                var debugData = debugList[debugIndex];
                GUILayout.Label($"{debugData.message}\r\n\r\n{debugData.stackTrace}", Array.Empty<GUILayoutOption>());
            }

            GUILayout.EndScrollView();
        }

        private void LogMessageReceived(string message, string stackTrace, LogType type)
        {
            var debugData = default(DebugData);
            debugData.type = type;
            debugData.dateTime = DateTime.Now.ToString("HH:mm:ss");
            debugData.stackTrace = stackTrace;
            debugData.message = message;
            var item = debugData;
            item.showTitle = $" [{item.dateTime}] [{item.type}] {item.message}";
            var logData = debugDict[type];
            logData.count++;
            debugList.Add(item);
            for (int num = debugDict.Keys.Count - 1; num >= 0; num--)
            {
                if (logData.count > 0)
                {
                    string text = logData.text;
                    var val = (LogType)3;
                    if (text != val.ToString())
                    {
                        windowColor = debugDict[type].color;
                        break;
                    }
                }
            }
        }

        private void MemoryWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUI.contentColor = Color.white;
            GUILayout.Label(" 内存信息", new GUILayoutOption[1] { DebugSetting.heightLowest });
            GUILayout.EndHorizontal();
            scrollMemoryView = GUILayout.BeginScrollView(scrollMemoryView, new GUIStyle("Box"));
            long num = Profiler.GetTotalReservedMemoryLong() / 1000000;
            if (num > maxTotalReservedMemory)
            {
                maxTotalReservedMemory = num;
            }

            if (num < minTotalReservedMemory)
            {
                minTotalReservedMemory = num;
            }

            GUILayout.Label($"全部内存: {num}MB\t[ 最小值: {minTotalReservedMemory}\t最大值: {maxTotalReservedMemory} ]", Array.Empty<GUILayoutOption>());
            num = Profiler.GetTotalAllocatedMemoryLong() / 1000000;
            if (num > maxTotalAllocatedMemory)
            {
                maxTotalAllocatedMemory = num;
            }

            if (num < minTotalAllocatedMemory)
            {
                minTotalAllocatedMemory = num;
            }

            GUILayout.Label($"使用内存: {num}MB\t[ 最小值: {minTotalAllocatedMemory}\t最大值: {maxTotalAllocatedMemory} ]", Array.Empty<GUILayoutOption>());
            num = Profiler.GetTotalUnusedReservedMemoryLong() / 1000000;
            if (num > maxTotalUnusedReservedMemory)
            {
                maxTotalUnusedReservedMemory = num;
            }

            if (num < minTotalUnusedReservedMemory)
            {
                minTotalUnusedReservedMemory = num;
            }

            GUILayout.Label($"空闲内存: {num}MB\t[ 最小值: {minTotalUnusedReservedMemory}\t最大值: {maxTotalUnusedReservedMemory} ]", Array.Empty<GUILayoutOption>());
            num = Profiler.GetMonoHeapSizeLong() / 1000000;
            if (num > maxMonoHeapSize)
            {
                maxMonoHeapSize = num;
            }

            if (num < minMonoHeapSize)
            {
                minMonoHeapSize = num;
            }

            GUILayout.Label($"全部Mono堆内存: {num}MB\t[ 最小值: {minMonoHeapSize}\t最大值: {maxMonoHeapSize} ]", Array.Empty<GUILayoutOption>());
            num = Profiler.GetMonoUsedSizeLong() / 1000000;
            if (num > maxMonoUsedSize)
            {
                maxMonoUsedSize = num;
            }

            if (num < minMonoUsedSize)
            {
                minMonoUsedSize = num;
            }

            GUILayout.Label($"使用Mono堆内存: {num}MB\t[ 最小值: {minMonoUsedSize}\t最大值: {maxMonoUsedSize} ]", Array.Empty<GUILayoutOption>());
            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("垃圾回收", new GUILayoutOption[1] { DebugSetting.height }))
            {
                GC.Collect();
            }

            GUILayout.EndHorizontal();
        }

        private void ProjectWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUI.contentColor = Color.white;
            GUILayout.Label(" 环境配置", new GUILayoutOption[1] { DebugSetting.heightLowest });
            GUILayout.EndHorizontal();
            scrollProjectView = GUILayout.BeginScrollView(scrollProjectView, new GUIStyle("Box"));
            GUILayout.Label($"项目名称: {Application.productName}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"项目标识: {Application.identifier}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"项目版本: {Application.version}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"项目路径: {Application.dataPath}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"公司名称: {Application.companyName}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"Unity版本: {Application.unityVersion}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"Unity专业版: {Application.HasProLicense()}", Array.Empty<GUILayoutOption>());
            var internetReachability = Application.internetReachability;
            if (1 == 0)
            {
            }

            string text = ((int)internetReachability == 1) ? "数据网络连接中" : (((int)internetReachability != 2) ? "无网络连接" : "WIFI网络连接中");
            if (1 == 0)
            {
            }

            string text2 = text;
            GUILayout.Label($"网络状态: {text2}", Array.Empty<GUILayoutOption>());
            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("退出游戏", new GUILayoutOption[1] { DebugSetting.height }))
            {
                Application.Quit();
            }

            GUILayout.EndHorizontal();
        }

        private void LoadComponent()
        {
            var typeFromHandle = typeof(IDebugComponent);
            var assembly = Assembly.GetAssembly(typeFromHandle);
            var types = assembly.GetTypes();
            var array = types;
            foreach (var type in array)
            {
                if (!typeFromHandle.IsAssignableFrom(type.BaseType))
                {
                    continue;
                }

                object[] customAttributes = type.GetCustomAttributes(typeof(DebugAttribute), inherit: true);
                object[] array2 = customAttributes;
                foreach (object obj in array2)
                {
                    if (obj is DebugAttribute debugAttribute)
                    {
                        typeDict.Add(debugAttribute.type, type);
                    }
                }
            }
        }

        private void SceneWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUILayout.Label($"GameObject [{transformList.Count}]", new GUIStyle("Button"), new GUILayoutOption[1] { DebugSetting.height });
            GUI.contentColor = Color.white;
            if (GUILayout.Button("Refresh", new GUILayoutOption[2]
            {
            DebugSetting.box,
            DebugSetting.height
            }))
            {
                RefreshTransform();
                RefreshComponent();
                return;
            }

            GUI.enabled = true;
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            ShowTransform();
            ShowComponent();
            GUILayout.EndHorizontal();
        }

        private void RefreshTransform()
        {
            transformList.Clear();
            transformList = Object.FindObjectsOfType<Transform>().ToList();
            transformIndex = -1;
        }

        private void RefreshComponent()
        {
            componentList.Clear();
            if (transformIndex != -1 && transformIndex < transformList.Count)
            {
                componentList = transformList[transformIndex].GetComponents<Component>().ToList();
            }

            componentIndex = -1;
            isAddComponent = false;
            debugComponent = null;
        }

        private void ShowTransform()
        {
            GUI.contentColor = Color.white;
            GUILayout.BeginVertical(new GUIStyle("Box"), new GUILayoutOption[1] { DebugSetting.box });
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            transformFilter = GUILayout.TextField(transformFilter, new GUILayoutOption[1] { DebugSetting.heightLow });
            GUILayout.EndHorizontal();
            transformView = GUILayout.BeginScrollView(transformView, Array.Empty<GUILayoutOption>());
            SceneTransform();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void SceneTransform()
        {
            for (int i = 0; i < transformList.Count; i++)
            {
                if (!transformList[i] || !transformList[i].name.Contains(transformFilter) || (int)transformList[i].gameObject.hideFlags == 1)
                {
                    continue;
                }

                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUI.contentColor = transformList[i].gameObject.activeSelf ? Color.cyan : Color.gray;
                bool flag = transformIndex == i;
                if (GUILayout.Toggle(flag, $" {transformList[i].name}", Array.Empty<GUILayoutOption>()) != flag)
                {
                    if (transformIndex != i)
                    {
                        transformIndex = i;
                        RefreshComponent();
                    }
                    else
                    {
                        transformIndex = -1;
                        RefreshComponent();
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                SelectTransform(i);
            }
        }

        private void SelectTransform(int index)
        {
            if (transformIndex == index)
            {
                GUI.contentColor = transformList[index].gameObject.activeSelf ? Color.white : Color.gray;
                GUILayout.BeginVertical(new GUIStyle("Box"), Array.Empty<GUILayoutOption>());
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUILayout.Label($"Tag: {transformList[index].tag}", Array.Empty<GUILayoutOption>());
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUILayout.Label($"Layer: {LayerMask.LayerToName(transformList[index].gameObject.layer)}", Array.Empty<GUILayoutOption>());
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }
        }

        private void CheckComponent()
        {
            if (transformIndex == -1)
            {
                return;
            }

            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            isAddComponent = GUILayout.Toggle(isAddComponent, "Add Component", new GUIStyle("Button"), new GUILayoutOption[1] { DebugSetting.heightLow });
            if (componentIndex != -1 && componentIndex < componentList.Count && componentList[componentIndex] && GUILayout.Button("Remove Component", DebugSetting.button))
            {
                var val = componentList[componentIndex];
                if (val is not DebugManager and not Transform)
                {
                    Object.Destroy(componentList[componentIndex]);
                    RefreshComponent();
                    return;
                }

                Debug.LogWarning($"销毁组件 {componentList[componentIndex].GetType().Name} 失败!");
            }

            GUILayout.EndHorizontal();
        }

        private void ShowComponent()
        {
            GUI.contentColor = Color.white;
            GUILayout.BeginVertical(new GUIStyle("Box"), new GUILayoutOption[1] { DebugSetting.box });
            CheckComponent();
            inspectorView = GUILayout.BeginScrollView(inspectorView, Array.Empty<GUILayoutOption>());
            if (transformIndex != -1)
            {
                if (isAddComponent)
                {
                    GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                    FindComponent();
                    GUILayout.EndHorizontal();
                    AddComponent();
                }
                else
                {
                    for (int i = 0; i < componentList.Count; i++)
                    {
                        if (componentList[i])
                        {
                            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                            GetComponent(i);
                            GUILayout.FlexibleSpace();
                            GUILayout.EndHorizontal();
                            SelectComponent(i);
                        }
                    }
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void FindComponent()
        {
            componentFilter = GUILayout.TextField(componentFilter, new GUILayoutOption[1] { DebugSetting.heightLow });
            if (!GUILayout.Button("Search", new GUILayoutOption[2]
            {
            DebugSetting.width,
            DebugSetting.heightLow
            }))
            {
                return;
            }

            typeList.Clear();
            var typeFromHandle = typeof(Component);
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var array = assemblies;
            foreach (var assembly in array)
            {
                var types = assembly.GetTypes();
                var array2 = types;
                foreach (var type in array2)
                {
                    if (type.IsSubclassOf(typeFromHandle) && type.Name.Contains(componentFilter))
                    {
                        string[] array3 = type.ToString().Split('.');
                        if (array3[0] == "UnityEngine")
                        {
                            typeList.Add(type);
                        }
                    }
                }
            }
        }

        private void AddComponent()
        {
            foreach (var type in typeList)
            {
                string fullName = type.FullName;
                if (fullName != null && GUILayout.Button(fullName.Split('.')[^1], new GUILayoutOption[1] { DebugSetting.heightLow }))
                {
                    transformList[transformIndex].gameObject.AddComponent(type);
                    isAddComponent = false;
                    RefreshComponent();
                    break;
                }
            }
        }

        private void SelectComponent(int index)
        {
            if (componentIndex == index)
            {
                GUILayout.BeginVertical(new GUIStyle("Box"), Array.Empty<GUILayoutOption>());
                GUI.contentColor = Color.white;
                if (debugComponent != null)
                {
                    debugComponent.OnSceneWindow();
                }
                else
                {
                    GUILayout.Label("No Debug GUI!", Array.Empty<GUILayoutOption>());
                }

                GUILayout.EndVertical();
            }
        }

        private void GetComponent(int i)
        {
            GUI.contentColor = Color.cyan;
            bool flag = componentIndex == i;
            if (GUILayout.Toggle(flag, $" {componentList[i].GetType().Name}", Array.Empty<GUILayoutOption>()) == flag)
            {
                return;
            }

            if (componentIndex != i)
            {
                componentIndex = i;
                debugComponent = null;
                var type = componentList[i].GetType();
                if (typeDict.ContainsKey(type))
                {
                    var type2 = typeDict[type];
                    debugComponent = (IDebugComponent)Activator.CreateInstance(type2);
                    if (debugComponent != null)
                    {
                        debugComponent.Target = componentList[i];
                    }
                }
            }
            else
            {
                componentIndex = -1;
                debugComponent = null;
            }
        }

        private void ScreenWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUI.contentColor = Color.white;
            GUILayout.Label(" 屏幕信息", new GUILayoutOption[1] { DebugSetting.heightLowest });
            GUILayout.EndHorizontal();
            scrollScreenView = GUILayout.BeginScrollView(scrollScreenView, new GUIStyle("Box"));
            GUILayout.Label($"DPI: {Screen.dpi}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"程序分辨率: {Screen.width} x {Screen.height}", Array.Empty<GUILayoutOption>());
            var currentResolution = Screen.currentResolution;
            GUILayout.Label($"设备分辨率: {currentResolution}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"设备休眠: {((Screen.sleepTimeout == -1) ? "从不休眠" : "沿用系统设置")}", Array.Empty<GUILayoutOption>());
            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("设备休眠", new GUILayoutOption[1] { DebugSetting.height }))
            {
                Screen.sleepTimeout = Screen.sleepTimeout == -1 ? -2 : -1;
            }

            if (GUILayout.Button("降低质量", new GUILayoutOption[1] { DebugSetting.height }))
            {
                QualitySettings.DecreaseLevel();
            }

            if (GUILayout.Button("提升质量", new GUILayoutOption[1] { DebugSetting.height }))
            {
                QualitySettings.IncreaseLevel();
            }

            if (GUILayout.Button("截取屏幕", new GUILayoutOption[1] { DebugSetting.height }))
            {
                StartCoroutine(ScreenShot());
            }

            GUILayout.EndHorizontal();
        }

        private IEnumerator ScreenShot()
        {
            debugEnabled = false;
            yield return new WaitForEndOfFrame();
            string title = DateTime.Now.ToString("yyyyMMddhhmmss") + ".png";
            Debug.Log($"截图保存路径：{Application.dataPath}/{title}");
            ScreenCapture.CaptureScreenshot(title);
            debugEnabled = true;
            yield return null;
        }

        private void SystemWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUI.contentColor = Color.white;
            GUILayout.Label(" 系统信息", new GUILayoutOption[1] { DebugSetting.heightLowest });
            GUILayout.EndHorizontal();
            scrollSystemView = GUILayout.BeginScrollView(scrollSystemView, new GUIStyle("Box"));
            GUILayout.Label($"操作系统: {SystemInfo.operatingSystem}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"系统内存: {SystemInfo.systemMemorySize}MB", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"处理器: {SystemInfo.processorType}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"处理器数量: {SystemInfo.processorCount}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"显卡名称: {SystemInfo.graphicsDeviceName}", Array.Empty<GUILayoutOption>());
            var graphicsDeviceType = SystemInfo.graphicsDeviceType;
            GUILayout.Label("显卡类型: " + graphicsDeviceType.ToString(), Array.Empty<GUILayoutOption>());
            GUILayout.Label($"显卡内存: {SystemInfo.graphicsMemorySize}MB", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"显卡标识: {SystemInfo.graphicsDeviceID}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"显卡供应商: {SystemInfo.graphicsDeviceVendor}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"显卡供应商标识: {SystemInfo.graphicsDeviceVendorID}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"设备模式: {SystemInfo.deviceModel}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"设备名称: {SystemInfo.deviceName}", Array.Empty<GUILayoutOption>());
            var deviceType = SystemInfo.deviceType;
            GUILayout.Label("设备类型: " + deviceType.ToString(), Array.Empty<GUILayoutOption>());
            GUILayout.Label($"设备唯一标识符: {SystemInfo.deviceUniqueIdentifier}", Array.Empty<GUILayoutOption>());
            GUILayout.EndScrollView();
        }

        private void TimeWindow()
        {
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUI.contentColor = Color.white;
            GUILayout.Label(" 时间信息", new GUILayoutOption[1] { DebugSetting.heightLowest });
            GUILayout.EndHorizontal();
            scrollTimeView = GUILayout.BeginScrollView(scrollTimeView, new GUIStyle("Box"));
            GUILayout.Label("DataTime: " + DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), Array.Empty<GUILayoutOption>());
            GUILayout.Label($"RealtimeSinceStartup: {(int)Time.realtimeSinceStartup}", Array.Empty<GUILayoutOption>());
            GUILayout.Label($"TimeScale: {Time.timeScale}", Array.Empty<GUILayoutOption>());
            GUILayout.Label("Time: " + Time.time.ToString("F"), Array.Empty<GUILayoutOption>());
            GUILayout.Label("Time (Fix): " + Time.fixedTime.ToString("F"), Array.Empty<GUILayoutOption>());
            GUILayout.Label("DeltaTime: " + Time.deltaTime.ToString("F"), Array.Empty<GUILayoutOption>());
            GUILayout.Label("DeltaTime (Fix): " + Time.fixedDeltaTime.ToString("F"), Array.Empty<GUILayoutOption>());
            GUILayout.Label("UnscaledTime: " + Time.unscaledTime.ToString("F"), Array.Empty<GUILayoutOption>());
            GUILayout.Label("UnscaledTime (Fix): " + Time.fixedUnscaledTime.ToString("F"), Array.Empty<GUILayoutOption>());
            GUILayout.Label("UnscaledDeltaTime: " + Time.unscaledDeltaTime.ToString("F"), Array.Empty<GUILayoutOption>());
            GUILayout.Label("UnscaledDeltaTime (Fix): " + Time.fixedUnscaledDeltaTime.ToString("F"), Array.Empty<GUILayoutOption>());
            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            if (GUILayout.Button("0.0x", new GUILayoutOption[1] { DebugSetting.height }))
            {
                Time.timeScale = 0f;
            }

            if (GUILayout.Button("0.1x", new GUILayoutOption[1] { DebugSetting.height }))
            {
                Time.timeScale = 0.1f;
            }

            if (GUILayout.Button("0.2x", new GUILayoutOption[1] { DebugSetting.height }))
            {
                Time.timeScale = 0.2f;
            }

            if (GUILayout.Button("0.5x", new GUILayoutOption[1] { DebugSetting.height }))
            {
                Time.timeScale = 0.5f;
            }

            if (GUILayout.Button("1x", new GUILayoutOption[1] { DebugSetting.height }))
            {
                Time.timeScale = 1f;
            }

            if (GUILayout.Button("2x", new GUILayoutOption[1] { DebugSetting.height }))
            {
                Time.timeScale = 2f;
            }

            if (GUILayout.Button("5x ", new GUILayoutOption[1] { DebugSetting.height }))
            {
                Time.timeScale = 5f;
            }

            if (GUILayout.Button("10x", new GUILayoutOption[1] { DebugSetting.height }))
            {
                Time.timeScale = 10f;
            }

            GUILayout.EndHorizontal();
        }

        private void Awake()
        {
            if (SetSingleton())
            {
                Object.DontDestroyOnLoad(gameObject);
                typeList = new List<Type>();
                debugList = new List<DebugData>();
                componentList = new List<Component>();
                transformList = new List<Transform>();
                typeDict = new Dictionary<Type, Type>();
                debugDict = new Dictionary<LogType, LogData>();
                windowDict = new Dictionary<WindowType, Action>();
                Application.logMessageReceived += new Application.LogCallback(LogMessageReceived);
                LoadComponent();
                AddWindow();
            }
        }

        private void Update()
        {
            if (debugEnabled && Time.realtimeSinceStartup - frameTime >= 1f)
            {
                frameCount = (int)(1.0 / (double)Time.deltaTime);
                frameTime = Time.realtimeSinceStartup;
            }
        }

        private void OnGUI()
        {
            if (debugEnabled)
            {
                var matrix = GUI.matrix;
                var skin = GUI.skin;
                var textField = skin.textField;
                GUI.matrix = Matrix4x4.Scale(DebugSetting.scale);
                textField.alignment = (TextAnchor)3;
                if (windowMax)
                {
                    GUI.Window(0, DebugSetting.rect, new GUI.WindowFunction(MaxWindow), "DEBUGGER");
                }
                else
                {
                    windowRect.size = new Vector2(100f, 60f);
                    windowRect = GUI.Window(0, windowRect, new GUI.WindowFunction(MinWindow), "调试器");
                }

                skin.textField = textField;
                GUI.skin = skin;
                GUI.matrix = matrix;
            }
        }

        private bool SetSingleton()
        {
            if (Instance == null)
            {
                Instance = this;
                return true;
            }

            debugEnabled = false;
            Debug.LogWarning("检测到有多个 <color=#00FFFF>DebugManager</color> 存在, 请确保只存在一个调试器!");
            return false;
        }

        private void AddWindow()
        {
            debugDict.Add((LogType)3, new LogData("Log", show: true, Color.white));
            debugDict.Add((LogType)2, new LogData("Warning", show: true, Color.yellow));
            debugDict.Add((LogType)4, new LogData("Exception", show: true, Color.magenta));
            debugDict.Add(0, new LogData("Error", show: true, Color.red));
            debugDict.Add((LogType)1, new LogData("Assert", show: true, Color.green));
            windowDict.Add(WindowType.Console, ConsoleWindow);
            windowDict.Add(WindowType.Scene, SceneWindow);
            windowDict.Add(WindowType.System, SystemWindow);
            windowDict.Add(WindowType.Memory, MemoryWindow);
            windowDict.Add(WindowType.Screen, ScreenWindow);
            windowDict.Add(WindowType.Time, TimeWindow);
            windowDict.Add(WindowType.Project, ProjectWindow);
        }

        private void MaxWindow(int id)
        {
            GUILayout.BeginArea(DebugSetting.windowRect, "", new GUIStyle("Box"));
            GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
            GUI.contentColor = windowColor;
            if (GUILayout.Button($"FPS: {frameCount}", DebugSetting.fpsButton))
            {
                windowMax = false;
            }

            GUI.contentColor = Color.white;
            foreach (var key in windowDict.Keys)
            {
                GUI.contentColor = (windowType == key) ? Color.white : Color.gray;
                if (GUILayout.Button(key.ToString(), new GUILayoutOption[1] { DebugSetting.height }))
                {
                    if (key == WindowType.Scene)
                    {
                        RefreshTransform();
                        RefreshComponent();
                    }

                    windowType = key;
                }
            }

            GUI.contentColor = Color.white;
            GUILayout.EndHorizontal();
            if (windowDict.ContainsKey(windowType))
            {
                windowDict[windowType]?.Invoke();
            }

            GUILayout.EndArea();
        }

        private void MinWindow(int id)
        {
            GUI.DragWindow(new Rect(0f, 0f, 10000f, 20f));
            GUI.contentColor = windowColor;
            if (GUILayout.Button($"FPS: {frameCount}", DebugSetting.fpsButton))
            {
                windowMax = true;
            }

            GUI.contentColor = Color.white;
        }

        private void OnDestroy() =>
            Application.logMessageReceived -= new Application.LogCallback(LogMessageReceived);
    }
}