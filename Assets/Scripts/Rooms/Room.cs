using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	CameraMovement cam;

	private Enemy[] roomEnemies;

	public bool playerInRoom = false;

	public LootRift lootRiftPrefab;
	public bool spawnLootRiftOnClear = false;
	public WinObject winObject;
	public bool spawnWinObject = false;

	public int floor;

	[Space(5)]

	public Room roomLeft;
	public Room roomRight;

	[Space(5)]

	public Collider2D colliderLeft;
	public Collider2D colliderRight;

	[Space(5)]

	public GameObject leftPos;
	public GameObject rightPos;

	private NextRoomTrigger triggerLeft;
	private NextRoomTrigger triggerRight;

	[Space(5)]

	public GameObject nextFloorPos;
	private NextRoomTrigger nextFloorTrigger;
	public bool nextFloorExit = false;

	[Space(5)]

	public bool isCleared = false;
	public bool checkCleared = true;

	public bool IsCleared()
	{
		for (int i = 0; i < roomEnemies.Length; i++)
			if (roomEnemies[i] != null)
				return false;
		return true;
	}
	private bool wasClearedLastFrame = false;

	private void Awake()
	{
		roomEnemies = GetComponentsInChildren<Enemy>();
		for (int i = 0; i < roomEnemies.Length; i++)
			roomEnemies[i].gameObject.SetActive(false);


		triggerLeft = leftPos.GetComponentInChildren<NextRoomTrigger>();
		triggerRight = rightPos.GetComponentInChildren<NextRoomTrigger>();
		nextFloorTrigger = nextFloorPos.GetComponentInChildren<NextRoomTrigger>();

		triggerLeft.OnTriggerEnter += (sender, args) => { PlayLeftRoom(); roomLeft.PlayerEnteredRoom(args.gameObject, true); };
		triggerRight.OnTriggerEnter += (sender, args) => { PlayLeftRoom(); roomRight.PlayerEnteredRoom(args.gameObject, false); };
		nextFloorTrigger.OnTriggerEnter += (sender, args) => { PlayLeftRoom(); roomRight.PlayerEnteredRoom(args.gameObject, false); };

		cam = Camera.main.GetComponent<CameraMovement>();
	}

	public void ActivateNextFloorExit()
	{
		nextFloorExit = true;

		rightPos.SetActive(false);
		nextFloorPos.SetActive(true);

		// just overtake rigth exit logic
		triggerRight = nextFloorTrigger;
		rightPos = nextFloorPos;
		// cannot go back
		if (roomRight != null)
			roomRight.roomLeft = null;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (checkCleared && IsCleared())
			isCleared = true;

		if (playerInRoom)
		{
			if (isCleared)
			{
				leftPos.SetActive(roomLeft != null);
				rightPos.SetActive(roomRight != null);

				// leave collider on if there is no room on that side
				colliderLeft.enabled = roomLeft == null;
				colliderRight.enabled = roomRight == null || nextFloorExit == true;
			}
			else
			{
				leftPos.SetActive(false);
				rightPos.SetActive(false);

				colliderLeft.enabled = true;
				colliderRight.enabled = true;
			}
		}
		else
		{
			leftPos.SetActive(false);
			rightPos.SetActive(false);
			colliderLeft.enabled = false;
			colliderRight.enabled = false;
		}


		if (wasClearedLastFrame == false && isCleared)
		{
			if (spawnLootRiftOnClear)
			{
				Instantiate(lootRiftPrefab, transform.position, Quaternion.identity).GetComponent<LootRift>().floor = floor;
			}
			if (spawnWinObject)
				Instantiate(winObject, transform.position, Quaternion.identity);
		}

		wasClearedLastFrame = isCleared;
	}

	public void PlayLeftRoom()
	{
		playerInRoom = false;
	}

	public void SetAllEnemyDifficulty(int maxNumber, int difficulty)
	{
		foreach (Enemy enemy in roomEnemies)
		{
			enemy.maxNumber = maxNumber;
			enemy.difficulty = difficulty;
			enemy.GenerateNumbers();
		}
	}

	public void PlayerEnteredRoom(GameObject player, bool fromTheRight = false)
	{
		// move cam
		cam.MoveToPosition(new Vector3(transform.position.x, transform.position.y, cam.transform.position.z));

		// move player
		//if ((player.transform.position - leftPos.transform.position).sqrMagnitude < (player.transform.position - rightPos.transform.position).sqrMagnitude)
		if (fromTheRight == false)
			player.transform.position = leftPos.transform.position;
		else
			player.transform.position = rightPos.transform.position;

		playerInRoom = true;

		// set enemies active
		foreach (Enemy enemy in roomEnemies)
			if (enemy != null)
				enemy.gameObject.SetActive(true);
	}
}
