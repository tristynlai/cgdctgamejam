using UnityEngine;
using System;
using System.IO;

public class FileDataHandler : MonoBehaviour
{
    private string DataDirPath = "";

    private string DataFileName = "";

    private bool UseEncryption = false;
    private readonly string EncryptionCodeWord = "Ghastly4EvaThatsMe^.^";

    public FileDataHandler(string DataDirPath, string DataFileName, bool UseEncryption) {
        this.DataDirPath = DataDirPath;
        this.DataFileName = DataFileName;
        this.UseEncryption = UseEncryption;
    }

    public GameData Load() {
        string FullPath = Path.Combine(DataDirPath, DataFileName);
        print(FullPath);
        GameData LoadedData = null;
        if (File.Exists(FullPath)) {
            try {
                string DataToLoad = "";
                using (FileStream stream = new FileStream(FullPath, FileMode.Open)) {
                    using (StreamReader reader = new StreamReader(stream)) {
                        DataToLoad = reader.ReadToEnd();
                    }
                }
                if (UseEncryption) {
                    DataToLoad = EncryptDecrypt(DataToLoad);
                }

                LoadedData = JsonUtility.FromJson<GameData>(DataToLoad);

            }
            catch (Exception e) {
                Debug.LogError("Error occured when trying to load data from the file: " + FullPath + "\n" + e);

            }
        }
        return LoadedData;

    }

    public void Save(GameData Data) {
        string FullPath = Path.Combine(DataDirPath, DataFileName);
        try {
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));

            string DataToStore = JsonUtility.ToJson(Data, true);
            
            if (UseEncryption) {
                DataToStore = EncryptDecrypt(DataToStore);
            }
            using (FileStream stream = new FileStream(FullPath, FileMode.Create)) {
                using (StreamWriter writer = new StreamWriter(stream)) {
                    writer.Write(DataToStore);
                }
            }
        }
        catch (Exception e) {
            Debug.LogError("Error occured when trying to save data to the file: " + FullPath + "\n" + e);
        }
    }

    private string EncryptDecrypt(string data) {
        string ModifiedData = "";
        for (int i = 0; i < data.Length; i++) {
            ModifiedData +=(char) (data[i] ^ EncryptionCodeWord[i % EncryptionCodeWord.Length]);
        }
        return ModifiedData;
    }
}
