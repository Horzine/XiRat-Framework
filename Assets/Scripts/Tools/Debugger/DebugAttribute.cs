using System;

namespace Xi.Tools
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    internal sealed class DebugAttribute : Attribute
    {
        public readonly Type type;

        public DebugAttribute(Type type) => this.type = type;
    }
}
