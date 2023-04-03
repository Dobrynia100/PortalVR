using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portaltrigger : MonoBehaviour
{
    public GameObject otherPortal;
    public Transform teleportTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
           // otherPortal.GetComponent<PortalTeleporter>().TeleportPlayer(teleportTan);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
