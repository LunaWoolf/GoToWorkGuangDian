using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMouseClickMovement : MonoBehaviour
{
    NavMeshAgent PlayerAgent;
    // Start is called before the first frame update
    void Start()
    {
        PlayerAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                PlayerAgent.SetDestination(hit.point);
            }
        }
    }
}
