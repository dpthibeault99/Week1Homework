using System.Collections;
using TMPro;
using UnityEngine;


public class ChestScript : MonoBehaviour
{
    CollectTriggerScripts collect;
    public TextMeshProUGUI notficationText;
    //public void OnTriggerEnter(Collider collision)
    //{
    //    if(collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("player hit chest");
    //        if(collect.coinCount >= 10)
    //        {
    //            notficationText.text = "Dubble Jump Unlocked";
    //            StartCoroutine(ResetNotification());
    //            Destroy(this.gameObject);
    //        }
    //        else
    //        {
    //            notficationText.text = "Need 10 coins to open";
    //            StartCoroutine(ResetNotification());
    //        }
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("player hit chest");
            if (collect.coinCount >= 10)
            {
                notficationText.text = "Dubble Jump Unlocked";
                StartCoroutine(ResetNotification());
                Destroy(this.gameObject);
            }
            else
            {
                notficationText.text = "Need 10 coins to open";
                StartCoroutine(ResetNotification());
            }
        }
    }


    IEnumerator ResetNotification()
    {
       yield return new WaitForSeconds(2f);
       notficationText.text = "  ";
    }
}
