using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SARPG;

public class Entity : MonoBehaviour {

    public string entityName;

    [Header("Entity Stats")]
    public StatCollectionData statTemplate;
    public StatCollection Stats { get; private set; }

    [Header("Effect Delivery")]
    public EffectDelivery effectDelivery;

    public Animator Animator { get; private set; }
    public int EntityID { get; private set; }
    public EntityAnimationManager AnimationManager { get; private set; }

    //[Header("Abilities")]
    public AbilityManager AbilityManager { get; protected set; }


    private NavMeshAgent navAgent;


    #region INIT
    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        AnimationManager = GetComponentInChildren<EntityAnimationManager>();
        AbilityManager = GetComponent<AbilityManager>();
        


        if (AnimationManager != null)
            AnimationManager.Initialize(Animator, this);

        Initialize();
    }

    public void Initialize()
    {
        Register();
        InitStats();

        if (AbilityManager != null)
            AbilityManager.Initialize(gameObject);

    }

    private void Register()
    {
        EntityID = IDFactory.GenerateEntityID();
        EntityManager.RegisterEntity(this);
    }

    private void InitStats()
    {
        if (Stats != null)
            return;

        Stats = new StatCollection(gameObject, OnStatChanged, statTemplate);

        UpdateNavAgent(BaseStat.StatType.MoveSpeed);
        UpdateNavAgent(BaseStat.StatType.RotateSpeed);
    }



    #endregion


    #region EVENTS

    private void OnStatChanged(BaseStat.StatType type, GameObject cause)
    {
        //Local Event
        UpdateNavAgent(type);

        //Global Event
        EventData data = new EventData();
        data.AddInt("Type", (int)type);
        data.AddGameObject("Target", this.gameObject);
        data.AddGameObject("Cause", cause);
        EventGrid.EventManager.SendEvent(Constants.GameEvent.StatChanged, data);
    }


    private void UpdateNavAgent(BaseStat.StatType type)
    {
        switch (type)
        {
            case BaseStat.StatType.MoveSpeed:
                navAgent.speed = Stats.GetStatModifiedValue(BaseStat.StatType.MoveSpeed);
                break;

            case BaseStat.StatType.RotateSpeed:
                navAgent.angularSpeed = Stats.GetStatModifiedValue(BaseStat.StatType.RotateSpeed);
                break;
        }
    }


    #endregion
}
