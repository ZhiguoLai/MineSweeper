using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameStartManager : MonoBehaviour
{
    [SerializeField]
    public UnityEvent Lose = new UnityEvent(); //add the lose effect
    [SerializeField]
    public UnityEvent Win = new UnityEvent();//add the win effect


    #region Singleton
    public static GameStartManager instance { get; set; }

    private void Awake()
    {
        instance = this;
    }
    #endregion
    Vector3 dragOrigin;//set the drag camera original position
    [SerializeField]
    float dragSpeed = 0;//set the drag camera speed
    [SerializeField]
    private int gridsWidth;// set the grids width
    [SerializeField]
    private int gridsHeight;//set the grids height
    // Set the number for how many grids player want.
    [SerializeField]
    private GameObject gridPrefab;//put the grid prefab here
    [SerializeField]
    private GameObject parent;// put the parent object here
    [SerializeField]
    private int mineCount;// put the amounts of mines player want here
    [SerializeField]
    private float mouseScrollScale;// set the zoom in/zoom out speed

    [SerializeField]
    private InputField textWidth, textHeight, textCount;// reference the value from the inout field

    Camera cameraMain;

    void Start()
    {
        //StartLevel();
    }

    public void StartLevel()
    {
        gridsWidth = Mathf.Max(10, int.Parse(textWidth.text));//get the value from the input field
        gridsHeight = Mathf.Max(10, int.Parse(textHeight.text));
        mineCount = Mathf.Min(gridsHeight * gridsWidth, int.Parse(textCount.text));

        textWidth.text = gridsWidth.ToString();//change the value if the value is illegal
        textHeight.text = gridsHeight.ToString();
        textCount.text = mineCount.ToString();

        #region SetCamera
        cameraMain = Camera.main.GetComponent<Camera>();//Get the main camera
        cameraMain.orthographicSize = Mathf.Max(gridsHeight, gridsWidth) / 2;// Change the camera size to suit the number of the grids.
        Vector3 cameraPosition = new Vector3(gridsWidth / 2, gridsHeight / 2 - 0.5f, -10);
        Camera.main.transform.position = cameraPosition;// Change the camera position to suit the number of the grids.
        #endregion

        foreach(Element element in Grid.instance.elements)//delete the old game object
        {
            if (element != null)
                Destroy(element.gameObject);
        }
        Grid.instance.ResetGrid();//Change the width and height valua

        for (int i = 0; i < gridsWidth; i++)//Generate the grids according to the difficulty player want.
        {
            for (int j = 0; j < gridsHeight; j++)
            {
                Grid.instance.elements[i, j] = Instantiate(gridPrefab, new Vector3(i, j, 0), Quaternion.identity, parent.transform).GetComponent<Element>();//put the gird under the mine group to clean the hierarch
            }
        }
        for (int c = 0; c < mineCount; c++)//set the grid to mine
        {
            int x = Random.Range(0, gridsWidth);//random the position of the mine
            int y = Random.Range(0, gridsHeight);
            while (Grid.instance.elements[x, y].isMine)//check whether this gird already has mine
            {
                x = Random.Range(0, gridsWidth);
                y = Random.Range(0, gridsHeight);
            }
            Grid.instance.elements[x, y].isMine = true;//this grid has mine
        }
    }

    private void Update()// change the camera scale and position
    {   
        if (Input.mouseScrollDelta.y > 0)
        {
            cameraMain.orthographicSize += cameraMain.orthographicSize < 50 ? Input.mouseScrollDelta.y * mouseScrollScale : 0;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            cameraMain.orthographicSize += cameraMain.orthographicSize > 5 ? Input.mouseScrollDelta.y * mouseScrollScale : 0;
        }
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 pos = cameraMain.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            
            Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

            cameraMain.transform.Translate(move, Space.World);
        }
    }
    public int GetGridWidth() // get the value of GridWidth
    {
        return gridsWidth;
    }
    public int GetGridHeight()// get the value of GridHeight
    {
        return gridsHeight;
    }
}
