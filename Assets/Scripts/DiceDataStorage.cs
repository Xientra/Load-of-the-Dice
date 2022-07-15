using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceDataStorage : MonoBehaviour, IPointerClickHandler
{
    public Dice dice;
    private void Start()
    {
        if (dice == null)
        {
            dice = new Dice(4);
        }
    }
    public void SetDice(Dice dice)
    {
        this.dice = dice;
    }

    public Dice GetDice()
    {
        return dice;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.AddDice(dice, this);
        GetComponent<Button>().interactable = false;
    }

    public void EnableButton()
    {
        GetComponent<Button>().interactable = false;
    }
}
