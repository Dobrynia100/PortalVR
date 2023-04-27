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
    private bool cubeSpawned = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !cubeSpawned)
        {
            cubeSpawned = true;
            GameObject newCube = Instantiate(cubePrefab, spawnPoint.position, spawnPoint.rotation);
            audioC.Play();
        }
    }

}
