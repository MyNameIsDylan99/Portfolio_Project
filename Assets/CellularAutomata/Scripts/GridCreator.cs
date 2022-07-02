using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    //Fields
    [SerializeField]
    GameObject cellPrefab;
    [SerializeField]
    GameObject gridPrefab;
    [SerializeField]
    int height;
    [SerializeField]
    int width;
    [SerializeField]
    int depth;
    [SerializeField]
    [Range(0, 100)]
    int randomFillPercent; 
    [SerializeField]
    GridContainer gridContainer;

    Cell[,,] grid;
    public void GenerateGrid()
    {
        if (gridContainer != null)
            gridContainer.DeleteGrid();

        grid = new Cell[width, height, depth];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    GameObject cellGO = Instantiate(cellPrefab);
                    int random = Random.Range(0, 100);
                    cellGO.transform.position = new Vector3(-width / 2 + x, -height / 2 + y, -depth / 2 + z);
                    grid[x, y, z] = cellGO.GetComponent<Cell>();
                    //grid[x, y, z].SetState(Cell.States.Fresh);
                    if (y <= height * 0.5f + 2 && y >= height * 0.5f - 2 && x <= width * 0.5f + 2 && x >= width * 0.5f - 2 && z <= depth * 0.5f + 2 && z >= depth * 0.5f - 2)
                        grid[x, y, z].SetState(Cell.States.Burning);

                    else if (random > randomFillPercent)
                        grid[x, y, z].SetState(Cell.States.Air);
                    else
                        grid[x, y, z].SetState(Cell.States.Fresh);
                }

            }
        }
        InstantiateGridContainer();
    }
    public void DeleteGrid()
    {
        if (grid == null)
            return;
        gridContainer.DeleteGrid();
    }
    public void InstantiateGridContainer()
    {
        GameObject gridGO = Instantiate(gridPrefab);
        gridContainer = gridGO.GetComponent<GridContainer>();
        gridContainer.SetGrid(grid);
    }
    public void UpdateGrid()
    {
        StartCoroutine(gridContainer.UpdateGrid());
    }
}
