﻿using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFBus
{


    public class SETTINGS
    {
        public const string AZURE_STORAGE = "AzureWebJobsStorage";
        public const string LOCKSAGAS = "LockSagas";
    }

    public class SettingsUtil
    {
        public static IConfiguration Configuration { get; set; }


        static SettingsUtil()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");


            Configuration = builder.Build();
        }

        public static T GetSettings<T>(string settingName) where T : IConvertible
        {                       
            return (T)Convert.ChangeType(System.Environment.GetEnvironmentVariable(settingName)?? Configuration[settingName], typeof(T)); 
            
        }
    }
}
