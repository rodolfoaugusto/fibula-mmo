﻿namespace OpenTibia.Server
{
    public class ServerConfiguration
    {
        public const string BaseDirLocalPath = @"data\";

        public const string LiveMapDirectory = BaseDirLocalPath + "map";
        public const string OriginalMapDirectory = BaseDirLocalPath + "origmap";
        
        public const string DataFilesDirectory = "dat";
        public const string MonsterFilesDirectory = "mon";

        public const string MoveUseFileName = "moveuse.dat";
        public const string ObjectsFileName = "objects.srv";
        public const string SpawnsFileName = "monster.db";

        public static bool SupressInvalidItemWarnings { get; set; }
    }
}
