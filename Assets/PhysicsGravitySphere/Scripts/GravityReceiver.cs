using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityReceiver : MonoBehaviour {

    #region SerializedFields
    public bool GravityActive;

    [SerializeField]
    float gravitationalConstant = 1;
    [SerializeField]
    Vector3 startingVelocity;
    #endregion

    #region Fields

    float massCenter;
    Transform GravitationalCenter;
    new Rigidbody rigidbody;

    #endregion

    #region Methods
    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = startingVelocity;
    }
    private void FixedUpdate() {
        if (GravityActive)
            AddGravity();

        MoveObject();
    }

    public void SetParameters(Transform center, float mass) {
        GravitationalCenter = center;
        massCenter = mass;
    }

    public void AddGravity() {
        Vector3 dir = (GravitationalCenter.position - transform.position);
        float dSquared = dir.sqrMagnitude;
        dir = dir.normalized;
        float strength = gravitationalConstant * massCenter / dSquared; // a = G * m / r^2
        rigidbody.velocity += (dir * strength * Time.deltaTime);
    }
    public void MoveObject() {
        transform.Translate(rigidbody.velocity * Time.deltaTime);

    }

    public void StopGravityAndMovement() {
        rigidbody.velocity = Vector3.zero;
        GravityActive = false;
    }
    #endregion

}
