using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Character : MonoBehaviour, IDamagable
{

    float moveSpeed = 2f;
    [SerializeField] float health = 100;
    [SerializeField] float maxHealth = 100;
    int lvl = 5;
    float exp;
    float levelUpExp = 100;
    List<Perks> perksObtained = new List<Perks>();
    float summonCooldown;
    int maxSummons = 5;
    float summonHealthToAdd = 0;
    float castingTime = 2f;
    float castingCoolDown = 3f;

    [SerializeField] bool readyToCast = true;

    [SerializeField] private List<Summon> currentSummons;
    [SerializeField] private List<Summon> selectedSummons;
    [SerializeField] private List<Summon> summons;

    List<Perks> currentPerks = new List<Perks>();
    enum State
    {
        Idle,
        Moving,
        Casting,
        Dead
    }
    [SerializeField] State state = State.Idle;

    Vector3 lastPos;


    private Camera cam;
    [SerializeField] private LayerMask clickable;
    [SerializeField] private LayerMask ground;

    [SerializeField] private RectTransform boxVisual;
    Rect selectionBox;
    Vector2 startPos;
    Vector2 endPos;

    [SerializeField] private CapsuleCollider playerCollider;

    IEnumerator enumerator = null;

    int keyPressed;

    [SerializeField] HealthBars healthBar;

    List<int> levelToUnlockSummon = new List<int> { 0, 3, 5 }; //Stores the level requirements to unlock summons in their respective index

    [SerializeField] ParticleSystem summonCircle;

    public float ColliderRadius { get { return playerCollider.radius; } }
    public float PlayerHealth { get { return health; } set { health = value; } }

    public float PlayerMaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float MaxSummons { get { return maxSummons; } set { maxSummons = (int)value; } }

    public float SummonMaxHealth { get { return summonHealthToAdd; } set { summonHealthToAdd = value; } }

    public float PlayerMoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    public float PlayerCastTime { get { return castingTime; } set { castingTime = value; } }

    public float PlayerCastCoolDown { get { return castingCoolDown; } set { castingCoolDown = value; } }

    private void OnEnable()
    {
        Actions.OnSummonKilled += SummonDestroyed;
        Actions.OnEnemyKilled += CalculateExp;
    }

    private void OnDisable()
    {
        Actions.OnSummonKilled -= SummonDestroyed;
        Actions.OnEnemyKilled -= CalculateExp;
    }

    private void Start()
    {
        cam = Camera.main;
        lastPos = transform.position;
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        DrawVisual();
        UIManager.Instance.UpdatePlayerLevel(lvl);
        UIManager.Instance.UpdatePlayerExp(exp, levelUpExp);

        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        SelectSummon();
        DragSelect();

        GiveOrder();

        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            KillSummon(selectedSummons);
        }

        if (health <= 0)
        {
            state = State.Dead;
        }

        switch (state)
        {
            case State.Idle:

                Move();
                if (currentSummons.Count < maxSummons && readyToCast == true)
                {
                    keyPressed = Summon();
                }

                if (IsMoving())
                {
                    state = State.Moving;
                }

                break;

            case State.Moving:

                Move();
                if (currentSummons.Count < maxSummons && readyToCast == true)
                {
                    keyPressed = Summon();
                }

                if (!IsMoving())
                {
                    state = State.Idle;
                }

                break;

            case State.Casting:

                if (enumerator == null)
                {
                    InstSummon(summons[keyPressed]); //sends the summon in the index which the key was pressed on                    
                }

                break;

            case State.Dead:

                //Game over
                RemoveSummons();
                GameManager.Instance.Lose();

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

    private int Summon() //checks whether the keys 1, 2 or 3 are pressed and returns the corresponding key - 1
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            //InstSummon(summons[0]);
            state = State.Casting;
            return 0;
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            //InstSummon(summons[1]);
            state = State.Casting;
            return 1;
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            //InstSummon(summons[2]);
            state = State.Casting;
            return 2;
        }
        return keyPressed;
    }

    private void InstSummon(Summon summon) //starts the coroutine that instantiates summons after waiting x seconds
    {
        if (enumerator == null)
        {
            if (lvl >= levelToUnlockSummon[keyPressed]) //checks if the level is equal to or greater than the level requirement to summon
            {
                enumerator = BeginCasting(summon);
                StartCoroutine(enumerator);
            }
            else
            {
                Debug.Log("You need to be lvl " + levelToUnlockSummon[keyPressed]);
                state = State.Idle;
            }
        }
    }

    IEnumerator BeginCasting(Summon summon) //waits for x seconds and instantiates a summon
    {
        summonCircle.gameObject.SetActive(true);

        yield return new WaitForSeconds(castingTime);

        Summon newSummon = Instantiate(summon, summonCircle.transform.position, transform.rotation);
        currentSummons.Add(newSummon);

        newSummon.SummonHealth += summonHealthToAdd;
        newSummon.SummonMaxHealth += summonHealthToAdd;

        state = State.Idle;

        enumerator = null;

        StartCoroutine(CastingCoolDownCoroutine());

        summonCircle.gameObject.SetActive(false);
    }

    IEnumerator CastingCoolDownCoroutine()
    {
        readyToCast = false;
        yield return new WaitForSeconds(castingCoolDown);
        readyToCast = true;
    }

    private void Die()
    {
        
    }

    void CalculateExp(Enemies enemyRef)
    {
        exp += enemyRef.EnemyExpGiven;

        UIManager.Instance.UpdatePlayerExp(exp, levelUpExp);

        if (exp >= levelUpExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        lvl++;
        exp -= levelUpExp;
        levelUpExp += 20f;

        UIManager.Instance.UpdatePlayerLevel(lvl);
        UIManager.Instance.UpdatePlayerExp(exp, levelUpExp);

        //Choose perk

        GameManager.Instance.OpenPerkSelectionScreen();

        Actions.OnLevelUp?.Invoke();
    }

    public void AddNewPerk(Perks newPerk)
    {
        currentPerks.Add(newPerk);
    }

    public void ApplyPerks()
    {
        for (int i = 0; i < currentSummons.Count; i++)
        {
            currentSummons[i].SummonMaxHealth += summonHealthToAdd;
            currentSummons[i].SummonHealth += summonHealthToAdd;
        }
    }

    private void GiveOrder()
    {
        if (Input.GetMouseButtonDown(1) && selectedSummons.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                foreach (Summon summon in selectedSummons)
                {
                    //summon.agent.SetDestination(hit.point);
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) //checks if the RayCast hits the ground
                    {
                        summon.DesignateTarget(hit.point); //Sends the Vector3 of the RayCast hit point
                    }
                    else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy")) //Checks if the RayCast hits an enemy
                    {
                        summon.DesignateTarget(hit.collider.gameObject.GetComponent<Enemies>()); //Sends the gameObject of the enemy hit
                    }
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
                if (!selectedSummons.Contains(hit.collider.gameObject.GetComponentInParent<Summon>()))
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

    private void RemoveSummons()
    {
        int selectedSummonsCount = currentSummons.Count;
        for (int i = selectedSummonsCount - 1; i >= 0; i--)
        {
            currentSummons[i].KillSummon();
        }
    }

    private void KillSummon(List<Summon> summonsToKill)
    {
        int selectedSummonsCount = summonsToKill.Count;
        for (int i = selectedSummonsCount - 1; i >= 0; i--)
        {
            UIManager.Instance.ClearSelectedSummon(summonsToKill[i]);

            currentSummons.Remove(summonsToKill[i]);

            summonsToKill[i].KillSummon();
        }
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

    void SummonDestroyed(Summon summonRef)
    {
        currentSummons.Remove(summonRef);
        selectedSummons.Remove(summonRef);

        if (summonRef.isDead == true)
        {
            UIManager.Instance.ClearSelectedSummon(summonRef);
        }

    }

    void TakeDamage(float damage)
    {
        health -= damage;

        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBar.HealthBarUpdate(health / maxHealth);
    }

    public void Damage(float damage)
    {
        TakeDamage(damage);
    }
}
