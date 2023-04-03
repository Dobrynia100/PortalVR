using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portalgun : MonoBehaviour
{
    [SerializeField] private float _fireRate = 25f;
    [SerializeField] private float _Range = 200f;
    [SerializeField] private GameObject _Blue_portal;
    [SerializeField] private GameObject _Orange_portal;
    [SerializeField] private AudioClip _shotSFX;
    [SerializeField] private AudioSource _shotSoundSource;
    [SerializeField] private Camera _cam;
    [SerializeField] private Portalgun portalgun;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private PortalPair portals;
    [SerializeField]
    private Crosshair crosshair;

    // Start is called before the first frame update
    private float _nextFire1 = 0f,_nextFire2=0f;
  

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("XRI_Right_TriggerButton") && Time.time > _nextFire1)
        {
            _nextFire1 = Time.time + 1f / _fireRate;
            _shotSoundSource.PlayOneShot(_shotSFX);
           // Shoot();
            FirePortal(0, portalgun.transform.position, portalgun.transform.forward, 250.0f);
        }
        else if (Input.GetButtonDown("XRI_Right_GripButton") && Time.time > _nextFire2)
        {
            _nextFire2 = Time.time + 1f / _fireRate;
            _shotSoundSource.PlayOneShot(_shotSFX);
          //  Shoot();
            FirePortal(1, portalgun.transform.position, portalgun.transform.forward, 250.0f);
        }

    }
    void Shoot()
    {
       
        RaycastHit hit;
        if (Physics.Raycast(portalgun.transform.position, portalgun.transform.forward, out hit, _Range))
        {
            if (hit.rigidbody != null)
            {
                var portal = Instantiate(_Blue_portal, hit.point, Quaternion.LookRotation(hit.normal), hit.collider.gameObject.transform);

            }
        }
    }
    

    private void FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;
        Physics.Raycast(pos, dir, out hit, distance, layerMask);
        Debug.Log("0");
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.tag);
            // If we shoot a portal, recursively fire through the portal.
            if (hit.collider.tag == "Portal")
            {
                Debug.Log("тэг портал");
                var inPortal = hit.collider.GetComponent<Portal>();

                if(inPortal == null)
                {
                    return;
                }

                var outPortal = inPortal.OtherPortal;

                // Update position of raycast origin with small offset.
                Vector3 relativePos = inPortal.transform.InverseTransformPoint(hit.point + dir);
                relativePos = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativePos;
                pos = outPortal.transform.TransformPoint(relativePos);

                // Update direction of raycast.
                Vector3 relativeDir = inPortal.transform.InverseTransformDirection(dir);
                relativeDir = Quaternion.Euler(0.0f, 180.0f, 0.0f) * relativeDir;
                dir = outPortal.transform.TransformDirection(relativeDir);

                distance -= Vector3.Distance(pos, hit.point);

                FirePortal(portalID, pos, dir, distance);

                return;
            }

            // Orient the portal according to camera look direction and surface direction.
            var cameraRotation = _cam.transform.rotation;
            var portalRight = cameraRotation * Vector3.right;
            
            if(Mathf.Abs(portalRight.x) >= Mathf.Abs(portalRight.z))
            {
                portalRight = (portalRight.x >= 0) ? Vector3.right : -Vector3.right;
            }
            else
            {
                portalRight = (portalRight.z >= 0) ? Vector3.forward : -Vector3.forward;
            }

            var portalForward = -hit.normal;
            var portalUp = -Vector3.Cross(portalRight, portalForward);

            var portalRotation = Quaternion.LookRotation(portalForward, portalUp);
            bool wasPlaced = portals.Portals[portalID].PlacePortal(hit.collider, hit.point, portalRotation);
            if (wasPlaced)
            {
                crosshair.SetPortalPlaced(portalID, true);
            }
        }
    }
}
