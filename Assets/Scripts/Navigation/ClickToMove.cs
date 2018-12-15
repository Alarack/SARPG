using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour {

    public LayerMask walkableArea;
    private NavMeshAgent navMeshAgent;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {


        if (Input.GetButtonDown("Fire1"))
        {
            SetDestination();
        }

    }



    private void SetDestination()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(hit.collider != null)
            {
                if (LayerTools.IsLayerInMask(walkableArea, hit.collider.gameObject.layer))
                {
                    navMeshAgent.SetDestination(hit.point);
                }
            }





        }
    }


}
