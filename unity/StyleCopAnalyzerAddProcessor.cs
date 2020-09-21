// macOS 10.14.6 (Mojave)
// Unity 2017.4.40f1
// Rider 2020.1.4

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Add StyleCop.Analyzer to csproj files.
/// </summary>
public class StyleCopAnalyzerAddProcessor : AssetPostprocessor
{
  private static void OnGeneratedCSProjectFiles()
  {
    var rootPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

    // Get analyzer dll files.
    var dllFiles = GetDllFiles(Path.Combine(rootPath, "Packages"));
    if (dllFiles == null || dllFiles.Length <= 0) {
      return;
    }

    // Add StyleCop.Analyzer to csproj files.
    var csProjFiles = Directory.GetFiles(rootPath, "*.csproj", SearchOption.TopDirectoryOnly);
    foreach (var csProjFile in csProjFiles) {
      AddStyleCopAnalyzer(csProjFile, dllFiles);
    }
  }

  private static string[] GetDllFiles(string rootPath)
  {
    if (!Directory.Exists(rootPath)) {
      return null;
    }

    var dllFiles = Directory.GetFiles(rootPath, "StyleCop.Analyzers*.dll", SearchOption.AllDirectories);
    return dllFiles
      .Where(f => !f.EndsWith(".resources.dll"))
      .Select(f => f.Substring(rootPath.Length + 1))
      .ToArray();
  }

  private static void AddStyleCopAnalyzer(string csProjPath, IEnumerable<string> dllFiles)
  {
    var csProj = XElement.Load(csProjPath);
    var itemGroup = new XElement(csProj.Name.Namespace + "ItemGroup");

    foreach (var dllFile in dllFiles) {
      var xe = new XElement(csProj.Name.Namespace + "Analyzer");
      xe.SetAttributeValue("Include", "packages\\" + dllFile.Replace('/', '\\'));

      itemGroup.Add(xe);
    }

    csProj.Add(itemGroup);
    csProj.Save(csProjPath);
  }
}
