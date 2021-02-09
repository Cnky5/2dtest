using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelChunk : MonoBehaviour{

    Mesh mesh;    
    int[] mapData;
    public int size;
    public int mapHeight;
    public int mapWidth;
    public int layer1Modifier,layer2Modifier,noisePx;
    public Color terrainColor;
    public Color skyColor;
    public Color32 grass;
    public Color32 stone;
    public float scale;
    public float shiftY,shiftX;
    PolygonCollider2D collider2D;
    List<Vector3> newVerts = new List<Vector3>();
    List<int> newTris = new List<int>();
    List<Color32> newColor = new List<Color32>();
    public int squareCount;
    public void GenerateChunk()
    {
        
		mesh.name = "Procedural Grid";

        Vector3[] vertices = new Vector3[(size + 1) * (size + 1)];

		for (int i = 0, y = 0; y <= size; y++) 
        {
			for (int x = 0; x <= size; x++, i++) 
            {
				vertices[i] = new Vector3(x, y);
            }
		}


        mesh.vertices = vertices;

        int vert = 0;
        int tris = 0;
        int[] triangles = new int[size*size*6];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++, tris+=6)
            {
                
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + size + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size + 1;
                triangles[tris + 5] = vert + size + 2;


                vert++;
            }
            vert++;
        }

        mesh.triangles = triangles;

        
        Vector3[] newVertices = new Vector3[triangles.Length];
        int[] newTriangles = new int[triangles.Length];

        // Rebuild mesh to get unique verts, otherwise it lerps tri colors
        for (int i = 0; i < triangles.Length; i++)
        {
            newVertices[i] = vertices[triangles[i]];
            newTriangles[i] = i;

        }

        mesh.vertices = newVertices;
        mesh.triangles = newTriangles;
        mesh.RecalculateNormals();

        RenderColors();
    }


    private void Start() 
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        
        //RenderLandscape(GenerateLandscapeData(mapWidth,mapHeight,false));
        //RenderLandscape(GenerateNoise());
        //RenderLandscape(GenerateNoise());
        //UpdateMesh();
    }

    private void Update() {
        RenderLandscape(testTerrain(mapWidth,mapHeight));
        UpdateMesh();
        
    }

    //geneda esialgne data, teha noisega üle ja geneda nö koopad, sööta renderisse

    int[,] GenerateNoise()
    {
        int[,] map = new int[mapWidth,mapHeight];
        //mapData = new int[size*size];
        //float sizeX = map.GetUpperBound(0);
        //float sizeY = map.GetUpperBound(1);

        //Debug.Log("upperX: " + map.GetUpperBound(0) + " upperY: " + map.GetUpperBound(1));

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                float xCoord = x / (float)map.GetUpperBound(0) * scale;
                float yCoord = y / (float)map.GetUpperBound(1) * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                Debug.Log(sample);
                if (sample < 0.45f)
                {
                    map[x, y] = 0;
                    //Debug.Log(map[x,y] + " to 0");
                }
                    

                

                else map[x, y] = 1;
            }
        }
        return map;
    }

    byte[,] GenTerrain(int width, int height)
    {
        byte[,] map = new byte[width,height];

        
        for(int px=0;px<map.GetLength(0);px++)
        {
            int dirt = Noise(px,0, 100,35,1);
            dirt+= Noise(px,0, 50,30,1);
            dirt+=layer2Modifier;
            
            int stone= Noise(px,0, 80,15,1);
            stone+= Noise(px,0, 50,30,1);
            stone+= Noise(px,0, 10,10,1);
            stone+=layer1Modifier;
            
            for(int py=0;py<map.GetLength(1);py++)
            {
                if(py<stone)
                {
                    map[px,py]=2;
                } 

                //The next three lines remove dirt and rock to make caves in certain places
                if(Noise(px,py*2,16,14,1)>10)
                { //Caves
                    map[px,py]=0;
                }

                else if(py<dirt) 
                {
                    map[px,py]=1;
                }
            }
        }
        return map;
    }

    int Noise (int x, int y, float scale, float mag, float exp)
    {
        return (int) (Mathf.Pow ((Mathf.PerlinNoise(x/scale+shiftX,y/scale+shiftY)*mag),(exp) )); 
    }
    
    byte[,] testTerrain(int width, int height)
    {
        byte[,] map = new byte[width,height];

        
        for(int px=0 ; px < map.GetLength(0) ; px++)
        {
            int stone= Noise(px,0, 80,15,1);
            stone+= Noise(px,0, 50,30,1);
            stone+= Noise(px,0, 10,10,1);
            stone+=layer2Modifier;//+=(int)transform.position.y;
            
            Debug.Log(stone);

            int dirt = Noise(px,0, 100,35,1);
            dirt+= Noise(px,0, 50,30,1);
            dirt+= -18;//+=(int)transform.position.y;
            
            //int stone2 = Noise(px,0, 1000,35,1);
            //stone2+= Noise(px,0, 50,30,1);
            //stone2+=noisePx;//+=(int)transform.position.y;
            

            for(int py=0 ; py<map.GetLength(1) ; py++)
            {

                if(py<stone)
                {
                    map[px,py]=1;
                }
                
                if (transform.position.y < 0)
                {
                    map[px,py]=2;
                }

                //The next three lines remove dirt and rock to make caves in certain places
                if(Noise(px,py*2,16,14,1)>9)
                { //Caves
                    map[px,py]=0;
                }

                else if(py<dirt) 
                {
                    map[px,py]=2;
                }
            }
        }
        return map;
    }

    int Nooisee(int x, int y, float scale, int chunkSize)
    {
        float xCoord = x / (float)chunkSize + (transform.position.x/chunkSize)+shiftX;
        float yCoord = y / (float)chunkSize + (transform.position.y/chunkSize)+shiftY;
        float sample = Mathf.PerlinNoise(xCoord, yCoord);

        return (int)sample;
    }

    int[,] GenerateLandscapeData(int width, int height, bool empty)
    {
        int[,] map = new int[width,height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                if (y == height-2)
                {
                    map[x, y] = 1;
                }
                else
                {
                    map[x, y] = 2;
                }
            }
        }
        return map;
    }

    void RenderLandscape(byte[,] map)
    {

        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x,y] == 1)
                {
                    GenerateSquare(x, y, grass);
                }

                if (map[x,y] == 2)
                {
                    GenerateSquare(x, y, stone);
                }

            }
        }
    }

    void GenerateSquare(int x, int y, Color32 color)
    {

        newVerts.Add(new Vector3(x, y, 0));
        newVerts.Add(new Vector3(x + 1, y, 0));
        newVerts.Add(new Vector3(x + 1, y - 1, 0));
        newVerts.Add(new Vector3(x, y - 1, 0));
        

        newTris.Add(squareCount*4);
        newTris.Add((squareCount*4)+1);
        newTris.Add((squareCount*4)+3);
        newTris.Add((squareCount*4)+1);
        newTris.Add((squareCount*4)+2);
        newTris.Add((squareCount*4)+3);

        newColor.Add(color);
        newColor.Add(color);
        newColor.Add(color);
        newColor.Add(color);


        squareCount++;
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = newVerts.ToArray();
        mesh.triangles = newTris.ToArray();
        mesh.colors32 = newColor.ToArray();
        mesh.RecalculateNormals();

        squareCount = 0;
        newVerts.Clear();
        newTris.Clear();
        newColor.Clear();

    }

















    void RenderColors()
    {
        int[] mapData = GenerateMapData();
        Color[] colors = new Color[mesh.vertexCount];
        int id = 0;
        for (int t = 0; t < colors.Length; t+=6)
        {
            if (mapData[id] == 1)
            {
                colors[t] = terrainColor;
                colors[t + 1] = terrainColor;
                colors[t + 2] = terrainColor;
                colors[t + 3] = terrainColor;
                colors[t + 4] = terrainColor;
                colors[t + 5] = terrainColor;
            }
            id++;
        }
        mesh.SetColors(colors);
    }

    /*void GenerateColliders()
        {
            int[] verts = mesh.vertices;
            //PolygonCollider2D.SetPath() 
        }
    */
    int[] GenerateMapData()
    {
        mapData = new int[size*size];

        for (int i = 0, x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++, i++)
            {
                float xCoord = x / (float)size + (transform.position.y/size) +shiftX;
                float yCoord = y / (float)size + (transform.position.x/size) +shiftY;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                if (sample < 0.45f)
                    mapData[i] = 0;

                else mapData[i] = 1;
            }
        }
        return mapData;
    }

        public enum tileType
    {
        Dirt = 0,
        Stone = 1
    }
}

