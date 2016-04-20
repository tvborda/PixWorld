using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityID : MonoBehaviour {

    public struct BlockTile {
        public string name;
        public Tile tile;
        public BlockTile(string name = null, Tile tile = null) {
            this.name = name;
            this.tile = tile;
        }
    }

    #region Blocks Region
    // General Blocks
    public static ushort B_DEBUG = 65535;
    public static ushort B_AIR = 0;
    public static ushort B_BOUNDARIES = 1;
    public static ushort B_BEDROCK = 2;

    #region Blocks Region - Front
    // (B)locks_(F)ront_(Type)_(Sequential)
    // Front Blocks Mask: None
    public static ushort B_F_MASK = (ushort)(1 << 10);
    public static ushort B_F_DIRT_01 = (ushort)(0001 | B_F_MASK);
    public static ushort B_F_DIRT_02 = (ushort)(0002 | B_F_MASK);
    public static ushort B_F_DIRT_03 = (ushort)(0003 | B_F_MASK);
    public static ushort B_F_DIRT_04 = (ushort)(0004 | B_F_MASK);
    public static ushort B_F_DIRT_05 = (ushort)(0005 | B_F_MASK);
    public static ushort B_F_DIRT_06 = (ushort)(0006 | B_F_MASK);
    public static ushort B_F_DIRT_07 = (ushort)(0007 | B_F_MASK);
    public static ushort B_F_DIRT_08 = (ushort)(0008 | B_F_MASK);
    public static ushort B_F_DIRT_09 = (ushort)(0009 | B_F_MASK);
    public static ushort B_F_DIRT_10 = (ushort)(0010 | B_F_MASK);
    public static ushort B_F_DIRT_11 = (ushort)(0011 | B_F_MASK);
    public static ushort B_F_DIRT_12 = (ushort)(0012 | B_F_MASK);
    public static ushort B_F_ROCK_01 = (ushort)(0013 | B_F_MASK);
    public static ushort B_F_ROCK_02 = (ushort)(0014 | B_F_MASK);
    public static ushort B_F_ROCK_03 = (ushort)(0015 | B_F_MASK);
    public static ushort B_F_ROCK_04 = (ushort)(0016 | B_F_MASK);
    #endregion

    #region Blocks Region - Back
    // (B)locks_(B)ack_(Type)_(Sequential)
    // Back Blocks Mask: 11bit
    public static ushort B_B_MASK = (ushort)(1 << 11);
    public static ushort B_B_DIRT_01 = (ushort)(0001 | B_B_MASK);
    public static ushort B_B_DIRT_02 = (ushort)(0002 | B_B_MASK);
    public static ushort B_B_DIRT_03 = (ushort)(0003 | B_B_MASK);
    public static ushort B_B_DIRT_04 = (ushort)(0004 | B_B_MASK);
    public static ushort B_B_DIRT_05 = (ushort)(0005 | B_B_MASK);
    public static ushort B_B_DIRT_06 = (ushort)(0006 | B_B_MASK);
    public static ushort B_B_DIRT_07 = (ushort)(0007 | B_B_MASK);
    public static ushort B_B_DIRT_08 = (ushort)(0008 | B_B_MASK);
    public static ushort B_B_DIRT_09 = (ushort)(0009 | B_B_MASK);
    public static ushort B_B_DIRT_10 = (ushort)(0010 | B_B_MASK);
    public static ushort B_B_DIRT_11 = (ushort)(0011 | B_B_MASK);
    public static ushort B_B_DIRT_12 = (ushort)(0012 | B_B_MASK);
    #endregion

    #endregion

    private Dictionary<ushort, BlockTile> tiles;


    public void Awake() {
        tiles = new Dictionary<ushort, BlockTile>();

        #region Blocks Region
        string B_path = "Blocks/";
        tiles.Add(EntityID.B_DEBUG, new BlockTile(B_path + "B_Debug"));
        tiles.Add(EntityID.B_AIR, new BlockTile(B_path + "B_Air"));
        tiles.Add(EntityID.B_BOUNDARIES, new BlockTile(B_path + "B_Boundaries"));
        tiles.Add(EntityID.B_BEDROCK, new BlockTile(B_path + "B_Bedrock"));

        #region Blocks Region - Front
        string B_F = B_path + "Front/";
        tiles.Add(EntityID.B_F_DIRT_01, new BlockTile(B_F + "B_F_Dirt_01"));
        tiles.Add(EntityID.B_F_DIRT_02, new BlockTile(B_F + "B_F_Dirt_02"));
        tiles.Add(EntityID.B_F_DIRT_03, new BlockTile(B_F + "B_F_Dirt_03"));
        tiles.Add(EntityID.B_F_DIRT_04, new BlockTile(B_F + "B_F_Dirt_04"));
        tiles.Add(EntityID.B_F_DIRT_05, new BlockTile(B_F + "B_F_Dirt_05"));
        tiles.Add(EntityID.B_F_DIRT_06, new BlockTile(B_F + "B_F_Dirt_06"));
        tiles.Add(EntityID.B_F_DIRT_07, new BlockTile(B_F + "B_F_Dirt_07"));
        tiles.Add(EntityID.B_F_DIRT_08, new BlockTile(B_F + "B_F_Dirt_08"));
        tiles.Add(EntityID.B_F_DIRT_09, new BlockTile(B_F + "B_F_Dirt_09"));
        tiles.Add(EntityID.B_F_DIRT_10, new BlockTile(B_F + "B_F_Dirt_10"));
        tiles.Add(EntityID.B_F_DIRT_11, new BlockTile(B_F + "B_F_Dirt_11"));
        tiles.Add(EntityID.B_F_DIRT_12, new BlockTile(B_F + "B_F_Dirt_12"));
        tiles.Add(EntityID.B_F_ROCK_01, new BlockTile(B_F + "B_F_Rock_01"));
        tiles.Add(EntityID.B_F_ROCK_02, new BlockTile(B_F + "B_F_Rock_02"));
        tiles.Add(EntityID.B_F_ROCK_03, new BlockTile(B_F + "B_F_Rock_03"));
        tiles.Add(EntityID.B_F_ROCK_04, new BlockTile(B_F + "B_F_Rock_04"));
        #endregion

        #region Blocks Region - Back
        string B_B = B_path + "Back/";
        tiles.Add(EntityID.B_B_DIRT_01, new BlockTile(B_B + "B_B_Dirt_01"));
        tiles.Add(EntityID.B_B_DIRT_02, new BlockTile(B_B + "B_B_Dirt_02"));
        tiles.Add(EntityID.B_B_DIRT_03, new BlockTile(B_B + "B_B_Dirt_03"));
        tiles.Add(EntityID.B_B_DIRT_04, new BlockTile(B_B + "B_B_Dirt_04"));
        tiles.Add(EntityID.B_B_DIRT_05, new BlockTile(B_B + "B_B_Dirt_05"));
        tiles.Add(EntityID.B_B_DIRT_06, new BlockTile(B_B + "B_B_Dirt_06"));
        tiles.Add(EntityID.B_B_DIRT_07, new BlockTile(B_B + "B_B_Dirt_07"));
        tiles.Add(EntityID.B_B_DIRT_08, new BlockTile(B_B + "B_B_Dirt_08"));
        tiles.Add(EntityID.B_B_DIRT_09, new BlockTile(B_B + "B_B_Dirt_09"));
        tiles.Add(EntityID.B_B_DIRT_10, new BlockTile(B_B + "B_B_Dirt_10"));
        tiles.Add(EntityID.B_B_DIRT_11, new BlockTile(B_B + "B_B_Dirt_11"));
        tiles.Add(EntityID.B_B_DIRT_12, new BlockTile(B_B + "B_B_Dirt_12"));
        #endregion

        #endregion
    }

    //tiles.Add(EntityID.B_B_DIRT_12, (Instantiate(Resources.Load(B_B + "B_B_Dirt_12", typeof(GameObject))) as GameObject).GetComponent<Tile>());
    public Tile GetTile(ushort tileId) {
        BlockTile t;

        if (tiles.TryGetValue(tileId, out t)) {
            if (t.tile == null) {
                GameObject obj = Instantiate(Resources.Load(t.name, typeof(GameObject))) as GameObject;
                obj.transform.SetParent(transform);
                t.tile = obj.GetComponent<Tile>();
                tiles[tileId] = t;
            }
            return t.tile;
        } else {
            Debug.Log("Missing entity id: " + tileId.ToString());
            return tiles[EntityID.B_AIR].tile;
        }
    }

}
