using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Player;
using UnityEngine;

public abstract class ConditionalMono : MonoBehaviour
{
    public static Dictionary<Type, List<ConditionalMono>> conditionalEvents = new Dictionary<Type, List<ConditionalMono>>();
    [SerializeField] protected GameObject conditionalObject;
    [SerializeField] protected event Callback conditionalEvent;
    [SerializeField] protected bool isAffectObject = true;
    protected Predicate<object> condition;
    protected virtual void Awake()
    {
        if (!conditionalEvents.ContainsKey(GetType()))
        {
            conditionalEvents.TryAdd(GetType(), new List<ConditionalMono>());
        }
        conditionalEvents[GetType()].Add(this);
        if (conditionalObject == null)
            conditionalObject = gameObject;
        condition = SetCondition();
    }
    protected virtual void OnDestroy()
    {
        conditionalEvents[GetType()].Remove(this);
    }
    protected virtual void Start()
    {
        UpdateObject();
    }
    protected abstract Predicate<object> SetCondition(); 
    public void UpdateObject()
    {
        bool isSastified = (condition?.Invoke(null)).Value;
        //Debug.Log(GetType() + isSastified.ToString());
        if (isSastified)
        {
            conditionalEvent?.Invoke();
        }
        if (isAffectObject)
        {
            conditionalObject.SetActive(isSastified);
        }
    }
    public static void UpdateObject(Type type)
    {
        if(conditionalEvents.ContainsKey(type))
            conditionalEvents[type].ForEach((con) => con.UpdateObject());
    }
}
