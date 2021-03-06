﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moxy.Core;
using Moxy.Data;
using Moxy.Services.Cms;
using Moxy.Services.Config;
using Moxy.Services.System;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using Moxy.Uploader;

namespace Moxy.Tests.ServiceTest
{
    public class App
    {
        public static readonly LoggerFactory MyLoggerFactory
        = new LoggerFactory(new[] { new Log4NetProvider("log4net.config") });
        public static IServiceProvider Init()
        {
            var Configuration = new ConfigurationBuilder()
                                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                                .AddJsonFile(path: $"appsettings.json")
                                .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
                                .Build();
            IServiceCollection services = new ServiceCollection();
            services.AddLogging(e => e.AddLog4Net("log4net.config"));
            services
                    .AddDbContext<MoxyDbContext>(opt =>
                    {
                        //优先使用mysql
                        if (!string.IsNullOrEmpty(Configuration.GetConnectionString("MySql")))
                        {
                            opt.UseMySql(Configuration.GetConnectionString("MySql"));
                        }
                        else if (!string.IsNullOrEmpty(Configuration.GetConnectionString("Memory")))
                        {
                            opt.UseInMemoryDatabase(Configuration.GetConnectionString("Memory"));
                        }
                        else
                        {
                            throw new NotImplementedException("请配置数据库连接字符串");
                        }
                    })
                    .AddUnitOfWork<MoxyDbContext>();

            services.AddTransient<ISystemService, SystemService>();
            services.AddTransient<IArticleService, ArticleService>();
            services.AddTransient<IConfigService, ConfigService>();
            services.AddTransient<IWebContext, DefaultWebContext>();
            services.AddAliyunUploader(options => Configuration.GetSection("AliyunOssSetting").Bind(options));

            services.AddDistributedMemoryCache();

            //构建容器
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ServiceLocator.Instance = serviceProvider;
            return serviceProvider;
        }
    }
}
