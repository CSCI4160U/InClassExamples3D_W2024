
using System.IO;
using UnityEngine;

public class JSONLoaderSaver {
    public static void SaveArmourAsJSON(string savePath,
                                        string filename, 
                                        Armour armour) {
        if (!Directory.Exists(savePath)) {
            Directory.CreateDirectory(savePath);
        }

        string json = JsonUtility.ToJson(armour);
        File.WriteAllText(savePath + filename, json);
    }

    public static Armour LoadArmourFromJSON(string savePath,
                                            string filename) { 
        if (File.Exists(savePath + filename)) {
            string json = File.ReadAllText(savePath + filename);
            Armour armour = JsonUtility.FromJson<Armour>(json);
            return armour;
        } else {
            Debug.Log("Cannot find file: " + savePath + filename);
        }
        return null;
    }    
}
