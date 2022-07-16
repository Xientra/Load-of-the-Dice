using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
	public int life = 3;
	public int maxLife = 3;

	public void TakeDamage(int amount = 1)
	{
		life -= amount;
		if (life < 0)
		{
			life = 0;
			Die();
		}
	}

	private void Die()
	{
		
	}
}
