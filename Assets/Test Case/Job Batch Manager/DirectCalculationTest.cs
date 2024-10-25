using UnityEngine;
using UnityEngine.Profiling;

namespace Xi.TestCase
{
    public class DirectCalculationTest : MonoBehaviour
    {
        private void Update()
        {
            Profiler.BeginSample($"{nameof(DirectCalculationTest)}");
            PerformCalculations();
            Profiler.EndSample();
        }

        private readonly int _numberOfCalculations = 200;

        private void PerformCalculations()
        {
            float[][] results = new float[_numberOfCalculations][];

            for (int i = 0; i < _numberOfCalculations; i++)
            {
                results[i] = IntensiveCalculation(i);
            }

            // Optional: Output results or measure performance
            Debug.Log("Direct calculations completed.");
        }

        private float[] IntensiveCalculation(int index)
        {
            float[] result = new float[_numberOfCalculations];
            for (int i = 0; i < _numberOfCalculations; i++)
            {
                result[i] = Mathf.Sqrt(index + i);
            }

            return result;
        }
    }
}
