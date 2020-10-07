using UnityEngine;

namespace RPG.AI
{
    public class LineOfSight : MonoBehaviour
    {
        public Transform target;

        float rotationSpeed = 2.0f;
        float speed = 2.0f;
        float visDist = 2.0f;
        float visAngle = 30f;
        float shootDist = 5.0f;

        string state = "IDLE";


        private void Update() {
            Vector3 direction = target.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);

            if (direction.magnitude < visDist && angle < visAngle)
            {
                direction.y = 0;

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                            Quaternion.LookRotation(direction), Time.deltaTime * rotationSpeed);

                if(direction.magnitude > shootDist)
                {
                    state = "RUNNING";
                }
                else
                {
                    state = "SHOOTING";
                }
            }
            else
            {
                state = "IDLE";
            }

            if (state == "RUNNING")
            {
                this.transform.Translate(0,0, Time.deltaTime * speed);
            }
        }

    }
}