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
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        dices.Add(new Dice(4));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Vector3 mousePosition = playerControls.Player.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //Debug.Log(mousePosition);
        relativeMousePos = mousePosition - transform.position;
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
    }

    public void OnFire()
    {
        if (damageRolls.Count > 0)
        {
            Bullet spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<Bullet>();
            spawnedBullet.transform.up = relativeMousePos;
            spawnedBullet.SetDamage(damageRolls[0]);
            damageRolls.RemoveAt(0);
            selectedDice.RemoveAt(0);
        }
        
    }

    public void OnReload()
    {
        if (rollReady)
        {
            foreach (Dice d in selectedDice)
            {
                damageRolls.Add(d.ThrowDice());
            }
            rollReady = false;
            rollDuration = 0;
        }
        
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
        Instantiate(diceButton, UIContainer.transform);
    }

}
