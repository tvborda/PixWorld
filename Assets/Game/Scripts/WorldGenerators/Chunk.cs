using UnityEngine;
using System.Collections;

/// <summary>
/// Base chunk class. Multiple chunks exist in a single world.
/// </summary>
public class Chunk : MonoBehaviour {

    /// <summary>
    /// Chunk data.
    /// </summary>
    public ushort[] Data;

    /// <summary>
    /// Neighbours.
    /// </summary>
    public Chunk Top, Left, Bot, Right;

    /// <summary>
    /// World to which chunk belong.
    /// </summary>
    public World myWorld;

    /// <summary>
    /// Mesh renderer reference.
    /// </summary>
    private MeshRenderer Render;

    /// <summary>
    /// Mesh filter reference.
    /// </summary>
    private MeshFilter Filter;

    /// <summary>
    /// Collider reference.
    /// </summary>
    private MeshCollider Coll;

    /// <summary>
    /// Is this chunk marked to be disabled?
    /// </summary>
    public bool markedToDisable;

    /// <summary>
    /// Wheter this chunk should be updated.
    /// </summary>
    public bool flaggedToUpdate;

    /// <summary>
    /// Has this chunk atleast one block?
    /// </summary>
    public bool hasOneBlock;

    /// <summary>
    /// Chunk finished generating?.
    /// </summary>
    public bool generating;


    /// <summary>
    /// Has the reference to the cmc while generating.
    /// </summary>
    public ChunkMeshCreator MeshCreator;


    public void Init(World world) {
        myWorld = world;

        if (Data == null) {
            Data = new ushort[World.CHUNK_SIZE * World.CHUNK_SIZE];
        }
        //    } else { //Data was set before the chunk is initialized - this is only possible when chunk was loaded

        //        //Call tile placed event on each tile
        //        for (int i = 0; i < Data.Length; i++) {
        //            //myWorld.GetTile(Data[i]).OnTilePlaced(i % World.CHUNK_SIZE, i / World.CHUNK_SIZE, this);
        //        }
        //    }

        //    PositionInChunks = World.PositionInChunks(transform.position);

        //Add componenets
        Render = gameObject.AddComponent<MeshRenderer>();
        Filter = gameObject.AddComponent<MeshFilter>();
        Coll = gameObject.AddComponent<MeshCollider>();
    }


    /// <summary>
    /// Updates chunk mesh. If the chunk was re-generating already. The process will stop and it will start again.
    /// </summary>
    public void UpdateChunk() {
        if (!generating) {
            generating = true;
            flaggedToUpdate = false;
            MeshCreator = myWorld.MeshCreatorPool.GetMeshCreator();
            MeshCreator.Build(this, Data, Filter, Render, Coll);
        } else {
            //Flag it for future updates
            flaggedToUpdate = true;
        }

    }


    private void Update() {
        if (generating) {
            MeshCreator.ThreadChecker();
        }
        //if (Time.time >= DisableTime) {
            if (markedToDisable) {
                if (generating) {
                    MeshCreator.Abort();
            }
            Disable();
            } else {
                //DisableTime = -1;
            }
        //}

    }


    /// <summary>
    /// Sets the tile at the given location, in the chunk. 
    /// </summary>
    public bool SetTileLocal(int x, int y, ushort tile, bool sendEvents = true) {
        if (x < 0 || y < 0 || x >= World.CHUNK_SIZE || y >= World.CHUNK_SIZE)
            return false;

        //if (x == 0 && Left)
        //    Left.FlaggedToUpdate = true;

        //if (y == 0 && Bot)
        //    Bot.FlaggedToUpdate = true;

        //if (x + 1 >= World.CHUNK_SIZE && Right)
        //    Right.FlaggedToUpdate = true;

        //if (y + 1 >= World.CHUNK_SIZE && Top)
        //    Top.FlaggedToUpdate = true;

        flaggedToUpdate = true;

        //Old tile
        //BaseTile oldTile = myWorld.GetTile(Data[x + y * World.CHUNK_SIZE]);
        //BaseTile newTile = myWorld.GetTile(tile);
        Tile newTile = myWorld.entityID.GetTile(tile);

        //if (sendEvents && newTile.ID != oldTile.ID)
        //    oldTile.OnTileRemoved(x, y, this);

        Data[x + y * World.CHUNK_SIZE] = tile;

        //Get tile
        //if (sendEvents && newTile.ID != oldTile.ID)
        //    newTile.OnTilePlaced(x, y, this);

        if (newTile.Type != TileType.ColliderOffRenderOff)
            hasOneBlock = true;

        return true;
    }


    /// <summary>
    /// Get tile only from this chunk and it's neighbours. Slow.
    /// </summary>
    public ushort GetTileGlobal(int x, int y) {
        if (x < 0) {
            if (Left != default(Chunk))
                return Left.GetTileGlobal(World.CHUNK_SIZE + x, y);
            else
                return ushort.MaxValue;
        }
        if (y < 0) {
            if (Bot != default(Chunk))
                return Bot.GetTileGlobal(x, World.CHUNK_SIZE + y);
            else
                return ushort.MaxValue;
        }
        if (x >= World.CHUNK_SIZE) {
            if (Right != default(Chunk))
                return Right.GetTileGlobal(x - World.CHUNK_SIZE, y);
            else
                return ushort.MaxValue;
        }
        if (y >= World.CHUNK_SIZE) {
            if (Top != default(Chunk))
                return Top.GetTileGlobal(x, y - World.CHUNK_SIZE);
            else
                return ushort.MaxValue;
        }

        return Data[x + y * World.CHUNK_SIZE];
    }


    /// <summary>
    /// Set the gameObject to Disabled.
    /// </summary>
    public void Disable() {
        gameObject.SetActive(false);
    }


    /// <summary>
    /// Set the gameObject to Enabled.
    /// </summary>
    public void Enable() {
        gameObject.SetActive(true);
    }
}
