using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Items
{
   
    public class Armor : MonoBehaviour
    {

        [SerializeField] UnityEvent onHit;
        
        public void OnHit()
        {
            onHit.Invoke();
        }
    }
}
