using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a MapChunk, a chunk of the global map with coordinates and the tiles to display.
 *
 * baseTiles = only base ground tiles for sand, forrest, water and deep water
 * smoothedTiles = ordered base tiles to avoid to have neighbour incompatibility, for instance, forrest and water
 * finalTiles = base tiles are here replaced by rounded tiles allowing sweet transitions between grounds
 *
 * the rand state allows to re-generate the exact same chunk
 *
 * the first generated chunk has coordinates x: 0, y:0
 */
public class MapChunk {

    private int [,] baseTiles;
    private int [,] smoothTiles;
    private int [,] finalTiles;
    private int positionX;
    private int positionY;
    private Random.State randSeed;

    public MapChunk(int [,] bTiles, int [,] sTiles, int [,] fTiles, int posX, int posY, Random.State seed)
    {
        baseTiles = bTiles;
        smoothTiles = sTiles;
        finalTiles = fTiles;
        positionX = posX;
        positionY = posY;
        randSeed = seed;
    }

    public int [,] getBaseTiles() {
        return baseTiles;
    }

    public int [,] getSmoothTiles() {
        return smoothTiles;
    }

    public int [,] getFinalTiles() {
        return finalTiles;
    }

    public int getPositionX() {
        return positionX;
    }

    public int getPositionY() {
        return positionY;
    }

    public Random.State getRandSeed() {
        return randSeed;
    }
}
