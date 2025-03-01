using System;
using System.Runtime.InteropServices;

public partial class Program
{
    [LibraryImport("PoorMansAot.dll")]
    private static partial IntPtr format_sql(IntPtr first);

    public static void Main()
    {
        string sqlToFormat = "SELECT * FROM SOME_TABLE";
        IntPtr sumPointer = Marshal.StringToCoTaskMemAnsi(sqlToFormat);
        IntPtr resInt = format_sql(sumPointer);
        string? resultString = Marshal.PtrToStringAnsi(resInt);
        Console.WriteLine(resultString);

        // Free the allocated memory
        Marshal.FreeCoTaskMem(sumPointer);

        Console.ReadKey();
    }
}
