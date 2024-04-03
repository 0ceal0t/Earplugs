using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;

namespace Earplugs.Data {
    [Serializable]
    public class Configuration : IPluginConfiguration {
        public int Version { get; set; } = 0;

        public bool Enabled = true;
        public List<FilterCategory> Categories = [];

        [NonSerialized]
        private DalamudPluginInterface PluginInterface;

        public void Initialize( DalamudPluginInterface pluginInterface ) {
            PluginInterface = pluginInterface;
        }

        public bool GetOverrideVolume( string path, int idx, out float volume ) {
            volume = 0;

            foreach( var category in Categories ) {
                if( category.GetOverrideVolume( path, idx, out var _volume ) ) {
                    volume = _volume;
                    return true;
                }
            }

            return false;
        }

        public void Save() {
            PluginInterface!.SavePluginConfig( this );
        }
    }
}
