using System;
using UnityEngine;


namespace RPG.Core
{    
    public class PersistantObjectSpawner : MonoBehaviour {

        [SerializeField] GameObject persistantObjectPrefab;

        static bool hasSpawned = false;

        private void Awake() {
            if (hasSpawned) return;

            SpawnPersistentObject();

            hasSpawned = true;


        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObject = Instantiate(persistantObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
    

    
}