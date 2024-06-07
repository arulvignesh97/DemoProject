using Microsoft.Extensions.Configuration;
using System;

namespace CTS.Applens.WorkProfiler.Entities
{
    public class DBContext
    {
        private string connectionString;

        public string ConnectionString
        {
            get { return connectionString;  }
            set { connectionString = value; }
        }
        public DBContext()
        {
             this.connectionString = DBContextSingleton.Instance().GetDBConnection();
        }
        public DBContext(IConfiguration config)
        {
            this.connectionString = DBContextSingleton.GetInstance(config.GetConnectionString("AppLensConnection")).GetDBConnection();
        }
    }
    public sealed class DBContextSingleton
    {
        private static DBContextSingleton instance = null;
        private readonly string con = string.Empty;
        private static Object _mutex = new Object();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static DBContextSingleton()
        {
        }

        private DBContextSingleton()
        {
        }
        private DBContextSingleton(string connectionString)
        {
            con = connectionString;
        }
        public static DBContextSingleton GetInstance(string connectionString)
        {
            if (instance == null)
            {
                lock (_mutex)
                {
                    if (instance == null)
                    {
                        instance = new DBContextSingleton(connectionString);
                    }
                }
            }

            return instance;
        }
        public static DBContextSingleton Instance()
        {
            if (instance == null)
            {
                lock (_mutex)
                {
                    if (instance == null)
                    {
                        instance = new DBContextSingleton("");
                    }
                }
            }

            return instance;
        }

        public string GetDBConnection()
        {
            return con;
        }
    }
}
