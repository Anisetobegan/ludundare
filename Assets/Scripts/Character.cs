using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{

    float moveSpeed = 0.005f;
    float health;
    int lvl;
    float exp;
    //Perks perk;
    float summonCooldown;
    int maxSummons = 5;
    public List<Summon> currentSummons;
    bool state = true;
    [SerializeField] private List<Summon> summons;
    enum State
    {
        Idle,
        Moving,
        Casting,
        Dead
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            Summon(summons[0]);            
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            Summon(summons[1]);
        }

        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            Summon(summons[2]);
        }
    }

       

    private void Move()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xDirection, 0.0f, zDirection);

        transform.position += moveDirection * moveSpeed;
    }

    private void Summon(Summon summon)
    {
        if (currentSummons.Count < maxSummons)
        {
            Instantiate(summon);
            currentSummons.Add(summon);
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
