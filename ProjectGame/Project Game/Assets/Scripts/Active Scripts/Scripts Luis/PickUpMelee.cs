using UnityEngine;

public class PickUpMelee : MonoBehaviour
{
    [SerializeField] meleeStats melee;
    [SerializeField] Collider col;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        IPickUp pick = other.GetComponent<IPickUp>();
        if (pick != null)
        {
            pick.getMeleeStats(melee);
            Destroy(gameObject);
        }
    }
}
