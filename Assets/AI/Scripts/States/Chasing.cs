using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class Chasing : State {

        NavMeshAgent agent;
        float chasingSpeed;
        Transform playerTransform;

        public Chasing(AI ai, NavMeshAgent agent, Transform playerTransform, float chasingSpeed) : base(ai) {
            this.agent = agent; 
            this.playerTransform = playerTransform;
            this.chasingSpeed = chasingSpeed;
        }

        public override void Update() {

            agent.destination = playerTransform.position;

        }

        public override void Enter() {
            
            agent.speed = chasingSpeed;
        }

    }
}