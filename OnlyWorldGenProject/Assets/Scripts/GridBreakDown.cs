using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBreakDown : MonoBehaviour
{
    public static GridBreakDown instance;
    
    public static int cellPixelSize = 100;
    public List<Cell> listCells;
    public Cell[,] Grid = new Cell[cellPixelSize, cellPixelSize];
    public int numCells;
    public Texture2D tex;
    public Terrain terrain;
    public TerrainData terData;

    //public void Start()
    //{
    //    ApplyTexture();
    //    Invoke("ClearTexture", 5f);
    //}
    public void ApplyTexture()
    {
        tex = null;
        
        byte[] fileData;

        fileData = System.IO.File.ReadAllBytes("coloredPng.png");
        tex = new Texture2D(cellPixelSize, cellPixelSize);
        tex.LoadImage(fileData);

        terData = terrain.terrainData;
        terData.terrainLayers[0].diffuseTexture = tex;
    }

    public void ClearTexture()
    {
        Color[] resetColors = tex.GetPixels();

        for (int i = 0; i < resetColors.Length; i++)
        {
            resetColors[i] = Color.white;
        }

        tex.SetPixels(resetColors);
        tex.Apply();
    }

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

//#if UNITY_EDITOR
    public void GenerateGrid()
    {
        Texture2D tex = null;
        byte[] fileData;

        fileData = System.IO.File.ReadAllBytes("coloredPng.png");
        tex = new Texture2D(cellPixelSize, cellPixelSize);
        tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.


        numCells = 1000 / cellPixelSize;
        //float[,] cellPixels = new float[cellPixelSize, cellPixelSize];

        for (int row = 0; row < numCells; row++)
        {
            for (int col = 0; col < numCells; col++)
            {
                print($"row: {row}  ,  col:  {col}");
                Grid[row, col] = new Cell(row, col);
                Grid[row, col].cellPixels = GetCellPixels(row, col, tex);
                listCells.Add(Grid[row, col]);
            }
        }

        foreach(Cell c in listCells)
        {
            float sum = 0;
            for (int x = 0; x < cellPixelSize; x++)
            {
                for (int y = 0; y < cellPixelSize; y++)
                {
                    sum += c.cellPixels[x,y];

                }

            }
            c.cellHue = sum / (cellPixelSize * cellPixelSize);
            print(c.cellHue);
        }
        
    }

    public float[,] GetCellPixels(int row, int col, Texture2D tex)
    {
        int rowMod = row * cellPixelSize;
        int colMod = col * cellPixelSize;

        float[,] cellPixels = new float[cellPixelSize, cellPixelSize];

        for (int y = colMod; y < colMod + cellPixelSize; y++)
        {
            for (int x = rowMod; x < rowMod + cellPixelSize; x++)
            {
                // HUE - H are degrees
                float H, S, V;
                Color.RGBToHSV(tex.GetPixel(x, y), out H, out S, out V);

                if (x != 0 && y != 0)
                {
                    cellPixels[x - rowMod, y - colMod] = H;
                }
                else if (x == 0 && y != 0)
                {
                    cellPixels[0, y-colMod] = H;
                }
                else if (x != 0 && y == 0)
                {
                    cellPixels[x-rowMod, 0] = H;
                }
                else
                {
                    cellPixels[0, 0] = H;
                }

            }
        }

        return cellPixels;
    }
//#endif
}

public class Cell : MonoBehaviour
{
    
    public float[,] cellPixels = new float[GridBreakDown.cellPixelSize, GridBreakDown.cellPixelSize];
    public int row;
    public int col;
    public float cellHue;
    //public string name;

    public Cell(int row, int col)
    {
        this.row = row;
        this.col = col;
        //ame = $"{row}{col}";
        //gameObject.name = $"{row}{col}";
    }
    /*
    if (H >= 0.5 && H< 0.8) // Blue
                {
                    H = 0.75f;
                }
                else if (H >= 0.2 && H < 0.5) // Green
{
    H = 0.5f;
}
else if (H >= 0.8 || H < 0.2) // Red
{
    H = 0.25f;
}
    
    */
}

