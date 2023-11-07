using JetBrains.Annotations;
using System;

namespace Reflex.Attributes
{
    [MeansImplicitUse(ImplicitUseKindFlags.Assign)]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    {
    }
}