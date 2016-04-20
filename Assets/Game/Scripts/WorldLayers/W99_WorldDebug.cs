using UnityEngine;
using System.Collections;

public class W99_WorldDebug : World {

    private void Log(string msg) {
        Debug.Log("[" + worldName + "] " + msg);
    }

    protected override void OnAwake() {
        World.X_RENDER_CHUNKS = worldWidth / World.CHUNK_SIZE / 2;
        World.Y_RENDER_CHUNKS = worldHeight / World.CHUNK_SIZE / 2;
        Log("Awake");

        Log("Allocating Base Tiles Memory");
        worldData = new ushort[worldWidth, worldHeight];

        Log("Creating Base Tiles");
        for (int worldX = 0; worldX < worldWidth; worldX++) {
            for (int worldY = 0; worldY < worldHeight; worldY++) {
                // Check World Level
                worldData[worldX, worldY] = EntityID.B_DEBUG;
            }
        }

        Log("End Awake");
    }

}
