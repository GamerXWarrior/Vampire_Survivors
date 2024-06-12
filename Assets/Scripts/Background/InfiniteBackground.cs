using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// To scroll the background infinitely with the player movement
public class InfiniteBackground : MonoBehaviour
{
    public float speed = 2f;
    public float tileSize = 1f;
    public GameObject tilePrefab;
    private Transform player;
    private Vector3 startPosition;
    private List<GameObject> tiles = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //speed = player.GetComponent<PlayerMovement>().GetPlayerSpeed();
        startPosition = transform.position;
        GenerateTiles();
    }

    void Update()
    {
        float newX = Mathf.Repeat(player.position.x, tileSize);
        float newY = Mathf.Repeat(player.position.y, tileSize);
        Vector3 newPosition = new Vector3(newX, newY, startPosition.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, speed * Time.deltaTime);
        UpdateTiles();
    }

    //Generating tiles
    void GenerateTiles()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity);
                tile.transform.parent = transform;
                tiles.Add(tile);
            }
        }
    }

    // Updating tiles according the movement of Player
    void UpdateTiles()
    {
        foreach (GameObject tile in tiles)
        {
            float tileX = tile.transform.position.x;
            float tileY = tile.transform.position.y;
            if (tileX < player.position.x - tileSize || tileX > player.position.x + tileSize || tileY < player.position.y - tileSize || tileY > player.position.y + tileSize)
            {
                tile.transform.position = new Vector3((tileX + (tileX < player.position.x ? tileSize : -tileSize)), (tileY + (tileY < player.position.y ? tileSize : -tileSize)), 0);
            }
        }
    }

}
