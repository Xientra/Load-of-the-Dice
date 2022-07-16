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

		triggerLeft = leftPos.GetComponentInChildren<NextRoomTrigger>();
		triggerRight = rightPos.GetComponentInChildren<NextRoomTrigger>();

		triggerLeft.OnTriggerEnter += (sender, args) => { PlayLeftRoom(); roomLeft.PlayerEnteredRoom(args.gameObject); };
		triggerRight.OnTriggerEnter += (sender, args) => { PlayLeftRoom(); roomRight.PlayerEnteredRoom(args.gameObject); };
	}

	private void Start()
	{
		cam = Camera.main.GetComponent<CameraMovement>();
	}

	private void Update()
	{
		bool isClear = IsCleared();

		if (playerInRoom)
		{
			if (isClear)
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

	public void PlayerEnteredRoom(GameObject player)
	{
		playerInRoom = true;
		cam.MoveToPosition(new Vector3(transform.position.x, transform.position.y, cam.transform.position.z));

		if ((player.transform.position - leftPos.transform.position).sqrMagnitude < (player.transform.position - rightPos.transform.position).sqrMagnitude)
			player.transform.position = leftPos.transform.position;
		else
			player.transform.position = rightPos.transform.position;

		// set enemies active
	}
}
