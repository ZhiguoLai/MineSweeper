using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    #region Singleton
    public static Grid instance { get; set; }

    private void Awake()
    {
        instance = this;
    }
    #endregion
    private int width;//this is the width of the grid
    private int height;//this is the height of the grid
    public Element[,] elements;// this is the matrix that can manage the mines
    private void Start()
    {
        ResetGrid();//Get the value from the Gamestartmanager
    }
    public void ResetGrid()
    {
        width = (int)GameStartManager.instance.GetGridWidth(); // this is the wideth
        height = (int)GameStartManager.instance.GetGridHeight(); // this is the height 
        elements = new Element[width, height];//this can generate the array to manage the mines

    }
    public void showMines()//when player hit the mine show every mines in the grids
    {
        foreach(Element element in elements)
        {
            if (element.isMine)
            {
                element.LoadTexture(0);
            }
        }
    }
    public bool isMine(int x, int y)//to check whether this grid has mine
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            return instance.elements[x,y].isMine;
        }
        return false;
    }
    public int nearMines(int x, int y)//to calculate how many mines hide in the nearest 8 grids
    {
        int count = 0;
        
        if (y + 1 < height)
        {
            if (isMine(x, y + 1)) ++count;// north
            if (x + 1 < width)
                if (isMine(x + 1, y + 1)) ++count;// northeast
            if (x - 1 >= 0)
                if (isMine(x - 1, y + 1)) ++count;  //northwest
        }
        if (y - 1 >= 0)
        {
            if (isMine(x, y - 1)) ++count;//south
            if (x + 1 < width)
                if (isMine(x+1, y - 1)) ++count;//southeast
            if (x - 1 >= 0)
                if (isMine(x - 1, y - 1)) ++count;//southwest
        }
        if (x + 1 < width)
            if (isMine(x + 1, y)) ++count;//east
        if (x - 1 >= 0)
            if (isMine(x - 1, y)) ++count;// west

        return count;
    }
    public void findNoOpen(int x, int y , bool[,] visited) //open the nearest girds if they do not have mines, until open a grid has mine in the nearest 8 grids
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            if (visited[x, y]) return;//stop if this grid has been visited;

            elements[x, y].LoadTexture(nearMines(x, y));//show the sprite according to the amounts of mines in the nearest 8 grids
            if (nearMines(x, y) > 0) return;// stop because it opens a grid has mine in the nearest 8 grids
            visited[x, y] = true;
            if(y + 1 < height)
            {
                findNoOpen(x, y + 1, visited);//north
                if (x + 1 < width)
                    findNoOpen(x + 1, y + 1, visited);//northeast
                if (x - 1 >= 0)
                    findNoOpen(x - 1, y + 1, visited);//northwest
            }
            if (y - 1 >= 0)
            {
                findNoOpen(x, y - 1, visited);//south
                if (x + 1 < width)
                    findNoOpen(x + 1, y - 1, visited);//southeast
                if (x - 1 >= 0)
                    findNoOpen(x - 1, y - 1, visited);//southwest
            }
            if(x + 1 < width)
                findNoOpen(x + 1, y, visited);//east
            if(x - 1 >= 0)
                findNoOpen(x - 1, y, visited);//west
            
        }
    }
    public bool CheckWin()
    {
        foreach(Element elem in elements)
        {

            if(elem.isMine)
            {
                if(elem.GetComponent<SpriteRenderer>().sprite.name != "flag")// if player do not mark every mine
                {
                    return false;
                }
                else if (!elem.isOpened() && !elem.isMine)// if player do not open every grids without mine
                {
                    return false;
                }
            }
        }
        return true;// if player marks all mines or open the every grids without mine, the player win
    }
    public  int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}
