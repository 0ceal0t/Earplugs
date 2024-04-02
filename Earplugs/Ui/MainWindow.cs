using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace Earplugs.Ui {
    public class MainWindow : Window {
        private readonly Plugin Plugin;

        public MainWindow( Plugin plugin ) : base( "Earplugs", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse ) {

        }

        public override void Draw() {

        }
    }
}
