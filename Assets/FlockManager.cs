using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject entityPrefab;
    public int numEntity;
    public GameObject[] allEntity;
    public GameObject leadObject;
    public GameObject dangerObject;

    // Fish Settings
    public float minSpeed;
    public float maxSpeed;
    public float neighbourDistance;
    public float leadDistance;
    public float dangerDistance;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        allEntity = new GameObject[numEntity];

        for (int i = 0; i < numEntity; ++i)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1));
            Vector3 randomize = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            allEntity[i] = (GameObject)Instantiate(entityPrefab, pos, Quaternion.LookRotation(randomize));
            allEntity[i].GetComponent<Flock>().myManager = this;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
