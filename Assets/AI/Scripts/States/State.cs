using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public abstract class State {
        #region Fields

        protected AI ai;

        #endregion

        #region Methods
        public State(AI ai) {
            this.ai = ai;
        }
        public virtual void Enter() {

        }

        public virtual void Exit() {


        }
        public virtual void Update() {

        }
        public virtual void FixedUpdate() {

        }

        #endregion
    }
}