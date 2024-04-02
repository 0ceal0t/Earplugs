using Dalamud.Game.Command;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using Earplugs.Interop;
using Earplugs.Ui;

namespace Earplugs {
    public sealed class Plugin : IDalamudPlugin {
        public static string Name => "Earplugs";
        private const string CommandName = "/earplugs";

        public WindowSystem WindowSystem = new( "Earplugs" );
        public Configuration Configuration { get; private set; }
        public SoundHandler SoundHandler { get; private set; }
        public MainWindow MainWindow { get; private set; }

        public Plugin( DalamudPluginInterface pluginInterface ) {
            pluginInterface.Create<Services>();

            Configuration = Services.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize( pluginInterface );

            Services.CommandManager.AddHandler( CommandName, new CommandInfo( OnCommand ) {
                HelpMessage = "Open config window"
            } );

            SoundHandler = new SoundHandler( this );

            MainWindow = new( this );
            WindowSystem.AddWindow( MainWindow );

            Services.PluginInterface.UiBuilder.Draw += DrawUI;
            Services.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose() {
            WindowSystem.RemoveAllWindows();
            Services.CommandManager.RemoveHandler( CommandName );

            SoundHandler.Dispose();
        }

        private void OnCommand( string command, string args ) {
            MainWindow.IsOpen = true;
        }

        private void DrawUI() {
            WindowSystem.Draw();
        }

        public void DrawConfigUI() {
            MainWindow.IsOpen = true;
        }
    }
}
