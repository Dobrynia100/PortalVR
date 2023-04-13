using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class frame_portal : MonoBehaviour
{
    [SerializeField]
    private PortalPair portals;
    [SerializeField]
    Collider collider;
    // Start is called before the first frame update
    public void SpawnPortal()
    {
        portals.Portals[1].PlacePortal(collider, collider.transform.position, collider.transform.rotation);
    }

    
}
