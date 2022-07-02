using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    float sightRange = 10f;
    Vector3 startingPosition;
    Vector3 roamingPosition;
    NavMeshAgent agent;
    NavMeshPath path;

    enum State
    {
        Roaming,
        Chasing,
    }
    State state;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        startingPosition = transform.position;
        roamingPosition = GetRoamingPosition();
        state = State.Roaming;
    }
    private void Update()
    {
        switch (state)
        {
            case State.Roaming:
                agent.destination = roamingPosition;
                if (Vector3.Distance(agent.destination, transform.position) < 2f)
                    roamingPosition = GetRoamingPosition();
                CheckIfPlayerInRange();
                break;
            case State.Chasing:
                agent.destination = playerTransform.position;
                CheckIfPlayerInRange();
                break;
        }

    }

    void CheckIfPlayerInRange()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < sightRange && agent.CalculatePath(playerTransform.position, path) && path.status != NavMeshPathStatus.PathPartial)
        {
            state = State.Chasing;
        }
        else
        {
            state = State.Roaming;
        }
    }

    Vector3 GetRoamingPosition()
    {
        bool roamingPosFound = false;
        Vector3 roamingPos = new Vector3();
        while (roamingPosFound == false)
        {
            roamingPos = startingPosition + GetRandomDirection() * Random.Range(10, 70);
            if (agent.CalculatePath(roamingPos, path) && path.status != NavMeshPathStatus.PathPartial)
                roamingPosFound = true;
        }
        return roamingPos;
    }
    Vector3 GetRandomDirection()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

}
