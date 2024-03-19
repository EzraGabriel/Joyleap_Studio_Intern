using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer (DataPlayer player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.fun";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerSaving data = new PlayerSaving(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerSaving LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.fun";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerSaving data = formatter.Deserialize(stream) as PlayerSaving;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("SSave file Error in" + path);
            return null;
        }
    }
}
