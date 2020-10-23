using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control

{

    public interface IControllable
    {
        void KeyMovement();
        void IssueCommand(ManualActions action, GameObject target);
        void IssueCommand(ManualActions action, Vector3 target);

        GameObject gameObject { get; }

    }
}
