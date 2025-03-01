using ExtraFormatter;
using System.Runtime.InteropServices;
namespace PoorMansAot;

// dotnet publish --use-current-runtime -o publish
public class FormatterAot
{
    [UnmanagedCallersOnly(EntryPoint = "format_sql")]
    public static IntPtr format_sql(IntPtr sqlPtr)
    {
        // Parse strings from the passed pointers 
        string inSql = Marshal.PtrToStringAnsi(sqlPtr);

        
        Formatter f = new Formatter();
        // Call the actual method
        string formattedSql = f.DoFormat(inSql);


        // Assign pointer of the string to sumPointer
        IntPtr formatterSqlPointer = Marshal.StringToCoTaskMemAnsi(formattedSql);
        // Return pointer
        return formatterSqlPointer;
    }
}
