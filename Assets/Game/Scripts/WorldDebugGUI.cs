using UnityEngine;
using System.Collections;

public class WorldDebugGUI : MonoBehaviour {
    public GUIStyle style;
    private int totalChunks = (2 * World.X_RENDER_CHUNKS + 1) * (2 * World.Y_RENDER_CHUNKS + 1);
    private int totalTilesPerChunk = World.CHUNK_SIZE * World.CHUNK_SIZE;

    private void OnGUI() {
        int totalTilesInWorld = (int)GetComponentInChildren<World>().worldSize * World.WORLD_HEIGHT_CHUNKS * World.CHUNK_SIZE * World.CHUNK_SIZE;

        GUILayout.BeginHorizontal();
        GUILayout.Label(totalChunks + " rendered chunks | ", style);
        GUILayout.Label(totalTilesPerChunk + " tiles per chunk | ", style);
        GUILayout.Label((totalChunks * totalTilesPerChunk) + " total rendered tiles | ", style);
        GUILayout.Label(totalTilesInWorld +  " total world tiles", style);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max placable tile height: " + (World.WORLD_HEIGHT_CHUNKS * World.CHUNK_SIZE), style);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("World ground level: " + GetComponentInChildren<World>().height, style);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("World size: " + GetComponentInChildren<World>().worldSize, style);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        World[] worlds = GetComponentsInChildren<World>();
        foreach (World w in worlds) {
            if(GUILayout.Button("Rebuild " + w.name)) {
                w.RebuildAll();
            }
        }
        GUILayout.EndHorizontal();
    }

}
