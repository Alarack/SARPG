using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimationManager : MonoBehaviour {


    private Animator animator;
    private Entity parentEntity;


    public void Initialize(Animator animator, Entity parent)
    {
        this.animator = animator;
        this.parentEntity = parent;
    }


    public bool SetAnimTrigger(string trigger)
    {
        //if(animator == null)
        //{
        //    Debug.LogError(parentEntity.entityName + " has not provided an animator to its animation manager");
        //    return false;
        //}

        if (string.IsNullOrEmpty(trigger) == true)
            return false;

        try
        {
            animator.SetTrigger(trigger);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(parentEntity.entityName + " Could not play an animation: " + e);
            return false;
        }


        //return true;
    }


}
