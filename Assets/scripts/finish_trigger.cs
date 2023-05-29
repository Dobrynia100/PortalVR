using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finish_trigger : MonoBehaviour
{
    [SerializeField]
    GameObject finish;
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            finish.SetActive(true);
        }

    }
}
