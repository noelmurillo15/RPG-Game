using UnityEngine;


namespace RPG.Core
{    
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantObjectPrefab;

        static bool hasSpawned = false;

        void Awake() {
            if(hasSpawned) return;
            SpawnPersistantObject();
            hasSpawned = true;
        }

        void SpawnPersistantObject()
        {
            GameObject peristantObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(peristantObject);
        }
    }
}