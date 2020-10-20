using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.SceneManagement{
    
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save"; 
        [SerializeField] float fadeInTime = 1f;

    private void Awake() 
    {
        StartCoroutine(LoadLastScene());

    }
        IEnumerator LoadLastScene() {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime); 


        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Load();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }


        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
        public void Delete()
        {
                GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
