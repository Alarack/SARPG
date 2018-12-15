using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

    public static EntityManager instance;

    public static List<Entity> allEntites = new List<Entity>();


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }




    public static void RegisterEntity(Entity target)
    {
        allEntites.AddUnique(target);
    }

    public static void UnRegisterEntity(Entity target)
    {
        allEntites.RemoveIfContains(target);
    }

    public static Entity GetEntityByID(int id)
    {
        int count = allEntites.Count;
        for (int i = 0; i < count; i++)
        {
            if (allEntites[i].EntityID == id)
                return allEntites[i];
        }

        return null;
    }

    



}
