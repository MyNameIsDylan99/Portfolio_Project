using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Roaming : State {

        NavMeshAgent agent;
        Vector3 startingPosition;
        Vector3 roamingPosition;
        Transform AITransform;
        float roamingSpeed;

        public Roaming(AI ai, NavMeshAgent agent, Transform AITransform, float roamingSpeed) : base(ai) {

            this.agent = agent;
            this.AITransform = AITransform;
            this.roamingSpeed = roamingSpeed;
            startingPosition = AITransform.position;
            roamingPosition = GetRoamingPosition();
            
        }

        public override void Update() {
            agent.destination = roamingPosition;
            if (ShouldGetNewPosition())
                roamingPosition = GetRoamingPosition();
        }

        public override void Enter() {

            agent.speed = roamingSpeed;

        }

        bool ShouldGetNewPosition() {
            bool returnValue = false;

            if (Vector3.Distance(agent.destination, AITransform.position) < 2f)
                returnValue = true;

            return returnValue;
        }

        Vector3 GetRoamingPosition() {
            NavMeshPath path = new NavMeshPath();
            bool roamingPosFound = false;
            Vector3 roamingPos = new Vector3();
            while (roamingPosFound == false) {
                roamingPos = startingPosition + GetRandomDirection() * Random.Range(10, 70);
                if (agent.CalculatePath(roamingPos, path) && path.status != NavMeshPathStatus.PathPartial)
                    roamingPosFound = true;
            }
            return roamingPos;
        }
        Vector3 GetRandomDirection() {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f)).normalized;
        }
    }
}

