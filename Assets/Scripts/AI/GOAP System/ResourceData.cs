using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "resourceddata", menuName = "Resource Data", order = 51)]
public class ResourceData : ScriptableObject
{
    public string resourceTag;
    public string resourceQueue;
    public string resourceState;
}
