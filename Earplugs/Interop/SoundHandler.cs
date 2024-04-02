using Dalamud.Hooking;
using Dalamud.Memory;
using System;

namespace Earplugs.Interop {
    public class SoundHandler {
        private readonly Plugin Plugin;

        public delegate IntPtr InitSoundPrototype( IntPtr a1, IntPtr a2, float a3, int a4, int a5, int a6, int a7, int a8, int a9, int a10, uint a11, uint a12, IntPtr a13, int a14, IntPtr a15, IntPtr a16 );
        public Hook<InitSoundPrototype> InitSoundHook { get; private set; }

        public delegate IntPtr InitSEPrototype( IntPtr a1, IntPtr path, float volume, int idx, int a5, uint a6, uint a7 );
        public Hook<InitSEPrototype> InitSEHook { get; private set; }

        // ========== 

        public SoundHandler( Plugin plugin ) {
            Plugin = plugin;

            InitSEHook = Services.Hooks.HookFromSignature<InitSEPrototype>( "E8 ?? ?? ?? ?? 8B 7D 77 85 FF", InitSEDetour );
            InitSEHook.Enable();

            InitSoundHook = Services.Hooks.HookFromSignature<InitSoundPrototype>( "E8 ?? ?? ?? ?? 48 8B 7D B0", InitSoundDetour );
            InitSoundHook.Enable();
        }

        private IntPtr InitSEDetour( IntPtr a1, IntPtr path, float volume, int idx, int a5, uint a6, uint a7 ) {
            return InitSEHook.Original( a1, path, GetVolume( path, volume, idx ), idx, a5, a6, a7 );
        }

        private IntPtr InitSoundDetour( IntPtr a1, IntPtr path, float volume, int idx, int a5, int a6, int a7, int a8, int a9, int a10, uint a11, uint a12, IntPtr a13, int a14, IntPtr a15, IntPtr a16 ) {
            return InitSoundHook.Original( a1, path, GetVolume( path, volume, idx ), idx, a5, a6, a7, a8, a9, a10, a11, a12, a13, a14, a15, a16 );
        }

        private float GetVolume( IntPtr pathPtr, float passedVolume, int idx ) {
            if( pathPtr == IntPtr.Zero ) return passedVolume;

            var path = MemoryHelper.ReadStringNullTerminated( pathPtr ).Trim();
            if( string.IsNullOrEmpty( path ) ) return passedVolume;

            if( Plugin.Configuration.GetOverrideVolume( path.ToLower(), idx, out var newVolume ) ) return newVolume;

            return passedVolume;
        }

        public void Dispose() {
            InitSEHook.Dispose();
            InitSoundHook.Dispose();
        }
    }
}
