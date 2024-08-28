using System;
using System.Reflection;

namespace Xi.EditorExtension
{
    public static class ReflectionHelper
    {
        /// <summary>
        /// 获取指定对象的私有字段的实例。
        /// </summary>
        /// <typeparam name="T">私有字段的类型。</typeparam>
        /// <param name="instance">包含私有字段的对象。</param>
        /// <param name="fieldName">私有字段的名称。</param>
        /// <returns>返回私有字段的实例。</returns>
        public static T GetPrivateField<T>(object instance, string fieldName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException(nameof(fieldName));
            }

            var field = instance.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return field == null
                ? throw new ArgumentException($"Field '{fieldName}' not found in type '{instance.GetType().FullName}'.")
                : (T)field.GetValue(instance);
        }

        /// <summary>
        /// 调用指定对象的私有方法。
        /// </summary>
        /// <typeparam name="TResult">方法返回值的类型。</typeparam>
        /// <param name="instance">包含私有方法的对象。</param>
        /// <param name="methodName">私有方法的名称。</param>
        /// <param name="parameters">传递给方法的参数。</param>
        /// <returns>返回方法的执行结果。</returns>
        public static TResult InvokePrivateMethod<TResult>(object instance, string methodName, params object[] parameters)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            var method = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return method == null
                ? throw new ArgumentException($"Method '{methodName}' not found in type '{instance.GetType().FullName}'.")
                : (TResult)method.Invoke(instance, parameters);
        }

        /// <summary>
        /// 调用指定对象的私有方法（无返回值）。
        /// </summary>
        /// <param name="instance">包含私有方法的对象。</param>
        /// <param name="methodName">私有方法的名称。</param>
        /// <param name="parameters">传递给方法的参数。</param>
        public static void InvokePrivateMethod(object instance, string methodName, params object[] parameters)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            var method = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)
                ?? throw new ArgumentException($"Method '{methodName}' not found in type '{instance.GetType().FullName}'.");
            method.Invoke(instance, parameters);
        }
    }
}