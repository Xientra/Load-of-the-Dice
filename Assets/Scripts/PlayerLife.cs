using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLife : MonoBehaviour
{
	public int life = 3;
	public int maxLife = 3;

	public event EventHandler OnDeath;

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
		OnDeath?.Invoke(this, EventArgs.Empty);
	}
}
