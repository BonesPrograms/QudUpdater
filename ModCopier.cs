namespace QudModUpdater;

//ModCopier does not create mod folders, it requires a target mod folder to already exist.
//It will copy your manifest.json, preview.png, and workshop.json but will not create them.
//Organization is lost, all CS and XML files are dumped into the mod root.
//Only image files in the Textures folder have their directories recreated at copy destination.
public class ModCopier
{
    readonly string CopyRoot; //root for the overlying directory - mods will be copied to matching subdirectories in this directory
    string? CopyDest;
    string? ModPath; //mod directory
    static readonly string[] CodeExtensions =
    [
      "*.xml", "*.cs"
    ];

    static readonly string[] TextureExtensions =
    [
        "*.jpg", "*.png"
    ];

    static readonly string[] ExcludedFolders = //explicitly exclude code by placing it into a "DoNotShip" folder
    [
      "DoNotShip", "obj", "bin", ".git", ".dotnet", ".vs", "Textures"
    ];

    public ModCopier(string copyroot)
    {
        CopyRoot = copyroot;
    }
    public void Update(string mod)
    {
        ModPath = mod;
        CopyDest = Path.Combine(CopyRoot, Path.GetFileName(mod));
    }
    public void Copy()
    {
        IEnumerable<string> codes = GetCode();
        Copy(codes, CodeCopyPath);
        IEnumerable<string> textures = GetTextures();
        CreateTextureDirectories(textures);
        Copy(textures, TextureCopyPath);
    }

    static void Copy(IEnumerable<string> paths, Func<string, (string, string)> expr)
    {
        IEnumerable<(string, string)> copyPaths = paths.Select(expr);
        foreach (var copy in copyPaths)
            File.Copy(copy.Item1, copy.Item2, true);
    }

    void CreateTextureDirectories(IEnumerable<string> textures)
    {
        string copyTextureDir = Path.Combine(CopyDest!, "Textures");
        if (!Directory.Exists(copyTextureDir))
            Directory.CreateDirectory(copyTextureDir);
        foreach (var path in textures)
        {
            string dir = Path.GetDirectoryName(path)!;
            string dirname = Path.GetFileName(dir);
            string copydir = Path.Combine(copyTextureDir, dirname);
            if (!Directory.Exists(copydir))
                Directory.CreateDirectory(copydir);
        }
    }

    IEnumerable<string> GetTextures() //textures however will be placed into their specific directory
    {
        IEnumerable<string> textures = Directory.GetDirectories(ModPath!, "Textures*", SearchOption.TopDirectoryOnly);
        return textures.SelectMany(dir => TextureExtensions.SelectMany(file => Directory.GetFiles(dir, file, SearchOption.AllDirectories)));
    }
    IEnumerable<string> GetCode() //cs and xml files will simply just be dumped into the target mod folder
    {
        IEnumerable<string> subdirectories = Directory.GetDirectories(ModPath!, "*", SearchOption.AllDirectories).Where(mod => !ExcludedFolders.Any(name => mod.Contains(name))).Concat([ModPath!]);
        return subdirectories.SelectMany(dir => CodeExtensions.SelectMany(file => Directory.GetFiles(dir, file, SearchOption.TopDirectoryOnly))).Concat(Directory.GetFiles(ModPath!, "*.json", SearchOption.TopDirectoryOnly)).Concat(Directory.GetFiles(ModPath!, "preview*.png", SearchOption.TopDirectoryOnly));
    }                                                                                                               //this is already a massive enumerable of all valid directories
                                                                                                                    //so top directory only that way we dont search subdirectories twic
    (string, string) TextureCopyPath(string sourcePath)
    {
        string subdir = sourcePath[sourcePath.IndexOf(@"Textures\")..];
        return (sourcePath, Path.Combine(CopyDest!, subdir));//as you can see it *expects* it to be an immediate subdirectory
    }
    (string, string) CodeCopyPath(string sourcePath)
    {
        return (sourcePath, Path.Combine(CopyDest!, Path.GetFileName(sourcePath)));
    }

}