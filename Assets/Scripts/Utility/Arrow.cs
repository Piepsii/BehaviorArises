using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorArises
{
    public class Arrow : MonoBehaviour
    {
        public float speed = 1f;

        private Transform target = default;
        private Rigidbody rb;

        public Arrow(Transform target, float speed)
        {
            this.target = target;
            this.speed = speed;
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            target = GameObject.FindWithTag("Player").transform;

            transform.LookAt(target);
            var distanceVec = target.position - transform.position;
            rb.AddForce(distanceVec.normalized * speed, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Arrow hit: " + collision.collider.name);
            Destroy(gameObject);
        }

    }
}
