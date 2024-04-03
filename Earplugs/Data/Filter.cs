using ImGuiNET;
using System;
using System.Text.RegularExpressions;

namespace Earplugs.Data {
    [Serializable]
    public class Filter {
        public string Path = "";
        public string Pattern = "";
        public int Sound = -1;
        public float Volume = 0f;

        public void Draw() {
            ImGui.TableNextColumn();
            ImGui.SetNextItemWidth( ImGui.GetContentRegionAvail().X );
            if( ImGui.InputTextWithHint( "##Path", "Path", ref Path, 255 ) ) {
                RefreshPattern();
                Plugin.Configuration.Save();
            }

            ImGui.TableNextColumn();
            ImGui.SetNextItemWidth( ImGui.GetContentRegionAvail().X );
            if( ImGui.InputInt( "##Sound", ref Sound, 0, 0 ) ) Plugin.Configuration.Save();

            ImGui.TableNextColumn();
            ImGui.SetNextItemWidth( ImGui.GetContentRegionAvail().X );
            if( ImGui.InputFloat( "##Volume", ref Volume, 0, 0 ) ) Plugin.Configuration.Save();
        }

        private void RefreshPattern() {
            Pattern = Path
                .Trim()
                .ToLower()
                .Replace( "\\", "/" )
                .Replace( "/", "\\/" )
                .Replace( "*", "(.*?)" );
        }

        public bool GetOverrideVolume( string path, int idx, out float volume ) {
            volume = 0;

            if( string.IsNullOrEmpty( Pattern ) ) return false;
            if( Sound != -1 && Sound != idx ) return false;
            var m = Regex.Match( path, Pattern, RegexOptions.IgnoreCase );
            if( m.Success ) {
                volume = Volume;
                return true;
            }

            return false;
        }
    }
}
