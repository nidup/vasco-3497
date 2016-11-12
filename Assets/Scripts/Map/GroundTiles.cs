using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents ground tile indexes in the tile set sprite file, 6 = 6th tile
 */
public class GroundTiles {

    public static int UNDEFINED_INDEX = -1;
    public static int FORREST_INDEX = 6;
    public static int SAND_INDEX = 6 + 15 * 1;
    public static int WATER_INDEX = 6 + 15 * 2;
    public static int DEEP_WATER_INDEX = 6 + 15 * 3;
    public static int[] STACK = {
        GroundTiles.FORREST_INDEX,
        GroundTiles.SAND_INDEX,
        GroundTiles.WATER_INDEX,
        GroundTiles.DEEP_WATER_INDEX
    };
}
