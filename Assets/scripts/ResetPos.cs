using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Collider ResetPoint;
    [SerializeField]
    private XROrigin xr;
    void Update()
    {
        if (Input.GetButtonDown("XRI_Right_Primary2DAxisClick"))
        {
            xr.transform.position=ResetPoint.transform.position;   
        }
    }

   
}
