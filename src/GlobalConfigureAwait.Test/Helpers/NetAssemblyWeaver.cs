namespace GlobalConfigureAwait.Test.Helpers
{
    public class NetAssemblyWeaver : AssemblyWeaver
    {
        public NetAssemblyWeaver() : base("AssemblyToProcess.Net.dll")
        {
        }
    }
}