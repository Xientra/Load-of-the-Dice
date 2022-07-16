using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private BoxCollider2D bc;

    public int maxMagazine = 6;
    public float rateOfFire = 180; //Rounds per minute
    private bool allowFire = true;

    private List<Dice> selectedDice = new List<Dice>();
    private List<int> damageRolls = new List<int>();

    private bool isEquipped = false;

    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
    }

    public void AddDiceToMag(Dice dice)
    {
        Debug.Log(dice);
        if (selectedDice.Count < maxMagazine && !selectedDice.Contains(dice))
        {
            selectedDice.Add(dice);
        }
    }

    public void Shoot(GameObject bullet, Vector2 relativeMousePos)
    {
        if (damageRolls.Count > 0 && allowFire)
        {
            Bullet spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
            spawnedBullet.transform.up = relativeMousePos;
            spawnedBullet.SetDamage(damageRolls[0]);
            damageRolls.RemoveAt(0);
            selectedDice.RemoveAt(0);
            allowFire = false;
            StartCoroutine(RPMLimit());
        }
    }

    IEnumerator RPMLimit()
    {
        yield return new WaitForSeconds(60f / rateOfFire);
        allowFire = true;
    }

    public void ReloadGun()
    {
        foreach (Dice d in selectedDice)
        {
            damageRolls.Add(d.ThrowDice());
        }
        //rollReady = false;
        //rollDuration = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("HIU");
            PlayerController player = collision.GetComponent<PlayerController>();
            player.EquipGun(this.gameObject);
        }
    }

    public void SetEquiped(bool equipped)
    {
        this.isEquipped = equipped;
        if (!isEquipped)
        {
            bc.enabled = false;
            StartCoroutine(IFrames());
        }
    }

    IEnumerator IFrames()
    {
        yield return new WaitForSeconds(1f);
        bc.enabled = true;
    }

}
