

namespace UseJobObject
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    internal class ProcessHandle
    {
        private static readonly int CtrlBreakEvent = 1;
        private static readonly DateTime Origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        private readonly Process process;
        private readonly ProcessStartInfo startInfo;
        private readonly int shutdownTimerInterval = 2000;
        private readonly int counterMonitorTimerInterval = 10000;
        private readonly int shutdownWaitTime;
        private readonly Job jobObject;
        private readonly ILogger logger;

        private Timer shutdownTimer;
        private Stopwatch shutDownTimeElapsed = new Stopwatch();
        private Timer processCounterMonitorTimer;

        private bool isDisposed;

        public ProcessHandle(
            ProcessStartInfo processStartInfo,
            string processIdentifier,
            int shutdownWaitTimeInMs,
            ILogger logger)
        {
            if (processStartInfo == null)
            {
                throw new ArgumentNullException(nameof(processStartInfo));
            }

            if (string.IsNullOrEmpty(processIdentifier))
            {
                throw new ArgumentNullException(nameof(processIdentifier));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.startInfo = processStartInfo;
            this.shutdownWaitTime = shutdownWaitTimeInMs;
            this.logger = logger;

            this.process = new Process()
            {
                StartInfo = this.startInfo,
                EnableRaisingEvents = true
            };

            this.process.OutputDataReceived += this.OnOutput;
            this.process.ErrorDataReceived += this.OnError;

            this.processCounterMonitorTimer = new Timer(this.JobCountersMonitor, null, Timeout.Infinite, Timeout.Infinite);

            var totalSecondsSinceEpoch = (long)(DateTime.UtcNow - Origin).TotalSeconds;
            this.jobObject = new Job(processIdentifier + "_" + totalSecondsSinceEpoch.ToString(CultureInfo.InvariantCulture));
        }

        public virtual int Id
        {
            get
            {
                return this.process.Id;
            }
        }

        public virtual void Start()
        {
            if (!this.process.Start())
            {
                throw new Exception(string.Format(CultureInfo.InvariantCulture, "Could not start process {0}", this.startInfo.FileName));
            }

            this.jobObject.AddProcess(this.process.Id);
            this.process.BeginErrorReadLine();
            this.process.BeginOutputReadLine();

            this.processCounterMonitorTimer.Change(0, this.counterMonitorTimerInterval);
        }

        public virtual void Kill()
        {
            if (!this.process.HasExited)
            {
                this.process.Kill();
            }

            this.jobObject.Close();
        }

        public virtual void WaitForExit()
        {
            this.process.WaitForExit();
        }

        public virtual bool IsRunning()
        {
            return !this.process.HasExited;
        }

        public Task Shutdown(CancellationToken token)
        {
            this.SendBreakToProcess();
            var tcs = new TaskCompletionSource<bool>();
            var shutdownState = new ShutdownTaskState()
            {
                TaskCompletion = tcs,
                CancellationToken = token
            };

            this.shutdownTimer = new Timer(this.ProcessShutDownMonitor, shutdownState, Timeout.Infinite, Timeout.Infinite);
            this.shutDownTimeElapsed.Start();
            this.shutdownTimer.Change(this.shutdownTimerInterval, Timeout.Infinite);
            return tcs.Task;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!this.isDisposed)
                {
                    this.isDisposed = true;
                    this.shutdownTimer.Dispose();
                    this.shutdownTimer = null;
                    this.processCounterMonitorTimer.Dispose();
                    this.processCounterMonitorTimer = null;
                    this.process.Dispose();
                    this.jobObject.Dispose();
                }
            }
        }

        protected virtual void SendBreakToProcess()
        {
            NativeMethods.GenerateConsoleCtrlEvent(CtrlBreakEvent, 0);
        }

        private void OnOutput(object sender, DataReceivedEventArgs e)
        {
            this.logger.LogInformation(e.Data);
        }

        private void OnError(object sender, DataReceivedEventArgs e)
        {
            this.logger.LogError(e.Data);
        }

        private void ProcessShutDownMonitor(object state)
        {
            var shutDownState = state as ShutdownTaskState;

            if (shutDownState.CancellationToken.IsCancellationRequested)
            {
                shutDownState.TaskCompletion.TrySetCanceled();
            }
            else
            {
                var timeElapsed = this.shutDownTimeElapsed.ElapsedMilliseconds >= this.shutdownWaitTime;
                if (timeElapsed || !this.IsRunning())
                {
                    if (timeElapsed)
                    {
                        this.Kill();
                    }

                    shutDownState.TaskCompletion.TrySetResult(true);
                }
                else if (!this.isDisposed)
                {
                    var dueTime = Math.Min(
                        this.shutdownTimerInterval,
                        this.shutdownWaitTime - this.shutDownTimeElapsed.ElapsedMilliseconds);
                    dueTime = Math.Max(0, dueTime);
                    this.shutdownTimer.Change(dueTime, Timeout.Infinite);
                }
            }
        }

        private void JobCountersMonitor(object state)
        {
            this.jobObject.QueryJobInformation();

            /*
            var processIdList = this.jobObject.GetProcessIdList();
            Console.WriteLine("The id of active processes is : {0}", processIdList == null ? "is null" : processIdList.Count == 0 ? "is empty" : string.Join(",", processIdList));
            */

            // set the total CPU time counter in Milliseconds for the job object
            var jobCpuTimeInMs = this.jobObject.TotalUserTimeInMs + this.jobObject.TotalKernelTimeInMs;
            Console.WriteLine("Job object CPU usage in Milliseconds: {0}", jobCpuTimeInMs);
            /*
            Counters.ServiceJobObjectCPUTimeUsageInMs.SetValue((ulong)jobCpuTimeInMs);
            */

            /*
            // set the amount of physical memory usage in KB for the job object
            Counters.ServiceJobObjectPhysicalMemoryUsageInKB.SetValue((ulong)this.jobObject.GetJobTotalWorkingSetInKB());
            */

            this.jobObject.GetProcessIdList();
        }

        private class ShutdownTaskState
        {
            public TaskCompletionSource<bool> TaskCompletion { get; set; }

            public CancellationToken CancellationToken { get; set; }
        }
    }
}
