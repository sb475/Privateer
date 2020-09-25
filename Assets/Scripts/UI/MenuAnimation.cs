using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI
{
    public class MenuAnimation : MonoBehaviour
    {

        [SerializeField] float speed = 45;
        [SerializeField] Vector3 direction = Vector3.forward;

        public void RapidSpinAndSelect()
        {
            LeanTween.rotateAroundLocal(gameObject, direction, 360f, speed);
            LeanTween.cancelAll(true);
        }
    }
}
