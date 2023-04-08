using Feature.Core.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Feature.Api.Config
{
    public class ApiConfig: ConfigBase
    {
        public string ApiUrl { get; set; }


        public static ApiConfig Build(IConfiguration configuration)
        {
            var prefix = "ApiConfig";
            var config = new ApiConfig();
            config.ConnectionString = configuration[$"{prefix}:ConnectionString"];
            config.ApiUrl = configuration[$"{prefix}:Url"];
            return config;
        }
    }
}
