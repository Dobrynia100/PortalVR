using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class hide_grab : MonoBehaviour
{
    [SerializeField]
    private GameObject Grabray;
    [SerializeField] private float _Range = 250f;
    [SerializeField] private Portalgun portalgun;
    // Start is called before the first frame update
    void Start()
    {
        Grabray.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Physics.Raycast(portalgun.transform.position, portalgun.transform.forward, out hit, _Range);
        if (hit.collider.tag=="Cube" && Input.GetButtonDown("XRI_Right_GrabButton"))
        {
            Grabray.SetActive(true);
            
        }
    }
}
