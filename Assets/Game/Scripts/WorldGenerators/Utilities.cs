using UnityEngine;
using System.Collections;

public class Utilities {

    /// <summary>
    /// Convert target position to chunk coords.
    /// </summary>
    public static Vector2 PositionInChunks(Vector3 targetPosition) {
        return new Vector2(Mathf.FloorToInt(targetPosition.x / World.CHUNK_SIZE), Mathf.FloorToInt(targetPosition.y / World.CHUNK_SIZE));
    }

}
