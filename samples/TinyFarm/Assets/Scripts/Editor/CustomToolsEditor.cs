using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class CustomToolsEditor
    {
        [MenuItem("Tools/🧹Clear Save Files", priority = 802)]
        private static void ClearPlayerSaveData()
        {
            CommonEditorUtil.ClearAllUnderDir(Application.persistentDataPath);
            Debug.Log("Save files cleared.");
        }
    }
}