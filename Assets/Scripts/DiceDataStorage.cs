using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceDataStorage : MonoBehaviour, IPointerClickHandler
{
    public Dice dice;
    private bool active = true;
    private bool disabled = false;
    private float cooldown = 1f;
    private float currentCD = 0f;

    public Image childImage;
    public Image childImageLoad;
    public Image disabledOverlay;
    public List<Sprite> diceImages = new List<Sprite>();

    private void Start()
    {

    }

    private void Update()
    {
        if (!active)
        {
            currentCD += Time.deltaTime;
            childImage.fillAmount = currentCD / cooldown;
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
        cooldown = dice.GetValue() / 2;
        childImageLoad.sprite = diceImages[dice.GetValue() - 1];
        childImage.sprite = diceImages[dice.GetValue() - 1];
    }

    public Dice GetDice()
    {
        return this.dice;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (active && !disabled)
        {
            PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            Gun gun = player.GetEquippedGun();
            gun.AddDiceToMag(this.dice);
            active = false;
            DisableButton();
            transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        }
    }



    public void EnableButton()
    {
        //GetComponent<Button>().interactable = true;
        disabledOverlay.enabled = false;
        disabled = false;
    }

    public void DisableButton()
    {
        //GetComponent<Button>().interactable = false;
        disabledOverlay.enabled = true;
        disabled = true;
    }
}
