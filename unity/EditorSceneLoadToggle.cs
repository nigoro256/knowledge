// Unity 2017.4.40f1

/*
Original code
  https://github.com/takupisu/EditorSceneActiveSwitcher-Unity

MIT License

Copyright (c) 2018 Takumi Hanzawa

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class EditorSceneLoadToggle
{
  // チェックボックスの表示位置
  private const float Width = 40.0f;

  static EditorSceneLoadToggle()
  {
    EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
  }

  private static void HierarchyWindowItemOnGUI(int instanceId, Rect selectionRect)
  {
    // 再生中は無視
    if (Application.isPlaying) return;

    // シーン以外は無視
    if (EditorUtility.InstanceIDToObject(instanceId) != null) return;

    // シーンオブジェクトを取得するメソッドを取得
    var getSceneByHandle = typeof(EditorSceneManager)
      .GetMethod("GetSceneByHandle", BindingFlags.NonPublic | BindingFlags.Static);
    if (getSceneByHandle == null) {
      Debug.LogWarning("[EditorSceneLoadToggle] No such method: EditorSceneManager.GetSceneByHandle");
      return;
    }

    // シーンオブジェクトを取得
    var scene = (Scene)getSceneByHandle.Invoke(null, new object[] { instanceId });

    // チェックボックス表示位置
    selectionRect.x += selectionRect.width - Width;
    selectionRect.width = Width;

    // チェックボックス表示
    if (scene.isLoaded == GUI.Toggle(selectionRect, scene.isLoaded, "")) return;

    // シーンがロードされていない場合はロードして終了
    if (!scene.isLoaded) {
      EditorSceneManager.OpenScene(scene.path, OpenSceneMode.Additive);
      return;
    }

    // ロードされているシーンの保存確認後、アンロード
    if (EditorSceneManager.SaveModifiedScenesIfUserWantsTo(new[] { scene })) {
      EditorSceneManager.CloseScene(scene, false);
    }
  }
}
