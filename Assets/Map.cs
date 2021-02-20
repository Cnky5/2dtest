using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
public GameObject ChunkPrefab;
public Texture2D worldMap;
public int chunkSize;
MapData mapData;


    void Start()
    {

        print((int)Mathf.Ceil(worldMap.width/chunkSize));
        print((int)Mathf.Ceil(worldMap.height/chunkSize));

        GenerateFromFile(worldMap);
        /*
        mapData = new MapData();

        mapData.chunks = new Chunk[9,9];
        mapData.chunks[0,0] = chunk(0,0);
        mapData.chunks[0,1] = chunk(0,1);
        mapData.chunks[1,0] = chunk(1,0);
        mapData.chunks[1,1] = chunk(1,1);

        mapData.chunks[2,1] = chunk(2,1);
        mapData.chunks[2,0] = chunk(2,0);
        mapData.chunks[0,2] = chunk(0,2);
        mapData.chunks[1,2] = chunk(1,2);
        mapData.chunks[2,2] = chunk(2,2);*/
    }


    public struct MapData
    {
        public Chunk[,] chunks;
    }


    void GenerateFromFile(Texture2D tex)
    {

        int chunksWidth = (int)Mathf.Ceil(tex.width/chunkSize);
        int chunksHeight = (int)Mathf.Ceil(tex.height/chunkSize);

        int amount = 0;
        for (int x = 0; x < chunksWidth; x++)
        {
            for (int y = 0; y < chunksHeight; y++)
            {
                GameObject chunkGO = Instantiate(ChunkPrefab);
                chunkGO.name = x +","+y;
                chunkGO.transform.position = new Vector2(x*chunkSize, y*chunkSize);
                chunkGO.transform.localScale = new Vector3(100,100,0);

                Chunk chunk = chunkGO.GetComponent<Chunk>();
                Texture2D texture = new Texture2D(chunkSize,chunkSize);
                texture.filterMode = FilterMode.Point;

                Color[] colors =  worldMap.GetPixels((int)(x * worldMap.width / chunksWidth), (int)(y * worldMap.height / chunksHeight), (int)(worldMap.width / chunksWidth), (int)(worldMap.height / chunksHeight));
                print(colors.Length);

                texture.SetPixels(colors);
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.Apply();

                Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));
                //tex.GetPixels32
                chunk.sprite.sprite = newSprite;
                chunk.texture = texture;

                //Texture2D newTex;
            }
        }

    }
/*
    Chunk chunk(int x, int y)
    {
        Chunk chunk = Instantiate(ChunkPrefab);
        chunk.transform.position = new Vector2(x*-chunkSize, y*-chunkSize);
        chunk.name = x +","+y;
        //chunk.transform.parent = transform;
        //chunk.GenerateChunk();

        return chunk;
    }*/
}