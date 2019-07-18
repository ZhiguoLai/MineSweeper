using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour
{
    public bool isMine = false; //Chcek whether this grid is a mine.
    [SerializeField]
    private Sprite[] numberTextures;// put the art asset for the grid which is not a mine
    [SerializeField]
    private Sprite mineTextures;// put the art asset for the grid is a mine
    [SerializeField]
    private Sprite flagTexture;//put the art asset for the grid player want to mark as it has mine
    [SerializeField]
    private Sprite doubtTexture;//put the art asset for the grid player want to mark as they have question
    [SerializeField]
    private Sprite originalTexture;//Put the art asset for the original grid 

    void Start()
    {
        int x = (int)transform.position.x;
        int y = (int)transform.position.y;
        Grid.instance.elements[x, y] = this;//save the position information of this gird into the matrix.
    }
    public void LoadTexture(int minescount)
    {
            if (isMine)
            {
                GetComponent<SpriteRenderer>().sprite = mineTextures;// if this grid is mine use the mine texture
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = numberTextures[minescount];// if this grid is nt mine use the texture according to the numbers of mine in the grids around it
            }
    }
    public bool isOpened()
    {
        return GetComponent<SpriteRenderer>().sprite.name != "Original"; //Check whether the texture here has been changed
    }
    public bool isFlag()
    {
        return GetComponent<SpriteRenderer>().sprite.name == "flag";//Check whether the texture here is flag
    }
    public bool isDoubt()
    {
        return GetComponent<SpriteRenderer>().sprite.name == "doubt";//Check whether the texture here is question mark
    }
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))//If player use left click to find the result in this grid
        {
            if (!isFlag())
            {
                if (isMine)//GameLose
                {
                    Grid.instance.showMines();// show every mine in the grids
                    GameStartManager.instance.Lose.Invoke();
                }
                else//if this grid is not mine
                {
                    int x = (int)transform.position.x;
                    int y = (int)transform.position.y;
                    LoadTexture(Grid.instance.nearMines(x, y));//According to the information in the matrix to show the texture that indicates how many mines near this grid
                    Grid.instance.findNoOpen(x, y, new bool[Grid.instance.GetWidth(), Grid.instance.GetHeight()]);//
                    Debug.Log(Grid.instance.GetWidth());
                    if (Grid.instance.CheckWin()) GameStartManager.instance.Win.Invoke();// if open the whole grids without mine, the player win;

                }
            }
        }
        else if (Input.GetMouseButtonDown(1))//If player use the right click to interact with the grid
        {
            if (isFlag())// if the grid is marked will turn it into question mark
            {
                GetComponent<SpriteRenderer>().sprite = doubtTexture;
            }
            else if (isDoubt())// if the grid is questioned will turn it into original texture
            {
                GetComponent<SpriteRenderer>().sprite = originalTexture;
            }
            else if (!isOpened())// if the grid is original will turn it into the marked texture
            {
                GetComponent<SpriteRenderer>().sprite = flagTexture;
            }
            if (Grid.instance.CheckWin()) GameStartManager.instance.Win.Invoke();
        }
    }
}
