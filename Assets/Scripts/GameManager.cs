using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	private PlayerLife player;

	public GameObject roomParent;

	public Tutorial tutorial;
	public Room startRoom;

	public Room[] rooms;
	public Room[] endRooms;
	public Vector2 roomDimensions = new Vector2(18, 11);

	public int roomsPerFloor = 5;
	public int floorCount = 3;

	public int[] maxCountPerFloor = new int[] { 4, 8, 14 };

	[Header("Game Result Screen:")]

	public GameObject gameResultScreen;
	public TMP_Text resultLabel;
	public GameObject continueBtn;

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
		gameResultScreen.SetActive(true);
		resultLabel.text = "Game Over";
	}

	public void WinGame()
	{
		gameResultScreen.SetActive(true);
		resultLabel.text = "You Win!\nThanks for playing.";
		continueBtn.SetActive(false);
	}


	public void Btn_Continue()
	{
		gameResultScreen.SetActive(false);
		// enable player
		player.life = player.maxLife;
	}

	public void Btn_TryAgain()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
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

				newRoom.floor = floorI;

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

				if (floorI == floorCount - 1 && roomI == roomsPerFloor - 1)
					newRoom.spawnWinObject = true;

				previousRoom = newRoom;
			}
		}
	}

	public void Btn_Play()
	{
		// set tutorial on
		tutorial.StartTutorial(player);

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
