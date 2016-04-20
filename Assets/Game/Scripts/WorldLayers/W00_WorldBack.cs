using UnityEngine;
using System.Collections;

public class W00_WorldBack : World {
    public float surfaceFrequency;
    public int surfaceAmplitude;
    public float cavesFrequency;
    public int cavesAmplitude;

    protected override void OnAwake() {
        World.X_RENDER_CHUNKS = worldWidth / World.CHUNK_SIZE / 2;
        World.Y_RENDER_CHUNKS = worldHeight / World.CHUNK_SIZE / 2;

        Log("Awake");

        Log("Allocating Base Tiles Memory");
        worldData = new ushort[worldWidth, worldHeight];

        CreateSurface();

        Log("End Awake");
    }

    private void CreateSurface() {
        Log("Creating Surface Tiles");
        for (int worldX = 0; worldX < worldWidth; worldX++) {
            for (int worldY = 0; worldY < worldHeight; worldY++) {
                NoiseSample ns = Noise.Perlin2D(new Vector3(seed + worldX, seed + worldY, 0), surfaceFrequency) * surfaceAmplitude;
                if (worldY > (height + (int)ns.value)) {
                    worldData[worldX, worldY] = EntityID.B_AIR;
                } else {
                    worldData[worldX, worldY] = EntityID.B_B_DIRT_01;
                }
            }
        }
    }

}
