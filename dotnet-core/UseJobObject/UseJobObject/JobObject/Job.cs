
namespace UseJobObject.JobObject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    internal sealed class Job : IDisposable
    {
        private IntPtr handle;
        private bool disposed;
        private List<int> jobProcessIdList;

        public Job(string name)
        {
            this.handle = NativeMethods.CreateJobObject(IntPtr.Zero, name);
            if (this.handle == null || this.handle == IntPtr.Zero)
            {
                this.GetLastErrorAndThrow("Unable to create job object");
            }

            var info = new JOBOBJECT_BASIC_LIMIT_INFORMATION
            {
                LimitFlags = LimitFlags.JOB_OBJECT_LIMIT_KILL_ON_JOB_CLOSE
            };

            var extendedInfo = new JOBOBJECT_EXTENDED_LIMIT_INFORMATION
            {
                BasicLimitInformation = info
            };

            int length = Marshal.SizeOf(extendedInfo);
            if (!NativeMethods.SetInformationJobObject(this.handle, JobObjectInfoType.ExtendedLimitInformation, ref extendedInfo, length))
            {
                this.GetLastErrorAndThrow("Unable to set information on jobobject");
            }

            this.jobProcessIdList = new List<int>();
            this.TotalUserTimeInMs = 0;
            this.TotalKernelTimeInMs = 0;
            this.TotalNumberOfProcesses = 0;
            this.NumberOfActiveProcesses = 0;
            this.NumberOfTerminatedProcesses = 0;
        }

        public long TotalUserTimeInMs
        {
            get;
            private set;
        }

        public long TotalKernelTimeInMs
        {
            get;
            private set;
        }

        public int TotalNumberOfProcesses
        {
            get;
            private set;
        }

        public int NumberOfActiveProcesses
        {
            get;
            private set;
        }

        public int NumberOfTerminatedProcesses
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (!this.disposed)
            {
                this.disposed = true;
                this.Close();
            }
        }

        public void Close()
        {
            if (this.handle != null && this.handle != IntPtr.Zero)
            {
                try
                {
                    if (!NativeMethods.CloseHandle(this.handle))
                    {
                        this.GetLastErrorAndThrow("Unable to close Job object handle");
                    }
                }
                finally
                {
                    this.handle = IntPtr.Zero;
                }
            }
        }

        public bool AddProcess(int processId)
        {
            Process.GetProcessById(123).
            return NativeMethods.AssignProcessToJobObject(this.handle, Process.GetProcessById(processId).Handle);
        }

        public void QueryJobInformation()
        {
            /* BUGBUG, as currently we cannot query job object processes, the refresh function is not for refreshing processes, just for query job object account information
            this.jobProcessIdList = this.GetProcessIdList();
            foreach (var processId in this.jobProcessIdList)
            {
                var process = Process.GetProcessById(processId);

                // Refresh will discard the obsolete cached property values, after refresh, the first request for each property value will cause the process component to obtain a new value from the associated process
                // take a look at here: https://msdn.microsoft.com/en-us/library/system.diagnostics.process.refresh(v=vs.110).aspx
                process.Refresh();
            }
            */

            this.GetJobBasicAccountingInfo();
        }

        public double GetJobTotalProcessorTimeInMs()
        {
            double totalProcessorTime = 0;
            foreach (var processId in this.jobProcessIdList)
            {
                var process = Process.GetProcessById(processId);
                totalProcessorTime += process.TotalProcessorTime.TotalMilliseconds;
            }

            return totalProcessorTime;
        }

        public long GetJobTotalWorkingSetInKB()
        {
            long totalWorkingSet = 0;
            foreach (var processId in this.jobProcessIdList)
            {
                var process = Process.GetProcessById(processId);
                totalWorkingSet += process.WorkingSet64 / 1024;
            }

            return totalWorkingSet;
        }

        public List<int> GetProcessIdList()
        {
            /* BUGBUG: the query for processIdList doesn't work now
             * we need to query job object directly, rather than create our own process list, because some process might create it child processes, which we cannot track
             */
            var basicProcessIdList = default(JOBOBJECT_BASIC_PROCESS_ID_LIST);

            // the length of struct JOBOBJECT_BASIC_PROCESS_ID_LIST
            var length = Marshal.SizeOf(typeof(JOBOBJECT_BASIC_PROCESS_ID_LIST));
            var lpReturnLength = 0;
            NativeMethods.QueryInformationJobObject(this.handle, JobObjectInfoType.BasicProcessIdList, ref basicProcessIdList, length, ref lpReturnLength);
            Console.Write("number of assigned processes: {0}, number of process id in list: {1}, pointer value: {2}, lpReturnLength: {3}, id list: ", basicProcessIdList.NumberOfAssignedProcesses, basicProcessIdList.NumberOfProcessIdsInList, basicProcessIdList.ProcessIdList, lpReturnLength);
            var processIdList = new List<int>();

            unsafe
            {
                int* ptr = (int*)basicProcessIdList.ProcessIdList;
                for (int i = 0; i < basicProcessIdList.NumberOfProcessIdsInList; i++)
                {
                    var id = ptr[i];
                    Console.Write("{0}, ", id);
                    processIdList.Add(id);
                }
            }

            Console.WriteLine();

            return processIdList;
        }

        private void GetJobBasicAccountingInfo()
        {
            /* the query for job object basic accounting information works
             * The information we can get includes: Total User time, total kernel time, the total number of processes, the number of active processes, the number of terminated processes
             */
            var basicAccountingInfo = default(JOBOBJECT_BASIC_ACCOUNTING_INFORMATION);
            var length = Marshal.SizeOf(basicAccountingInfo);
            var lpReturnLength = 0;
            NativeMethods.QueryInformationJobObject(this.handle, JobObjectInfoType.BasicAccountingInformation, ref basicAccountingInfo, length, ref lpReturnLength);

            /*
            Console.WriteLine("Total User time: {0}, total kernel time: {1}, this period user time: {2}, this period kernel time: {3}, total pagefault: {4}, total processes: {5}, active processes: {6}, terminated processes: {7}, lpReturnLength: {8}", basicAccountingInfo.TotalUserTime, basicAccountingInfo.TotalKernelTime, basicAccountingInfo.ThisPeriodTotalUserTime, basicAccountingInfo.ThisPeriodTotalKernelTime, basicAccountingInfo.TotalPageFaultCount, basicAccountingInfo.TotalProcesses, basicAccountingInfo.ActiveProcesses, basicAccountingInfo.TotalTerminatedProcesses, lpReturnLength);
            */

            this.TotalUserTimeInMs = basicAccountingInfo.TotalUserTime;
            this.TotalKernelTimeInMs = basicAccountingInfo.TotalKernelTime;
            this.TotalNumberOfProcesses = basicAccountingInfo.TotalProcesses;
            this.NumberOfActiveProcesses = basicAccountingInfo.ActiveProcesses;
            this.NumberOfTerminatedProcesses = basicAccountingInfo.TotalTerminatedProcesses;
        }

        private void GetLastErrorAndThrow(string errorMessage)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error(), errorMessage);
        }
    }
}
