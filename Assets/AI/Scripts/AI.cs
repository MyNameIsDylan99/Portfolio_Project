using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace AI {

    public class AI : MonoBehaviour {

        #region SerializedFields
        [SerializeField]
        Animator animator;
        [SerializeField]
        Transform playerTransform;
        [SerializeField]
        float sightRange = 10f;
        [SerializeField]
        float chasingSpeed = 5f;
        [SerializeField]
        float roamingSpeed = 3f;
        #endregion

        #region Fields

        NavMeshAgent agent;

        float hitCooldown;
        float timeSinceLastHit;


        Dictionary<string, State> states = new Dictionary<string, State>();
        State state;

        #endregion

        #region Methods

        private void Awake() {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            Add("Roaming", new Roaming(this, agent, transform, roamingSpeed));
            Add("Chasing", new Chasing(this, agent, playerTransform, chasingSpeed));
            SetCurrentState(GetState("Roaming"));

        }

        private void Update() {

            state.Update();

            if (CheckIfPlayerInChasingRange()) {
                SetCurrentState(GetState("Chasing"));
            }
            else {
                SetCurrentState(GetState("Roaming"));
            }

            animator.SetFloat("Speed", agent.speed);

        }

        public void Add(string key, State state) {

            states.Add(key, state);

        }

        public State GetState(string key) {

            return states[key];

        }

        public void SetCurrentState(State state) {

            if (state != null) {

                state.Exit();

            }

            this.state = state;

            if (state != null) {

                state.Enter();

            }
        }

        bool CheckIfPlayerInChasingRange() {

            bool returnValue = false;

            RaycastHit hit = new RaycastHit();
            int ignoreMask = ~LayerMask.NameToLayer("Action");
            Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit, sightRange, ignoreMask);


            if (hit.collider != null) {


                switch (hit.collider.tag) {

                    case "Player":

                        returnValue = true;
                        break;

                    case "Wall":

                        returnValue = false;
                        break;

                }
            }

            return returnValue;

        }


        #endregion

    }
}