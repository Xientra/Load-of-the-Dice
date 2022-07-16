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
		Instantiate(dropLootEffectPrefab, transform.position, Quaternion.identity);

		for (int i = 0; i < numGuns; i++)
		{
			Gun newGun = Instantiate(gunPrefabs[Random.Range(0, gunPrefabs.Length)].gameObject, transform.position, Quaternion.identity).GetComponent<Gun>();
			
			// generate weapon

			StartCoroutine(AnimateItem(newGun.gameObject));
		}

		for (int i = 0; i < numDice; i++)
		{
			CollectableDice newDice = Instantiate(dicePrefab.gameObject, transform.position, Quaternion.identity).GetComponent<CollectableDice>();

			// generate dice cooler
			newDice.SetValue(Random.Range(1, 20 + 1));

			StartCoroutine(AnimateItem(newDice.gameObject));
		}

		Destroy(this.gameObject, throwTimeMinMax.y);
		GetComponent<Collider2D>().enabled = false;
		GetComponentInChildren<Renderer>().enabled = false;
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
