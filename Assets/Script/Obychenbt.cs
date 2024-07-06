using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obychenbt : MonoBehaviour
{
    public GameObject target;
    public GameObject obuchenya;
    private void Start()
    {
        target.SetActive(false);
        obuchenya.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        target.SetActive(true);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            obuchenya.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            ExitPodz();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        target.SetActive(false);
        obuchenya.SetActive(false);
    }
    public void ExitPodz()
    {
        obuchenya.SetActive(false);
        Time.timeScale = 1f;
    }
}
