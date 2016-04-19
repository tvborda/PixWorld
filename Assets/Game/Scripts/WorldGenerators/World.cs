using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

/// <summary>
/// Base world class. Multiple worlds are allowed.
/// </summary>
public class World : MonoBehaviour {

    /// <summary>
    /// Static values across the worlds.
    /// </summary>
    public static readonly int CHUNK_SIZE = 16; //Chunk size in all worlds, can not be changed per world ( by default )
    public static readonly int X_RENDER_CHUNKS = 3; //Number of chunks to render on x axis
    public static readonly int Y_RENDER_CHUNKS = 2; //Number of chunks to render on y axis
    public static readonly int WORLD_HEIGHT_CHUNKS = 16; // The number of chunks that define the world height.
    public enum WorldSizes {
        Small = 32,
        Medium = 64,
        Large = 128
    }

    /// <summary>
    /// Used for loading and saving the world.
    /// </summary>
    public string worldName = "BaseWorld";

    /// <summary>
    /// Used for data generation of the world.
    /// </summary>
    public WorldSizes worldSize = WorldSizes.Small;

    /// <summary>
    /// Used for defining the world surface height.
    /// </summary>
    [Range(0, 128)]
    public int height = 64;

    /// <summary>
    /// Used as seed to noise generators.
    /// </summary>
    public int seed = 0;

    /// <summary>
    /// Render target.
    /// </summary>
    public Transform target;

    /// <summary>
    /// Center the target in the world.
    /// </summary>
    public bool CenterTargetInWorld = false;

    /// <summary>
    /// All chunks loaded in the world.
    /// </summary>
    private Dictionary<Vector2, Chunk> loadedChunks;

    /// <summary>
    /// Old target chunk position at which world got rendered.
    /// </summary>
    private Vector2 oldChunkPosition;

    /// <summary>
    /// World Data.
    /// </summary>
    protected ushort[] worldData;

    /// <summary>
    /// Helper variables to calculate tiles totals and iterators. 
    /// </summary>
    protected int worldWidth;
    protected int worldHeight;

    /// <summary>
    /// Reference to the class per world.
    /// </summary>
    internal ChunkMeshCreatorPool MeshCreatorPool;

    /// <summary>
    /// Helper variables to calculate tiles totals and iterators. 
    /// </summary>
//    [HideInInspector]
    public EntityID entityID;

    // Use this for initialization. Executed before Start method.
    public void Awake() {
        loadedChunks = new Dictionary<Vector2, Chunk>();
        oldChunkPosition = new Vector2(float.MaxValue, float.MaxValue);

        worldWidth = (int)worldSize * CHUNK_SIZE;
        worldHeight = WORLD_HEIGHT_CHUNKS * CHUNK_SIZE;
        
        CenterTarget();

        //ChunksBuffer = new List<Chunk>();
        MeshCreatorPool = new ChunkMeshCreatorPool();
        //ChunksToBeGenerated = new List<Chunk>();
        //SaveManager = new ChunkSave();
        //SaveManager.Load(WorldName);

        OnAwake();
    }


    // Use this for initialization
    void Start () {
	    if (target == null) {
            Debug.LogError("Missing world target!");
            Debug.Break();
        }
	}
	

	// Update is called once per frame
	void Update () {
        Vector2 targetChunkPosition = Utilities.PositionInChunks(target.position);

        if (UpdateChunksToDisable(targetChunkPosition)) {
            UpdateChunksToEnable(targetChunkPosition);
        }
        ReBuildChunks();
    }


    /// <summary>
    /// Verify if when moving the target, distant chunks need to be disabled.
    /// </summary>
    private bool UpdateChunksToDisable(Vector2 targetChunkPosition) {
        bool positionUpdated = false;

        //Check position of the target
        if (targetChunkPosition != oldChunkPosition) {
            //Update position
            oldChunkPosition = targetChunkPosition;
            positionUpdated = true;

            foreach (Vector2 chunkCoord in loadedChunks.Keys) {
                if ((Mathf.Abs(targetChunkPosition.x - chunkCoord.x) > X_RENDER_CHUNKS) ||
                      (Mathf.Abs(targetChunkPosition.y - chunkCoord.y) > Y_RENDER_CHUNKS)) {
                    loadedChunks[chunkCoord].markedToDisable = true;
                    //Debug.Log("Chunk [X: " + chunkCoord.x + " Y: " + chunkCoord.y + "]");
                    //loadedChunks[chunkCoord].Disable();
                }
            }
            //var k = loadedChunks.Keys;
            //for (int x = 0; x < k.Count; x++) {
            //    Vector2 chunkCoord = loadedChunks.Keys.ElementAt(x);
            //    if ((Mathf.Abs(targetChunkPosition.x - chunkCoord.x) > X_RENDER_CHUNKS) ||
            //          (Mathf.Abs(targetChunkPosition.y - chunkCoord.y) > Y_RENDER_CHUNKS)) {
            //        Debug.Log("Chunk [X: " + chunkCoord.x + " Y: " + chunkCoord.y + "]");
            //        GameObject.Destroy(loadedChunks[chunkCoord].gameObject);
            //        loadedChunks.Remove(chunkCoord);
            //    }
            //}
        }
        return positionUpdated;
    }


