using System.Collections.Generic;
using UnityEngine;

public class inf_Gens : MonoBehaviour
{
    [Header("Terrain Prefabs")]
    public GameObject rotation1;
    public GameObject rotation2;

    [Header("Rock Reference")]
    public GameObject rock;
    public bool autoPlaceRock = false;

    [Header("Generation Settings")]
    public int initialPieces = 7;
    public float pieceLength = 10f;
    public float moveSpeed = 10f;

    private List<GameObject> spawnedPieces = new List<GameObject>();

    void Start()
    {
        Vector3 nextPos = transform.position;

        // Spawn initial terrain
        for (int i = 0; i < initialPieces; i++)
        {
            SpawnRandomPiece(nextPos);
            nextPos += Vector3.forward * pieceLength;
        }

        // Place rock at first piece
        if (autoPlaceRock && rock && spawnedPieces.Count > 0)
        {
            rock.transform.position =
                spawnedPieces[0].transform.position + Vector3.up * 1f;
        }
    }

    void Update()
    {
        // ONLY move while holding W
        if (!Input.GetKey(KeyCode.W))
            return;

        if (spawnedPieces.Count == 0) return;

        // Move terrain backward
        for (int i = 0; i < spawnedPieces.Count; i++)
        {
            spawnedPieces[i].transform.Translate(
                Vector3.back * moveSpeed * Time.deltaTime,
                Space.World
            );
        }

        // Spawn new terrain
        GameObject lastPiece = spawnedPieces[spawnedPieces.Count - 1];

        if (lastPiece.transform.position.z <
            transform.position.z + pieceLength * (initialPieces - 1))
        {
            Vector3 nextPos =
                lastPiece.transform.position + Vector3.forward * pieceLength;

            SpawnRandomPiece(nextPos);

            // Remove oldest
            if (spawnedPieces.Count > initialPieces)
            {
                Destroy(spawnedPieces[0]);
                spawnedPieces.RemoveAt(0);
            }
        }
    }

    void SpawnRandomPiece(Vector3 position)
    {
        if (!rotation1 || !rotation2) return;

        GameObject prefab =
            Random.value > 0.5f ? rotation1 : rotation2;

        GameObject newPiece =
            Instantiate(prefab, position, Quaternion.identity);

        spawnedPieces.Add(newPiece);
    }
}