using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Attributes;
using RPG.Items;
using RPG.Base;
using RPG.Control;

namespace RPG.Movement
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterEngine : MonoBehaviour, IAction, ISaveable, IEngine
    {
        public Transform cam;
        public CharacterController controller;

        [SerializeField] Transform target;
        [SerializeField] Transform followTarget;
        [SerializeField] float maxSpeed = 6f;
        [SerializeField] float followDistance = 4f;
        [SerializeField] bool isFollowing;
        private Rigidbody body;
        NavMeshAgent navMeshAgent;
        Health health;
        public float moveSpeed = 0f;
        private float speedSmoothVelocity = 0f;
        private float speedSmoothTime = 0.1f;
        bool keyMove;
        
        float angleSmoothVelocity = 0f;
        private float angleSmoothTime = 0.1f ;

        private void Awake() 
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
            isFollowing = false;
            body = GetComponent<Rigidbody>();
            controller = GetComponent<CharacterController>();
            cam = Camera.main.transform;
        }
        // Update is called once per frame
        void Update()
        {
            navMeshAgent.enabled = !health.IsDead();

            if (keyMove)
            {
                GetComponent<Animator>().SetFloat("forwardMovement", moveSpeed);
                
            }
            else
            {
                NavMeshAnimator();
            }
            

            if (isFollowing)
            {
                MoveTo(followTarget.position, 1f);
                FaceTarget();
            }
        }

        public void KeyMovement()
        {

            float horizonal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            

            Vector3 direction = new Vector3(horizonal, 0f, vertical).normalized;
       
            if (direction.magnitude >= 0.1f)
            {
                keyMove = true;
                
                GetComponent<ActionScheduler>().CancelCurrectAction();

                GetComponent<ActionScheduler>().StartAction(this);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref angleSmoothVelocity, angleSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);


                
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir * maxSpeed * Time.deltaTime);

                float targetSpeed = maxSpeed * direction.magnitude;
                moveSpeed = Mathf.SmoothDamp(moveSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
                
                }
            else 
            {
                if (Mathf.Round(moveSpeed * 100f) >= .2f )
                {
                    moveSpeed = Mathf.SmoothDamp(moveSpeed, 0, ref speedSmoothVelocity, speedSmoothTime);
                }
                else
                {
                    keyMove = false;
                }               
            }
          
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            
            MoveTo(destination, speedFraction);
        }

        public bool MoveTo(Vector3 destination, float speedFraction)
        {
            
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;

            if (Vector3.Distance(transform.position, destination) > .02f)
            {
                return false;
            }
            else{
                return true;
            }
        }

        public void GracefullyStopAnimate ()
        {
            GetComponent<Animator>().SetFloat("forwardMovement", moveSpeed -= 4f * Time.deltaTime);
        }
        public void Cancel()
        {
            navMeshAgent.isStopped = true;

        }
        private void NavMeshAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            if (GetComponent<Animator>() == null) return;
             GetComponent<Animator>().SetFloat("forwardMovement", speed);
        }

        public void FollowTarget (CrewMember newTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            if (!isFollowing)
            {
                
            navMeshAgent.stoppingDistance = newTarget.followDistance * .8f;
            navMeshAgent.updateRotation = false;
            isFollowing = true;
            followTarget = newTarget.transform;
            //condition to follow + offset
            }
            FaceTarget ();
            MoveTo (followTarget.position, 1f);

        }      

        public void StopFollowTarget()
        {
            if (isFollowing)
            {
                navMeshAgent.isStopped = true;
            }
            navMeshAgent.stoppingDistance = 0f;
            navMeshAgent.updateRotation = true;
            followTarget = null;
            isFollowing = false;
        }

        private void FaceTarget()
        {
            Vector3 direction = (followTarget.position - transform.position).normalized;
            Quaternion loookRoation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, loookRoation, Time.deltaTime * 5f);
        }


        public void MoveToInteract (Interactable newTarget)
        {
            navMeshAgent.stoppingDistance = newTarget.interactRadius * .8f;
            StartMoveAction(newTarget.transform.position, 1f);
        }

        public void MoveToLocation(Vector3 newTarget)
        {
            navMeshAgent.stoppingDistance = 0f;
            StartMoveAction(newTarget, 1f);
        }




        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;

        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData data = (MoverSaveData)state;
            GetComponent<NavMeshAgent>().enabled = false; //disables navmash to not get in way
            transform.position = data.position.ToVector();
            transform.eulerAngles = data.rotation.ToVector();
            GetComponent<NavMeshAgent>().enabled = true; // restarts navmesh to allow character to move
        }
    }
}
