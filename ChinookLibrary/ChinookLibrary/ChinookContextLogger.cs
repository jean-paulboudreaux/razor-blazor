// Jean-Paul Boudreaux, Andrew Kieu 
// C00416940, C00014562
// CMPS 358 .NET/C# Programming
// project Chinook Razor/Blazor Website Project
using static System.Environment;

namespace ChinookLibrary.Models;

public class ChinookContextLogger
{
    public static void WriteToLog(string message)
    {
        string path = Path.Combine(
            GetFolderPath(SpecialFolder.DesktopDirectory), 
            "chinooklog.txt");

        StreamWriter logfile = File.AppendText(path);
        logfile.WriteLine(message);
        logfile.Close();
    }
}