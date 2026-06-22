using System.Xml.Linq;
using BonesClassLibrary.FileFinders;

namespace QudModUpdater;

//This gets the directories for your mods' source codes, it does not get the directories of where you will be *copying* mod files to.
public static class ModDirectories
{
    readonly static string ModLab = Path.Combine(KnownFolders.Desktop, "QudModLab");
    readonly static string XmlPath = Path.Combine(ModLab, "mods.xml");
    static readonly XDocument XML = XDocument.Load(XmlPath); //will throw a filenotfoundexception of the modlab or mods.xml cannot be located
    public static List<string> GetModDirectories()
    {
        IEnumerable<string> subdirectories = Directory.EnumerateDirectories(ModLab);
        string[] xmlValues = [.. XML.Descendants("mod").Select(x => x.Value)];
        List<string> dirs = new(xmlValues.Length);
        foreach (var dir in subdirectories)
        {
            if (xmlValues.Contains(Path.GetFileName(dir)))
                dirs.Add(dir);
            if (dirs.Count >= xmlValues.Length)
                break;
        }
        return dirs;
    }

}