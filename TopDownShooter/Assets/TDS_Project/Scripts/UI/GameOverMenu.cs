﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour {

	public void RestartGame()
	{
		GameManager.Instance.RestartGame ();
	}
}
