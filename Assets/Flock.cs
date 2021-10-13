using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    public Vector3 direction;
    public float speed;
    public float dangerDistance;
    public GameObject dangerObject;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        direction = Flocking();

        transform.rotation = Quaternion.Slerp(transform.rotation,
                             Quaternion.LookRotation(direction),
                             myManager.rotationSpeed * Time.deltaTime);
        transform.Translate(0.0f, 0.0f, Time.deltaTime * speed);
    }

    public Vector3 Flocking()
    {
        Vector3 cohesion = Vector3.zero;
        Vector3 align = Vector3.zero;
        Vector3 separation = Vector3.zero;
        int num = 0;

        //if(transform.position.x < myManager.limits.transform.position.x || transform.position.y < myManager.limits.transform.position.y
        //    || transform.position.z < myManager.limits.transform.position.z)
        //{
        //    transform.forward = -transform.forward;
        //}

        foreach (GameObject go in myManager.allFish)
        {
            if (go != this.gameObject)
            {
                float distance = Vector3.Distance(go.transform.position,
                                                  transform.position);
                if (distance <= myManager.neighbourDistance)
                {
                    cohesion += go.transform.position;
                    num++;

                    align += go.GetComponent<Flock>().direction;
                    num++;

                    separation -= (transform.position - go.transform.position) / (distance * distance);
                }
            }

            // Leader
            float distLead = Vector3.Distance(go.transform.position, myManager.leadObject.transform.position);
            float distDanger = Vector3.Distance(go.transform.position, myManager.dangerObject.transform.position);
            if(distLead <= myManager.leadDistance)
            {
                separation -=  2 * (transform.position - myManager.leadObject.transform.position) / (distLead * distLead);
                num++;
            }
            // Danger
            if (distDanger <= myManager.leadDistance)
            {
                separation -= 2 * (myManager.leadObject.transform.position - transform.position) / (distDanger * distDanger);
                num++;
            }

            if (num > 0)
            {
                cohesion = (cohesion / num - transform.position).normalized * speed;
                
                align /= num;
                speed = Mathf.Clamp(align.magnitude, myManager.minSpeed, myManager.maxSpeed);
            }
        }

        return (cohesion + align + separation).normalized * speed;
    }
}