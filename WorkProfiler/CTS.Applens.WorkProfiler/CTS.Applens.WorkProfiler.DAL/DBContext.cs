using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace CTS.Applens.LearningWeb.DAL
{
    class DBContext
    {
    }
    public sealed class DBContext
    {
        private static readonly DBContext instance = new DBContext();
        
       // private readonly string con = new string(Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(, "ApplensConnection"));

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static DBContext()
        {
        }

        private DBContext()
        {
        }

        public static DBContext Instance
        {
            get
            {
                return instance;
            }
        }

        public string GetDBConnectionString()
        {
            return "";
        }
    }
}
