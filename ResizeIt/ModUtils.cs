using ColossalFramework.Plugins;
using System.Reflection;

namespace ResizeIt
{
    public static class ModUtils
    {
        public static bool IsModEnabled(string name)
        {
            foreach (PluginManager.PluginInfo plugin in PluginManager.instance.GetPluginsInfo())
            {
                foreach (Assembly assembly in plugin.GetAssemblies())
                {
                    if (assembly.GetName().Name.ToLower() == name)
                    {
                        return plugin.isEnabled;
                    }
                }
            }

            return false;
        }
    }
}