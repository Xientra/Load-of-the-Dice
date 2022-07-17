using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootRift : MonoBehaviour
{
    public GameObject dropLootEffectPrefab;

    public int numGuns = 2;
    public int numDice = 3;

    public CollectableDice dicePrefab;

    public Gun[] gunPrefabs;

    public Vector2 throwDistanceMinMax = new Vector2(2f, 5f);
    public Vector2 throwTimeMinMax = new Vector2(2f, 5f);

    public AnimationCurve throwSpeedCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DropLoot();
    }

    public void DropLoot()
    {
        Destroy(this.gameObject, throwTimeMinMax.y);
        GetComponent<Collider2D>().enabled = false;
        GetComponentInChildren<Renderer>().enabled = false;

        Instantiate(dropLootEffectPrefab, transform.position, Quaternion.identity);

        for (int i = 0; i < numGuns; i++)
        {
            Gun newGun = Instantiate(gunPrefabs[Random.Range(0, gunPrefabs.Length)].gameObject, transform.position, Quaternion.identity).GetComponent<Gun>();

            // generate weapon
            int value = Random.Range(0, 100);
            SetGunRoll(newGun, value);
            newGun.SetRarity(value / 20);


            StartCoroutine(AnimateItem(newGun.gameObject));
        }

        for (int i = 0; i < numDice; i++)
        {
            CollectableDice newDice = Instantiate(dicePrefab.gameObject, transform.position, Quaternion.identity).GetComponent<CollectableDice>();

            // generate dice cooler
            newDice.SetValue(Random.Range(1, 20 + 1));

            StartCoroutine(AnimateItem(newDice.gameObject));
        }
    }

    private void SetGunRoll(Gun gun, int rand)
    {
        if (rand < LOOOOT(10))
        {
            //kein chamber
            for (int i = 0; i < gun.GetMagSize(); i++)
            {
                gun.AddChamber(new Gun.Chamber());
            }
        }
        else if (rand < LOOOOT(20))
        {
            //ein chamber
            for (int i = 0; i < gun.GetMagSize(); i++)
            {
                gun.AddChamber(new Gun.Chamber());
            }
            Gun.Chamber chamber = new Gun.Chamber();
            chamber.SetRandom();
            gun.ChangeChamber(chamber, Random.Range(0, gun.GetMagSize()));
        }
        else if (rand < LOOOOT(30))
        {
            // zwei chamber
            for (int i = 0; i < gun.GetMagSize(); i++)
            {
                gun.AddChamber(new Gun.Chamber());
            }
            Gun.Chamber chamber = new Gun.Chamber();
            chamber.SetRandom();
            Gun.Chamber chamber1 = new Gun.Chamber();
            chamber1.SetRandom();
            List<int> random = GenerateRandomList(gun.GetMagSize());
            gun.ChangeChamber(chamber, random[0]);
            if (random.Count > 1)
                gun.ChangeChamber(chamber1, random[1]);
        }
        else if (rand < LOOOOT(40))
        {
            //drei chamber
            for (int i = 0; i < gun.GetMagSize(); i++)
            {
                gun.AddChamber(new Gun.Chamber());
            }
            Gun.Chamber chamber = new Gun.Chamber();
            chamber.SetRandom();
            Gun.Chamber chamber1 = new Gun.Chamber();
            chamber1.SetRandom();
            Gun.Chamber chamber2 = new Gun.Chamber();
            chamber2.SetRandom();
            List<int> random = GenerateRandomList(gun.GetMagSize());
            gun.ChangeChamber(chamber, random[0]);
            if (random.Count > 1)
            {
                gun.ChangeChamber(chamber1, random[1]);
                gun.ChangeChamber(chamber2, random[2]);
            }
        }
        else if (rand < LOOOOT(50))
        {
            //4 chamber
            for (int i = 0; i < gun.GetMagSize(); i++)
            {
                gun.AddChamber(new Gun.Chamber());
            }
            Gun.Chamber chamber = new Gun.Chamber();
            chamber.SetRandom();
            Gun.Chamber chamber1 = new Gun.Chamber();
            chamber1.SetRandom();
            Gun.Chamber chamber2 = new Gun.Chamber();
            chamber2.SetRandom();
            Gun.Chamber chamber3 = new Gun.Chamber();
            chamber3.SetRandom();
            List<int> random = GenerateRandomList(gun.GetMagSize());
            gun.ChangeChamber(chamber, random[0]);
            if (random.Count > 1)
            {
                gun.ChangeChamber(chamber1, random[1]);
                gun.ChangeChamber(chamber2, random[2]);
                gun.ChangeChamber(chamber3, random[3]);
            }
        }
        else if (rand < LOOOOT(60))
        {
            //5 chamber
            for (int i = 0; i < gun.GetMagSize(); i++)
            {
                gun.AddChamber(new Gun.Chamber());
            }
            Gun.Chamber chamber = new Gun.Chamber();
            chamber.SetRandom();
            Gun.Chamber chamber1 = new Gun.Chamber();
            chamber1.SetRandom();
            Gun.Chamber chamber2 = new Gun.Chamber();
            chamber2.SetRandom();
            Gun.Chamber chamber3 = new Gun.Chamber();
            chamber3.SetRandom();
            Gun.Chamber chamber4 = new Gun.Chamber();
            chamber4.SetRandom();
            List<int> random = GenerateRandomList(gun.GetMagSize());
            gun.ChangeChamber(chamber, random[0]);
            if (random.Count > 1)
            {
                gun.ChangeChamber(chamber1, random[1]);
                gun.ChangeChamber(chamber2, random[2]);
                gun.ChangeChamber(chamber3, random[3]);
            }
            if (random.Count > 4)
            {
                gun.ChangeChamber(chamber4, random[4]);
            }

        }
        else if (rand < LOOOOT(70))
        {
            //6 chamber
            for (int i = 0; i < gun.GetMagSize(); i++)
            {
                gun.AddChamber(new Gun.Chamber());
            }
            Gun.Chamber chamber = new Gun.Chamber();
            chamber.SetRandom();
            Gun.Chamber chamber1 = new Gun.Chamber();
            chamber1.SetRandom();
            Gun.Chamber chamber2 = new Gun.Chamber();
            chamber2.SetRandom();
            Gun.Chamber chamber3 = new Gun.Chamber();
            chamber3.SetRandom();
            Gun.Chamber chamber4 = new Gun.Chamber();
            chamber4.SetRandom();
            Gun.Chamber chamber5 = new Gun.Chamber();
            chamber5.SetRandom();
            List<int> random = GenerateRandomList(gun.GetMagSize());
            gun.ChangeChamber(chamber, random[0]);
            if (random.Count > 1)
            {
                gun.ChangeChamber(chamber1, random[1]);
                gun.ChangeChamber(chamber2, random[2]);
                gun.ChangeChamber(chamber3, random[3]);
            }
            if (random.Count > 4)
            {
                gun.ChangeChamber(chamber4, random[4]);
                gun.ChangeChamber(chamber5, random[5]);
            }
        }
        else if (rand < LOOOOT(80))
        {
            //5 chamber
            for (int i = 0; i < gun.GetMagSize(); i++)
            {
                gun.AddChamber(new Gun.Chamber());
            }
            Gun.Chamber chamber = new Gun.Chamber();
            chamber.SetRandom();
            Gun.Chamber chamber1 = new Gun.Chamber();
            chamber1.SetRandom();
            Gun.Chamber chamber2 = new Gun.Chamber();
            chamber2.SetRandom();
            Gun.Chamber chamber3 = new Gun.Chamber();
            chamber3.SetRandom();
            Gun.Chamber chamber4 = new Gun.Chamber();
            chamber4.SetRandom();
            Gun.Chamber chamber5 = new Gun.Chamber();
            chamber5.SetRandom();
            Gun.Chamber chamber6 = new Gun.Chamber();
            chamber6.SetRandom();
            List<int> random = GenerateRandomList(gun.GetMagSize());
            gun.ChangeChamber(chamber, random[0]);
            if (random.Count > 1)
            {
                gun.ChangeChamber(chamber1, random[1]);
                gun.ChangeChamber(chamber2, random[2]);
                gun.ChangeChamber(chamber3, random[3]);
            }
            if (random.Count > 4)
            {
                gun.ChangeChamber(chamber4, random[4]);
                gun.ChangeChamber(chamber4, random[5]);
                gun.ChangeChamber(chamber4, random[6]);
            }
        }
        else
        {
            //5 chamber
            for (int i = 0; i < gun.GetMagSize(); i++)
            {
                gun.AddChamber(new Gun.Chamber());
            }
            Gun.Chamber chamber = new Gun.Chamber();
            chamber.SetRandom();
            Gun.Chamber chamber1 = new Gun.Chamber();
            chamber1.SetRandom();
            Gun.Chamber chamber2 = new Gun.Chamber();
            chamber2.SetRandom();
            Gun.Chamber chamber3 = new Gun.Chamber();
            chamber3.SetRandom();
            Gun.Chamber chamber4 = new Gun.Chamber();
            chamber4.SetRandom();
            Gun.Chamber chamber5 = new Gun.Chamber();
            chamber5.SetRandom();
            Gun.Chamber chamber6 = new Gun.Chamber();
            chamber6.SetRandom();
            Gun.Chamber chamber7 = new Gun.Chamber();
            chamber7.SetRandom();
            List<int> random = GenerateRandomList(gun.GetMagSize());
            gun.ChangeChamber(chamber, random[0]);
            if (random.Count > 1)
            {
                gun.ChangeChamber(chamber1, random[1]);
                gun.ChangeChamber(chamber2, random[2]);
                gun.ChangeChamber(chamber3, random[3]);
            }
            if (random.Count > 4)
            {
                gun.ChangeChamber(chamber4, random[4]);
                gun.ChangeChamber(chamber5, random[5]);
                gun.ChangeChamber(chamber6, random[6]);
            }
            if (random.Count > 6)
            {
                gun.ChangeChamber(chamber6, random[7]);
            }
        }
    }

    private float LOOOOT(int x)
    {
        return 100 - 100 * Mathf.Exp(-0.05f * x);
    }

    public List<int> GenerateRandomList(int maxNum)
    {
        List<int> uniqueNumbers = new List<int>();
        List<int> finishedNumbers = new List<int>();
        for (int i = 0; i < maxNum; i++)
        {
            uniqueNumbers.Add(i);
        }
        for (int i = 0; i < maxNum; i++)
        {
            int ranNum = uniqueNumbers[Random.Range(0, uniqueNumbers.Count)];
            finishedNumbers.Add(ranNum);
            uniqueNumbers.Remove(ranNum);
        }
        return finishedNumbers;
    }

    private IEnumerator AnimateItem(GameObject item)
    {
        float distance = Random.Range(throwDistanceMinMax.x, throwDistanceMinMax.y);
        Vector3 direction = Random.insideUnitCircle.normalized;

        item.GetComponent<Collider2D>().enabled = false;

        float time = Random.Range(throwTimeMinMax.x, throwTimeMinMax.y);
        float t = 0;
        while (t < time)
        {
            if (item == null)
                break;

            item.transform.position = Vector3.Lerp(transform.position, transform.position + distance * direction, throwSpeedCurve.Evaluate(t / time));
            t += Time.deltaTime;
            yield return null;
        }

        item.GetComponent<Collider2D>().enabled = true;
    }
}
