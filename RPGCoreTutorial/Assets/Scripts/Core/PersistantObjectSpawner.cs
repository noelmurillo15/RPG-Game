using UnityEngine;


namespace RPG.Core
{
    public class PersistantObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistantObjectPrefab = null;

        private static bool _hasSpawned = false;


        private void Awake()
        {
            if (_hasSpawned) return;
            SpawnPersistantObject();
            _hasSpawned = true;
        }

        private void SpawnPersistantObject()
        {
            GameObject peristantObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(peristantObject);
        }
    }
}