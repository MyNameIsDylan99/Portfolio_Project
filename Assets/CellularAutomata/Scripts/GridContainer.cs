using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridContainer : MonoBehaviour
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

      //The idea behind serializableGrid is to "flatten" the 3D array into a 1D array so unity is able so serialize it and doesn't lose the data when you start playmode.
    [SerializeField]
    Cell[] serializableGrid;

    Cell[,,] grid;


    public IEnumerator UpdateGrid()
    {
        bool keepUpdating = true;
        int amountOfBurnedCells = 0;
        while (keepUpdating)
        {
            if (grid == null)
                break;
            Cell[,,] updatedGrid = grid;
                int random;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int z = 0; z < depth; z++)
                        {
                            switch (grid[x,y,z].State)
                            {
                                case Cell.States.Starting:
                                    random = Random.Range(0, 100);
                                    //15% Chance to make current cell switch from state "Starting" to state "Burning".
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
                                                    if (!(neighbourY == y && neighbourX == x && neighbourZ == z || neighbourY < 0 || neighbourY > height - 1 || neighbourX < 0 || neighbourX > width - 1 || neighbourZ < 0 || neighbourZ > depth - 1))
                                                    {
                                                        if (grid[neighbourX, neighbourY, neighbourZ].State == Cell.States.Fresh)
                                                        {
                                                            updatedGrid[neighbourX, neighbourY, neighbourZ].SetState(Cell.States.Starting);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    if (Time.time - grid[x, y, z].TimeWhenStartedBurning < 10)
                                        continue;
                                    //Make the current cell switch from state "Burning" to state "Burned" after 10 seconds.
                                    updatedGrid[x, y, z].SetState(Cell.States.Burned);
                                    amountOfBurnedCells++;
                                    break;
                            }
                        }

                    }
                }

                grid = updatedGrid;
                if (amountOfBurnedCells == grid.Length)
                    keepUpdating = false;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SetGrid(Cell[,,] cellGrid)
    {
        grid = cellGrid;
        width = grid.GetLength(0);
        height = grid.GetLength(1);
        depth = grid.GetLength(2);
    }
    public void DeleteGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    grid[x, y, z].DeleteCell();
                }

            }
        }
        DestroyImmediate(gameObject);
    }
    void MakeGridSerializable()
    {
        serializableGrid = new Cell[grid.Length];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    
                }

            }
        }
    }
}
