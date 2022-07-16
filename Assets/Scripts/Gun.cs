using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Gun : MonoBehaviour
{
    private BoxCollider2D bc;

    public int maxMagazine = 6;
    public float rateOfFire = 180; //Rounds per minute
    private bool allowFire = true;
    private bool loaded = false;

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
        if (selectedDice.Count < maxMagazine && !selectedDice.Contains(dice) && !loaded)
        {
            selectedDice.Add(dice);
        }
    }

    public void Shoot(GameObject bullet, Vector2 relativeMousePos)
    {
        if (damageRolls.Count > 0 && allowFire)
        {
            UpdateMagUI();

            Bullet spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
            spawnedBullet.transform.up = relativeMousePos;
            spawnedBullet.SetDamage(damageRolls[0]);
            damageRolls.RemoveAt(0);
            selectedDice.RemoveAt(0);
            allowFire = false;
            StartCoroutine(RPMLimit());

            if (damageRolls.Count <= 0)
            {
                loaded = false;
            }
        }
    }

    private void UpdateMagUI()
    {
        GameObject UIContainer = GameObject.FindGameObjectWithTag("MagUIContainer");
        foreach (Transform child in UIContainer.transform)
        {
            if (child.GetSiblingIndex() != UIContainer.transform.childCount - 1)
            {
                child.GetChild(0).GetComponent<TMP_Text>().SetText(UIContainer.transform.GetChild(child.GetSiblingIndex() + 1).GetChild(0).GetComponent<TMP_Text>().text);
            } else
            {
                child.GetChild(0).GetComponent<TMP_Text>().SetText("");
            }
        }
    }

    IEnumerator RPMLimit()
    {
        yield return new WaitForSeconds(60f / rateOfFire);
        allowFire = true;
    }

    public void ReloadGun()
    {
        if (!loaded)
        {
            int counter = 0;

            foreach (Dice d in selectedDice)
            {
                StartCoroutine(RollTheDice(counter, d));
                counter++;
            }

            if (selectedDice.Count > 0)
            {
                loaded = true;
            }
        }
        
        //rollReady = false;
        //rollDuration = 0;
    }

    private IEnumerator RollTheDice(int counter, Dice d)
    {
        GameObject UIContainer = GameObject.FindGameObjectWithTag("MagUIContainer");

        int randomDiceSide = 0;
        int finalSide = 0;

        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = UnityEngine.Random.Range(1, d.GetValue()+1);

            UIContainer.transform.GetChild(counter).GetChild(0).GetComponent<TMP_Text>().SetText("" + randomDiceSide);

            yield return new WaitForSeconds(0.05f);
        }
        finalSide = d.ThrowDice();
        UIContainer.transform.GetChild(counter).GetChild(0).GetComponent<TMP_Text>().SetText("" + finalSide);

        damageRolls.Add(finalSide);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
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

    public int GetMagSize()
    {
        return maxMagazine;
    }

}
