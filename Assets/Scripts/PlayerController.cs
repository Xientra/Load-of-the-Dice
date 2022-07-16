using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
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


    private void Awake()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Move.ReadValue<Vector2>();
        playerControls.Player.Fire.performed += _ => OnFire();
        playerControls.Player.Reload.performed += _ => OnReload();
        rb = GetComponent<Rigidbody2D>();
        currentEquippedGun = currentGun.transform.GetChild(0).GetComponent<Gun>();
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
