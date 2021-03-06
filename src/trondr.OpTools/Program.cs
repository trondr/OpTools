﻿using System;
using System.Diagnostics;
using System.Threading;
using Common.Logging;
using NCmdLiner;
using NCmdLiner.Exceptions;
using trondr.OpTools.Infrastructure;
using trondr.OpTools.Library.Infrastructure;

namespace trondr.OpTools
{
    class Program
    {
        [STAThread]
        static int Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new AssemblyResolver().ResolveHandler;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            int returnValue;
            try
            {                
                returnValue = WireUpAndRun(args);
            }
            catch (Exception ex)
            {
                var message = $"Fatal error when wiring up the application.{Environment.NewLine}{ex}";
                WriteErrorToEventLog(message);
                returnValue = 3;
            }
            return returnValue;
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            WriteErrorToEventLog(unhandledExceptionEventArgs.ExceptionObject.ToString());
        }

        private static int WireUpAndRun(string[] args)
        {
            var returnValue = 0;
            var logger = GetMainLogger();
            using (var bootStrapper = new BootStrapper())
            {
                var applicationInfo = bootStrapper.Container.Resolve<IApplicationInfo>();
                try
                {
                    applicationInfo.Authors = @"trondr@outlook.com";
                    // ReSharper disable once CoVariantArrayConversion
                    object[] commandTargets = bootStrapper.Container.ResolveAll<CommandDefinition>();
                    logger.InfoFormat("Start: {0}.{1}. Command line: {2}", applicationInfo.Name, applicationInfo.Version, Environment.CommandLine);
                    returnValue = CmdLinery.Run(commandTargets, args, applicationInfo, bootStrapper.Container.Resolve<IMessenger>());
                    return returnValue;
                }
                catch (MissingCommandException ex)
                {
                    logger.ErrorFormat("Missing command. {0}", ex.Message);
                    returnValue = 1;
                }
                catch (UnknownCommandException ex)
                {
                    logger.Error(ex.Message);
                    returnValue = 1;
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error when exeuting command. {0}", ex.ToString());
                    returnValue = 2;
                }
                finally
                {
                    logger.InfoFormat("Stop: {0}.{1}. Return value: {2}", applicationInfo.Name, applicationInfo.Version, returnValue);
#if DEBUG
                    // ReSharper disable once RedundantNameQualifier
                    System.Console.WriteLine("Terminating in 5 seconds...");
                    Thread.Sleep(5000);
#endif
                }
            }
            return returnValue;
        }


        private static ILog GetMainLogger()
        {
            try
            {
                var logger = LogManager.GetLogger<Program>();
                return logger;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get logger. ", ex);
            }
        }

        private static void WriteErrorToEventLog(string message)
        {
            // ReSharper disable once RedundantNameQualifier
            System.Console.WriteLine(message);
            const string logName = "Application";
            using (var eventLog = new EventLog(logName))
            {
                eventLog.Source = logName;
                eventLog.WriteEntry(message, EventLogEntryType.Information, 101, 1);
            }
        }

    }
}
