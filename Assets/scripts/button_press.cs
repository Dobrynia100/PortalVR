using UnityEngine;
using System.Collections;

public class button_press : MonoBehaviour
{
    [SerializeField]
    public GameObject door1;
    [SerializeField]
    public GameObject door2;
    [SerializeField]
    public float doorOpenSpeed = 1.0f; 
    public Vector3 door1OpenPositionOffset;
    public Vector3 door2OpenPositionOffset;
    [SerializeField]
    public float buttonMoveDistance = 5.0f;
    [SerializeField]
    private Vector3 door1ClosedPosition;
    [SerializeField]
    private Vector3 door2ClosedPosition;
    [SerializeField]
    private Vector3 door1OpenedPosition;
    [SerializeField]
    private Vector3 door2OpenedPosition;
    
    private Vector3 buttonInitialPosition;
    private Vector3 buttonPressedPosition;

    private bool doorsOpened = false;
    
    [SerializeField]
    public button_press otherButton;
    private void Awake()
    {
        CheckInitialDoorState();
     
    }
    
    private void Start()
    {
        door1ClosedPosition = door1.transform.localPosition;
        door2ClosedPosition = door2.transform.localPosition;
        //door1OpenedPosition = door1ClosedPosition + door1OpenPositionOffset;
        //door2OpenedPosition = door2ClosedPosition + door2OpenPositionOffset;
        
        buttonInitialPosition = transform.position;
        buttonPressedPosition = buttonInitialPosition - new Vector3(0, buttonMoveDistance, 0);
        
    }
    private void CheckInitialDoorState()
    {
        if (doorsOpened && (otherButton == null || !otherButton.doorsOpened))
        {
            //StartCoroutine(CloseDoors());
            doorsOpened = false;
        }

    }
    private void OnTriggerEnter(Collider other)
    {

       
        if ((other.CompareTag("Player") || other.CompareTag("Cube")) && !doorsOpened )
        {
            StartCoroutine(OpenDoors());
            doorsOpened = true;
           
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.CompareTag("Cube") || other.CompareTag("Player")) && doorsOpened)
        {
            StartCoroutine(CloseDoors());
            doorsOpened = false;
        }
    }

    private IEnumerator OpenDoors()
    {
        float progress = 0;
        if (otherButton != null && !otherButton.doorsOpened)
        {
            yield break;
        }
        while (progress < 1)
        {
            door1.transform.localPosition = Vector3.Lerp(door1ClosedPosition, door1OpenedPosition, progress);
            door2.transform.localPosition = Vector3.Lerp(door2ClosedPosition, door2OpenedPosition, progress);
            transform.position = Vector3.Lerp(buttonInitialPosition, buttonPressedPosition, progress);
            progress += Time.deltaTime * doorOpenSpeed;
            yield return null;
        }
        door1.transform.localPosition = door1OpenedPosition;
        door2.transform.localPosition = door2OpenedPosition;
        transform.position = buttonPressedPosition;
        
        
    }

    private IEnumerator CloseDoors()
    {
        float progress = 0;
        
        while (progress < 1)
        {
            door1.transform.localPosition = Vector3.Lerp(door1OpenedPosition, door1ClosedPosition, progress);
            door2.transform.localPosition = Vector3.Lerp(door2OpenedPosition, door2ClosedPosition, progress);
            transform.position = Vector3.Lerp(buttonPressedPosition, buttonInitialPosition, progress);
            progress += Time.deltaTime * doorOpenSpeed;
            yield return null;
        }
        door1.transform.localPosition = door1ClosedPosition;
        door2.transform.localPosition = door2ClosedPosition;
        transform.position = buttonInitialPosition;
    }
}