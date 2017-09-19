using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager> {

	public bool GameOver { get; set; }

	public Player player;
	private ItemSpawner itemSpawner;
	public GameOverController gameOverController;

	public void ProcessInputs(InputPackage p) {
		if (GameOver) {
			if(p.Select) {
				GameOver = false;
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);			
			}
			player.SetMoveDirections(0, 0);
		}
		else {
			if (p.Pause) {
				//Pause();
			}
			else {
				player.SetMoveDirections(p.Horizontal, p.Vertical);
				player.Fire( p.Fire );
			}
		}
	}

	public void SpawnItem(Vector2 position) {
		if(itemSpawner == null) {
			itemSpawner = GetComponent<ItemSpawner>();
		}
		itemSpawner.spawnItem(position);
	}

	public void SetGameOver() {
		GameOver = true;

		//display gameover text
		gameOverController.gameObject.SetActive(true);
	}
}