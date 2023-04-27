using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class play_Sound : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioG;
    // Update is called once per frame
    private bool played=false;
    private void OnTriggerEnter(Collider other)
    {
        if (!played && other.CompareTag("Player")) { audioG.Play(); played = true; }
    }
}
