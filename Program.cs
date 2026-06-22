global using static QudModUpdater.DebugTools;
using BonesClassLibrary.FileFinders;
using QudModUpdater;


string locallow = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low";
string copyTo = Path.Combine(locallow, @"Freehold Games\CavesOfQud\Mods");

if (!Directory.Exists(copyTo))
    throw new DirectoryNotFoundException("Cannot find Qud Mods directory!");

ModCopier copier = new(copyTo);
foreach (var mod in ModDirectories.GetModDirectories())
{
    copier.Update(mod);
    copier.Copy();
}


namespace QudModUpdater
{
    public class DebugTools
    {
        public static void Read<T>(IEnumerable<T> values)
        {
            Console.WriteLine("BEGIN READ");
            foreach (var val in values)
                Console.WriteLine("read" + val?.ToString());
        }
    }
}