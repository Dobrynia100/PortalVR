using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{

    
    [SerializeField]
    private Transform target;

    // Update is called once per frame
    void Update()
    {
       
        if (target.localRotation.x >= 0.1f || transform.rotation.x <= -0.1f)
        {
            Debug.Log("rotate");
            Debug.Log(target.localRotation);
            Debug.Log(target.localRotation.x);
            Debug.Log(target.localRotation.y);
            Debug.Log(target.localRotation.z);

            transform.localRotation = Quaternion.Euler(-target.localRotation.x, 0, 0);
            Debug.Log("rotated");
            Debug.Log(transform.localRotation);
            Debug.Log(target.localRotation.x);
            Debug.Log(target.localRotation.y);
            Debug.Log(transform.localRotation.z);


        }
        else if (transform.rotation.z <= -0.1f || target.localRotation.z >= 0.1f)
        {
            transform.localRotation = Quaternion.Euler(0, 0, -target.localRotation.z);
        }
       }
   
}
