using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public int objectivesInRange = 0;
    public List<Vector3> objectiveDir;
    Vector3 exitDir;
    public static Objective instance;
    Renderer model;

    [SerializeField] GameObject exit;
    [SerializeField] int pointSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        exitDir = exit.transform.position;
        model = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (objectivesInRange > 0 && objectivesInRange < 2)
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(objectiveDir[0].x - transform.position.x, 0, objectiveDir[0].z - transform.position.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * pointSpeed);
        }
        else if (objectivesInRange > 1)
        {
            gameObject.SetActive(true);
            Vector3 closestObj = compareDist(objectiveDir[0], objectiveDir[1]);
            for (int i = 2; i < objectivesInRange; i++)
            {
                closestObj = compareDist(objectiveDir[i], closestObj);
            }
            Quaternion rot = Quaternion.LookRotation(new Vector3(closestObj.x - transform.position.x, 0, closestObj.z - transform.position.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * pointSpeed);
        }
        else if (objectivesInRange <= 0)
        {
            model.material.color = Color.yellow;
            Quaternion rot = Quaternion.LookRotation(new Vector3(exitDir.x - transform.position.x, 0, exitDir.z - transform.position.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * pointSpeed);
        }
    }

    public void addDir(Vector3 dir)
    {
        objectiveDir.Add(dir);
        objectivesInRange++;
    }

    public void removeDir(Vector3 dir)
    {
        objectiveDir.Remove(dir);
        objectivesInRange--;
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
