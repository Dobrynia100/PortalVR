using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameratransformsync : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Move()
    {
        transform.localPosition = Camera.main.transform.position;
        transform.localRotation = Camera.main.transform.rotation;
    }
}
