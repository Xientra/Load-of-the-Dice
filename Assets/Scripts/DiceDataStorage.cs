using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceDataStorage : MonoBehaviour, IPointerClickHandler
{
    public Dice dice;
    private bool active = true;
    private float cooldown = 1f;
    private float currentCD = 0f;

    private void Start()
    {

    }

    private void Update()
    {
        if (!active)
        {
            currentCD += Time.deltaTime;
            transform.GetChild(0).GetComponent<Image>().fillAmount = currentCD / cooldown;
            if (currentCD >= cooldown)
            {
                active = true;
                currentCD = 0;
            }
        }
    }

    public void SetDice(Dice dice)
    {
        this.dice = dice;
        Debug.Log(this.dice);
    }

    public Dice GetDice()
    {
        return this.dice;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (active)
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            Gun gun = player.GetEquippedGun();
            gun.AddDiceToMag(this.dice);
            active = false;
            transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        }
        //GetComponent<Button>().interactable = false;
    }

    public void EnableButton()
    {
        //GetComponent<Button>().interactable = true;
    }
}
