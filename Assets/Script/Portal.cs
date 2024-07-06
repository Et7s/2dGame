using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public GameObject target;
    private void Start()
    {
        target.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        target.SetActive(true);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene("Level2");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        target.SetActive(false);
    }

}
