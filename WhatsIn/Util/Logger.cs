using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace WhatsIn.Util
{
    public static class Logger
    {
        public static void LogError(Type source, Exception ex)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(source);
            logger.Error(ex.ToString());
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendLogToEmail"]))
                LogToEmail(source, ex.ToString());
        }

        public static void LogInfo(Type source, string message)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(source);
            logger.Info(message);
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendLogToEmail"]))
                LogToEmail(source, message);
        }

        public static void LogDebug(Type source, string message)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(source);
            logger.Debug(message);
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["SendLogToEmail"]))
                LogToEmail(source, message);
        }

        public static void LogToEmail(Type source, string message)
        {
            WebMail.SmtpServer = ConfigurationManager.AppSettings["mailHost"];
            WebMail.From = ConfigurationManager.AppSettings["mailFromSupport"];
            WebMail.SmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["mailPort"]);
            WebMail.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["enableSsl"]);
            WebMail.UserName = ConfigurationManager.AppSettings["mailFromSupport"];
            WebMail.Password = ConfigurationManager.AppSettings["mailPassword"];

            WebMail.Send(
             to: ConfigurationManager.AppSettings["mailTo"],
             subject: string.Concat("ERROR: ", source.ToString()),
             body: message,
             isBodyHtml: false
            );
        }

    }
}