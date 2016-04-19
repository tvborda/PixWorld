using UnityEngine;
#if UNITY_ENGINE
using UnityEditor;
#endif
using System.Collections;
using System;


/// <summary>
/// Possible tile types.
/// </summary>
public enum TileType {
    ColliderOffRenderOff = 0,
    ColliderOffRenderOn = 1,
    ColliderOnRenderOff = 2,
    ColliderOnRenderOn = 3
}

/// <summary>
/// Possible tile sizes.
/// </summary>
public enum TileSize {
    Tile16 = 16,
    Tile32 = 32
}


/// <summary>
/// Tile class.
/// </summary>
[Serializable]
public class Tile : MonoBehaviour {

    //Events
    //public delegate void _TileEvent(int x, int y, Chunk chunk);

    //public event _TileEvent OnTilePlacedEvent;
    //public event _TileEvent OnTileRemovedEvent;

    /// <summary>
    /// Each offset is noather submesh.
    /// </summary>
    public int zOffest;

    /// <summary>
    /// Unique tile id. This is initialized when creating tile for the first time!
    /// </summary>
    [NonSerialized]
    public int ID = -1;

    /// <summary>
    /// Tile type.
    /// </summary>
    public TileType Type = TileType.ColliderOnRenderOn;

    /// <summary>
    /// Material from which tile is extracted. 
    /// </summary>
    public Material Mat;

    /// <summary>
    /// X Coordiante of the tile in the texture.
    /// </summary>
    public int OffsetX;

    /// <summary>
    /// Y Coordiante of the tile in the texture.
    /// </summary>
    public int OffsetY;

    /// <summary>
    /// Tile width.
    /// </summary>
    public int Width = (int)TileSize.Tile32;

    /// <summary>
    /// Tile height.
    /// </summary>
    public int Height = (int)TileSize.Tile32;

    //Pre calculated
    public Vector2 Unit;
    public Vector2 Offset;

    //public void OnTilePlaced(int x, int y, Chunk chunk) {
    //    if (OnTilePlacedEvent != null)
    //        OnTilePlacedEvent.Invoke(x, y, chunk);
    //}

    //public void OnTileRemoved(int x, int y, Chunk chunk) {
    //    if (OnTileRemovedEvent != null)
    //        OnTileRemovedEvent.Invoke(x, y, chunk);
    //}

}

