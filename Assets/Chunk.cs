using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Color32[] colors;
    public SpriteRenderer sprite;
    public Texture2D texture;


    void Start() 
    {
        colors = texture.GetPixels32();
    }
}
