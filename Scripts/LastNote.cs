using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LastNote : MonoBehaviour
{
    
    private IEnumerator coroutine;


    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
            coroutine = WaitAndPrint(15f);
            StartCoroutine(coroutine);

        }

    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            
        }

    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("Credits");
    }


}
