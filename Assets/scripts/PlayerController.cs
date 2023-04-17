using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class PlayerController : PortalableObject
{
    // private CameraMove cameraMove;
    private XROrigin origin;
    private CapsuleCollider Collider;
    private void Start()
    {
        origin=GetComponent<XROrigin>();
        Collider = GetComponent<CapsuleCollider>();  
    }
    protected override void Awake()
    {
        base.Awake();

       // cameraMove = GetComponent<CameraMove>();
    }
    private void Update()
    {
       // Debug.Log("место");
        var center = origin.CameraInOriginSpacePos;
        Collider.center = new Vector3(center.x, Collider.center.y, center.z);
        Collider.height = origin.CameraInOriginSpaceHeight + 0.1f;
        //if (!origin.transform.rotation.x.Equals(0))
        //{
        //    Debug.Log("ротация х 0");
        //    origin.transform.rotation.Set(0.0f, transform.rotation.y, transform.rotation.z, transform.rotation.w);

        //}
    }
    public override void Warp()
    {
        Debug.Log("Обнаружен игрок");
        base.Warp();
       // cameraMove.ResetTargetRotation();
    }
}
