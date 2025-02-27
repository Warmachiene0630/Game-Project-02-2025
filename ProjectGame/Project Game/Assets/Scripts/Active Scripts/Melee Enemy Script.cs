using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyScript : MonoBehaviour
{
    [SerializeField] float roamRadius = 7f;
    [SerializeField] float roamInterval = 5f;
    private float roamTimer;

    void Update()
    {
        roamTimer += Time.deltaTime;
        if (roamTimer >= roamInterval)
        {
            Vector3 randomPoint = transform.position + Random.insideUnitSphere * roamRadius;
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, roamRadius, NavMesh.AllAreas))
            {
                SetRoamDestination(hit.position);
            }
            roamTimer = 0f;
        }
    }

    void SetRoamDestination(Vector3 destination)
    {
        if (TryGetComponent<NavMeshAgent>(out NavMeshAgent localAgent))
        {
            localAgent.SetDestination(destination);
        }
    }
}
