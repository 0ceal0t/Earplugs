using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace Earplugs {
    [Serializable]
    public class Configuration : IPluginConfiguration {
        public int Version { get; set; } = 0;

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface PluginInterface;

        public void Initialize( DalamudPluginInterface pluginInterface ) {
            PluginInterface = pluginInterface;
        }

        public void Save() {
            PluginInterface!.SavePluginConfig( this );
        }
    }
}