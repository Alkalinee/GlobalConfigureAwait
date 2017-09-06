using System;

namespace Fody
{
    /// <summary>
    ///     Use this attribute to configure all unconfigured awaits in the region
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct |
                    AttributeTargets.Method)]
    public class ConfigureAwaitAttribute : Attribute
    {
        /// <summary>
        ///     Configures all awaiters used in the region this attribute was applied on
        /// </summary>
        /// <param name="continueOnCapturedContext">
        ///     <code>true</code> to attempt to marshal the continuation back to the original
        ///     context captured; otherwise, <code>false</code>.
        /// </param>
        public ConfigureAwaitAttribute(bool continueOnCapturedContext)
        {
        }
    }
}