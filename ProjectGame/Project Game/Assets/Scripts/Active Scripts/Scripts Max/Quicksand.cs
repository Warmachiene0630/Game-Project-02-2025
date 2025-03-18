using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Quicksand : MonoBehaviour
{
    [SerializeField] int dmgAmount;
    [SerializeField] int gravAmount;

    bool isDamaging = false;
    int dmgCount = 0;
    bool isInside;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.player.SendMessage("changeGravity", gravAmount);
            isInside = true;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IDamage dmg = GameManager.instance.player.GetComponent<IDamage>();
            if (dmg != null)
            {
                if (!isDamaging)
                {
                    StartCoroutine(damageOther(dmg));
                }
            }
        }
    }

    IEnumerator damageOther(IDamage dmg)
    {
        isDamaging = true;
        if (dmgCount == 0)
        {
            dmgCount++;
            yield return new WaitForSeconds(1);
            if (isInside)
            {
                GameManager.instance.playerQuicksandScreen.SetActive(true);
            }
        }
        yield return new WaitForSeconds(1);
        if (isInside)
        {
            dmg.takeDamage(dmgAmount);
        }
        isDamaging = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.player.SendMessage("revertGravity");
            GameManager.instance.playerQuicksandScreen.SetActive(false);
            dmgCount = 0;
            isInside = false;
        }
    }
}
