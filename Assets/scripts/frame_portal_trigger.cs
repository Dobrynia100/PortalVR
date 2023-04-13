using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class frame_portal_trigger : frame_portal
{
    // Start is called before the first frame update
    Collision collision;
    //[SerializeField] GameObject trigger;

    // Update is called once per frame
    void Update()
    {
        if (collision.gameObject.CompareTag("Player"))//  capsuleCollider.CompareTag("Player") )
        {
           SpawnPortal();
        }
    }
}
