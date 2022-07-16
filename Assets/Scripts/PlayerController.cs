using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource audioSource;
    public PlayerControls playerControls;
    public GameObject bullet;
    public GameObject diceButton;
    public GameObject currentGun;
    public GameObject sprite;
    private Gun currentEquippedGun;
    private Vector2 relativeMousePos;
    [SerializeField] float moveSpeed = 5;
    private float rollDurationCD = 2f;
    private float rollDuration = 0;
    private int maxMagazine = 6;
    bool rollReady = true;
    private List<Dice> dices = new List<Dice>();
    private List<Dice> selectedDice = new List<Dice>();
    private List<int> damageRolls = new List<int>();
    private List<DiceDataStorage> uiButtons = new List<DiceDataStorage>();


    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Move.ReadValue<Vector2>();
        playerControls.Player.Fire.performed += _ => OnFire();
        playerControls.Player.Reload.performed += _ => OnReload();
        playerControls.Player.DiceOne.performed += _ => Button(1);
        playerControls.Player.DiceTwo.performed += _ => Button(2);
        playerControls.Player.DiceThree.performed += _ => Button(3);
        playerControls.Player.DiceFour.performed += _ => Button(4);
        playerControls.Player.DiceFive.performed += _ => Button(5);
        playerControls.Player.DiceSix.performed += _ => Button(6);
        playerControls.Player.DiceSeven.performed += _ => Button(7);
        playerControls.Player.DiceEight.performed += _ => Button(8);
        playerControls.Player.DiceNine.performed += _ => Button(9);
        playerControls.Player.DiceTen.performed += _ => Button(10);
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        currentEquippedGun = currentGun.transform.GetChild(0).GetComponent<Gun>();
    }

    private void Button(int v)
    {
        if (uiButtons.Count >= v)
        {
            uiButtons[v - 1].OnPointerClick(null);
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        dices.Add(new Dice(4));
    }

    void Update()
    {
        Move();

        //get current mouse direction relative to the player
        Vector3 mousePosition = playerControls.Player.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        relativeMousePos = mousePosition - transform.position;

        currentGun.transform.right = relativeMousePos;
        Transform gun = currentGun.transform.GetChild(0);
        gun.localScale = new Vector3(gun.localScale.x, Mathf.Sign(relativeMousePos.x), gun.localScale.z);

        if (!rollReady)
        {
            rollDuration += Time.deltaTime;
            if (rollDuration >= rollDurationCD)
            {
                rollReady = true;
            }
        }
    }

    private void Move()
    {
        rb.velocity = playerControls.Player.Move.ReadValue<Vector2>() * moveSpeed;
        sprite.transform.localScale = new Vector3(Mathf.Sign(rb.velocity.x), 1, 1);
        if (rb.velocity != Vector2.zero)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.8f, 1f);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    public void OnFire()
    {
        currentEquippedGun.Shoot(bullet, relativeMousePos);        
    }

    public void OnReload()
    {
        currentEquippedGun.ReloadGun();        
    }

    public void AddDiceToMag(Dice dice, DiceDataStorage button)
    {
        if (selectedDice.Count < maxMagazine)
        {
            selectedDice.Add(dice);
        }
    }

    public void PickUpDice(Dice dice)
    {
        //dices.Add(dice);
        GameObject UIContainer = GameObject.FindGameObjectWithTag("DiceUIContainer");
        DiceDataStorage UIDice = Instantiate(diceButton, UIContainer.transform).GetComponent<DiceDataStorage>();
        UIDice.SetDice(dice);
        uiButtons.Add(UIDice);
    }

    public Gun GetEquippedGun()
    {
        return currentEquippedGun;
    }

    public void EquipGun(GameObject gun)
    {
        //remove old gun and equip the new one
        currentGun.transform.right = new Vector3(1, 0, 0);
        currentEquippedGun.SetEquiped(false);

        Transform gun1 = currentGun.transform.GetChild(0);
        gun1.localScale = new Vector3(1, 1, 1);

        currentGun.transform.DetachChildren();
        gun1.position = gun.transform.position;

        gun.transform.parent = currentGun.transform;
        gun.transform.localPosition = new Vector3(1f, 0.05001628f, 0f);
        currentEquippedGun = gun.GetComponent<Gun>();
        currentEquippedGun.SetEquiped(true);

        //set magazine UI
        GameObject UIContainer = GameObject.FindGameObjectWithTag("MagUIContainer");
        int magSize = currentEquippedGun.GetMagSize();
        foreach (Transform child in UIContainer.transform)
        {
            if (child.GetSiblingIndex() >= magSize)
            {
                child.gameObject.SetActive(false);
            } else
            {
                child.gameObject.SetActive(true);
            }
        }
    }

}
