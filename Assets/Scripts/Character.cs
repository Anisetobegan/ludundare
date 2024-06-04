using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character : MonoBehaviour
{

    float moveSpeed = 2f;
    float health;
    int lvl;
    float exp;
    //Perks perk;
    float summonCooldown;
    int maxSummons = 5;
    [SerializeField] private List<Summon> currentSummons;
    [SerializeField] private List<Summon> selectedSummons;
    [SerializeField] private List<Summon> summons;
    enum State
    {
        Idle,
        Moving,
        Casting,
        Dead
    }
    State state = State.Idle;

    Vector3 lastPos;


    private Camera cam;
    [SerializeField] private LayerMask clickable;
    [SerializeField] private LayerMask ground;

    [SerializeField] private RectTransform boxVisual;
    Rect selectionBox;
    Vector2 startPos;
    Vector2 endPos;

    private void Start()
    {
        cam = Camera.main;
        lastPos = transform.position;
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        DrawVisual();
    }

    // Update is called once per frame
    void Update()
    {
        SelectSummon();
        DragSelect();
        
        GiveOrder();
        

        switch (state) 
        {
            case State.Idle:

                Move();
                Summon();
                
                if (IsMoving())
                {
                    state = State.Moving;
                }
                break;

            case State.Moving:

                Move();
                Summon();
                
                if(!IsMoving())
                {
                    state = State.Idle;
                }
                break;

            case State.Casting:
                break;

            case State.Dead: 
                break;
        }
    }
           
    private void Move() //moves the player
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);

        transform.position += (moveDirection * moveSpeed) * Time.deltaTime;      
    }

    private bool IsMoving() //checks if the player is moving
    {
        if (transform.position != lastPos)
        {
            lastPos = transform.position;
            return true;
        }
        else
        {
            lastPos = transform.position;
            return false;
        }
    }

    private void Summon() //checks whether the keys 1, 2 or 3 are pressed and summons the corresponding summon
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            InstSummon(summons[0]);
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            InstSummon(summons[1]);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            InstSummon(summons[2]);
        }
    }

    private void InstSummon(Summon summon) //instantiates a summon
    {
        if (currentSummons.Count < maxSummons)
        {
            Summon newSummon = Instantiate(summon);
            currentSummons.Add(newSummon);
        }
    }

    private void Die()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    private void LevelUp()
    {

    }

    private void AddPerk(/*Perk perk*/)
    {

    }

    private void GiveOrder()
    {
        if (Input.GetMouseButtonDown(1) && selectedSummons.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                foreach (Summon summon in selectedSummons)
                {
                    summon.agent.SetDestination(hit.point);
                }
               
            }
        }
    }

    private void SelectSummon()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if(!selectedSummons.Contains(hit.collider.gameObject.GetComponentInParent<Summon>()))
                {
                    selectedSummons.Add(hit.collider.gameObject.GetComponentInParent<Summon>());
                    string summonName = hit.collider.gameObject.name;
                    UIManager.Instance.UpdateSummon(hit.collider.gameObject.GetComponentInParent<Summon>());
                }
                
            }
            else
            {
                DeselectAll();
            }

        }

    } //selects a single summon with the mouse left click


    private void DragSelect() //selects multiple summons while dragging the mouse left click
    {

        if (Input.GetMouseButtonDown(0)) //checks if the mouse left click is pressed
        {
            startPos = Input.mousePosition;
            selectionBox = new Rect();
        }

        if (Input.GetMouseButton(0)) //checls if the mouse left click is being held
        {
            endPos = Input.mousePosition;
            DrawVisual();
            CalculateBoxSize();
        }

        if (Input.GetMouseButtonUp(0)) //checks if the mouse left click is released
        {
            SelectMultipleSummons();
            startPos = Vector2.zero;
            endPos = Vector2.zero;
            DrawVisual();
        }
    }

    private void RemoveSummon(Summon deadSummon) 
    {

    }

    private void KillSummon(Summon selectedSummon)
    {

    }

    private void DrawVisual() //draws the canvasÅLs selection box while dragging
    {

        Vector2 boxStart = startPos;
        Vector2 boxEnd = endPos;
        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;

    }

    private void CalculateBoxSize()
    {
        if (Input.mousePosition.x < startPos.x)
        {
            //left drag
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = startPos.x;
        }
        else
        {
            //right drag
            selectionBox.xMin = startPos.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        if (Input.mousePosition.y < startPos.y)
        {
            //down drag
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = startPos.y;
        }
        else
        {
            //up drag
            selectionBox.yMin = startPos.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    } //declares the size of the box that selects the summons while dragging

    private void SelectMultipleSummons() //selects the summons inside the selection box while dragging
    {
        foreach (var summon in currentSummons)
        {
            if (selectionBox.Contains(cam.WorldToScreenPoint(summon.transform.position)))
            {
                if (!selectedSummons.Contains(summon))
                {
                    selectedSummons.Add(summon);
                    //string summonName = summon.transform.GetChild(0).name;
                    UIManager.Instance.UpdateSummon(summon);
                }
            }
        }
    }

    private void DeselectAll()
    {
        selectedSummons.Clear();
        UIManager.Instance.ClearSelectedSummons();
    }

}
