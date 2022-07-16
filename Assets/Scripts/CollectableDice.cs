using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableDice : MonoBehaviour
{
    [SerializeField] int value = 4;
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
        Debug.Log("TES");
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            player.PickUpDice(new Dice(value));
            Destroy(this.gameObject);
        }
    }
}
