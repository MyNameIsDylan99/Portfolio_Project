using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityReceiver : MonoBehaviour
{

    public bool GravityActive;
    public Vector3 Velocity;

    [SerializeField]
    float gravitationalConstant = 1;
    [SerializeField]
    Vector3 startingVelocity;

    float massCenter;
    Transform GravitationalCenter;

    private void Start()
    {
        Velocity = startingVelocity;
    }
    private void FixedUpdate()
    {
        if (GravityActive)
            AddGravity();

        MoveObject();

    }

    public void SetParameters(Transform center, float mass)
    {
        GravitationalCenter = center;
        massCenter = mass;
    }
    public void AddGravity()
    {
        Vector3 dir = (GravitationalCenter.position - transform.position);
        float dSquared = dir.sqrMagnitude;
        dir = dir.normalized;
        float strength = gravitationalConstant * massCenter / dSquared; // a = G * m / r^2
        Velocity += (dir * strength * Time.deltaTime);
    }

    public void MoveObject()
    {
        transform.Translate(Velocity * Time.deltaTime);
    }
}
