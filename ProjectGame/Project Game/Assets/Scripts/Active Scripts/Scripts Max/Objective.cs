using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public int objectivesInRange = 0;
    public List<Vector3> objectiveDir;

    [SerializeField] int pointSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (objectivesInRange > 0 && objectivesInRange < 2)
        {
            gameObject.SetActive(true);
            Quaternion rot = Quaternion.LookRotation(new Vector3(objectiveDir[0].x, 0, objectiveDir[0].z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * pointSpeed);
        }
        else if (objectivesInRange > 1)
        {
            gameObject.SetActive(true);
            Vector3 closestObj = compareDist(objectiveDir[0], objectiveDir[1]);
            for (int i = 1; i < objectivesInRange; i++)
            {
                closestObj = compareDist(objectiveDir[i], closestObj);
            }
            Quaternion rot = Quaternion.LookRotation(new Vector3(closestObj.x, 0, closestObj.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * pointSpeed);
        }
        else if (objectivesInRange <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Objective"))
        {
            objectivesInRange++;
            objectiveDir.Add(other.transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Objective"))
        {
            objectivesInRange--;
            objectiveDir.Remove(other.transform.position);
        }
    }

    Vector3 compareDist(Vector3 a, Vector3 b)
    {
        Vector3 closer = a;
        if (Vector3.Distance(a, transform.position) > Vector3.Distance(b, transform.position))
        {
            closer = b;
        }
        return closer;
    }
}
