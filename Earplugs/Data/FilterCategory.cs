using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Earplugs.Data {
    [Serializable]
    public class FilterCategory {
        public string Name = "New Filter";
        public bool Enabled = true;
        public List<Filter> Filters = [];

        public bool Draw( string searchInput ) {
            using( var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing ) ) {
                if( ImGui.Checkbox( "##Enabled", ref Enabled ) ) Plugin.Configuration.Save();

                using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                    ImGui.SameLine();
                    if( ImGui.Button( FontAwesomeIcon.Edit.ToIconString() ) ) ImGui.OpenPopup( "Edit" );
                }

                using( var popup = ImRaii.Popup( "Edit" ) ) {
                    if( popup ) {
                        if( ImGui.InputTextWithHint( "##Name", "Name", ref Name, 255 ) ) Plugin.Configuration.Save();

                        using var color = ImRaii.PushColor( ImGuiCol.Button, UiUtils.RED_COLOR );
                        using var font = ImRaii.PushFont( UiBuilder.IconFont );
                        ImGui.SameLine();
                        if( ImGui.Button( FontAwesomeIcon.Trash.ToIconString() ) ) {
                            Plugin.Configuration.Categories.Remove( this );
                            Plugin.Configuration.Save();
                            ImGui.CloseCurrentPopup();
                            return true;
                        }
                    }
                }

                ImGui.SameLine();
                if( !ImGui.CollapsingHeader( $"{Name}###Header" ) ) return false;
            }

            DrawTable( searchInput );

            ImGui.Separator();

            return false;
        }

        private void DrawTable( string searchInput ) {
            using var style = ImRaii.PushStyle( ImGuiStyleVar.CellPadding, new Vector2( 4, 4 ) );
            using var table = ImRaii.Table( "Table", 4,
                ImGuiTableFlags.RowBg | ImGuiTableFlags.NoHostExtendX | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.PadOuterX );
            if( !table ) return;

            ImGui.TableSetupColumn( "##Icon", ImGuiTableColumnFlags.None, 25f );
            ImGui.TableSetupColumn( "Path", ImGuiTableColumnFlags.WidthStretch );
            ImGui.TableSetupColumn( "Sound", ImGuiTableColumnFlags.None, 100f );
            ImGui.TableSetupColumn( "Volume", ImGuiTableColumnFlags.None, 100f );

            ImGui.TableHeadersRow();

            // ===========

            foreach( var (item, idx) in Filters.WithIndex() ) {
                if( !item.SearchMatches( searchInput ) ) continue;
                using var _ = ImRaii.PushId( idx );

                ImGui.TableNextRow();
                ImGui.TableNextColumn();

                using( ImRaii.PushColor( ImGuiCol.Button, UiUtils.RED_COLOR ) )
                using( ImRaii.PushFont( UiBuilder.IconFont ) ) {
                    if( ImGui.Button( FontAwesomeIcon.Trash.ToIconString() ) ) {
                        Filters.Remove( item );
                        Plugin.Configuration.Save();
                        break;
                    }
                }

                item.Draw();
            }

            // ===========

            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            using var font = ImRaii.PushFont( UiBuilder.IconFont );
            if( ImGui.Button( FontAwesomeIcon.Plus.ToIconString() ) ) {
                Filters.Add( new() );
                Plugin.Configuration.Save();
            }
        }

        public bool GetOverrideVolume( string path, int idx, out float volume ) {
            volume = 0;

            foreach( var filter in Filters ) {
                if( filter.GetOverrideVolume( path, idx, out var _volume ) ) {
                    volume = _volume;
                    return true;
                }
            }

            return false;
        }

        public bool SearchMatches( string search ) =>
            string.IsNullOrEmpty( search )
            || Name.Contains( search, StringComparison.CurrentCultureIgnoreCase )
            || Filters.Any( x => x.SearchMatches( search ) );
    }
}
