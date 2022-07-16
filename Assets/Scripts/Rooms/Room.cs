using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	CameraMovement cam;

	private Enemy[] roomEnemies;

	public bool playerInRoom = false;

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

	public bool isCleared = false;

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

		triggerLeft.OnTriggerEnter += (sender, args) => { PlayLeftRoom(); roomLeft.PlayerEnteredRoom(args.gameObject, true); };
		triggerRight.OnTriggerEnter += (sender, args) => { PlayLeftRoom(); roomRight.PlayerEnteredRoom(args.gameObject, false); };

		cam = Camera.main.GetComponent<CameraMovement>();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (IsCleared())
			isCleared = true;

		if (playerInRoom)
		{
			if (isCleared)
			{
				leftPos.SetActive(roomLeft != null);
				rightPos.SetActive(roomRight != null);

				// leave collider on if there is no room on that side
				colliderLeft.enabled = roomLeft == null;
				colliderRight.enabled = roomRight == null;
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
			// spawn gatcha here
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
			enemy.gameObject.SetActive(true);
	}
}
