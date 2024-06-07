namespace CTS.Applens.WorkProfiler.DAL
{
    using Microsoft.Extensions.Configuration;
    using System;

    /// <summary>
    /// Data Access Class
    /// Via this class , SingletonDB class will be instantialed
    /// </summary>
    public class AppLensDataAccess
    {
        private string connectionstring;

        public string ConnectionString
        {
            get { return connectionstring ;  }
            set { connectionstring = value; }
        }
        public AppLensDataAccess()
        {
           this.connectionstring = AppLensSingletonDB.Instance().GetDBConnection();
        }
        public AppLensDataAccess(IConfiguration config)
        {
           this.connectionstring = AppLensSingletonDB.GetInstance(config.GetConnectionString("AppLensConnection")).GetDBConnection();
            
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public sealed class AppLensSingletonDB
    {
        private static AppLensSingletonDB instance = null;
        private readonly string con = string.Empty;
        private static Object _mutex = new Object();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AppLensSingletonDB()
        {
        }

        private AppLensSingletonDB()
        {
        }
        private AppLensSingletonDB(string connectionstring)
        {
            con = connectionstring;
        }
        public static AppLensSingletonDB GetInstance(string connectionstring)
        {
            if (instance == null)
            {
                lock (_mutex) 
                {
                    if (instance == null)
                    {
                        instance = new AppLensSingletonDB(connectionstring);
                    }
                }
            }

            return instance;
        }
        public static AppLensSingletonDB Instance()
        {
            if (instance == null)
            {
                lock (_mutex)
                {
                    if (instance == null)
                    {
                        instance = new AppLensSingletonDB("");
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
