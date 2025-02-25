using UnityEngine;

public class Key : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        GameManager.instance.updateGameGoal(1);
    }
}
