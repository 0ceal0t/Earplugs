using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace Earplugs.Data {
    [Serializable]
    public class Configuration : IPluginConfiguration {
        public int Version { get; set; } = 0;

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface PluginInterface;

        public void Initialize( DalamudPluginInterface pluginInterface ) {
            PluginInterface = pluginInterface;
        }

        public bool GetOverrideVolume( string path, int idx, out float volume ) {
            volume = 0;

            return false;
        }

        public void Save() {
            PluginInterface!.SavePluginConfig( this );
        }
    }
}
