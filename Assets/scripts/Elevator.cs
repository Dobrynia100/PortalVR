using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Elevator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public GameObject Lift;
    

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = Lift.transform.position;
            other.transform.rotation = Quaternion.Euler(0,180f, 0);
        }
    }
}