    /// <summary>
    /// When target moves activate or create new chunks.
    /// </summary>
    private void UpdateChunksToEnable(Vector2 targetChunkPosition) {
        int minX = (int)targetChunkPosition.x - X_RENDER_CHUNKS;
        int maxX = (int)targetChunkPosition.x + X_RENDER_CHUNKS;
        int minY = (int)targetChunkPosition.y - Y_RENDER_CHUNKS;
        int maxY = (int)targetChunkPosition.y + Y_RENDER_CHUNKS;
        minX = (minX < 0) ? 0 : minX;
        maxX = (maxX > ((int)worldSize - 1)) ? ((int)worldSize - 1) : maxX;
        minY = (minY < 0) ? 0 : minY;
        maxY = (maxY > (WORLD_HEIGHT_CHUNKS - 1)) ? (WORLD_HEIGHT_CHUNKS - 1) : maxY;

        for (int x = minX; x <= maxX; x++) {
            for (int y = minY; y <= maxY; y++) {
                if (loadedChunks.ContainsKey(new Vector2(x, y))) {
                    EnableChunk(x, y);
                } else {
                    AddChunk(x, y);
                }
            }
        }
    }


    /// <summary>
    /// This cost a lot, call it when you finish editing all chunks - in order to see the result.
    /// </summary>
    public void ReBuildChunks() {
        float closest = Mathf.Infinity;
        Chunk chunk = null;

        foreach (Chunk loadedChunk in loadedChunks.Values) {
            if (loadedChunk.flaggedToUpdate && loadedChunk.hasOneBlock && !loadedChunk.generating) {
                float dist = Vector2.Distance((Vector2)loadedChunk.transform.position, target.position);
                if (dist < closest) {
                    closest = dist;
                    chunk = loadedChunk;
                }
            }
        }

        if (chunk == null)
            return;

        chunk.flaggedToUpdate = false;
        chunk.UpdateChunk();
    }


    /// <summary>
    /// Enable a chunk that already exist on the Hierarchy.
    /// </summary>
    private void EnableChunk(int x, int y) {
        Chunk chunk;
        //Chunks is already created at this position
        chunk = loadedChunks[new Vector2(x, y)];
        if (!chunk.gameObject.activeSelf)
            chunk.Enable();
        chunk.markedToDisable = false;
    }


    /// <summary>
    /// Create a new chunk and place at the Hierarchy.
    /// </summary>
    private void AddChunk(int x, int y) {
        Chunk chunk;

        //We need to create new chunk
        GameObject obj = new GameObject("Chunk [X: " + x + " Y: " + y + "]");
        obj.transform.SetParent(transform);
        obj.layer = gameObject.layer;

        chunk = obj.AddComponent<Chunk>();
        chunk.transform.position = new Vector3(x * CHUNK_SIZE, y * CHUNK_SIZE, transform.position.z);

        //Neighbours
        Chunk LeftChunk, RightChunk, TopChunk, BotChunk;

        //Get and set neighbours
        loadedChunks.TryGetValue(new Vector2(x - 1, y), out LeftChunk);
        if (LeftChunk) {
            LeftChunk.Right = chunk;
            chunk.Left = LeftChunk;
        }

        loadedChunks.TryGetValue(new Vector2(x + 1, y), out RightChunk);
        if (RightChunk) {
            RightChunk.Left = chunk;
            chunk.Right = RightChunk;
        }

        loadedChunks.TryGetValue(new Vector2(x, y + 1), out TopChunk);
        if (TopChunk) {
            TopChunk.Bot = chunk;
            chunk.Top = TopChunk;
        }

        loadedChunks.TryGetValue(new Vector2(x, y - 1), out BotChunk);
        if (BotChunk) {
            BotChunk.Top = chunk;
            chunk.Bot = BotChunk;
        }

        //if (loadedChunk) {
        //    chunk.Data = savedChunk.Data;
        //    chunk.HasOneBlock = savedChunk.HasOneBlock;
        //}

        //Init chunk
        chunk.Init(this);

        //Add to dictionary
        loadedChunks.Add(new Vector2(x, y), chunk);

        //ChunkData savedChunk = SaveManager.LoadChunk(new Vector2(x, y));
        //bool loadedChunk = savedChunk == null ? false : true;
        bool loadedChunk = false;
        if (!loadedChunk) {
            //On chunk created
            OnChunkCreated(x, y, chunk);
        }

        //Flag
        chunk.flaggedToUpdate = true;
    }

    private void CenterTarget() {
        if (!CenterTargetInWorld)
            return;
        int worldCenterX = worldWidth / 2;
        int worldCenterY = worldHeight / 2;
        target.position = new Vector3(worldCenterX, worldCenterY, transform.position.z);
    }

    /// <summary>
    /// Called when Awake has ended.
    /// </summary>
    protected virtual void OnAwake() {
        worldData = new ushort[worldWidth * worldHeight];
        Debug.LogWarning("[FallBackWorld] Creating Base Tiles with EntityID.B_AIR");
        for (int x = 0; x < worldWidth; x++) {
            for (int y = 0; y < worldHeight; y++) {
                worldData[x + y * CHUNK_SIZE] = EntityID.B_AIR;
            }
        }
    }


    /// <summary>
    /// Called when chunk is created!
    /// </summary>
    protected virtual void OnChunkCreated(int x, int y, Chunk chunk) {
        for (int chunkX = 0; chunkX < CHUNK_SIZE; chunkX++) {
            for (int chunkY = 0; chunkY < CHUNK_SIZE; chunkY++) {
                int worldX = (x * CHUNK_SIZE) + chunkX;
                int worldY = (y * CHUNK_SIZE) + chunkY;
                chunk.SetTileLocal(chunkX, chunkY, worldData[worldX + worldY * CHUNK_SIZE]);
            }
        }
    }

}
