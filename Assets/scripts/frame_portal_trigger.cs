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
    public float x=-90, y=0, z=0;
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("триггер рамка ");
            portals.Portals[1].PlacePortal(frame, frame.transform.position, frame.transform.rotation*Quaternion.Euler(x,y,z));
            crosshair.SetPortalPlaced(1,true);
        }
    }
}
