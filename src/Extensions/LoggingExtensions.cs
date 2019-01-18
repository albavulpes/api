using AlbaVulpes.API.Models.Config;
using AlbaVulpes.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Seq;
using NLog.Targets.Wrappers;

namespace AlbaVulpes.API.Extensions
{
    public static class LoggingExtensions
    {
        public static void UseCustomLogging(this IApplicationBuilder builder)
        {
            var config = new LoggingConfiguration();

            var logconsole = new ConsoleTarget();

            config.AddRule(NLog.LogLevel.Info, NLog.LogLevel.Fatal, logconsole);

            NLog.LogManager.Configuration = config;
        }

        public static void UseSeqLogging(this IApplicationBuilder builder)
        {
            var appSettings = builder.ApplicationServices.GetService<IOptions<AppSettings>>().Value;
            var appSecrets = builder.ApplicationServices.GetService<SecretsManagerService>().Get();

            var logseq = new SeqTarget
            {
                ServerUrl = appSettings.Seq.ServerUrl,
                ApiKey = appSecrets.Seq_ApiKey
            };

            // Ensures that writes to Seq do not block the application
            var bufferingWrapper = new BufferingTargetWrapper(logseq);

            NLog.LogManager.Configuration.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, bufferingWrapper);
        }
    }
}