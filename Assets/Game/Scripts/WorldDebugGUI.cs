using UnityEngine;
using System.Collections;

public class WorldDebugGUI : MonoBehaviour {
    public GUIStyle style;
    private int totalChunks = (2 * World.X_RENDER_CHUNKS + 1) * (2 * World.Y_RENDER_CHUNKS + 1);
    private int totalTilesPerChunk = World.CHUNK_SIZE * World.CHUNK_SIZE;

    private void OnGUI() {
        GUILayout.BeginHorizontal();
        GUILayout.Label(totalChunks + " chunks | ", style);
        GUILayout.Label(totalTilesPerChunk + " tiles per chunk | ", style);
        GUILayout.Label((totalChunks * totalTilesPerChunk) + " total squares", style);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Max placable tile height: " + (World.WORLD_HEIGHT_CHUNKS * World.CHUNK_SIZE), style);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("World ground level: " + GetComponent<World>().height, style);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("World size: " + GetComponent<World>().worldSize, style);
        GUILayout.EndHorizontal();
    }

}
