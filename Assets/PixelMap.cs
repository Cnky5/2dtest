using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelMap : MonoBehaviour
{

public PixelChunk pixelChunkPrefab;
public int chunkSize;
MapData mapData;


    void Start()
    {
        mapData = new MapData();

        mapData.chunks = new PixelChunk[9,9];
        mapData.chunks[0,0] = chunk(0,0);
        mapData.chunks[0,1] = chunk(0,1);
        mapData.chunks[1,0] = chunk(1,0);
        mapData.chunks[1,1] = chunk(1,1);

        mapData.chunks[2,1] = chunk(2,1);
        mapData.chunks[2,0] = chunk(2,0);
        mapData.chunks[0,2] = chunk(0,2);
        mapData.chunks[1,2] = chunk(1,2);
        mapData.chunks[2,2] = chunk(2,2);
    }


    public struct MapData
    {
        public PixelChunk[,] chunks;
    }


    PixelChunk chunk(int x, int y)
    {
        PixelChunk chunk = Instantiate(pixelChunkPrefab);
        chunk.transform.position = new Vector2(x*chunkSize, y*chunkSize);
        chunk.name = x +","+y;
        chunk.transform.parent = transform;
        chunk.GenerateChunk();

        return chunk;
    }
}