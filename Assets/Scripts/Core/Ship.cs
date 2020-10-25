using RPG.AI;
using RPG.Combat;
using RPG.Control;
using RPG.Items;
using RPG.Movement;
using RPG.Stats;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

namespace RPG.Core
{
    public class Ship : MonoBehaviour, IControllable, IStat, IInventory
    {
        public GameObject target;

        public float defaultHealth;

        public Rigidbody body;
        public Animator animator;
        public IEngine engine;
        public IFighter fighter;
        public GAgent agent;

        public DefaultEquip defaultEquip;
        public StatList statData;
        float currentActionPoints;

        public Dictionary<StatName, float> characterModAdd;
        public Dictionary<StatName, float> characterBase;
        public Dictionary<StatName, float> characterModPercent;

        public Inventory cargoHold;
        public InventoryData cargoLedger;
        public CharacterEquipment equipment;

        public Inventory inventory { get { return cargoHold; } set { cargoHold = value; } }

        public virtual void Awake()
        {
            animator = GetComponent<Animator>();
            engine = GetComponent<IEngine>();
            body = GetComponent<Rigidbody>();
            fighter = GetComponent<Fighter>();
            agent = GetComponent<GAgent>();

            characterBase = new Dictionary<StatName, float>();
            characterModAdd = new Dictionary<StatName, float>();
            characterModPercent = new Dictionary<StatName, float>();
            //characterPoints = new Dictionary<CharacterPointTypes, int>();
            //cargoHold = new Inventory(cargoLedger.inventoryData);
            //equipment = new CharacterEquipment(null);
        }

        private void Start()
        {
            //Invoke("DebugEvent", 20f);
        }

        private void DebugEvent()
        {
            //fighter.EquipPrimary();
        }

        public void InitializePoints()
        {
            //characterPoints.Add(CharacterPointTypes.action, 2);
        }
        public void InitializeStats()
        {
            if (statData != null)
            {
                foreach (Modifier stat in statData.statList)
                {
                    characterBase.Add(stat.statType, stat.statValue);
                }
            }
        }
        public float GetStat(StatName stat)
        {
            float value;
            if (!characterBase.TryGetValue(stat, out value))
            {
                Debug.LogError("myDict doesn't have the value " + stat);
            }
            else
            {
                return value;
            }

            return 0;
        }

        public void KeyMovement()
        {
            throw new System.NotImplementedException();
        }

        public void IssueCommand(ManualActions action, GameObject target)
        {
            throw new System.NotImplementedException();
        }

        public void IssueCommand(ManualActions action, Vector3 target)
        {
            throw new System.NotImplementedException();
        }
    }

 }
