using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableDice : PickupItem
{
    [SerializeField] int value = 4;

	private void Awake()
	{
        MapValueToRarity();
        base.Awake();
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("TES");
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.PickUpDice(new Dice(value));
            Destroy(this.gameObject);
        }
    }

    public void SetValue(int value)
    {
        this.value = value;
        MapValueToRarity();
    }

    private void MapValueToRarity()
    {
        if (value >= 0)
            rarity = 0;
        if (value >= 4)
            rarity = 1;
        if (value >= 8)
            rarity = 2;
        if (value >= 12)
            rarity = 3;
        if (value >= 14)
            rarity = 4;
        
    }

    private void OnValidate()
	{
        MapValueToRarity();
        base.OnValidate();
    }
}
