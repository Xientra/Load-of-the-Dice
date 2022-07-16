using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice
{
    private int value;

    public Dice(int value)
    {
        this.value = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int ThrowDice()
    {
        int rand = Random.Range(1, value);
        Debug.Log(rand);
        return rand;
    }
}
