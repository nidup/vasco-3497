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

    private static int STATE_DEATH = 0;
    private static int STATE_ALIVE_ONE = 1;
    private static int STATE_ALIVE_TWO = 2;
    private static int STATE_ALIVE_THREE = 3;

    /**
     * Generate new tiles and ensure that init tiles are kept to ensure smooth transitions between chunks
     */
    public int[,] Generate(int width, int height/*, int[,] initTiles = null*/) {
        var chanceToStartAlive = 4;
        var numberOfSteps = 2;
        var deathLimit = 3;
        var birthLimit = 4;

        var baseTiles = Initialize(width, height, chanceToStartAlive);
        for (var i = 0; i < numberOfSteps; i++) {
            baseTiles = DoSimulationStep(baseTiles, width, height, deathLimit, birthLimit);
        }

        /* TODO
        if (initTiles !== null) {
            baseTiles = CopyInitTiles(baseTiles, initTiles);
        }*/

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
                        BaseTilesGenerator.STATE_ALIVE_ONE : (Random.Range(1, 10) < 5) ?
                        BaseTilesGenerator.STATE_ALIVE_TWO : BaseTilesGenerator.STATE_ALIVE_THREE;
                } else {
                    baseTiles[x, y] = BaseTilesGenerator.STATE_DEATH;
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
                if (baseTiles[x, y] > BaseTilesGenerator.STATE_DEATH) {
                    if (nbs < deathLimit) {
                        newTiles[x, y] = BaseTilesGenerator.STATE_DEATH;
                    } else {
                        newTiles[x, y] = GetDominantNeighbourActiveState(baseTiles, width, height, x, y);
                    }
                } else {
                    if (nbs > birthLimit) {
                        newTiles[x, y] = GetDominantNeighbourActiveState(baseTiles, width, height, x, y);
                    } else {
                        newTiles[x, y] = BaseTilesGenerator.STATE_DEATH;
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
                } else if (baseTiles[nbX, nbY] > BaseTilesGenerator.STATE_DEATH) {
                    count = count + 1;
                }
            }
        }

        return count;
    }

    private int GetDominantNeighbourActiveState(int[,] baseTiles, int width, int height, int x, int y) {
        var counterAliveOne = 0;
        var counterAliveTwo = 0;
        var counterAliveThree = 0;

        for (var i = -1; i < 2; i++) {
            for (var j = -1; j < 2; j++) {
                var nbX = i + x;
                var nbY = j + y;
                if (nbX < 0 || nbY < 0 || nbX >= width || nbY >= height) {
                    continue;
                } else if (baseTiles[nbX, nbY] == BaseTilesGenerator.STATE_ALIVE_ONE) {
                    counterAliveOne = counterAliveOne + 1;
                } else if (baseTiles[nbX, nbY] == BaseTilesGenerator.STATE_ALIVE_TWO) {
                    counterAliveTwo = counterAliveTwo + 1;
                } else if (baseTiles[nbX, nbY] == BaseTilesGenerator.STATE_ALIVE_THREE) {
                    counterAliveThree = counterAliveThree + 1;
                }
            }
        }

        if (counterAliveOne > counterAliveTwo && counterAliveOne > counterAliveThree) {
            return BaseTilesGenerator.STATE_ALIVE_ONE;
        } else if (counterAliveTwo > counterAliveOne && counterAliveTwo > counterAliveThree) {
            return BaseTilesGenerator.STATE_ALIVE_TWO;
        } else {
            return BaseTilesGenerator.STATE_ALIVE_THREE;
        }
    }

    /**
     * TODO Copy init tiles to ensure smooth transition with neighbour chunk
     *
    private copyInitTiles(baseTiles: Array<Array<number>>, initTiles: Array<Array<number>>) {
        for (let x = 0; x < baseTiles.length; x++) {
            for (let y = 0; y < baseTiles[x].length; y++) {
                if (initTiles !== null && initTiles[x][y] >= 0) {
                    baseTiles[x][y] = initTiles[x][y];
                }
            }
        }

        return baseTiles;
    }*/
}
