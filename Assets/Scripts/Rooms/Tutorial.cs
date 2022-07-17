using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Room tutorialRoom;

    public CollectableDice diceToSpawn;

    public Gun gunToSpawn;

    public List<Enemy> enemiesToSpawn;

    public TMPro.TMP_Text label;

    private PlayerLife player;
    private Vector3 originalPlayerPos;

    public enum TutorialSteps { moveWithWASD, pickupGun, pickupDice, clickDiceAndReloadWithR, shootEnemiesWithCorrectNumber, done }
    public TutorialSteps currentStep = TutorialSteps.moveWithWASD;

    void Start()
    {
        label.text = "";
        tutorialRoom.isCleared = false;
        tutorialRoom.checkCleared = false;

        diceToSpawn.gameObject.SetActive(false);
        gunToSpawn.gameObject.SetActive(false);
        foreach (Enemy e in enemiesToSpawn)
            e.gameObject.SetActive(false);
    }

    public void StartTutorial(PlayerLife player)
    {
        this.player = player;
        label.text = "Move with WASD.";
        originalPlayerPos = player.transform.position;
    }

    void Update()
    {
        switch (currentStep)
        {
            case (TutorialSteps.moveWithWASD):
                {
                    if (player != null)
                        if (player.transform.position != originalPlayerPos)
                            NextStep();
                }
                break;
            case (TutorialSteps.pickupGun):
                {
                    if (gunToSpawn.IsEquipped == true)
                        NextStep();
                }
                break;
            case (TutorialSteps.pickupDice):
                {
                    if (diceToSpawn == null)
                        NextStep();
                }
                break;
            case (TutorialSteps.clickDiceAndReloadWithR):
                {
                    if (gunToSpawn.GetLoaded())
                        NextStep();
                }
                break;
            case (TutorialSteps.shootEnemiesWithCorrectNumber):
                {
                    if (enemiesToSpawn.FindAll(e => e != null).Count == 0)
                        NextStep();
                }
                break;
        }
    }

    private void NextStep()
    {
        switch (currentStep)
        {
            case (TutorialSteps.moveWithWASD):
                {
                    currentStep = TutorialSteps.pickupGun;
                    label.text = "Pickup the weapon.";
                    gunToSpawn.gameObject.SetActive(true);
                }
                break;
            case (TutorialSteps.pickupGun):
                {
                    currentStep = TutorialSteps.pickupDice;
                    label.text = "Pickup the dice.";
                    diceToSpawn.gameObject.SetActive(true);
                }
                break;
            case (TutorialSteps.pickupDice):
                {
                    currentStep = TutorialSteps.clickDiceAndReloadWithR;
                    label.text = "Click the dice (or press 1, 2, 3, ...) to load the dice into the weapon. Then press R to roll all of them.";
                }
                break;
            case (TutorialSteps.clickDiceAndReloadWithR):
                {
                    currentStep = TutorialSteps.shootEnemiesWithCorrectNumber;
                    label.text = "Shoot the enemies.\nEnemies only take damage from shots with the same number as they have above their heads.";
                    foreach (Enemy enemy in enemiesToSpawn)
                        enemy.gameObject.SetActive(true);
                }
                break;
            case (TutorialSteps.shootEnemiesWithCorrectNumber):
                {
                    tutorialRoom.isCleared = true;
                    label.text = "Well done, proceed to the next room.";
                }
                break;
        }
    }
}
