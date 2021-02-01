using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelChunk : MonoBehaviour{

    Mesh mesh;    
    int[] mapData;
    public int size;
    public Color terrainColor;
    public Color skyColor;
    public float scale = 1.1f;
    public float shift;
    PolygonCollider2D collider2D;

    public void GenerateChunk()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
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

    //for fun :DD
    private void Update() {
        RenderColors();
    }
    void RenderColors()
    {
        int id = 0;
        int[] mapData = GenerateMapData();
        Color[] colors = new Color[mesh.vertexCount];
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

            else if (mapData[id] == 0)
            {
                colors[t] = skyColor;
                colors[t + 1] = skyColor;
                colors[t + 2] = skyColor;
                colors[t + 3] = skyColor;
                colors[t + 4] = skyColor;
                colors[t + 5] = skyColor;                
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
                float xCoord = x / (float)size + (transform.position.y/size) +shift;
                float yCoord = y / (float)size + (transform.position.x/size) +shift;
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

