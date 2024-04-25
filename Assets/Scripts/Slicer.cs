using UnityEngine;
using EzySlice;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.GraphicsBuffer;

public enum SwordColor {
    Red,
    Blue
}

public class Slicer : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public Material hullMaterial;
    public float minimumVelocity = 5;
    public LayerMask sliceMask;
    public GameObject target;
    public float sliceForce = 100f;
    public SwordColor swordColor;
    public GameObject sparksVFX;

    private ActionBasedController xrController;
    private Vector3 previousEndSlicePos;

    private void Awake()
    {
        xrController = GetComponent<ActionBasedController>();
        previousEndSlicePos = endSlicePoint.position;
    }

    void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceMask);

        if (hasHit)
        {
            Instantiate(sparksVFX, hit.point, Quaternion.identity, endSlicePoint);
        }

        if (hasHit && velocityEstimator.GetVelocityEstimate().magnitude > minimumVelocity / 1000)
        {
            target = hit.collider.gameObject;
            Slice(target);
        } else if (hasHit && velocityEstimator.GetVelocityEstimate().magnitude <= minimumVelocity / 1000)
        {
            Debug.Log("Velocity too low to slice: " + velocityEstimator.GetVelocityEstimate().magnitude);
        }
        previousEndSlicePos = endSlicePoint.position;
    }

    public void Slice(GameObject target)
    {
        CalculateScore(target.GetComponent<Block>());
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity).normalized;


        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, hullMaterial);
            SetupSlicedComponent(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(target, hullMaterial);
            SetupSlicedComponent(lowerHull);

            Destroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject hull)
    {
        Rigidbody rb = hull.AddComponent<Rigidbody>();
        MeshCollider collider = hull.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(sliceForce, hull.transform.position, 1f);
    }

    void CalculateScore(Block block)
    {
        if ((swordColor == SwordColor.Red && block.color == BlockColor.Red) || (swordColor == SwordColor.Blue && block.color == BlockColor.Blue))
        {
            GameManager.instance.HitBlock();
        }
        else
        {
            GameManager.instance.WrongBlock();
        }
    }
}
