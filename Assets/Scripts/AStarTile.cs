using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarTile : MonoBehaviour
{

    public bool isWalkable = true;
    public bool isStart = false;
    public bool isEnd = false;
    public bool isPath = false;
    public bool isVisited = false;
    public bool isCurrent = false;
    public bool isWall = false;

    public int gCost = 0;
    public int hCost = 0;
    public int fCost = 0;

    public AStarTile parent = null;

    public int x = 0;
    public int y = 0;

    public List<AStarTile> neighbours = new List<AStarTile>();

    public void SetNeighbours(List<AStarTile> neighbours)
    {
        this.neighbours = neighbours;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public void SetTileColor(Color color)
    {
        GetComponent<Renderer>().material.color = color;
    }

    public void SetTileColor(Color color, float alpha)
    {
        Color newColor = new Color(color.r, color.g, color.b, alpha);
        GetComponent<Renderer>().material.color = newColor;
    }

    public void SetTileColor(float r, float g, float b)
    {
        Color newColor = new Color(r, g, b);
        GetComponent<Renderer>().material.color = newColor;
    }

    public void SetTileColor(float r, float g, float b, float a)
    {
        Color newColor = new Color(r, g, b, a);
        GetComponent<Renderer>().material.color = newColor;
    }

    public void SetTileColor(int r, int g, int b)
    {
        Color newColor = new Color(r / 255f, g / 255f, b / 255f);
        GetComponent<Renderer>().material.color = newColor;
    }

    public void SetTileColor(int r, int g, int b, int a)
    {
        Color newColor = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        GetComponent<Renderer>().material.color = newColor;
    }

    public void SetTileColor(string hex)
    {
        Color newColor = HexToColor(hex);
        GetComponent<Renderer>().material.color = newColor;
    }

    public void SetTileColor(string hex, float alpha)
    {
        Color newColor = HexToColor(hex);
        newColor.a = alpha;
        GetComponent<Renderer>().material.color = newColor;
    }

    public Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
        hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
        byte a = 255; //assume fully visible unless specified in hex
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        //Only use alpha if the string has enough characters
        if (hex.Length == 8)
        {
            a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
        }
        return new Color32(r, g, b, a);
    }   

}
