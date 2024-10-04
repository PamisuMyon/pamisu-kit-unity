using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class MiscEditor
    {
        public const string DataDirectory = @"..\Data";
        
        [MenuItem("自定义工具/🧾更新__tables__.xlsx", priority = 801)]
        private static void UpdateTablesFile()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            
            var fileExtension = isWindows? ".bat" : ".sh";
            var workingDirectory = Path.Combine(Application.dataPath, "..", DataDirectory);
            var filePath = Path.Combine(DataDirectory, $"_点我更新tables{fileExtension}");
            filePath = Path.Combine(Application.dataPath, "..", filePath);
            
            var process = ExecuteProcess(workingDirectory, filePath);
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            UnityEngine.Debug.Log(output);
            if (!string.IsNullOrEmpty(error))
            {
                UnityEngine.Debug.LogError(error);
                UnityEngine.Debug.LogError("更新__tables__.xlsx执行出现错误，请查看错误信息。");
            }
            else
                UnityEngine.Debug.Log("更新__tables__.xlsx执行完毕");
        }
        
        [MenuItem("自定义工具/🧾导出表", priority = 802)]
        private static void LubanExportAll()
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            
            var fileExtension = isWindows? ".bat" : ".sh";
            var workingDirectory = Path.Combine(Application.dataPath, "..", DataDirectory);
            var filePath = Path.Combine(DataDirectory, $"_点我导出表格{fileExtension}");
            filePath = Path.Combine(Application.dataPath, "..", filePath);

            var process = ExecuteProcess(workingDirectory, filePath);
            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            UnityEngine.Debug.Log(output);
            if (!string.IsNullOrEmpty(error))
            {
                UnityEngine.Debug.LogError(error);
                UnityEngine.Debug.LogError("导出表执行出现错误，请查看错误信息。");
            }
            else
                UnityEngine.Debug.Log("导出表执行完毕");
        }

        private static Process ExecuteProcess(string workingDirectory, string fileName)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var process = new Process();
            process.StartInfo.WorkingDirectory = workingDirectory;
            if (isWindows)
            {
                process.StartInfo.FileName = fileName;
            }
            else
            {
                process.StartInfo.FileName = "/bin/bash";
                process.StartInfo.Arguments = fileName;
            }
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();
            process.WaitForExit();

            return process;
        }
    }
}