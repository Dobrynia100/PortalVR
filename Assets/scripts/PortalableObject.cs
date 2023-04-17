using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PortalableObject : MonoBehaviour
{
    private GameObject cloneObject;

    private int inPortalCount = 0;
    
    private Portal inPortal;
    private Portal outPortal;

    private new Rigidbody rigidbody;
    protected new Collider collider;

    private static readonly Quaternion halfTurn = Quaternion.Euler(0.01f, 180.0f, 0.01f);
   
    protected virtual void Awake()
    {
        
        cloneObject = new GameObject();
        cloneObject.SetActive(false);
        var meshFilter = cloneObject.AddComponent<MeshFilter>();
        var meshRenderer = cloneObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = GetComponent<MeshFilter>().mesh;
        meshRenderer.materials = GetComponent<MeshRenderer>().materials;
        cloneObject.transform.localScale = transform.localScale;

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        Debug.Log("Создан клон");
    }

    private void LateUpdate()
    {
       
        if (inPortal == null || outPortal == null)
        {
            
            return;
        }

        if(cloneObject.activeSelf && inPortal.IsPlaced && outPortal.IsPlaced)
        {
            var inTransform = inPortal.transform;
            var outTransform = outPortal.transform;

            // Update position of clone.
            Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
            relativePos = halfTurn * relativePos;
            cloneObject.transform.position = outTransform.TransformPoint(relativePos);

            // Update rotation of clone.
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = halfTurn * relativeRot;
            cloneObject.transform.rotation = outTransform.rotation * relativeRot;
            Debug.Log("перемещение клона?");
        }
        else
        {
            
            cloneObject.transform.position = new Vector3(-1000.0f, 1000.0f, -1000.0f);
        }
    }

    public void SetIsInPortal(Portal inPortal, Portal outPortal, Collider wallCollider)
    {
        this.inPortal = inPortal;
        this.outPortal = outPortal;

        Physics.IgnoreCollision(collider, wallCollider);

        cloneObject.SetActive(false);
        Debug.Log("В портале");
        ++inPortalCount;
    }

    public void ExitPortal(Collider wallCollider)
    {
        Physics.IgnoreCollision(collider, wallCollider, false);
        --inPortalCount;
        
        if (inPortalCount == 0)
        {
            cloneObject.SetActive(false);
        }
    }

    public virtual void Warp()
    {
        var inTransform = inPortal.transform;
        var outTransform = outPortal.transform;

        // Update position of object.
        Vector3 relativePos = inTransform.InverseTransformPoint(transform.position);
        relativePos = halfTurn * relativePos;
        transform.position = outTransform.TransformPoint(relativePos);

        // Update rotation of object.
       
            

        if (collider.tag.Equals("Player"))
        {
            Debug.Log("тэг игрока");
            
            Debug.Log(inTransform.rotation.y);
            Debug.Log(inTransform.rotation.z);
            Debug.Log(outTransform.rotation.y);
            Debug.Log(outTransform.rotation.z);
            if (inTransform.rotation.y <=-0.5f ||  inTransform.rotation.z<=-0.5f || outTransform.rotation.z <= -0.5f  || outTransform.rotation.y <= -0.5f)
            {
                Debug.Log("!!!!!!!!!!!верх-низ,вращение!!!!!!!!!");
               transform.rotation = outTransform.rotation;
                transform.rotation = Quaternion.Euler(0.01f, 180.0f, 0.01f);
            }
            else
            {
                Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
                
                Debug.Log(relativeRot.y);
                Debug.Log(relativeRot.z);
                relativeRot = halfTurn * relativeRot;
                // Camera.main.transform.rotation.Set(0.0f, transform.rotation.y + 1.0f, transform.rotation.z, transform.rotation.w);
                // transform.rotation.Set(0.0f, transform.rotation.y + 1.0f, transform.rotation.z, transform.rotation.w);
                
                Debug.Log(relativeRot.y);
                Debug.Log(outTransform.rotation.y);
                Debug.Log(relativeRot.z);
                Debug.Log(outTransform.rotation.z);
                transform.rotation = outTransform.rotation * relativeRot;      
                Debug.Log(transform.rotation.y);              
                Debug.Log(transform.rotation.z);
            }
            
        }
        else {
            Quaternion relativeRot = Quaternion.Inverse(inTransform.rotation) * transform.rotation;
            relativeRot = halfTurn * relativeRot;
            transform.rotation = outTransform.rotation * relativeRot;//сделать только с одной осью,или как то оставлять ротацию определенных
        }
        // Update velocity of rigidbody.
        Vector3 relativeVel = inTransform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        relativeVel = halfTurn * relativeVel;
        rigidbody.velocity = outTransform.TransformDirection(relativeVel);

        // Swap portal references.
        var tmp = inPortal;
        inPortal = outPortal;
        outPortal = tmp;
        Debug.Log("Переместился?");
    }
}
