using UnityEngine;
using System.Collections;

public class Layer_00_Debug : World {

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
                // Check World Level
                worldData[x + y * CHUNK_SIZE] = EntityID.B_DEBUG;
            }
        }

        Log("End Awake");
    }

}
