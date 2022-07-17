using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Gun : PickupItem
{
    private BoxCollider2D bc;

    public int maxMagazine = 6;
    public float rateOfFire = 180; //Rounds per minute
    private bool allowFire = true;
    private bool loaded = false;

    private List<Dice> selectedDice = new List<Dice>();
    private List<int> damageRolls = new List<int>();
    public List<Chamber> chambers = new List<Chamber>();

    private bool isEquipped = false;
    private int addCounter = 0;

    public GameObject muzzlePosition;

    public GameObject muzzleEffectPrefab;

    private int chamberCount = 0;


    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        for (int i = 0; i < maxMagazine; i++)
        {
            Chamber chamber = new Chamber();
            chamber.SetMultiplier(UnityEngine.Random.Range(-2, 3));
            chamber.SetAmount(UnityEngine.Random.Range(1, 5));
            if (i == 3) chamber.Ricochet();
            if (i == 4) chamber.Pierce();
            if (i == 5) chamber.Rebound();
            if (i == 6) chamber.ThreeBurstSpread();
            if (i == 1) chamber.ThreeBurstRow();
            chambers.Add(chamber);
        }
    }

	public void AddDiceToMag(Dice dice)
    {
        GameObject UIContainer = GameObject.FindGameObjectWithTag("MagUIContainer");

        if (selectedDice.Count < maxMagazine && !selectedDice.Contains(dice) && !loaded)
        {
            selectedDice.Add(dice);
            UIContainer.transform.GetChild(addCounter).GetChild(0).GetComponent<TMP_Text>().SetText("D" + dice.GetValue());
            addCounter++;

            if (selectedDice.Count >= maxMagazine)
            {
                //disable all UI dice
                GameObject DiceUIContainer = GameObject.FindGameObjectWithTag("DiceUIContainer");
                foreach (Transform child in DiceUIContainer.transform)
                {
                    child.GetComponent<DiceDataStorage>().DisableButton();
                }
            }
        }
    }

    public void Shoot(GameObject bullet, Vector2 relativeMousePos)
    {
        if (damageRolls.Count > 0 && allowFire)
        {
            UpdateMagUI();
            if (chambers[chamberCount].GetThreeBurstSpread())
            {
                Bullet spawnedBullet1 = SpawnBullet(bullet, damageRolls[0], null);
                spawnedBullet1.transform.Rotate(new Vector3(0, 0, 20));

                Bullet spawnedBullet2 = SpawnBullet(bullet, damageRolls[0], null);
                spawnedBullet2.transform.Rotate(new Vector3(0, 0, -20));
            }

            if (chambers[chamberCount].GetThreeBurstRow())
            {
                StartCoroutine(ThreeBurst(bullet, damageRolls[0]));
            }

            // effects
            Instantiate(muzzleEffectPrefab, muzzlePosition.transform.position, Quaternion.identity).transform.right = transform.right;

            SpawnBullet(bullet, damageRolls[0], chambers[chamberCount]);
            damageRolls.RemoveAt(0);
            selectedDice.RemoveAt(0);
            allowFire = false;
            StartCoroutine(RPMLimit());

            //last bullet shot
            if (damageRolls.Count <= 0)
            {
                loaded = false;

                //reenable all UI dice
                GameObject UIContainer = GameObject.FindGameObjectWithTag("DiceUIContainer");
                foreach (Transform child in UIContainer.transform)
                {
                    child.GetComponent<DiceDataStorage>().EnableButton();
                }
            }
            chamberCount++;
        }
    }

    IEnumerator ThreeBurst(GameObject bullet, int damage)
    {
        yield return new WaitForSeconds(0.1f);
        SpawnBullet(bullet, damage, null);

        yield return new WaitForSeconds(0.1f);
        SpawnBullet(bullet, damage, null);
    }

    private Bullet SpawnBullet(GameObject bullet, int damage, Chamber chamber)
    {
        Bullet spawnedBullet = Instantiate(bullet, muzzlePosition.transform.position, Quaternion.identity).GetComponent<Bullet>();
        spawnedBullet.transform.up = muzzlePosition.transform.right;
        spawnedBullet.SetEffects(chamber != null ? chamber : new Chamber());
        spawnedBullet.SetDamage(damage);
        return spawnedBullet;
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
        if (!loaded && selectedDice.Count > 0)
        {
            int counter = 0;

            foreach (Dice d in selectedDice)
            {
                StartCoroutine(RollTheDice(counter, d));
                counter++;
            }

            loaded = true;
            addCounter = 0;
            chamberCount = 0;

            //disable all UI dice
            GameObject UIContainer = GameObject.FindGameObjectWithTag("DiceUIContainer");
            foreach (Transform child in UIContainer.transform)
            {
                child.GetComponent<DiceDataStorage>().DisableButton();
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
        shineEffect.gameObject.SetActive(!isEquipped);

        if (!isEquipped)
        {
            bc.enabled = false;
            StartCoroutine(IFrames());
        }
        else
            bc.enabled = false;
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

    public bool GetLoaded()
    {
        return loaded;
    }

    [System.Serializable]
    public class Chamber
    {
        bool ricochet = false;
        bool pierce = false;
        bool rebound = false;
        bool threeBurstSpread = false;
        bool threeBurstRow = false;
        int multiplier;
        int amount;

        public void SetAmount(int i) { amount = i;  }
        public int GetAmount() { return amount; }

        public void SetMultiplier(int i) { multiplier = i; }
        public int GetMultiplier() { return multiplier; }

        public void Ricochet() { ricochet = true; }
        public void Pierce() { pierce = true; }
        public void Rebound() { rebound = true; }
        public void ThreeBurstSpread() { threeBurstSpread = true; }
        public void ThreeBurstRow() { threeBurstRow = true; }

        public bool GetRicochet() { return ricochet; }
        public bool GetPierce() { return pierce; }
        public bool GetRebound() { return rebound; }
        public bool GetThreeBurstSpread() { return threeBurstSpread; }
        public bool GetThreeBurstRow() { return threeBurstRow; }

    }

}
