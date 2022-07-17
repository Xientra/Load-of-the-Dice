using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
	protected ParticleSystem shineEffect;

	[Range(0, 4)]
	public int rarity = 0;

	protected virtual void Awake()
	{
		shineEffect = GetComponentInChildren<ParticleSystem>();
		ParticleSystem.MainModule mm = shineEffect.main;
		mm.startColor = rarityColors[rarity];
	}


	public static Color[] rarityColors = new Color[]
	{
		new Color(0.25f, 0.25f, 0.25f, 0.6f), // gray / white
		new Color(0, 0.2f, 1f, 0.7843137f), // blue
		new Color(1, 0f, 0.87f, 0.7843137f), // pink
		new Color(1, 0.37f, 0f, 0.7843137f), // gold
		new Color(1, 0f, 0f, 1f), // red
	};

	public void SetColor()
	{
		ParticleSystem.MainModule mm = shineEffect.main;
		mm.startColor = rarityColors[rarity];
	}

    public void SetRarity(int rarity)
    {
        this.rarity = rarity;
        SetColor();
    }

	protected void OnValidate()
	{
		shineEffect = GetComponentInChildren<ParticleSystem>();
		ParticleSystem.MainModule mm = shineEffect.main;
		mm.startColor = rarityColors[rarity];
	}
}
