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
using System;

namespace RPG.Movement
{
    
    public class ShipEngine : MonoBehaviour, IAction, ISaveable, IEngine
    {
        public Transform cam;

        [SerializeField] Transform followTarget;

        [Header("Engine Attributes")]
        public float maxSpeed = 6f;
        public float turningSpeed = 6f;
        public float thrust = 6f;
        Vector3 acceleration;
        Vector3 velocity;
        float storeMaxSpeed;
        float targetSpeed;
        
        public Vector3 moveToLocation;
        public bool aIMovement;



        [SerializeField] float followDistance = 4f;
        [SerializeField] bool isFollowing;
        private Rigidbody body;
        Rigidbody shipBody;
        Health health;
        Quaternion rotateToTarget;


        [Header("Controller Variables")]
        bool keyMove;
        float horizontal;
        float vertical;
        float w;
        float s;
        float angleSmoothVelocity = 0f;
        private float angleSmoothTime = 0.1f ;
        private float speedSmoothVelocity = 0f;
        private float speedSmoothTime = 0.1f;
        NavMeshAgent navMeshAgent;

        private void Awake() 
        {
            shipBody = GetComponent<Rigidbody>();
            health = GetComponent<Health>();
            isFollowing = false;
            body = GetComponent<Rigidbody>();
            navMeshAgent =GetComponent<NavMeshAgent>();

            storeMaxSpeed = maxSpeed;
            targetSpeed = storeMaxSpeed;
           

            cam = Camera.main.transform;
        }
        // Update is called once per frame

        private void Start() {
            navMeshAgent.Warp(transform.position);
            storeMaxSpeed = maxSpeed;
            targetSpeed = storeMaxSpeed;
        }

        void FixedUpdate()
        {
            
            if (horizontal != 0 || vertical != 0)
            {
                ManualOverride();
            }
            else
            {
                if (aIMovement)
                {
                    Debug.DrawLine(transform.position, moveToLocation);
                    MoveTo(moveToLocation, 1f);
                }
            }
        }

        private void ManualOverride()
        {

            Debug.Log("Manual Override");
            aIMovement = false;
            velocity += acceleration * thrust * Time.deltaTime * vertical;

            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }

            Debug.Log(horizontal + "Hoirzontal");
            Debug.Log(vertical + "Vert");


            shipBody.AddForce(transform.forward * thrust * vertical, ForceMode.Force);  

            shipBody.AddTorque(transform.up * turningSpeed * horizontal, ForceMode.Force);

            Debug.Log(Vector3.forward * thrust * vertical);

        }

        Vector3 GetTargetDistance(Vector3 target)
        {
            Vector3 distance = target - transform.position;

            if (distance.magnitude < 25f)
            {
                return distance.normalized * -maxSpeed;
            }
            else
            {
                return distance.normalized * maxSpeed;
            }
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, 1f);

        }


        public bool MoveTo(Vector3 destination, float speedFraction)
        {

            if (Vector3.Distance(transform.position, destination) < .5f) return true;

            Debug.DrawRay(transform.position, destination, Color.yellow);
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
            return false;


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

        //     Vector3 forces = GetTargetDistance(newTarget);

        //     //add more varables if need be
        //     acceleration = forces * thrust;

        //     velocity += acceleration * Time.deltaTime;

        //     if (velocity.magnitude > maxSpeed)
        //     {
        //         velocity = velocity.normalized * maxSpeed;
        //     }
        //     Debug.Log(velocity);
        //     shipBody.AddForce(velocity);

        //     Quaternion desiredRotation = Quaternion.LookRotation(velocity);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * turningSpeed);
        
        public void KeyMovement()
        {

//            Debug.Log(gameObject.name);

            horizontal = Input.GetAxisRaw("Horizontal");


            vertical = Input.GetAxisRaw("Vertical");

            //add more varables if need be

        }

        public void MoveToInteract(Interactable newTarget)
        {
            navMeshAgent.stoppingDistance = newTarget.interactRadius * .8f;
            StartMoveAction(newTarget.transform.position, 1f);
        }

        public void MoveToLocation(Vector3 newTarget)
        {
            navMeshAgent.stoppingDistance = 0f;
            StartMoveAction(newTarget, 1f);
        }

        public void GracefullyStopAnimate()
        {
            GetComponent<Animator>().SetFloat("forwardMovement", thrust);
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

        public void SetSpeed(float speed)
        {
            
        }
    }
}
