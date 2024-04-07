﻿using UnityEngine;

namespace Water2D_Surface.scripts
{
  public class Water2DScript : MonoBehaviour
  {
    public Vector2 speed = new Vector2(0.01f, 0f);

    private Renderer rend;
    private Material mat;
    
    private void Awake()
    {
      rend = GetComponent<Renderer>();
      mat = rend.material;
    }

    private void LateUpdate()
    {
      Vector2 scroll = Time.deltaTime * speed;
      mat.mainTextureOffset += scroll;
    }
  }
}