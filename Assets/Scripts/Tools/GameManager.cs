using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public StatusManager statusManager;
    public EntityManager entityManager;
    

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public StatusManager StatusManager { get { return statusManager; } }
    public EntityManager EntityManager { get { return entityManager; } }


}
