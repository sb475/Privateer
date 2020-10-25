using RPG.AI;
using RPG.Combat;
using RPG.Items;
using RPG.Movement;
using RPG.Stats;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour, IStat, IInventory
{


    Inventory IInventory.inventory { get { return inventory; } set { inventory = value; } }
    public float defaultHealth;

    public Rigidbody body;
    public Animator animator;
    public IEngine engine;
    public Fighter fighter;
    public GAgent agent;

    public DefaultEquip defaultEquip;
    public StatList statData;
    float currentActionPoints;

    public CharacterClass charClass;
    public CharacterRace charRace;
    public Dictionary<StatName, float> characterModAdd;
    public Dictionary<StatName, float> characterBase;
    public Dictionary<StatName, float> characterModPercent;

    //update to implant.
    public Dictionary<string, int> characterImplant;
    public Dictionary<CharacterPointTypes, int> characterPoints;
    public Inventory inventory;
    public CharacterEquipment equipment;
    public CharacterPerks perks;


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
        characterPoints = new Dictionary<CharacterPointTypes, int>();
        perks = new CharacterPerks(this);
        inventory = new Inventory();
        equipment = new CharacterEquipment(this);

        characterBase.Add(StatName.Health, defaultHealth);
        InitializePoints();
        equipment.InitializeEquipment();
        currentActionPoints = characterPoints[CharacterPointTypes.action];
    }

    private void Start()
    {
        //Invoke("DebugEvent", 20f);
    }

    private void DebugEvent()
    {
        fighter.EquipPrimary();
    }

    public void InitializePoints()
    {
        characterPoints.Add(CharacterPointTypes.action, 2);
    }
    public void InitializeStats()
    {
        if (statData != null)
        {
            foreach(Modifier stat in statData.statList)
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
}



