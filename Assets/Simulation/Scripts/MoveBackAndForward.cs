using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackAndForward : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    Vector3 startPoint;
    [SerializeField]
    Vector3 endPoint;

    bool goingTowardsStartPoint;

    void Start()
    {
        goingTowardsStartPoint = true;
    }


    void Update()
    {
        if (goingTowardsStartPoint) {
            transform.Translate((startPoint-transform.position).normalized * speed,Space.World);
            if (Vector3.Distance(startPoint, transform.position) < 0.01f) {
                goingTowardsStartPoint=false;
            }
        }
        else {
            transform.Translate((endPoint-transform.position).normalized * speed,Space.World);
            if (Vector3.Distance(endPoint, transform.position) < 0.01f) {
                Debug.Log(Vector3.Distance(endPoint, transform.position).ToString());
                Debug.Log(transform.position);
                goingTowardsStartPoint = true;
            }
        }
    }
}
