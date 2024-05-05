using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    private void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {        
        
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
           
    private void Move()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);

        transform.position += (moveDirection * moveSpeed) * Time.deltaTime;      
    }

    private bool IsMoving()
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

    private void Summon()
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

    private void InstSummon(Summon summon)
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

    private void GiveOrder(Summon selectedSummons)
    {

    }

    private void SelectSummon()
    {

    }

    private void RemoveSummon(Summon deadSummon) 
    {

    }

    private void KillSummon(Summon selectedSummon)
    {

    }

}
