using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrigger : MonoBehaviour
{

    public float Mass;
    [SerializeField]
    float gravityRadius;
    [SerializeField]
    bool checkForGravityObjects;
    private void Start()
    {
        StartCoroutine(TriggerGravity());
    }
    IEnumerator TriggerGravity()
    {
        while (checkForGravityObjects)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, gravityRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.tag == "gravityAffected")
                {
                    GravityReceiver collider = hitCollider.GetComponent<GravityReceiver>();
                    collider.SetParameters(transform, Mass);
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GravityReceiver collisionGR = collision.gameObject.GetComponent<GravityReceiver>();
        collisionGR.Velocity = Vector3.zero;
        collisionGR.GravityActive = false;
    }

}
