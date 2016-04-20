using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class W02_WorldFront : World {
    public float surfaceFrequency;
    public int surfaceAmplitude;
    public float cavesFrequency;
    public int cavesAmplitude;
    public float rocksFrequency;
    public int rocksAmplitude;

    private ushort[] dirts, rocks;

    protected override void OnAwake() {
        World.X_RENDER_CHUNKS = worldWidth / World.CHUNK_SIZE / 2;
        World.Y_RENDER_CHUNKS = worldHeight / World.CHUNK_SIZE / 2;
        dirts = new ushort[] { EntityID.B_F_DIRT_01, EntityID.B_F_DIRT_02, EntityID.B_F_DIRT_03, EntityID.B_F_DIRT_04 };
        rocks = new ushort[] { EntityID.B_F_ROCK_01, EntityID.B_F_ROCK_02, EntityID.B_F_ROCK_03, EntityID.B_F_ROCK_04 };

        Log("Awake");

        Log("Allocating Base Tiles Memory");
        worldData = new ushort[worldWidth, worldHeight];

        CreateCaves();
        CreateSurface();
        CreateBedrock();

        Log("End Awake");
    }

    private void CreateSurface() {
        Log("Creating Surface Tiles");
        for (int worldX = 0; worldX < worldWidth; worldX++) {
            for (int worldY = 0; worldY < worldHeight; worldY++) {
                NoiseSample ns = Noise.Perlin2D(new Vector3(seed + worldX, seed + worldY, 0), surfaceFrequency) * surfaceAmplitude;
                if (worldY > (height + (int)ns.value)) {
                    worldData[worldX, worldY] = EntityID.B_AIR;
                } else if (worldData[worldX, worldY] == EntityID.B_DEBUG){
                    worldData[worldX, worldY] = EntityID.B_AIR;
                } else {
                    ns = Noise.Perlin2D(new Vector3(seed + worldX, seed + worldY, 0), rocksFrequency) * rocksAmplitude;
                    if (worldY < ((2*height/3) - (int)ns.value)) {
                        worldData[worldX, worldY] = rocks[Random.Range(0, rocks.Length)];
                    } else {
                        worldData[worldX, worldY] = dirts[Random.Range(0, dirts.Length)];
                    }
                }
            }
        }
    }

    private void CreateCaves() {
        Log("Creating Caves Tiles");
        for (int worldX = 0; worldX < worldWidth; worldX++) {
            for (int worldY = 0; worldY < worldHeight; worldY++) {
                NoiseSample ns = Noise.Perlin2D(new Vector3(seed + worldX, seed + worldY, 0), cavesFrequency) * cavesAmplitude;
                //ns += Noise.Perlin2D(new Vector3(worldX, worldY, 0), 1 / 10f) * 20;
                if (ns.value > 15f) {
                    if (worldY <= height)
                    worldData[worldX, worldY] = EntityID.B_DEBUG;
                }
            }
        }
    }

    private void CreateBedrock() {
        for (int worldX = 0; worldX < worldWidth; worldX++) {
            worldData[worldX, 0] = EntityID.B_BEDROCK;
        }
    }
}
