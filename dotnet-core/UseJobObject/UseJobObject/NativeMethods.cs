

namespace UseJobObject
{
    using System;
    using System.Runtime.InteropServices;
    using JobObject;

    internal static class NativeMethods
    {
        [DllImport("kernel32.dll", SetLastError = true)]
#pragma warning disable CA1401 // P/Invokes should not be visible.
        internal static extern bool GenerateConsoleCtrlEvent(int eventId, uint dwProcessGroupId);
#pragma warning restore CA1401 // P/Invokes should not be visible.

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr CreateJobObject(IntPtr lpJobAttributes, string lpName);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "SetInformationJobObject")]
        internal static extern bool SetInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, ref JOBOBJECT_EXTENDED_LIMIT_INFORMATION lpJobObjectInfo, int cbJobObjectInfoLength);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "QueryInformationJobObject")]
        internal static extern bool QueryInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, ref JOBOBJECT_BASIC_PROCESS_ID_LIST lpJobObjectInfo, int cbJobObjectInfoLength, ref int lpReturnLength);

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "QueryInformationJobObject")]
        internal static extern bool QueryInformationJobObject(IntPtr hJob, JobObjectInfoType infoType, ref JOBOBJECT_BASIC_ACCOUNTING_INFORMATION lpJobObjectInfo, int cbJobObjectInfoLength, ref int lpReturnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool AssignProcessToJobObject(IntPtr job, IntPtr process);

        [DllImport("kernel32", SetLastError = true)]
        internal static extern bool CloseHandle(IntPtr jobHandle);
    }
}
