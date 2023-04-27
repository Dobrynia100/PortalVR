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
            if (objPos.z > 0.0f)
            {
                Debug.Log("перемещение?");
                Debug.Log(objPos.z);
                portalObjects[i].Warp();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("коллайдер попал в портал");
        var obj = other.GetComponent<PortalableObject>();
        if (obj != null)
        {
            portalObjects.Add(obj);
            Debug.Log("Обнаружен в списке");
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
        Debug.Log("Ставим");
        testTransform.position = pos;
        testTransform.rotation = rot;
        testTransform.position -= testTransform.forward * 0.001f;

        FixOverhangs();//проверка выступов
        FixIntersects();//проверка пересечений

        if (CheckOverlap())
        {
            Debug.Log("чек");
            this.wallCollider = wallCollider;
            transform.position = testTransform.position;
            transform.rotation = testTransform.rotation;

            gameObject.SetActive(true);
            IsPlaced = true;
            return true;
        }

        return false;
    }

    // обеспечить что портал не может выйти за границы поверхности
    private void FixOverhangs()
    {
        Debug.Log("overhangs");
        float x=44f, y=88f;
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
            
            if (Physics.CheckSphere(raycastPos, 0.04f, placementMask))
            {
                break;
            }
            else if (Physics.Raycast(raycastPos, raycastDir, out hit, y, placementMask))//2.1f, placementMask))
            {
                Debug.Log("3");
                var offset = hit.point - raycastPos;
                testTransform.Translate(offset, Space.World);
            }
        }
    }

    // Исправление пересечений с частью стены
    private void FixIntersects()
    {
        Debug.Log("intersects");
        float x = 0.44f, y = 0.88f;
        var testDirs = new List<Vector3>
        {
             Vector3.right,
            -Vector3.right,
             Vector3.up,
            -Vector3.up
        };

        var testDists = new List<float> { x, x, y, y };//{ 0.11f, 0.11f, 0.21f, 0.21f };

        for (int i = 0; i < 4; ++i)
        {
            RaycastHit hit;
            Vector3 raycastPos = testTransform.TransformPoint(0.0f, 0.0f, -0.01f);
            Vector3 raycastDir = testTransform.TransformDirection(testDirs[i]);

            if (Physics.Raycast(raycastPos, raycastDir, out hit, testDists[i], placementMask))
            {
                Debug.Log("5");
                var offset = (hit.point - raycastPos);
                var newOffset = -raycastDir * (testDists[i] - offset.magnitude);
                testTransform.Translate(newOffset, Space.World);
            }
        }
    }

    // Как только выбрана позиция,убеждаемся что портал ни с чем не пересекается
    private bool CheckOverlap()
    {
        float x = 44f, y = 88f;
        var checkExtents = new Vector3(0.9f, 1.9f, 0.05f);
        Debug.Log("overlap");
        var checkPositions = new Vector3[]
        {
            testTransform.position + testTransform.TransformVector(new Vector3( 0.0f,  0.0f, -0.1f)),

            testTransform.position + testTransform.TransformVector(new Vector3(-x, -y, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3(-x,  y, -0.1f)),
            testTransform.position + testTransform.TransformVector(new Vector3( x, -y, -0.1f )),
            testTransform.position + testTransform.TransformVector(new Vector3(x, y, -0.1f )),

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
            Debug.Log("чек2");
            return false;
        }
        else if (intersections.Length == 4)
        {
            Debug.Log("чек3");
            // можно пересекать,если это старая позиция портала
            if (intersections[0] != collider)
            {
               // Debug.Log("чек4");
                return false;
            }
        }

        // углы портала покрывают поверхность.
        bool isOverlapping = true;
       
        //for (int i = 1; i < checkPositions.Length - 1; ++i)
        //{
        //    Debug.Log(isOverlapping);
        //    isOverlapping &= Physics.Linecast(checkPositions[i],
        //        checkPositions[i] + checkPositions[checkPositions.Length - 1], placementMask);
        //}
        //Debug.Log("чек6");
       // Debug.Log(isOverlapping);
        return isOverlapping;
    }

    public void RemovePortal()
    {
        gameObject.SetActive(false);
        IsPlaced = false;
    }
}
