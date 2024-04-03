using Dalamud.Interface.Style;
using System.Numerics;

namespace Earplugs {
    public class UiUtils {
        public static readonly Vector4 RED_COLOR = new( 0.89098039216f, 0.30549019608f, 0.28980392157f, 1.0f );

        public static readonly Vector4 GREEN_COLOR = new( 0.36078431373f, 0.72156862745f, 0.36078431373f, 1.0f );

        public static Vector4 DALAMUD_RED => StyleModel.GetFromCurrent().BuiltInColors.DalamudRed.Value;

        public static Vector4 DALAMUD_YELLOW => StyleModel.GetFromCurrent().BuiltInColors.DalamudYellow.Value;

        public static Vector4 DALAMUD_ORANGE => StyleModel.GetFromCurrent().BuiltInColors.DalamudOrange.Value;

        public static Vector4 PARSED_GREEN => StyleModel.GetFromCurrent().BuiltInColors.ParsedGreen.Value;
    }
}
