using EcsRx.Infrastructure;
using EcsRx.Infrastructure.Dependencies;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TNet.Dependencies;

namespace EcsRx.App.Application
{
    public class TNet_App : EcsRxApplication
    {
        /// <summary>
        /// 
        /// </summary>
        protected static string LoginCharFormat =
@"
┏^ǒ^*★*^ǒ^*☆*^ǒ^*★*^ǒ^*☆*^ǒ^★*^ǒ^┓ 
┃╭の╮┏┯┓┏┯┓ ┏┯┓┏┯┓　╬　┃ 
┃ ╲╱ ┠登┨┠录Lǒvの服┨┠务┨╭║╮┃ 
┃┗灵┛┗┷┛┗┷┛ ┗┷┛┗┷┛ ╲╱ ┃ 
┃ TNet Server version {0}
┃ Login Server 
┗^ǒ^*★*^ǒ^*☆*^ǒ^*★*^ǒ^*☆*^ǒ^★*^ǒ^┛      
 ";
        private static string CharFormat =
$@"
┏^ǒ^*★*^ǒ^*☆*^ǒ^*★*^ǒ^*☆*^ǒ^★*^ǒ^┓ 
┃╭の╮┏┯┓┏┯┓ ┏┯┓┏┯┓　╬　┃ 
┃ ╲╱ ┠名┨┠将Lǒvの三┨┠国┨╭║╮┃ 
┃┗灵┛┗┷┛┗┷┛ ┗┷┛┗┷┛ ╲╱ ┃ 
┃TNet  
┃{TNet.Runtime.GameZone.bit} bit  
┃Running in {TNet.Runtime.GameZone.ps} platform 
┗^ǒ^*★*^ǒ^*☆*^ǒ^*★*^ǒ^*☆*^ǒ^★*^ǒ^┛      
 ";


    protected override IDependencyContainer DependencyContainer => new NinjectDependencyContainer();//throw new NotImplementedException();

        protected override void ApplicationStarted()
        {
            RegisterAllBoundSystems();
            var defaultPool = EntityCollectionManager.GetCollection();
        var    _enemy = defaultPool.CreateEntity();
           _enemy.AddComponent<mjsg.CS_Script>();

            ReadAllSettings();
           
            Console.Title = ConfigurationManager.AppSettings["Name"];

            //LogManager.Configuration = new XmlLoggingConfiguration("NLog.config");

            ////            var config = LogManager.Configuration;// new LoggingConfiguration();



            //var consoleTarget = new ColoredConsoleTarget("target12")
            //{
            //    Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            //};
            //LogManager.Configuration.AddTarget(consoleTarget);
            ////var fileTarget = new FileTarget("target2")
            ////{
            ////    FileName = "${basedir}/file.txt",
            ////    Layout = "${longdate} ${level} ${message}  ${exception}"
            ////};
            ////config.AddTarget(fileTarget);

            ////config.AddRuleForOneLevel(LogLevel.Error, fileTarget); // only errors to file
            //LogManager.Configuration.AddRuleForAllLevels(consoleTarget); // all to console
            ////LogManager.Configuration = config;
            //Logger logger = LogManager.GetLogger("Name.Space.Class2");
            //logger.Trace("trace log message");
            //logger.Debug("debug log message");
            //logger.Info("info log message");
            //logger.Warn("warn log message");
            //logger.Error(CharFormat);
            //logger.Fatal(fas);
            // TNet.Log.TraceLog.WriteError("fafa");
            TNet.Log.TraceLog.WriteInfo(CharFormat);
            ConsoleColor currentForeColor = Console.BackgroundColor;
          //  SetColor(ConsoleColor.Cyan);
         
        //    SetColor(currentForeColor);

        }

        private void SetColor(ConsoleColor color)
        {
            try
            {
                Console.BackgroundColor = color;
            }
            catch { }
        }

        static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
               
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }
    }
}
