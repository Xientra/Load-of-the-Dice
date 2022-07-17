using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gun : PickupItem
{
    private BoxCollider2D bc;

    public int maxMagazine = 6;
    public float rateOfFire = 180; //Rounds per minute
    private bool allowFire = true;
    private bool loaded = false;

    [Space(5)]
    public List<Sprite> effectSprites;

    [Space(5)]

    private List<Dice> selectedDice = new List<Dice>();
    private List<int> damageRolls = new List<int>();
    public List<Chamber> chambers = new List<Chamber>();

    private bool isEquipped = false;
    public bool IsEquipped { get => isEquipped; }

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

    public void AddChamber(Chamber chamber)
    {
        chambers.Add(chamber);
    }

    public void ChangeChamber(Chamber chamber, int index)
    {
        chambers[index] = chamber;
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
        if (damageRolls.Count > 0 && allowFire && !IsMouseOverUI())
        {
            UpdateMagUI();
            if (chambers[chamberCount].GetThreeBurstSpread())
            {
                Bullet spawnedBullet1 = SpawnBullet(bullet, AfterEffectDamage(chambers[chamberCount], damageRolls[0]), null);
                spawnedBullet1.transform.Rotate(new Vector3(0, 0, 20));

                Bullet spawnedBullet2 = SpawnBullet(bullet, AfterEffectDamage(chambers[chamberCount], damageRolls[0]), null);
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

                //reset mag effects
                SetupMagUI();
            }
            chamberCount++;
        }
    }

    IEnumerator ThreeBurst(GameObject bullet, int damage)
    {
        yield return new WaitForSeconds(0.1f);
        SpawnBullet(bullet, damage + 1, null);

        yield return new WaitForSeconds(0.1f);
        SpawnBullet(bullet, damage + 2, null);
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
                child.GetChild(1).GetChild(0).GetComponent<Image>().sprite = UIContainer.transform.GetChild(child.GetSiblingIndex() + 1).GetChild(1).GetChild(0).GetComponent<Image>().sprite;
            }
            else
            {
                child.GetChild(0).GetComponent<TMP_Text>().SetText("");
                child.GetChild(1).GetChild(0).GetComponent<Image>().sprite = effectSprites[18];
            }
        }
    }

    private void SetupMagUI()
    {
        GameObject UIContainer = GameObject.FindGameObjectWithTag("MagUIContainer");
        foreach (Transform child in UIContainer.transform)
        {
            if (child.GetSiblingIndex() < chambers.Count)
            {
                child.GetChild(0).GetComponent<TMP_Text>().SetText("");
                Sprite sprite = ChamberToString(chambers[child.GetSiblingIndex()]);
                child.GetChild(1).GetChild(0).GetComponent<Image>().sprite = sprite;
                Debug.Log(child.GetChild(1).GetChild(0).name);
            }
        }
    }

    private Sprite ChamberToString(Chamber chamber)
    {
        int mult = chamber.GetMultiplier();
        if (chamber.GetRicochet())
        {
            if (mult == 1) return effectSprites[0];
            if (mult == -1) return effectSprites[1];
            if (mult == -2) return effectSprites[2];
            if (mult == 2) return effectSprites[3];
        }

        if (chamber.GetThreeBurstRow())
        {
            if (mult == 1) return effectSprites[4];
            if (mult == -1) return effectSprites[5];
        }

        if (chamber.GetPierce())
        {
            if (mult == 1) return effectSprites[6];
            if (mult == -1) return effectSprites[7];
            if (mult == -2) return effectSprites[8];
            if (mult == 2) return effectSprites[9];
        }

        if (chamber.GetRebound())
        {
            if (mult == 1) return effectSprites[10];
            if (mult == -1) return effectSprites[11];
            if (mult == -2) return effectSprites[12];
            if (mult == 2) return effectSprites[13];
        }

        if (chamber.GetThreeBurstSpread())
        {
            if (mult == 1) return effectSprites[14];
            if (mult == -1) return effectSprites[15];
            if (mult == -2) return effectSprites[16];
            if (mult == 2) return effectSprites[17];
        }
        return effectSprites[18];
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
            randomDiceSide = UnityEngine.Random.Range(1, d.GetValue() + 1);

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
        {
            bc.enabled = false;
            SetupMagUI();
        }
    }

    IEnumerator IFrames()
    {
        yield return new WaitForSeconds(1f);
        bc.enabled = true;
    }

    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public int GetMagSize()
    {
        return maxMagazine;
    }

    public bool GetLoaded()
    {
        return loaded;
    }

    private int AfterEffectDamage(Chamber effects, int damage)
    {
        int mult = effects.GetMultiplier();
        return Mathf.Abs(mult) == 1 ? damage + effects.GetMultiplier() : (Mathf.Sign(mult) == 1 ? damage * 2 : damage / 2);
    }

    [System.Serializable]
    public class Chamber
    {
        bool ricochet = false;
        bool pierce = false;
        bool rebound = false;
        bool threeBurstSpread = false;
        bool threeBurstRow = false;
        bool noEffect = true;
        int multiplier;
        int amount;

        public void SetAmount(int i) { amount = i; }
        public int GetAmount() { return amount; }
        public void SetRandom()
        {
            int rand = UnityEngine.Random.Range(0, 5);
            if (rand == 0) Ricochet();
            if (rand == 1) Pierce();
            if (rand == 2) Rebound();
            if (rand == 3) ThreeBurstRow();
            if (rand == 4) ThreeBurstSpread();
            rand = UnityEngine.Random.Range(0, 5);
            if (rand == 0) multiplier = -2;
            if (rand == 1) multiplier = 2;
            if (rand == 2) multiplier = -1;
            if (rand == 3) multiplier = 1;

            if (threeBurstRow)
            {
                rand = UnityEngine.Random.Range(0, 2);
                if (rand == 0) multiplier = -1;
                if (rand == 1) multiplier = 1;
            }
        }

        public void SetMultiplier(int i) { multiplier = i; }
        public int GetMultiplier() { return multiplier; }

        public void Ricochet()
        {
            ricochet = true;
            noEffect = false;
        }
        public void Pierce()
        {
            pierce = true;
            noEffect = false;
        }
        public void Rebound()
        {
            rebound = true;
            noEffect = false;
        }
        public void ThreeBurstSpread()
        {
            threeBurstSpread = true;
            noEffect = false;
        }
        public void ThreeBurstRow()
        {
            threeBurstRow = true;
            noEffect = false;
        }

        public bool GetRicochet() { return ricochet; }
        public bool GetPierce() { return pierce; }
        public bool GetRebound() { return rebound; }
        public bool GetThreeBurstSpread() { return threeBurstSpread; }
        public bool GetThreeBurstRow() { return threeBurstRow; }
        public bool GetNoEffect() { return noEffect; }

    }

}
