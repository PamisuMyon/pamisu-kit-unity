using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using OdinSerializer;
using UnityEngine;

namespace Game.Save
{
    public class SaveHandler
    {
        // private const DataFormat SerializeFormat = DataFormat.JSON;
        private const DataFormat SerializeFormat = DataFormat.Binary;
        private const string FileExtension = ".awesometinyfarmdat";
        
        public string RootPath { get; private set; }

        public SaveHandler(string rootPath)
        {
            RootPath = rootPath;
        }
        
        private StringBuilder GetFilePath(string key)
        {
            var sb = new StringBuilder();
            sb.Append(RootPath);
            sb.Append("/");
            sb.Append(key);
            sb.Append(FileExtension);
            return sb;
        }
        
        public async UniTaskVoid Save<T>(T data)
        {
            var bytes = SerializationUtility.SerializeValue(data, SerializeFormat);
            var name = typeof(T).Name;
            
            var sb = GetFilePath(name);
            var filePath = sb.ToString();
            sb.Append(".tmp");
            var tempFilePath = sb.ToString();

            Debug.Log($"SaveHandler Saving to: {filePath}");
            await File.WriteAllBytesAsync(tempFilePath, bytes);
            if (File.Exists(filePath))
                File.Delete(filePath);
            File.Move(tempFilePath, filePath);
        }

        public UniTask<T> Load<T>()
        {
            return Load<T>(typeof(T).Name);
        }

        public async UniTask<T> Load<T>(string name)
        {
            Debug.Log($"SaveHandler Load begin: {name}");

            var filePath = GetFilePath(name).ToString();
            
            if (!File.Exists(filePath))
            {
                Debug.Log($"SaveHandler Load file not found: {filePath}");
                return default;
            }

            var bytes = await File.ReadAllBytesAsync(filePath);
            var obj = SerializationUtility.DeserializeValue<T>(bytes, SerializeFormat);
            return obj;
        }
        
    }
}