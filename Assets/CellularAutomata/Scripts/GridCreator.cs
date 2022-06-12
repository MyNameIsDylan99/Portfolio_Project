using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    //Fields
    [SerializeField]
    GameObject cellPrefab;
    [SerializeField]
    int height;
    [SerializeField]
    int width;
    [SerializeField]
    int depth;

    Cell[,,] grid;


    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
        StartCoroutine(UpdateGrid());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            CreateGrid();
    }

    IEnumerator UpdateGrid()
    {
        bool keepUpdating = true;
        int amountOfBurnedCells = 0;
        while (keepUpdating)
        {
            if (grid != null)
            {
                Cell[,,] updatedGrid = grid;

                int random;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            switch (grid[x, y, z].State)
                            {
                                case Cell.States.Starting:
                                    random = Random.Range(0, 100);
                                    //30% Chance to make current cell switch from state "Starting" to state "Burning".
                                    if (random < 15)
                                        updatedGrid[x, y, z].SetState(Cell.States.Burning);
                                    break;
                                case Cell.States.Burning:
                                    random = Random.Range(0, 100);
                                    //50% Chance to make a neighbouring cell switch to state "Starting" if it's state is "Fresh".
                                    if (random < 50)
                                        for (int neighbourY = y - 2; neighbourY < y + 3; neighbourY++)
                                        {
                                            for (int neighbourX = x - 2; neighbourX < x + 3; neighbourX++)
                                            {
                                                for (int neighbourZ = z - 2; neighbourZ < z + 3; neighbourZ++)
                                                {
                                                    if (!(neighbourY == y && neighbourX == x && neighbourZ == z  || neighbourY < 0 || neighbourY > height - 1 || neighbourX < 0 || neighbourX > width - 1 || neighbourZ < 0 || neighbourZ > depth - 1))
                                                    {
                                                        if (grid[neighbourX, neighbourY, neighbourZ].State == Cell.States.Fresh)
                                                        {
                                                            updatedGrid[neighbourX, neighbourY, neighbourZ].SetState(Cell.States.Starting);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    if (Time.time - grid[x, y,z].TimeWhenStartedBurning < 10)
                                        continue;
                                    //Make the current cell switch from state "Burning" to state "Burned" after 10 seconds.
                                    updatedGrid[x, y,z].SetState(Cell.States.Burned);
                                    amountOfBurnedCells++;
                                    break;
                            }
                        }
                    }
                }

                grid = updatedGrid;
                if (amountOfBurnedCells == grid.Length)
                    keepUpdating = false;


            }
            yield return new WaitForSeconds(0.1f);
        }



    }
    void CreateGrid()
    {

        grid = new Cell[height, width, depth];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    GameObject cellGO = Instantiate(cellPrefab);
                    cellGO.transform.position = new Vector3(-width / 2 + x, -height / 2 + y, -depth / 2 + z);
                    grid[x, y, z] = cellGO.GetComponent<Cell>();
                    if (y <= height * 0.5f + 2 && y >= height * 0.5f - 2 && x <= width * 0.5f + 2 && x >= width * 0.5f - 2)
                        grid[x, y, z].SetState(Cell.States.Burning);
                    else
                        grid[x, y, z].SetState(Cell.States.Fresh);
                }

            }
        }
    }

    //void OnDrawGizmos()
    //{
    //    if (grid != null)
    //        for (int y = 0; y < height; y++)
    //        {
    //            for (int x = 0; x < width; x++)
    //            {
    //                switch (grid[x, y].State)
    //                {
    //                    case Cell.States.Fresh:
    //                        Gizmos.color = Color.white;
    //                        break;

    //                    case Cell.States.Starting:
    //                        Gizmos.color = Color.yellow;
    //                        break;
    //                    case Cell.States.Burning:
    //                        Gizmos.color = Color.red;
    //                        break;
    //                    case Cell.States.Burned:
    //                        Gizmos.color = Color.black;
    //                        break;
    //                }
    //                Gizmos.DrawCube(new Vector3(-width / 2 + x, -height / 2 + y, 0), Vector3.one);
    //            }
    //        }

    //}
}
