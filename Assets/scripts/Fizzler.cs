using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class Fizzler : XRBaseInteractor
{
    [SerializeField]
    private PortalPair Pair;
    [SerializeField]
    private Crosshair crosshair;
    [SerializeField] private AudioSource sound;
    
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))//  capsuleCollider.CompareTag("Player") )
        {
            Debug.Log("триггер физзлер");
            crosshair.portaldeactivated();
            Pair.Portals[0].RemovePortal();
            Pair.Portals[1].RemovePortal();
            sound.Play();
            
        }
    }
}
