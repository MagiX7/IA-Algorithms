using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AI;
using System;
using System.Linq;

public class Movement : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;

    private GameObject[] hidingSpots;

    public GameObject[] wayPoints;
    private int patrolWP = 0;

    private float freq = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        hidingSpots = GameObject.FindGameObjectsWithTag("Hide");
    }

    // Update is called once per frame
    void Update()
    {
        if (freq >= 2)
        {
            //Seek(target.transform.position);
            //Wander();
            //Pursue(target.transform.position);
            //Evade(target.transform.position);
            Hide(target);

            //if (!agent.pathPending && agent.remainingDistance < 0.5f) Patrol();

            freq = 0;
        }
        else
        {
            freq += .5f;
        }

    }

    void Seek(Vector3 target)
    {
        agent.destination = target;
    }

    void Flee(Vector3 origin)
    {
        agent.destination = -origin;
    }

    void Wander()
    {
        float radius = 0.5f;
        float offset = 0.7f;

        Vector3 localTarget = new Vector3(UnityEngine.Random.Range(-1, 1), 0, UnityEngine.Random.Range(-1, 1));
        localTarget.Normalize();

        Debug.Log(localTarget);

        localTarget *= radius;
        localTarget += new Vector3(0, 0, offset);

        Vector3 worldTarget = transform.TransformPoint(localTarget);
        worldTarget.y = 0;

        Seek(worldTarget);

    }

    void Pursue(Vector3 targetPos)
    {
        Vector3 targetDir = targetPos - transform.position;
        float lookAhead = targetDir.magnitude / agent.speed;
        Seek(targetPos + target.transform.forward * lookAhead);
    }

    void Evade(Vector3 origin)
    {
        Vector3 targetDir = origin - transform.position;
        float lookAhead = targetDir.magnitude / agent.speed;
        Flee(origin + target.transform.forward * lookAhead);
    }

    void Hide(GameObject target)
    {
        float minDist = 65553;
        var closestHidingSpot = hidingSpots[0];
        for(int i = 0 ; i < hidingSpots.Length; ++i)
        {
            float d = Vector3.Distance(agent.transform.position, hidingSpots[i].transform.position);
            if (minDist > d)
            {
                minDist = d;
                closestHidingSpot = hidingSpots[i];
            }
        }

        Vector3 dir = closestHidingSpot.transform.position - target.transform.position;
        Ray backRay = new Ray(closestHidingSpot.transform.position, -dir.normalized);
        RaycastHit info;
        closestHidingSpot.GetComponent<Collider>().Raycast(backRay, out info, 2.0f);
        Seek(info.point + dir.normalized + closestHidingSpot.transform.position);
    }

    float GetDistance(GameObject hidingSpot)
    {
        return Vector3.Distance(target.transform.position, hidingSpot.transform.position);
    }

    void Patrol()
    {
        patrolWP = (patrolWP + 1) % wayPoints.Length;
        Seek(wayPoints[patrolWP].transform.position);
    }

}