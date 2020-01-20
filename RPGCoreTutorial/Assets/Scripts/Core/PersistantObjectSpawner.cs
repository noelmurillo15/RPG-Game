using UnityEngine;


namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistantObjectPrefab = null;

        static bool hasSpawned = false;


        private void Awake()
        {
            if (hasSpawned) return;
            SpawnPersistantObject();
            hasSpawned = true;
        }

        private void SpawnPersistantObject()
        {
            GameObject peristantObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(peristantObject);
        }
    }
}