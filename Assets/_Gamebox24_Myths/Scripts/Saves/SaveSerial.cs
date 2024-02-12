using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using UnityEngine;

public class SaveSerial : MonoBehaviour
{
    public static List<SerializeSlot> Storages { get; private set; } = new();

    public static SerializableVector3 PlayerPosition { get; private set; } = new Vector3(4.5f, 0, -4.5f);

    public static SerializableQuaternion PlayerRotation { get; private set; } = new Quaternion();

    public static int CurrentQi { get; private set; } = 0;

    private void Start()
    {
        PlayerIndicators.OnUpdateQi += OnUpdateQi;
    }

    #region Storages

    public static void AddStoragesSlot(SerializeSlot slot)
    {
        SerializeSlot inStorageSlot = StorageContainsSlot(slot);

        if (inStorageSlot == null)
        {
            Storages.Add(slot);
        }
        else
        {
            inStorageSlot.amount = slot.amount;
        }

        SaveGame();
    }

    public static void RemoveStoragesSlot(SerializeSlot slot)
    {
        SerializeSlot inStorageSlot = StorageContainsSlot(slot);

        if (inStorageSlot != null)
        {
            Storages.Remove(inStorageSlot);
        }

        SaveGame();
    }

    #endregion

    #region Player position

    public static void SavePlayerPosition(Vector3 position)
    {
        PlayerPosition = position;
    }

    public static void SavePlayerRotation(Quaternion rotation)
    {
        PlayerRotation = rotation;
    }

    #endregion

    #region Player indicators

    private void OnUpdateQi(int qi, bool save)
    {
        if (!save) return;

        CurrentQi = qi;
        SaveGame();
    }

    #endregion

    [CanBeNull]
    public static SerializeSlot StorageContainsSlot(SerializeSlot slot)
    {
        SerializeSlot inStorageSlot = null;

        foreach (SerializeSlot storageSlot in Storages)
        {
            if (storageSlot.owner != slot.owner) continue;
            if (storageSlot.item.itemName != slot.item.itemName) continue;

            inStorageSlot = storageSlot;
            break;
        }

        return inStorageSlot;
    }

    public static void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.dat");

        SaveData data = new SaveData();

        data.Storages = Storages;
        data.PlayerPosition = PlayerPosition;
        data.PlayerRotation = PlayerRotation;
        data.CurrentQi = CurrentQi;

        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            SaveData data = (SaveData) bf.Deserialize(file);
            file.Close();

            Storages = data.Storages;
            PlayerPosition = data.PlayerPosition;
            PlayerRotation = data.PlayerRotation;
            CurrentQi = data.CurrentQi;
        }
    }

    private void Awake()
    {
        LoadGame();
    }
}