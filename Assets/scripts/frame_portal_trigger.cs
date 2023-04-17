using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class frame_portal_trigger : XRBaseInteractor
{
    // Start is called before the first frame update

    [SerializeField]
    private PortalPair portals;
    [SerializeField]
    Collider frame;
    [SerializeField]
    private Crosshair crosshair;
    //[SerializeField] GameObject trigger;


    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))//  capsuleCollider.CompareTag("Player") )
        {
            Debug.Log("триггер рамка ");
            portals.Portals[1].PlacePortal(frame, frame.transform.position, Quaternion.Euler(0.00f, 90.0f, 0.00f));
            crosshair.SetPortalPlaced(1,true);
        }
    }
}
