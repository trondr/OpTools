#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits:
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion

using System;
using Common.Logging;

namespace trondr.OpTools.Library.Module.Common
{
    /// <summary>
    /// Measure time of an operation.
    /// </summary>
    /// <example>
    /// using(StopWatch stopWatch = new StopWatch("Some describing name"))          //Start the watch on construction
    /// {
    ///    //Measure the time of all operations in this scope.
    /// }                                                                           //Stop the watch and log the result on dispose
    /// </example>
    public class StopWatch : IDisposable
    {
        private readonly ILog _logger;
        private DateTime _startTime;
        private DateTime _stopTime;
        private readonly string _name;
        private bool _disposed;
        public StopWatch(string name, ILog logger = null)
        {
            _name = name;
            _logger = logger;
            LogStart();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~StopWatch()
        {
            if (_disposed) return;
            _stopTime = DateTime.Now;
            LogStop();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                LogStop();
                _disposed = true;
            }
        }

        private void LogStart()
        {
            _startTime = DateTime.Now;
            WriteLog($"Start: {_name} at {_startTime:s}");            
        }

        private void LogStop()
        {
            _stopTime = DateTime.Now;
            var resultTime = _stopTime - _startTime;
            WriteLog($"Stop: {_name} at {_stopTime:s}. Duration: {resultTime.TotalSeconds} seconds. Duration in minutes: {resultTime.TotalMinutes} minutes");
        }

        private void WriteLog(string message)
        {
            if (_logger != null)
            {
                _logger.Info(message);
            }
            else
            {
                Console.WriteLine(message);
            }
        }
    }
}
