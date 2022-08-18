using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsGravitySphere {
    public class GravityTrigger : MonoBehaviour {

        #region SerializedFields
        public float Mass;
        [SerializeField]
        float gravityRadius;
        [SerializeField]
        bool checkForGravityObjects;
        #endregion

        #region Methods
        private void Start() {
            StartCoroutine(TriggerGravity());
        }
        IEnumerator TriggerGravity() {
            while (checkForGravityObjects) {
                Collider[] hitColliders = Physics.OverlapSphere(transform.position, gravityRadius);
                foreach (var hitCollider in hitColliders) {
                    if (hitCollider.tag == "gravityAffected") {
                        GravityReceiver collider = hitCollider.GetComponent<GravityReceiver>();
                        collider.SetParameters(transform, Mass);
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
        private void OnCollisionEnter(Collision collision) {
            GravityReceiver collisionGR = collision.gameObject.GetComponent<GravityReceiver>();
            collisionGR.StopGravityAndMovement();
        }
        #endregion

    }
}