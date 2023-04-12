using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : MonoBehaviour
{
    [field: SerializeField]
    public Portal OtherPortal { get; private set; }

    [SerializeField]
    private Renderer outlineRenderer;

    [field: SerializeField]
    public Color PortalColour { get; private set; }

    [SerializeField]
    private LayerMask placementMask;

    [SerializeField]
    private Transform testTransform;

    private List<PortalableObject> portalObjects = new List<PortalableObject>();
    public bool IsPlaced { get; private set; } = false;
    private Collider wallCollider;

    // Components.
    public Renderer Renderer { get; private set; }
    private new BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
        Renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        outlineRenderer.material.SetColor("_OutlineColour", PortalColour);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        Renderer.enabled = OtherPortal.IsPlaced;

        for (int i = 0; i < portalObjects.Count; ++i)
        {
            Vector3 objPos = transform.InverseTransformPoint(portalObjects[i].transform.position);
           // Debug.Log(objPos.z);
            if (objPos.z > 0.0f)//�� ������? ������� ��������
            {
                Debug.Log("�����������?");
                Debug.Log(objPos.z);
                portalObjects[i].Warp();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("��������� ����� � ������");
        var obj = other.GetComponent<PortalableObject>();
        if (obj != null)
        {
            portalObjects.Add(obj);
            Debug.Log("��������� � ������");
            obj.SetIsInPortal(this, OtherPortal, wallCollider);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<PortalableObject>();

        if (portalObjects.Contains(obj))
        {
            portalObjects.Remove(obj);
            obj.ExitPortal(wallCollider);
        }
    }

    public bool PlacePortal(Collider wallCollider, Vector3 pos, Quaternion rot)
    {
        Debug.Log("������");
        testTransform.position = pos;
        testTransform.rotation = rot;
        testTransform.position -= testTransform.forward * 0.001f;

        FixOverhangs();
        FixIntersects();

        if (CheckOverlap())
        {
            Debug.Log("���");
            this.wallCollider = wallCollider;
            transform.position = testTransform.position;
            transform.rotation = testTransform.rotation;

            gameObject.SetActive(true);
            IsPlaced = true;
            return true;
        }

        return false;
    }

    // Ensure the portal cannot extend past the edge of a surface.
    private void FixOverhangs()
    {
        float x=0.45f, y=0.85f;
        var testPoints = new List<Vector3>
        {
            //new Vector3(-1.1f,  0.0f, 0.1f),
            //new Vector3( 1.1f,  0.0f, 0.1f),
            //new Vector3( 0.0f, -2.1f, 0.1f),
            //new Vector3( 0.0f,  2.1f, 0.1f)
             new Vector3(-x,  0.0f, 0.1f),
            new Vector3( x,  0.0f, 0.1f),
            new Vector3( 0.0f, -y, 0.1f),
            new Vector3( 0.0f,  y, 0.1f)
        };

        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = testTransform.TransformPoint(testPoints[i]);
            Vector3 raycastDir = testTransform.TransformDirection(testDirs[i]);
            
            if (Physics.CheckSphere(raycastPos, 0.05f, placementMask))
            {
                break;
            }
            else if (Physics.Raycast(raycastPos, raycastDir, out hit, 0.85f, placementMask))//2.1f, placementMask))
            {
                var offset = hit.point - raycastPos;
                testTransform.Translate(offset, Space.World);
            }
        }
    }

    // Ensure the portal cannot intersect a section of wall.
    private void FixIntersects()
    {
        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        var testDists = new List<float> { 0.45f, 0.45f, 0.85f, 0.85f };//{ 0.11f, 0.11f, 0.21f, 0.21f };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = testTransform.TransformPoint(0.0f, 0.0f, -0.01f);
            Vector3 raycastDir = testTransform.TransformDirection(testDirs[i]);

            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], placementMask))
            {
                var offset = (hit.point - raycastPos);
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                testTransform.Translate(newOffset, Space.World);
            }
        }
    }

    // Once positioning has taken place, ensure the portal isn't intersecting anything.
    private bool CheckOverlap()
    {
        var checkExtents = new Vector3(0.9f, 1.9f, 0.05f);
       
        var checkPositions = new Vector3[]
        {
            testTransform.position + testTransform.TransformVector(new Vector3( 0.0f,  0.0f, -0.1f)),

            testTransform.position + testTransform.TransformVector(new Vector3(-1.0f, -2.0f, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3(-1.0f,  2.0f, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3( 1.0f, -2.0f, -0.1f )),
            testTransform.position + testTransform.TransformVector(new Vector3(1.0f, 2.0f, -0.1f )),

            testTransform.TransformVector(new Vector3(0.0f, 0.0f, 0.2f))
        };
       
        // Ensure the portal does not intersect walls.
        var intersections = Physics.OverlapBox(checkPositions[0], checkExtents, testTransform.rotation, placementMask);
        //Debug.Log(checkPositions[0]);
        //Debug.Log(checkExtents);
        //Debug.Log(testTransform.rotation);
        //Debug.Log(placementMask);
        //Debug.Log(intersections.Length);
        if (intersections.Length > 4)
        {
          //  Debug.Log("���2");
            return false;
        }
        else if (intersections.Length == 4)
        {
           // Debug.Log("���3");
            // We are allowed to intersect the old portal position.
            if (intersections[0] != collider)
            {
               // Debug.Log("���4");
                return false;
            }
        }

        // Ensure the portal corners overlap a surface.
        bool isOverlapping = true;
       
        //for (int i = 1; i < checkPositions.Length - 1; ++i)
        //{
        //    Debug.Log(isOverlapping);
        //    isOverlapping &= Physics.Linecast(checkPositions[i],
        //        checkPositions[i] + checkPositions[checkPositions.Length - 1], placementMask);
        //}
        //Debug.Log("���6");
       // Debug.Log(isOverlapping);
        return isOverlapping;
    }

    public void RemovePortal()
    {
        gameObject.SetActive(false);
        IsPlaced = false;
    }
}
