using SaveSystem.Serialization;
using UnityEngine;

namespace SaveSystem
{
    public class SaveManager : MonoBehaviour
    {
        public string saveName = "Save";
        
        
        public void OnSave()
        {
            SerializationManager.Save(saveName, SaveData.current);
            Debug.Log("Game Saved");
        }
    }
}