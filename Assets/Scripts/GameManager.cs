using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private PlayerLife player;

	public GameObject roomParent;

	public Room startRoom;

	public Room[] rooms;
	public Room[] endRooms;
	public Vector2 roomDimensions = new Vector2(18, 11);

	public int roomsPerFloor = 5;
	public int floorCount = 3;

	private void Start()
	{
		if (roomParent == null)
			roomParent = this.gameObject;

		player = FindObjectOfType<PlayerLife>();

		SpawnGame();
	}

	private void SpawnGame()
	{
		Room previousRoom = null;
		for (int floorI = 0; floorI < floorCount; floorI++)
		{
			for (int roomI = 0; roomI < roomsPerFloor; roomI++)
			{
				if (floorI == 0 && roomI == 0)
				{
					startRoom.PlayerEnteredRoom(player.gameObject);
					previousRoom = startRoom;
					continue;
				}

				Room newRoom = Instantiate(rooms[Random.Range(0, rooms.Length)], new Vector3(roomDimensions.x * roomI, roomDimensions.y * floorI), Quaternion.identity, roomParent.transform).GetComponent<Room>();

				if (previousRoom != null)
					previousRoom.roomRight = newRoom;
				newRoom.roomLeft = previousRoom;

				previousRoom = newRoom;
			}
		}
	}

	private void Update()
	{

	}
}
