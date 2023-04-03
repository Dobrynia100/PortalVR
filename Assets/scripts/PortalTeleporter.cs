using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    public void Teleportplayer(Vector3 targetPosition)
    {
        player.transform.position = targetPosition; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
