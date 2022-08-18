using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CellularAutomata {
    public class GridCreator : MonoBehaviour {

        #region SerializedFields
        [SerializeField]
        GameObject cellPrefab;
        [SerializeField]
        GameObject gridPrefab;
        [SerializeField]
        RaycastManager raycastManager;
        [SerializeField]
        int height;
        [SerializeField]
        int width;
        [SerializeField]
        int depth;
        [SerializeField]
        [Tooltip("The chance of each cell to be generated as a solid cell.")]
        [Range(0, 100)]
        int randomFillPercent;
        [SerializeField]
        bool createSingleBlockInCenter;
        [SerializeField]
        [Tooltip("The grid will be updated every [this value] seconds.")]
        float updateFrequency = 0.1f;
        [SerializeField]
        GridContainer gridContainer;
        [SerializeField]
        Material destroyPreviewMat;
        #endregion

        #region Fields
        GameObject previewCell = null;
        Cell[,,] grid;
        #endregion

        #region Methods

        #region Grid
        public void GenerateGrid() {
            if (gridContainer != null)
                gridContainer.DeleteGrid();

            grid = new Cell[width, height, depth];

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    for (int z = 0; z < depth; z++) {
                        GameObject cellGO = (GameObject)PrefabUtility.InstantiatePrefab((Object)cellPrefab as GameObject);
                        int random = Random.Range(1, 101);
                        cellGO.transform.position = new Vector3(-width / 2 + x, -height / 2 + y, -depth / 2 + z);
                        grid[x, y, z] = cellGO.GetComponent<Cell>();
                        if (createSingleBlockInCenter) {
                            if (x == Mathf.Round(width * 0.5f) && y == Mathf.Round(height * 0.5f) && z == Mathf.Round(depth * 0.5f))
                                grid[x, y, z].SetState(Cell.States.Fresh);
                            else
                                grid[x, y, z].SetState(Cell.States.Air);
                        }
                        else {
                            if (random > randomFillPercent)
                                grid[x, y, z].SetState(Cell.States.Air);
                            else
                                grid[x, y, z].SetState(Cell.States.Fresh);
                        }
                    }

                }
            }

            InstantiateGridContainer();
        }

        public void DeleteGrid() {
            if (grid == null)
                return;
            gridContainer.DeleteGrid();
        }

        //Instantiate a GridContainer instance and give it the current grid.
        public void InstantiateGridContainer() {
            GameObject gridGO = (GameObject)PrefabUtility.InstantiatePrefab((Object)gridPrefab as GameObject);
            foreach (Cell cell in grid) {
                cell.gameObject.transform.SetParent(gridGO.transform);
            }
            gridContainer = gridGO.GetComponent<GridContainer>();
            gridContainer.SetGrid(grid);
            gridContainer.SetUpdateFrequency(updateFrequency);
        }
        public void UpdateGrid() {
            StartCoroutine(gridContainer.UpdateGrid());
        }

        public void SaveGridContainerAsPrefab() {
            if(gridContainer != null) {
                string path = path = "Assets/CellularAutomata/Prefabs/GridContainers/" + "gridContainer";
                PrefabGenerator.SaveAsPrefab(gridContainer.gameObject, path);
            }
        }
        #endregion

        #region BuildMode
        public void ShowPreviewCellBuildMode() {

            //Checks if Raycast doesn't hit any colliders. If so it destroys the previewCell.
            if (raycastManager.hit.collider == null) {
                if (previewCell != null)
                    DestroyImmediate(previewCell);
                return;
            }
            // Checks if the Collider is not the previewCell itself.
            else if (raycastManager.hit.collider.gameObject != previewCell) {

                //Sets the position to the Raycast Hit Position - 0.01 * the normalized direction of the Ray and rounds it to the next whole number (Snaps it into the grid like placing minecraft blocks).
                Vector3 hitPoint = raycastManager.hit.point + (raycastManager.ray.direction.normalized * -0.01f);
                Vector3 roundedPosition = new Vector3(Mathf.Round((hitPoint.x)), Mathf.Round((hitPoint.y)), Mathf.Round(hitPoint.z));

                //Check if the preview cell is inside the bounds of the grid.
                if (roundedPosition.x + 1 > Mathf.Round(width * 0.5f + 0.01f) || roundedPosition.y + 1 > Mathf.Round(height * 0.5f + 0.01f) || roundedPosition.z + 1 > Mathf.Round(depth * 0.5f + 0.01f) || roundedPosition.x < (-width * 0.5f) || roundedPosition.y < (-height * 0.5f) || roundedPosition.z < (-depth * 0.5f))
                    return;

                if (previewCell == null)
                    previewCell = Instantiate(cellPrefab);

                previewCell.transform.position = roundedPosition;

            }
        }
        public void DestroyPreviewCellBuildMode() {
            DestroyImmediate(previewCell);
        }

        public void AddPreviewCellToGrid() {
            if (grid != null && previewCell != null) {
                int x = (int)(previewCell.transform.position.x + width * 0.5f);
                int y = (int)(previewCell.transform.position.y + height * 0.5f);
                int z = (int)(previewCell.transform.position.z + depth * 0.5f);
                grid[x, y, z].SetState(Cell.States.Fresh);
            }
        }
        #endregion

        #region DestroyMode
        public void PreviewRaycastTargetDestroyMode() {

            RemovePreviewRaycastTargetIfColliderChangesDestroyMode();

            raycastManager.currentRaycastTarget = raycastManager.hit.collider;
            if (raycastManager.currentRaycastTarget == null)
                return;

            Cell colliderCell = raycastManager.currentRaycastTarget.GetComponent<Cell>();
            if (colliderCell != null) {
                if (colliderCell.State != Cell.States.Air)
                    colliderCell.GetComponent<MeshRenderer>().material = destroyPreviewMat;
            }
        }
        public void SetRaycastTargetToAir() {
            
            if (raycastManager.currentRaycastTarget == null)
                return;

            Cell colliderCell = raycastManager.currentRaycastTarget.GetComponent<Cell>();
            if (colliderCell != null) {
                colliderCell.SetState(Cell.States.Air);
            }
        }
        public void RemovePreviewRaycastTargetIfColliderChangesDestroyMode() {
            if (raycastManager.currentRaycastTarget != null && raycastManager.currentRaycastTarget != raycastManager.hit.collider) {
                Cell colliderCell = raycastManager.currentRaycastTarget.GetComponent<Cell>();
                if (colliderCell != null && colliderCell.State != Cell.States.Air) {
                    colliderCell.SetState(Cell.States.Fresh);
                }
            }
        }
        #endregion

        #region Other
        public void RemoveAllPreviews() {
            if (raycastManager.currentRaycastTarget != null) {
                Cell colliderCell = raycastManager.currentRaycastTarget.GetComponent<Cell>();
                if (colliderCell != null && colliderCell.State != Cell.States.Air) {
                    colliderCell.SetState(Cell.States.Fresh);
                }
            }
            DestroyPreviewCellBuildMode();
        }
        #endregion

        #endregion
    }
}