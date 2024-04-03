using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Earplugs.Data;
using Earplugs.Interop;
using Earplugs.Ui;

namespace Earplugs {
    public sealed class Plugin : IDalamudPlugin {
        public static string Name => "Earplugs";
        private const string CommandName = "/earplugs";

        private readonly WindowSystem WindowSystem = new( "Earplugs" );

        public static MainWindow MainWindow { get; private set; }
        public static Configuration Configuration { get; private set; }
        public static SoundHandler SoundHandler { get; private set; }

        public Plugin( DalamudPluginInterface pluginInterface ) {
            pluginInterface.Create<Services>();

            Configuration = Services.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize( pluginInterface );

            Services.CommandManager.AddHandler( CommandName, new CommandInfo( OnCommand ) {
                HelpMessage = "Open config window"
            } );

            SoundHandler = new SoundHandler( this );

            MainWindow = new();
            WindowSystem.AddWindow( MainWindow );

            Services.PluginInterface.UiBuilder.Draw += DrawUi;
            Services.PluginInterface.UiBuilder.OpenConfigUi += OpenUi;
            Services.PluginInterface.UiBuilder.OpenMainUi += OpenUi;
        }

        public void Dispose() {
            WindowSystem.RemoveAllWindows();
            Services.CommandManager.RemoveHandler( CommandName );

            SoundHandler.Dispose();
        }

        private void OnCommand( string command, string args ) {
            MainWindow.IsOpen = true;
        }

        private void DrawUi() {
            WindowSystem.Draw();
        }

        public void OpenUi() {
            MainWindow.IsOpen = true;
        }
    }
}
