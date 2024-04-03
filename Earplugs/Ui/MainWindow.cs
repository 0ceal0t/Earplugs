using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Earplugs.Ui {
    public class MainWindow : Window {
        private string SearchInput = "";

        public readonly LogTab LogTab = new();

        public MainWindow() : base( "Earplugs", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse ) {
            Size = new( 500, 700 );
            SizeCondition = ImGuiCond.FirstUseEver;
            LogTab = new();
        }

        public override void Draw() {
            using var _ = ImRaii.PushId( "Earplugs" );

            using var tabBar = ImRaii.TabBar( "TabBar" );
            if( !tabBar ) return;

            using( var tab = ImRaii.TabItem( "Filters" ) ) {
                if( tab ) DrawFilters();
            }

            using( var tab = ImRaii.TabItem( "Log" ) ) {
                if( tab ) LogTab.Draw();
            }

        }

        private void DrawFilters() {
            if( ImGui.Checkbox( "Enable Filters", ref Plugin.Configuration.Enabled ) ) Plugin.Configuration.Save();

            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                if( ImGui.Button( FontAwesomeIcon.Plus.ToIconString() ) ) {
                    Plugin.Configuration.Categories.Add( new() );
                    Plugin.Configuration.Save();
                }
            }

            ImGui.SameLine();
            ImGui.SetNextItemWidth( ImGui.GetContentRegionAvail().X );
            ImGui.InputTextWithHint( "##Search", "Search", ref SearchInput, 255 );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 3 );
            ImGui.Separator();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 3 );

            using var child = ImRaii.Child( "Child" );

            foreach( var (item, idx) in Plugin.Configuration.Categories.WithIndex() ) {
                using var _ = ImRaii.PushId( idx );
                if( !item.SearchMatches( SearchInput.ToLower() ) ) continue;
                if( item.Draw( SearchInput.ToLower() ) ) break;
            }
        }
    }
}
