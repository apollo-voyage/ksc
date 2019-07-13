namespace kOS.Cli
{
    public static class Constants
    {
        public static readonly string DefaultKSPSteamInstallationWin64 = "C:/Program Files (x86)/Steam/steamapps/common/Kerbal Space Program";
        public static readonly string DefaultKSPSteamInstallationWin32 = "C:/Program Files/Steam/SteamApps/common/Kerbal Space Program";
        public static readonly string ArchivePath = "/Ships/Script";
        public static readonly string ConfigFileName = "ksconfig.json";
        public static readonly string DefaultVolumePath = "./src";
        public static readonly string DefaultBootVolumePath = "./boot";
        public static readonly string CurrentDirectory = ".";
        public static readonly string DistDirectory = "./dist";
        public static readonly string DistBootDirectory = "./dist/boot";
        public static readonly string KerboscriptExtension = ".ks";
        public static readonly string KerboscriptSearchPattern = "*.ks";
        public static readonly string KerboscriptCompiledExtension = ".ksm";
        public static readonly string AllVolumes = "all";
    }
}
