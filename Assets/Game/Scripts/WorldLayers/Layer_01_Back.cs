using UnityEngine;
using System.Collections;

public class Layer_01_Back : World {

    private void Log(string msg) {
        Debug.Log("[" + worldName + "] " + msg);
    }

    protected override void OnAwake() {
        Log("Awake");

        Log("Allocating Base Tiles Memory");
        worldData = new ushort[worldWidth * worldHeight];

        Log("Creating Base Tiles");
        for (int x = 0; x < worldWidth; x++) {
            for (int y = 0; y < worldHeight; y++) {
                worldData[x + y * World.CHUNK_SIZE] = EntityID.B_F_DIRT_02;
                //// Check World Level
                //if (y > height) {
                //    worldData[x + y * CHUNK_SIZE] = EntityID.B_B_DIRT_01;
                //} else {
                //    worldData[x + y * CHUNK_SIZE] = EntityID.B_AIR;
                //}
            }
        }

        worldData[17] = EntityID.B_DEBUG;

        Log("End Awake");
    }
}
