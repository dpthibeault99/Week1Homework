using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class CollectTriggerScripts : MonoBehaviour
{
    [Header("Texts for collectablies")]
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI notficationText;

    [Header("Numbers for collectablies")]
    public float coinCount;


    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Coin is touched");
            coinCount++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Chest"))
        {
            Debug.Log("player hit chest");
            if (coinCount >= 10)
            {
                notficationText.text = "Dubble Jump Unlocked";
                StartCoroutine(ResetNotification());
                Destroy(collision.gameObject);
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

    private void Update()
    {
        coinText.text = $"{coinCount}";
    }
    // fun fact
    // there is diifrent steps to make a pro builder asset a trigger
    // its dumb, tools, somthing, set trigger on the mesh
    // also, i forgot .text for the text

}
