using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellularAutomata {
    [Serializable]
    public class Cell : MonoBehaviour {

        #region Properties
        public States State { get => state; private set => state = value; }
        public float TimeWhenStartedBurning { get; private set; }
        #endregion

        #region SerializedFields

        [SerializeField]
        Material cellMaterialReference;

        [SerializeField]
        States state = States.Fresh;

        [SerializeField]
        public float SecondsForCellToBeBurned { get; private set; } = 20;

        #endregion

        #region Fields

        Material cellMaterial;

        float secondsPassedSinceBurning = 0;

        public enum States {
            Air,
            Fresh,
            Starting,
            Burning,
            Burned
        }

        GameObject[] fireAnimations;

        #endregion

        #region Methods
        private void Awake() {
            cellMaterial = new Material(cellMaterialReference);
            GetComponent<MeshRenderer>().material = cellMaterial;

            fireAnimations = new GameObject[transform.childCount];
            for (int i = 0; i < transform.childCount; i++) {
                fireAnimations[i] = transform.GetChild(i).gameObject;
            }
        }

        private void Update() {

            if (state == States.Burning) {
                if (secondsPassedSinceBurning > SecondsForCellToBeBurned) {
                    secondsPassedSinceBurning = SecondsForCellToBeBurned;
                }
                secondsPassedSinceBurning = Time.time - TimeWhenStartedBurning;
                cellMaterial.SetFloat("_CurrentValue", secondsPassedSinceBurning);
                cellMaterial.SetVector("_MinAndMaxValue", new Vector2(0, SecondsForCellToBeBurned));

            }
        }

        private void OnCollisionEnter(Collision collision) {
            if (collision.gameObject.tag == "igniter" && state == States.Fresh) {
                SetState(States.Burning);
                StartCoroutine(GetComponentInParent<GridContainer>().UpdateGrid());
            }
        }

        public void SetState(States state) {
            this.state = state;
            if (gameObject.activeInHierarchy == false)
                gameObject.SetActive(true);
            switch (state) {
                case States.Air:
                    gameObject.SetActive(false);
                    break;
                case States.Fresh:
                    GetComponent<MeshRenderer>().material = cellMaterialReference;
                    break;
                case States.Starting:
                    break;
                case States.Burning:
                    TimeWhenStartedBurning = Time.time;
                    foreach (var fireQuad in fireAnimations) {
                        fireQuad.SetActive(true);
                    }
                    break;
                case States.Burned:
                    cellMaterial.SetFloat("_CurrentValue", SecondsForCellToBeBurned);
                    foreach (var fireQuad in fireAnimations) {
                        fireQuad.SetActive(false);
                    }
                    break;
            }

        }

        public void DeleteCell() {
            DestroyImmediate(gameObject);
        }
        #endregion
    }
}