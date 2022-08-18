using CellularAutomata;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayConverter
{
    public static Cell[] ConvertCellArray3DIntoCellArray1D(Cell[,,] cellArray3D) {

        int width = cellArray3D.GetLength(0);
        int height = cellArray3D.GetLength(1);
        int depth = cellArray3D.GetLength(2);

        Cell[] cells = new Cell[width  * height * depth];

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                for (int z = 0; z < depth; z++) {
                    cells[x + width * (y + height * z)] = cellArray3D[x, y, z];
                }
            }
        }
        return cells;
    }

    public static int Convert3DIndexTo1DIndex(int x, int y, int z, int width, int height) {
        int index1D;
        index1D = x + width * (y + height * z);
        return index1D;
    }
}
