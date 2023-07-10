using System;
using System.Data.SQLite;

namespace LoootCreate
{
    public class SqLiteBaseRepository
    {
        public static string DbFile
        {
            get { return Environment.CurrentDirectory + @"\LoootDB.db"; }
        }

        public static SQLiteConnection SimpleDBConnection()
        {
            //return new SQLiteConnection($"Date Source={DbFile};Version=3;");
            return new SQLiteConnection($"Date Source={DbFile}");
        }

        public static string DBConnectionString
        {
            get { return $"Data Source='{DbFile}'"; }
        }
    }
}