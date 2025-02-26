using UnityEngine;

public class Key : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.updateGameGoal(1);
        Destroy(gameObject);

    }
}
