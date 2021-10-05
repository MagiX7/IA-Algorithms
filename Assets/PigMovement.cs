using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigMovement : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Seek(target.transform.position);
    }

    void Seek(Vector3 target)
    {
        Vector3 o = transform.position;
        Vector3 d = target;

        if (o.x > d.x)
            agent.destination = target;
    }
}
