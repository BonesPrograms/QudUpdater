using System.Xml.Linq;

namespace QudModUpdater;

//This gets the directories for your mods' source codes, it does not get the directories of where you will be *copying* mod files to.
public class ModDirectories
{
    readonly string Root;
    readonly XDocument XML;
    public IEnumerable<string> Mods //So these paths are going to be from your source code, it will be the directory for each mod
    {
        get
        {
            return _mods ??= [..GetMods()];
        }
    }
    string[]? _mods;
    public ModDirectories(string root, string xmlPath)
    {
        Root = root;
        XML = XDocument.Load(xmlPath);
    }
    IEnumerable<string> GetMods()
    {
        string[] subdirectories = Directory.GetDirectories(Root);
        string[] xmlValues = [.. XML.Descendants("mod").Select(x => x.Value)];
        return subdirectories.Where(x => xmlValues.Contains(Path.GetFileName(x)));
    }

}