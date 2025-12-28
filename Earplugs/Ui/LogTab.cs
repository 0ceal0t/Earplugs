using Dalamud.Bindings.ImGui;
using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using System.Collections.Generic;
using System.Linq;

namespace Earplugs.Ui {
    public class LogTab {
        private bool Log = false;
        private string SearchInput = "";
        private readonly Queue<string> Paths = new();

        public void Draw() {
            using var _ = ImRaii.PushId( "Log" );

            ImGui.Checkbox( "Enable Logging", ref Log );

            ImGui.SameLine();
            ImGui.SetNextItemWidth( 100f );
            if( ImGui.InputInt( "Limit", ref Plugin.Configuration.LogLimit, 0, 0 ) ) Plugin.Configuration.Save();

            using( var color = ImRaii.PushColor( ImGuiCol.Button, UiUtils.RED_COLOR ) )
            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                if( ImGui.Button( FontAwesomeIcon.Trash.ToIconString() ) ) {
                    Paths.Clear();
                }
            }
            ImGui.SameLine();
            ImGui.SetNextItemWidth( ImGui.GetContentRegionAvail().X );
            ImGui.InputTextWithHint( "##Search", "Search", ref SearchInput, 255 );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var child = ImRaii.Child( "Child", new( -1, -1 ), true );

            var searchPaths = string.IsNullOrEmpty( SearchInput ) ?
                [.. Paths] :
                Paths.Where( x => x.Contains( SearchInput, System.StringComparison.CurrentCultureIgnoreCase ) ).ToList();

            ImGuiListClipperPtr clipper;
            unsafe {
                clipper = new ImGuiListClipperPtr( ImGuiNative.ImGuiListClipper() );
            }

            clipper.Begin( searchPaths.Count );

            while( clipper.Step() ) {
                for( var idx = clipper.DisplayStart; idx < clipper.DisplayEnd; idx++ ) {
                    if( idx < 0 || idx >= searchPaths.Count ) continue;
                    var path = searchPaths[idx];
                    if( ImGui.Selectable( $"{path}##{idx}", false ) ) ImGui.SetClipboardText( path );
                }
            }

            clipper.End();
        }

        public void AddPath( string path ) {
            if( !Log ) return;
            Paths.Enqueue( path );
            while( Paths.Count > Plugin.Configuration.LogLimit ) Paths.Dequeue();
        }
    }
}
