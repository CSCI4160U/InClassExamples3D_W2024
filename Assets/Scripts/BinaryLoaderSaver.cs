
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryLoaderSaver {
    public static BinaryFormatter GetBinaryFormatter() {
        BinaryFormatter formatter = new BinaryFormatter();
        return formatter;
    }

    public static void SaveArmourAsBinary(string savePath,
                                        string filename,
                                        Armour armour) {
        if (!Directory.Exists(savePath)) {
            Directory.CreateDirectory(savePath);
        }

        FileStream file = File.Create(savePath + filename);
        BinaryFormatter formatter = GetBinaryFormatter();
        formatter.Serialize(file, armour);
        file.Close();
    }

    public static Armour LoadArmourFromBinary(string savePath,
                                            string filename) {
        if (File.Exists(savePath + filename)) {
            BinaryFormatter formatter = GetBinaryFormatter();
            FileStream file = File.Open(savePath + filename,
                                        FileMode.Open);
            Armour armour = null;
            try {
                armour = (Armour)formatter.Deserialize(file);
            } catch {
                Debug.Log("File format error: " + savePath + filename);
            } finally {
                file.Close();
            }
            return armour;
        } else {
            Debug.Log("Cannot find file: " + savePath + filename);
        }
        return null;
    }
}
