using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
	Camera mainCam;

	private Enemy[] roomEnemies;

	public bool playerInRoom = false;

	[Space(5)]

	public Room roomLeft;
	public Room roomRight;

	[Space(5)]

	public Collider2D colliderLeft;
	public Collider2D colliderRight;

	[Space(5)]

	public NextRoomTrigger triggerLeft;
	public NextRoomTrigger triggerRight;

	[Space(5)]

	public bool DEBUG_isCleared = false;

	public bool IsCleared()
	{
		return DEBUG_isCleared;

		for (int i = 0; i < roomEnemies.Length; i++)
			if (roomEnemies[i] != null)
				return false;
		return true;
	}
	private bool wasClearedLastFrame = false;

	private void Awake()
	{
		roomEnemies = GetComponentsInChildren<Enemy>();

		triggerLeft.OnTriggerEnter += (sender, args) => { PlayLeftRoom(); roomLeft.PlayerEnteredRoom(); };
		triggerRight.OnTriggerEnter += (sender, args) => { PlayLeftRoom(); roomRight.PlayerEnteredRoom(); };
	}

	private void Start()
	{
		mainCam = Camera.main;
	}

	private void Update()
	{
		bool isClear = IsCleared();

		if (playerInRoom)
		{
			if (isClear)
			{
				triggerLeft.gameObject.SetActive(roomLeft != null);
				triggerRight.gameObject.SetActive(roomRight != null);

				// leave collider on if there is no room on that side
				colliderLeft.enabled = roomLeft == null;
				colliderRight.enabled = roomRight == null;
			}
			else
			{
				triggerLeft.gameObject.SetActive(false);
				triggerRight.gameObject.SetActive(false);

				colliderLeft.enabled = true;
				colliderRight.enabled = true;
			}
		}
		else
		{
			triggerLeft.gameObject.SetActive(false);
			triggerRight.gameObject.SetActive(false);
			colliderLeft.enabled = false;
			colliderRight.enabled = false;
		}


		if (wasClearedLastFrame == false && isClear)
		{
			// spawn gatcha here
		}

		wasClearedLastFrame = isClear;
	}

	public void PlayLeftRoom()
	{
		playerInRoom = false;

	}

	public void PlayerEnteredRoom()
	{
		playerInRoom = true;
		mainCam.transform.position = new Vector3(transform.position.x, transform.position.y, mainCam.transform.position.z);

		// set enemies active
	}
}
