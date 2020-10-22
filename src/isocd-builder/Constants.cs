using System;
using System.IO;

namespace isocd_builder {
    public static class isocd_builder_constants {
        public const string STANDARD_IDENTIFIER = "CD001";
        public const string SYSTEM_IDENTIFIER = "CDTV                            ";
        public const string VOLUME_IDENTIFIER = "CD32_TEST";

        public static string ISOCDWIN_DATA_PREPARER_IDENTIFIER = $" - ISOCD-Win by Ben Squibb -";
        public static string ISOCDWIN_PUBLIC_DOCUMENTS_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Amiga Files", "ISOCD-Win");

        public const string CDTV_TRADEMARK_FILE = "CDTV.TM";
        public const int    CDTV_TRADEMARK_FILE_SIZE = 22152;
        public const string CDTV_TRADEMARK_FILE_SHA1_HASH = "fd3e764e6393974dea05612909e25ddb2124eb8b";
        public const string CD32_TRADEMARK_FILE = "CD32.TM";
        public const int    CD32_TRADEMARK_FILE_SIZE = 2048;
        public const string CD32_TRADEMARK_FILE_SHA1_HASH = "c5ffcef2a5e33d2df606185823cd95d1c174d65f";
        public const string TRADEMARK_FILE_SOURCES_NAME = "TmFileSources.json";
		public const string TRADEMARK_FILE_SOURCES_URL = "https://raw.githubusercontent.com/Stat-Mat/isocd-win/master/Source/isocd-builder/" + TRADEMARK_FILE_SOURCES_NAME;

        public const string CACHE_DATA_NAME = "CR";
        public const string CACHE_DIR_NAME = "CD";
        public const string FILE_LOCK_NAME = "PL";
        public const string FILE_HANDLE_NAME = "PF";
        public const string RETRIES_NAME = "RC";
        public const string DIRECT_READ_NAME = "DR";
        public const string FAST_SEARCH_NAME = "FS";
        public const string SPEED_INDEPENDENT_NAME = "SI";
        public const string TRADEMARK_NAME = "TM";

        public const int SECTOR_SIZE = 2048;
        public const int MAX_SECTORS_CDR74 = 333000;
        public const int MAX_SECTORS_CDR80 = 360000;
        public const int BIG_ENDIAN_PATH_TABLE_SECTOR = 19;
        public const int DIR_FLAG = 2;
        public const int MINIMUM_PATH_TABLE_RECORD_SIZE = 9;
        public const int MINIMUM_DIR_RECORD_SIZE = 34;

		public const int COPYTO_BUF_SIZE = 1048576;

        public const string WINUAE_ATTRIBUTES_FILE = "_UAEFSDB.___;1";

        public const int DEFAULT_DATA_CACHE = 8;
        public const int DEFAULT_DIR_CACHE = 16;
        public const int DEFAULT_FILE_LOCK = 40;
        public const int DEFAULT_FILE_HANDLE = 16;
        public const int DEFAULT_RETRIES = 32;
    }
}
