using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
	public GameObject graphics;

	public GameObject crosshair;

	private GameObject player;

	public TMP_Text text;

	[Space(5)]

	public GameObject bulletPrefab;

	public GameObject deathEffect;

	[Header("Numbers:")]

	public int maxNumber = 6;
	public int difficulty = 4;

	[Space(5)]

	public EnemyLife life;

	[Space(5)]

	public List<int> allNumbers;

	public string DEBUG_numbersString;

	private void OnValidate()
	{
		allNumbers = life.GetAllNumbers();
		DEBUG_numbersString = GetNumbersString();
	}

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").gameObject;
		Spawn();
	}

	// Update is called once per frame
	private void Update()
	{
		crosshair.transform.right = player.transform.position - transform.position;

		text.text = GetNumbersString();
		DEBUG_numbersString = GetNumbersString();

		float xDir = Mathf.Sign(player.transform.position.x - transform.position.x);
		graphics.transform.localScale = new Vector3(xDir, graphics.transform.localScale.y, graphics.transform.localScale.z);
	}

	public void Spawn()
	{
		int normalNumCount = 4;

		for (int i = 0; i <= Mathf.Max(normalNumCount - difficulty, 1); i++)
		{
			int effect = Random.Range(0, i == 0 ? 2 : 3); // ensure 0 or 1 once
			Debug.Log("effect: " + effect);
			switch (effect)
			{
				// add random numbers (add LESS if difficulty is HIGHER)
				case 0:
					for (int x = 0; x < Mathf.Max(normalNumCount - i, 1); x++)
						life.numberList.Add(Random.Range(1, maxNumber + 1));
					break;
				// add range:
				case 1:
					int rangeLength = Random.Range(1, normalNumCount - difficulty);
					int rangeStart = Random.Range(1, maxNumber + 1 - rangeLength);
					life.numberRange = new Vector2Int(rangeStart, rangeStart + rangeLength);
					break;
				// add even or odd
				case 2:
					if (Random.Range(0, 1) == 0)
					{
						life.onlyEven = true;
						life.onlyOdd = false;
					}
					else
					{
						life.onlyOdd = true;
						life.onlyEven = false;
					}
					break;
			}
		}

		allNumbers = life.GetAllNumbers();
	}

	private string GetNumbersString()
	{
		List<int> allNum = allNumbers;

		string result = "";

		for (int i = 0; i < allNum.Count; i++)
		{
			int c = i;
			// count c up while the next list entry is just +1 the previous
			while (c + 1 < allNum.Count && allNum[c] + 1 == allNum[c + 1])
				c++;

			if (c == i)
				result += "" + allNum[i];
			else
				result += allNum[i] + "-" + allNum[c];

			i = c;
			if (i != allNum.Count - 1)
				result += ", ";
		}

		return result;
	}

	public bool Hit(int number)
	{
		bool numberHits = false;

		if (allNumbers.Contains(number))
			numberHits = true;

		if (numberHits)
			Die();
		return numberHits;
	}

	private void Die()
	{
		Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(this.gameObject);
	}

	private void Shoot()
	{
		Instantiate(bulletPrefab, crosshair.transform.position, Quaternion.identity).transform.up = player.transform.position - transform.position;
	}

	[System.Serializable]
	public class EnemyLife
	{
		public List<int> numberList = new List<int>();
		[Tooltip("includes both sides in the range. So 1-4 equals { 1, 2, 3, 4 }.")]
		public Vector2Int numberRange = new Vector2Int(0, 0);

		public bool onlyOdd = false;
		public bool onlyEven = false;

		public List<int> GetAllNumbers()
		{
			List<int> allNumbers = new List<int>();

			allNumbers.AddRange(numberList);
			for (int i = numberRange.x; i <= numberRange.y; i++)
				if (i > 0)
					allNumbers.Add(i);

			allNumbers = new List<int>(new HashSet<int>(allNumbers)); // remove duplicates
			allNumbers.RemoveAll(n => ((onlyEven && (n % 2) == 1) || (onlyOdd && (n % 2) == 0)));
			allNumbers.Sort((n1, n2) => n1 - n2);

			return allNumbers;
		}
	}
}
