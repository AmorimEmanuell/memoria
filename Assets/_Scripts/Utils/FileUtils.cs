using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class FileUtils
{
    public static bool TryLoad<T>(string fileName, out T file) where T : ISaveData
    {
        var filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(filePath))
        {
            file = default;
            return false;
        }

        var fs = File.OpenRead(filePath);
        var bf = new BinaryFormatter();
        file = (T)bf.Deserialize(fs);
        fs.Close();
        return true;
    }

    public static void Save(string fileName, ISaveData saveData)
    {
        var filePath = Path.Combine(Application.persistentDataPath, fileName);
        var fs = File.Create(filePath);
        var bf = new BinaryFormatter();
        bf.Serialize(fs, saveData);
        fs.Close();
    }
}
