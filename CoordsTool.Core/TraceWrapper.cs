using System.Diagnostics;

namespace CoordsTool.Core;

public static class TraceWrapper
{
    public static void WriteLine(string message)
    {
        Trace.WriteLine($"[{DateTime.Now:O}] {message}");
    }
}