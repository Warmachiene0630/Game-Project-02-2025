using UnityEngine;

public class PickUpKelee : MonoBehaviour
{
    [SerializeField] meleeStats melee;


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
