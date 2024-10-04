using System.Text;
using PamisuKit.Common.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static class MiscEditor
    {
        [MenuItem("GameObject/📄Copy Path", priority = -600)]
        private static void CopyGameObjectPathToClipboard()
        {
            var selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject != null)
            {
                var gameObjectPath = GetGameObjectPath(selectedGameObject);
                EditorGUIUtility.systemCopyBuffer = gameObjectPath;
            }
        }

        private static string GetGameObjectPath(GameObject gameObject)
        {
            var path = new StringBuilder("/").Append(gameObject.name);
            var parent = gameObject.transform.parent;
            while (parent != null)
            {
                path.Insert(0, $"/{parent.name}");
                parent = parent.parent;
            }
            return path.ToString();
        }
        
        // [MenuItem("Custom Tools/📁Open Save Directory", priority = 101)]
        // private static void OpenSaveDir()
        // {
        //     UnityUtil.ShowInExplorer(Application.persistentDataPath);
        // }

        [MenuItem("🛠️Custom Tools/🧹Clear Save Files", priority = 102)]
        private static void ClearPlayerSaveData()
        {
            EditorUtil.ClearAllUnderDir(Application.persistentDataPath);
            Debug.Log("Save files cleared.");
        }
        
    }
}