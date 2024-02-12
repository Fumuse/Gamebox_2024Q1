using Cysharp.Threading.Tasks;
using UnityEngine;

public class AutoSaver : MonoBehaviour
{
    [SerializeField] private float _minutesToAutoSave = 2;
    
    private void Start()
    {
        AutoSave();
    }

    private async void AutoSave()
    {
        while (true)
        {
            await UniTask.WaitForSeconds(_minutesToAutoSave * 60);
            SaveSerial.SaveGame();
        }
    }
}