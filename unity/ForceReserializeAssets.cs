using System.Collections.Generic;
using System.IO;
using UnityEditor;

public static class ForceReserializeAssets
{
  private const string MenuItemName = "Assets/Force Reserialize Assets";

  [MenuItem(MenuItemName)]
  public static void Perform()
  {
    var assetGuids = Selection.assetGUIDs;
    var assetPaths = new List<string>(assetGuids.Length);
    var searchInFolders = new string[1];

    foreach (var assetGuid in assetGuids) {
      var path = AssetDatabase.GUIDToAssetPath(assetGuid);
      assetPaths.Add(path);

      if (!Directory.Exists(path)) continue;

      searchInFolders[0] = path;
      var assetGuidsInFolders = AssetDatabase.FindAssets("", searchInFolders);
      foreach (var guid in assetGuidsInFolders) {
        assetPaths.Add(AssetDatabase.GUIDToAssetPath(guid));
      }
    }

    AssetDatabase.ForceReserializeAssets(assetPaths);
  }

  [MenuItem(MenuItemName, validate = true)]
  public static bool Validate()
  {
    return 0 < Selection.assetGUIDs.Length;
  }
}
