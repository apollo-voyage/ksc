namespace kOS.Cli
{
    public static class Constants
    {
        public static readonly string DefaultKSPSteamInstallationWin64 = "C:/Program Files (x86)/Steam/steamapps/common/Kerbal Space Program";
        public static readonly string DefaultKSPSteamInstallationWin32 = "C:/Program Files/Steam/SteamApps/common/Kerbal Space Program";
        public static readonly string ArchivePath = "/Ships/Script";
        public static readonly string ConfigFileName = "ksc.json";
        public static readonly string DefaultVolumePath = "./src";
        public static readonly string DefaultBootVolumePath = "./boot";
        public static readonly string DefaultBootScriptFilename = "boot.ks";
        public static readonly string DefaultBootScriptContent = "print \"Implement your boot logic here...\".";
        public static readonly string DefaultScriptVolumePath = "./scripts";
        public static readonly string DefaultCompileScriptFilename = "compile.ks";
        public static readonly string DefaultCompileScriptContent = "print \"Implement your compile logic here...\".";
        public static readonly string DefaultDeployScriptFilename = "deploy.ks";
        public static readonly string DefaultDeployScriptContent = "print \"Implement your deploy logic here...\".";
        public static readonly string CurrentDirectory = ".";
        public static readonly string DistDirectory = "./dist";
        public static readonly string DistBootDirectory = "./dist/boot";
        public static readonly string KerboscriptExtension = ".ks";
        public static readonly string KerboscriptSearchPattern = "*.ks";
        public static readonly string KerboscriptCompiledExtension = ".ksm";
        public static readonly string AllVolumes = "all";
        public static readonly string ProgramKsc = "ksc";
    }
}
