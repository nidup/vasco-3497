using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Copy neighbour tiles, to ensure that we can generate a new chunk with relevant tiles related to the next chunk,
 * for instance, if the right side of a chunk contains a forest, be sure that the left side of the next chunk contains
 * a forrest too
 *
 * TODO: still limited cause taking in account only a neighbour and not all existing neighbours, for instance,
 * top and left
 */
public class NeighbourTilesCopier {

    /**
     * Copy nbTilesToCopy right original tiles to left neighbour tiles
     */
    public int[,] copyRightTiles(int[,] originalTiles, int nbTilesToCopy) {
        int[,] neighbourTiles = buildEmptyTiles(originalTiles);
        int sourceColumnIndex = neighbourTiles.GetLength(0) - 1;
        int copyColumnIndex = 0;
        for (var row = 0; row < neighbourTiles.GetLength(1); row++) {
            var copySrc = nbTilesToCopy;
            for (var copyDest = 0; copyDest < nbTilesToCopy; copyDest++) {
                copySrc--;
                neighbourTiles[copyColumnIndex + copyDest, row] = originalTiles[sourceColumnIndex - copySrc, row];
            }
        }

        return neighbourTiles;
    }

    /**
     * Copy nbTilesToCopy left original tiles to right neighbour tiles
     */
    public int[,] copyLeftTiles(int[,] originalTiles, int nbTilesToCopy) {
        var neighbourTiles = buildEmptyTiles(originalTiles);
        var sourceColumnIndex = 0;
        var copyColumnIndex = neighbourTiles.GetLength(0) - 1;
        for (var row = 0; row < neighbourTiles.GetLength(1); row++) {
            var copyDest = nbTilesToCopy;
            for (var copySrc = 0; copySrc < nbTilesToCopy; copySrc++) {
                copyDest--;
                neighbourTiles[copyColumnIndex - copyDest, row] = originalTiles[sourceColumnIndex + copySrc, row];
            }
        }

        return neighbourTiles;
    }

    /**
     * Copy nbTilesToCopy top original tiles to bottom neighbour tiles
     */
    public int[,] copyTopTiles(int[,] originalTiles, int nbTilesToCopy) {
        var neighbourTiles = buildEmptyTiles(originalTiles);
        var sourceRowIndex = 0;
        var copyRowIndex = neighbourTiles.GetLength(0) - 1;

        for (var column = 0; column < neighbourTiles.GetLength(0); column++) {
            var copyDest = nbTilesToCopy;
            for (var copySrc = 0; copySrc < nbTilesToCopy; copySrc++) {
                copyDest--;
                neighbourTiles[column, copyRowIndex - copyDest] = originalTiles[column, sourceRowIndex + copySrc];
            }
        }

        return neighbourTiles;
    }

    /**
     * Copy nbTilesToCopy bottom original tiles to top neighbour tiles
     */
    public int[,] copyBottomTiles(int[,] originalTiles, int nbTilesToCopy) {
        var neighbourTiles = buildEmptyTiles(originalTiles);
        var sourceRowIndex = neighbourTiles.GetLength(1) - 1;
        var copyRowIndex = 0;
        for (var column = 0; column < neighbourTiles.GetLength(0); column++) {
            var copySrc = nbTilesToCopy;
            for (var copyDest = 0; copyDest < nbTilesToCopy; copyDest++) {
                copySrc--;
                neighbourTiles[column, copyRowIndex + copyDest] = originalTiles[column, sourceRowIndex - copySrc];
            }
        }

        return neighbourTiles;
    }

    /**
     * Fulfil a tiles map with undefined tiles
     */
    private int[,] buildEmptyTiles(int[,] originalTiles) {
        int[,] emptyTiles = new int[originalTiles.GetLength(0), originalTiles.GetLength(1)];
        for (var row = 0; row < originalTiles.GetLength(0); row++) {
            for (var column = 0; column < originalTiles.GetLength(1); column++) {
                emptyTiles[row, column] = GroundTiles.UNDEFINED_INDEX;
            }
         }
        return emptyTiles;
    }
}
