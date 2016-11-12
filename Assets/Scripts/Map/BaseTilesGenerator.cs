using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Procedural tiles map generator using cellular automata, it only uses base tiles (forrest, sand, water, deep water),
 * the passed init cells are kept and not updated to ensure smooth transitions between map chunks
 *
 * @see https://en.wikipedia.org/wiki/Cellular_automaton
 * @see https://gamedevelopment.tutsplus.com/tutorials/generate-random-cave-levels-using-cellular-automata--gamedev-9664
 * @see http://fiddle.jshell.net/neuroflux/qpnf32fu/
 */
public class BaseTilesGenerator {
    /**
     * Generate new tiles and ensure that init tiles are kept to ensure smooth transitions between chunks
     */
    public int[,] Generate(int width, int height, int[,] initTiles = null) {
        var chanceToStartAlive = 4;
        var numberOfSteps = 2;
        var deathLimit = 3;
        var birthLimit = 4;

        var baseTiles = Initialize(width, height, chanceToStartAlive);
        for (var i = 0; i < numberOfSteps; i++) {
            baseTiles = DoSimulationStep(baseTiles, width, height, deathLimit, birthLimit);
        }

        if (initTiles != null) {
            baseTiles = CopyInitTiles(baseTiles, initTiles);
        }

        return baseTiles;
    }

    /**
     * Generate random tiles to fulfil the map
     */
    private int[,] Initialize(int width, int height, int chanceToStartAlive) {
        var baseTiles = new int[width, height];
        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++) {
                if (Random.Range(1, 10) < chanceToStartAlive) {
                    baseTiles[x, y] = (Random.Range(1, 10) < 3) ?
                        GroundTiles.SAND_INDEX : (Random.Range(1, 10) < 5) ?
                        GroundTiles.WATER_INDEX : GroundTiles.DEEP_WATER_INDEX;
                } else {
                    baseTiles[x, y] = GroundTiles.FORREST_INDEX;
                }
            }
        }

        return baseTiles;
    }

    /**
     * Change random tiles depending on neighbour tiles
     */
    private int[,] DoSimulationStep(int[,] baseTiles, int width, int height, int deathLimit, int birthLimit) {
        var newTiles = new int[width, height];
        for (var x = 0; x < width; x++) {
            for (var y = 0; y < height; y++) {
                var nbs = CountAliveNeighbours(baseTiles, width, height, x, y);
                if (baseTiles[x, y] > GroundTiles.FORREST_INDEX) {
                    if (nbs < deathLimit) {
                        newTiles[x, y] = GroundTiles.FORREST_INDEX;
                    } else {
                        newTiles[x, y] = GetDominantNeighbourActiveState(baseTiles, width, height, x, y);
                    }
                } else {
                    if (nbs > birthLimit) {
                        newTiles[x, y] = GetDominantNeighbourActiveState(baseTiles, width, height, x, y);
                    } else {
                        newTiles[x, y] = GroundTiles.FORREST_INDEX;
                    }
                }
            }
        }

        return newTiles;
    }

    private int CountAliveNeighbours(int[,] baseTiles, int width, int height, int x, int y) {
        var count = 0;
        for (var i = -1; i < 2; i++) {
            for (var j = -1; j < 2; j++) {
                var nbX = i + x;
                var nbY = j + y;
                if (nbX < 0 || nbY < 0 || nbX >= width || nbY >= height) {
                    count = count + 1;
                } else if (baseTiles[nbX, nbY] > GroundTiles.FORREST_INDEX) {
                    count = count + 1;
                }
            }
        }

        return count;
    }

    private int GetDominantNeighbourActiveState(int[,] baseTiles, int width, int height, int x, int y) {
        var counterAliveSand = 0;
        var counterAliveWater = 0;
        var counterAliveDeepWater = 0;

        for (var i = -1; i < 2; i++) {
            for (var j = -1; j < 2; j++) {
                var nbX = i + x;
                var nbY = j + y;
                if (nbX < 0 || nbY < 0 || nbX >= width || nbY >= height) {
                    continue;
                } else if (baseTiles[nbX, nbY] == GroundTiles.SAND_INDEX) {
                    counterAliveSand = counterAliveSand + 1;
                } else if (baseTiles[nbX, nbY] == GroundTiles.WATER_INDEX) {
                    counterAliveWater = counterAliveWater + 1;
                } else if (baseTiles[nbX, nbY] == GroundTiles.DEEP_WATER_INDEX) {
                    counterAliveDeepWater = counterAliveDeepWater + 1;
                }
            }
        }

        if (counterAliveSand > counterAliveWater && counterAliveSand > counterAliveDeepWater) {
            return GroundTiles.SAND_INDEX;
        } else if (counterAliveWater > counterAliveSand && counterAliveWater > counterAliveDeepWater) {
            return GroundTiles.WATER_INDEX;
        } else {
            return GroundTiles.DEEP_WATER_INDEX;
        }
    }

    /**
     * Copy init tiles to ensure smooth transition with neighbour chunk
     */
    private int[,] CopyInitTiles(int[,] baseTiles, int[,] initTiles) {
        for (var x = 0; x < baseTiles.GetLength(0); x++) {
            for (var y = 0; y < baseTiles.GetLength(1); y++) {
                if (initTiles != null && initTiles[x, y] >= 0) {
                    baseTiles[x, y] = initTiles[x, y];
                }
            }
        }

        return baseTiles;
    }
}
