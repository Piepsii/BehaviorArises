using System.Collections;
using UnityEngine;

namespace BehaviorArises.Utility
{
    public class CanISeeThisObject : MonoBehaviour
    {
        GameObject obj;
        Collider objCollider;

        Camera cam;
        Plane[] planes;

        void Start()
        {
            obj = gameObject;
            cam = Camera.main;
            objCollider = GetComponent<Collider>();
        }

        void Update()
        {
            planes = GeometryUtility.CalculateFrustumPlanes(cam);
            if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
            {
                Debug.Log(obj.name + " has been detected!");
            }
            else
            {
                Debug.Log("Nothing has been detected.");
            }
        }

    }
}