using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public List<SerializeSlot> Storages { get; set; }
    public SerializableVector3 PlayerPosition { get; set; }
    public SerializableQuaternion PlayerRotation { get; set; }

    public int CurrentQi { get; set; }
}