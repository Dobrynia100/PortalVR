using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrigCubeDisp : MonoBehaviour
{
    // Start is called before the first frame update



    public GameObject cubePrefab;
    public Transform spawnPoint;
    [SerializeField]
    private AudioSource audioC;
    [SerializeField]
    private GameObject cubedropper;
    public bool cubeSpawned = false;
    private bool IsOpen = false;
    private void Start()
    {
        IsOpen = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !cubeSpawned)
        {
            cubeSpawned = true;
            IsOpen= true;
            cubedropper.GetComponent<Animator>().SetBool("open", IsOpen);
            GameObject newCube = Instantiate(cubePrefab, spawnPoint.position, spawnPoint.rotation);
            audioC.Play();
            

        }
       
    }

}
