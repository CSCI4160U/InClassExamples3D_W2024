using UnityEngine;

public class SaveManager : MonoBehaviour {
    private string savePath;

    public Armour armour;

    private void Start() {
        savePath = Application.persistentDataPath + "/";

        this.armour = new Armour();
    }

    [ContextMenu("Save Armour")]
    public void SaveArmour() {
        //JSONLoaderSaver.SaveArmourAsJSON(savePath,
        //                                 "armour.json",
        //                                 armour);
        BinaryLoaderSaver.SaveArmourAsBinary(savePath,
                                         "armour.bin",
                                         armour);
    }

    [ContextMenu("Load Armour")]
    public void LoadArmour() {
        //this.armour = JSONLoaderSaver.LoadArmourFromJSON(savePath,
        //                                                 "armour.json");
        this.armour = BinaryLoaderSaver.LoadArmourFromBinary(savePath,
                                                         "armour.bin");
    }
}
