using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum ToolType { PEN = 0, BRUSH, NONE };
/// <summary>
/// Drawing Tool controls every pen and brush in the scene
/// </summary>

public class DrawingTool : MonoBehaviour
{

    // General Variables
    [Header ("General Variables")]
    public Camera mainCamera;
    public ToolType toolType = ToolType.NONE;
    [Space(30)]

    // Pen Variables
    [Header("Pen Variables")]
    public float linePointsMinStep;
    public float lineWidth;
    
    public BasicPen[] penPrefabs;
    [SerializeField]
    private BasicPen chosenPenPrefab;

    private int chosenPenPrefabIdex;
    
    public GameObject[] ButtonPrefabs;
    public GameObject chosenButtonPrefab;

    public LayerMask cantDrawOverLayer;
    private int cantDrawOverLayerIndex;

    BasicPen drawnObject;
    [Space(30)]

    // Brush Variables
    [Header("Brush Variables")]
    public BasicBrush[] brushPrefabs;
    [SerializeField]
    private BasicBrush chosenBrushPrefab;


    /// <summary>
    /// General Functions
    /// </summary>
    private void Start()
    {
        mainCamera = Camera.main;
        cantDrawOverLayerIndex = LayerMask.NameToLayer("CantDrawOver");
        chosenPenPrefab = penPrefabs[0];
        chosenPenPrefabIdex = 0;
    }

    private void Update()
    {
        if (toolType == ToolType.PEN)
        {
            if (Input.GetMouseButtonDown(0) && GameManager.remainInk > 0)
            {
                BeginDraw();
            }
            if (null != drawnObject && GameManager.remainInk > 0)
            {
                Draw();
            }
            if (Input.GetMouseButtonUp(0) || GameManager.remainInk < 0)
            {
                EndDraw();
            }
        }
        else if (toolType == ToolType.BRUSH)
        {
            // if mouse is on a mutable object, change it's properties
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

    public void SwitchTools(int prefabIndex)
    {
        chosenPenPrefabIdex = prefabIndex;
        chosenPenPrefab = penPrefabs[prefabIndex];
    }


    /// <summary>
    /// Pen Drawing Functions
    /// </summary>
    // Start drawing
    void BeginDraw()
    {
        if (GameManager.remainInk > 0)
        {
            // Instantiate line prefab
            drawnObject = Instantiate(chosenPenPrefab, this.transform).GetComponent<BasicPen>();
            // Set parameters
            drawnObject.UsePhysics(false);
            
            drawnObject.InitializePen(linePointsMinStep, lineWidth);

            drawnObject.AddPoint(mainCamera.ScreenToWorldPoint(Input.mousePosition));
        }


    }

    // Drawing the line
    void Draw()
    {
        var pos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // Prevent crossing between lines
        RaycastHit2D hit = Physics2D.CircleCast(pos, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer);
        if (hit)
            EndDraw();
        else
        {
            // add remain ink
            float distance = Vector2.Distance(pos, drawnObject.points[^1]);
            if (distance > drawnObject.pointsMinDistance)
            {
                GameManager.Instance.updateInk(distance);
                drawnObject.AddPoint(pos);
            }            
        }

    }

    // The end of drawing
    void EndDraw()
    {

        if (null == drawnObject) return;
        if (drawnObject.pointCount < 2)
        {
            Destroy(drawnObject.gameObject);
        }
        else
        {
            drawnObject.gameObject.layer = cantDrawOverLayerIndex;
            drawnObject.UsePhysics(true);
            drawnObject = null;
        }
    }

    /// <summary>
    /// Brush Fucntions
    /// </summary>



}
