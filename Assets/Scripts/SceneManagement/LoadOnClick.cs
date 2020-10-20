using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class LoadOnClick : MonoBehaviour
    {
        [SerializeField] float fadeOutTime = 2f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = .5f;

        public void LoadScene(int level)
        {
            if (level < 0)
            {
                Debug.LogError("You need to add a scene to the Portal prefab");
                return;
            }
            SceneManager.LoadScene(level);
        }
    }
}
