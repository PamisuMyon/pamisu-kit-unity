using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace PamisuKit.Common.Editor
{
    public static class EditorUtil
    {
        public static void ReplaceDir(string src, string dst, List<string> whiteList)
        {
            if (!Directory.Exists(dst))
            {
                Directory.Move(src, dst);
                return;
            }

            foreach (var dstSubDir in Directory.GetDirectories(dst))
            {
                var dirName = Path.GetFileName(dstSubDir);
                if (whiteList.Contains(dirName))
                    continue;
                CopyDir(Path.Combine(src, dirName), dstSubDir);
            }

            foreach (var dstFile in Directory.GetFiles(dst))
            {
                var fileName = Path.GetFileName(dstFile);
                if (!File.Exists(Path.Combine(src, fileName)))
                    continue;
                if (whiteList.Contains(fileName))
                    continue;
                File.Delete(dstFile);
                File.Copy(Path.Combine(src, fileName), dstFile);
            }
        }

        public static void CopyDir(string src, string dst)
        {
            RemoveDir(dst);
            Directory.CreateDirectory(dst);
            // ignore .meta files
            var metaRegex = new Regex("\\.meta$");
            foreach (var file in Directory.GetFiles(src))
            {
                if (metaRegex.IsMatch(file))
                    continue;
                File.Copy(file, $"{dst}/{Path.GetFileName(file)}");
            }

            foreach (var subDir in Directory.GetDirectories(src))
            {
                CopyDir(subDir, $"{dst}/{Path.GetFileName(subDir)}");
            }
        }

        public static void RemoveDir(string dir)
        {
            if (!Directory.Exists(dir))
                return;
            foreach (var file in Directory.GetFiles(dir))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (var subDir in Directory.GetDirectories(dir))
            {
                RemoveDir(subDir);
            }

            Directory.Delete(dir);
        }

        public static void RemoveDirOrFile(string dirOrFile)
        {
            if (Directory.Exists(dirOrFile))
                RemoveDir(dirOrFile);
            else if (File.Exists(dirOrFile))
                File.Delete(dirOrFile);
        }

        public static void ClearAllUnderDir(string dir)
        {
            if (!Directory.Exists(dir))
                return;
            foreach (var file in Directory.GetFiles(dir))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (var subDir in Directory.GetDirectories(dir))
            {
                RemoveDir(subDir);
            }
        }
        
    }
}