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

	public int[] maxCountPerFloor = new int[] { 4, 8, 14 };

	private void Start()
	{
		if (roomParent == null)
			roomParent = this.gameObject;

		player = FindObjectOfType<PlayerLife>();
		player.OnDeath += Player_OnDeath;
		player.gameObject.SetActive(false);


		SpawnGame();
	}

	private void Player_OnDeath(object sender, System.EventArgs e)
	{
		Debug.Log("Player Died could end game here.");
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
					player.transform.position = startRoom.transform.position;
					previousRoom = startRoom;
					continue;
				}

				Room newRoom = Instantiate(rooms[Random.Range(0, rooms.Length)], new Vector3(roomDimensions.x * roomI, roomDimensions.y * floorI), Quaternion.identity, roomParent.transform).GetComponent<Room>();

				if (previousRoom != null)
					previousRoom.roomRight = newRoom;

				
				newRoom.roomLeft = previousRoom;

				// new floor
				if (roomI == 0)
					newRoom.roomLeft = null;

				// set different rooms
				if (roomI == 3 - 1)
					newRoom.spawnLootRiftOnClear = true;
				if (roomI == 5 - 1)
					newRoom.ActivateNextFloorExit();

				// set enemy difficulty
				newRoom.SetAllEnemyDifficulty(maxCountPerFloor[floorI] + roomI, floorI);

				previousRoom = newRoom;
			}
		}
	}

	public void Btn_Play()
	{
		// set tutorial on


		// set player on
		player.gameObject.SetActive(true);
	}

	public void Btn_Exit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}
