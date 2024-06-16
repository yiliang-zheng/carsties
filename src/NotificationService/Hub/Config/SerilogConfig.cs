﻿using Serilog;

namespace Hub.Config;

public static class SerilogConfig
{
    public static IHostBuilder AddSerilog(this IHostBuilder host, IConfiguration config)
    {
        host.UseSerilog((context, loggerConfig) =>
        {
            loggerConfig.ReadFrom.Configuration(context.Configuration);
        });
        return host;
    }
}