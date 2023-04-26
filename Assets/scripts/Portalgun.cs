using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portalgun : MonoBehaviour
{
    [SerializeField] private float _fireRate = 25f;
    [SerializeField] private float _Range = 250f;
   
    [SerializeField] private AudioClip blueP;
    [SerializeField] private AudioClip OrangeP;
    [SerializeField] private AudioClip Grabbing;
    [SerializeField] private AudioClip InvalidSurface;
    [SerializeField] private AudioSource _shotSoundSource;
    [SerializeField] private Camera _cam;
    [SerializeField] private Portalgun portalgun;
    [SerializeField] private bool orangeactive;
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
           
          
            FirePortal(0, portalgun.transform.position, portalgun.transform.forward, _Range);
        }
        else if (orangeactive && Input.GetButtonDown("XRI_Right_PrimaryButton") && Time.time > _nextFire2)
        {
            _nextFire2 = Time.time + 1f / _fireRate;
            
          
            FirePortal(1, portalgun.transform.position, portalgun.transform.forward, _Range);
        }
        if (Input.GetButtonDown("XRI_Right_GripButton"))
        {           
               // _shotSoundSource.PlayOneShot(Grabbing);
            
        }
        
       
    }

    private void FirePortal(int portalID, Vector3 pos, Vector3 dir, float distance)
    {
        RaycastHit hit;
        Physics.Raycast(pos, dir, out hit, distance, layerMask);
       
        if (hit.collider != null && (hit.collider.tag == "white"|| hit.collider.tag == "Floor"))//проверка подходящей поверхности
        {
            Debug.Log(hit.collider.tag);
            // Ориентирует портал согласно направлению камеры и поверхности
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
            if (wasPlaced)//При успешной установке отображается индикатор и воспроизводится звук
            {
                Debug.Log("Placed");
                if (portalID == 0)
                {
                    _shotSoundSource.PlayOneShot(blueP);
                }
                else if (portalID == 1) { _shotSoundSource.PlayOneShot(OrangeP); }
                crosshair.SetPortalPlaced(portalID, true);
               
            }
            else
            {
                _shotSoundSource.PlayOneShot(InvalidSurface);
            }
        }
        else
        {
            _shotSoundSource.PlayOneShot(InvalidSurface);
        }
    }
}
