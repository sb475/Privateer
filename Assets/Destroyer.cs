using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] GameObject targetDestroy = null;
    
    public void DestroyTarget()
    {
        Destroy(targetDestroy);
    }
}
