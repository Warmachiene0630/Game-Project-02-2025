using System.Collections;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform originPoint;
    [SerializeField] Transform teleportPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    /*void Update()
    {
        //if conditon is met then teleport (prompt user to press button in ui)
        if (Input.GetKeyDown(KeyCode.E))
        {
            //if attatched to player use game object, if not create a serialized field and drag in player
            //attatch to first object/position and create serialized field for second?
            StartCoroutine("Teleport");
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            //Doesn't work if key is pressed NEED KEY FOR NOT TRAP
            // if (Input.GetKeyDown(KeyCode.E))
            //{
            //if attatched to player use game object, if not create a serialized field and drag in player
            //attatch to first object/position and create serialized field for second?
            StartCoroutine("Teleport");
            // }
        }
    }

    IEnumerator Teleport()
    {
        GameManager.instance.playerScript.disabled = true;
        yield return new WaitForSeconds(.2f);
        GameManager.instance.player.transform.position = new Vector3(teleportPoint.position.x, teleportPoint.position.y, teleportPoint.position.z);
        yield return new WaitForSeconds(.2f);
        GameManager.instance.playerScript.disabled = false;
    }
}
