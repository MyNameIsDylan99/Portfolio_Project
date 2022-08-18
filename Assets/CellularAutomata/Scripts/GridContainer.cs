using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CellularAutomata {
    public class GridContainer : MonoBehaviour {

        #region SerializedFields
        [SerializeField]
        GameObject cellPrefab;
        [SerializeField]
        int height;
        [SerializeField]
        int width;
        [SerializeField]
        int depth;
        [SerializeField]
        float updateFrequency = 0.1f;
        [SerializeField]
        [Tooltip("After this amount of seconds the UpdateGrid function will automatically stop.")]
        float updateLimit=100;
        [SerializeField]
        Cell[] grid;
        #endregion

        #region Fields

        #endregion

        #region Methods

        public IEnumerator UpdateGrid() {
            float timeWhenStartedUpdating = Time.time;
            bool keepUpdating = true;
            int amountOfBurnedCells = 0;
            while (keepUpdating) {

                if (grid == null)
                    break;

                Cell[] updatedGrid = grid;
                int random;

                for (int y = 0; y < height; y++) {
                    for (int x = 0; x < width; x++) {
                        for (int z = 0; z < depth; z++) {
                            int index = ArrayConverter.Convert3DIndexTo1DIndex(x, y, z, width, height);
                            switch (grid[index].State) {

                                case Cell.States.Starting:
                                    random = Random.Range(0, 100);

                                    //15% Chance to make current cell switch from state "Starting" to state "Burning".
                                    if (random < 15)
                                        updatedGrid[index].SetState(Cell.States.Burning);
                                    break;

                                case Cell.States.Burning:

                                    random = Random.Range(0, 100);

                                    //50% Chance to make a neighbouring cell switch to state "Starting" if it's state is "Fresh".
                                    if (random > 50)
                                        continue;

                                    for (int neighbourY = y - 1; neighbourY < y + 2; neighbourY++) {
                                        for (int neighbourX = x - 1; neighbourX < x + 2; neighbourX++) {
                                            for (int neighbourZ = z - 1; neighbourZ < z + 2; neighbourZ++) {

                                                if (!(neighbourY == y && neighbourX == x && neighbourZ == z || neighbourY < 0 || neighbourY > height - 1 || neighbourX < 0 || neighbourX > width - 1 || neighbourZ < 0 || neighbourZ > depth - 1)) {

                                                    int neighbourIndex = ArrayConverter.Convert3DIndexTo1DIndex(neighbourX, neighbourY, neighbourZ, width, height);


                                                    if (grid[neighbourIndex].State == Cell.States.Fresh) {
                                                        updatedGrid[neighbourIndex].SetState(Cell.States.Starting);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    //Make the current cell switch from state "Burning" to state "Burned" if the the amount of seconds for the cell to be burned is reached.

                                    if (!(Time.time - grid[index].TimeWhenStartedBurning < grid[index].SecondsForCellToBeBurned)) {

                                        

                                        updatedGrid[index].SetState(Cell.States.Burned);
                                        amountOfBurnedCells++;
                                    }
                                    break;
                            }
                        }

                    }
                }

                grid = updatedGrid;

                //If all cells are burned or the updateLimit is exceeded the grid will stop updating.

                if (amountOfBurnedCells == grid.Length || Time.time - timeWhenStartedUpdating > updateLimit) {
                    keepUpdating = false;
                }

                yield return new WaitForSeconds(updateFrequency);
            }
        }

        public void SetGrid(Cell[,,] cellGrid) {
            grid = ArrayConverter.ConvertCellArray3DIntoCellArray1D(cellGrid);
            width = cellGrid.GetLength(0);
            height = cellGrid.GetLength(1);
            depth = cellGrid.GetLength(2);
        }
        public void SetUpdateFrequency(float updateFrequency) {
            this.updateFrequency = updateFrequency;
        }

        public void DeleteGrid() {
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    for (int z = 0; z < depth; z++) {
                        int index = ArrayConverter.Convert3DIndexTo1DIndex(x, y, z, width, height);
                        grid[index].DeleteCell();
                    }

                }
            }
            DestroyImmediate(gameObject);
        }

        #endregion
    }
}