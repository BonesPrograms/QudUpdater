global using static QudModUpdater.DebugTools;
using QudModUpdater;

const string start = @"C:\Users\user\Desktop\DevVersions";
const string copy = @"C:\Users\user\AppData\LocalLow\Freehold Games\CavesOfQud\Mods";
const string xml = @"C:\Users\user\Desktop\DevVersions\mods.xml";

ModDirectories directories = new(start, xml);
IEnumerable<string> mods = directories.Mods;
ModCopier copier = new(copy);
foreach (var mod in mods)
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