using Luban.Editor;
using UnityEditor;

namespace Editor
{
    public static class MiscEditor
    {
        [MenuItem("自定义工具/🧾导出表 (代码与数据)", priority = 980)]
        public static void LubanExportAll()
        {
            var config = AssetDatabase.LoadAssetAtPath<LubanExportConfig>("Assets/Res/Settings/LubanExportConfig.asset");
            if (config)
            {
                config.RunCommand();
            }
        }
    }
}