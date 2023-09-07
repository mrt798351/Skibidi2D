using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour 
{
	public GameObject menuUI;
	public GameObject gameUI;
	public GameObject gameOverUI;

	//Game UI
	public TextMeshProUGUI timeElapsed;
	public Slider planetHealthBar;

	//Game Over UI
	public TextMeshProUGUI goDefendTime;
	public TextMeshProUGUI goHighscoreTime;

	public static UI ui;

	void Awake () { ui = this; }

	void Start ()
	{
		planetHealthBar.maxValue = Planet.p.health;
		planetHealthBar.value = Planet.p.health;
	}

	void Update ()
	{
		if(Game.g.gameActive)
		{
			SetTimeElapsed();
		}
	}

	//On the menu screen, when the "Play" button gets pressed.
	public void OnPlayButton ()
	{
		CameraController.c.TransitionToGameView();
		menuUI.SetActive(false);
	}

	//On the menu or game over screen, when the "Quit" button gets pressed.
	public void OnQuitButton ()
	{
		Application.Quit();
	}

	//On the game over screen, when the "Menu" button gets pressed.
	public void OnMenuButton ()
	{
		Application.LoadLevel(0);
	}

	//Enables the menu UI game object.
	public void SetMenuUI ()
	{
		menuUI.SetActive(true);
	}

	//Enables the game UI game object.
	public void SetGameUI ()
	{
		gameUI.SetActive(true);
	}

	//Enables the game over UI game object.
	public void SetGameOverUI ()
	{
		gameOverUI.SetActive(true);
		gameUI.SetActive(false);

		//Setting text values.
		goDefendTime.text = "ВЫ ЗАЩИЩАЛИ ПЛАНЕТУ...\n<size=50>" + GetTimeAsString(Game.g.gameTime) + "</size>  МИНУТ";

		//Set highscore text.
		if(Game.g.gameTimeHighscore == 0)
		{
			goHighscoreTime.text = "ВАШ ЛУЧШИЙ СЧЕТ...\n<size=50>" + GetTimeAsString(Game.g.gameTime) + "</size>  МИНУТ";
			Game.g.SetTimeAsHighscore();
		}
		else
		{
			goHighscoreTime.text = "ВАШ ЛУЧШИЙ СЧЕТ...\n<size=50>" + GetTimeAsString(Game.g.gameTimeHighscore) + "</size>  МИНУТ";

			//If the current time is higher than the highscore, set that as the highscore.
			if(Game.g.gameTime > Game.g.gameTimeHighscore)
				Game.g.SetTimeAsHighscore();
		}
	}

	//Sets the value of the planet health bar. Called when the planet takes damage.
	public void SetPlanetHealthBarValue (int value)
	{
		planetHealthBar.value = value;
		StartCoroutine(PlanetHealthBarFlash());
	}

	//Flashes the health bar red quickly.
	IEnumerator PlanetHealthBarFlash ()
	{
		Image fill = planetHealthBar.transform.Find("Fill Area/Fill").GetComponent<Image>();

		if(fill.color != Color.red)
		{
			Color dc = fill.color;
			fill.color = Color.red;

			yield return new WaitForSeconds(0.05f);

			fill.color = dc;
		}
	}

	//Sets the text that shows how long the game has been going for.
	void SetTimeElapsed ()
	{
		timeElapsed.text = "ПРОШЛО ВРЕМЕНИ\n<size=55>" + GetTimeAsString(Game.g.gameTime) + "</size>";
	}

	//Converts a number to a MINS:SECS time format.
	string GetTimeAsString (float t)
	{
		string mins = Mathf.FloorToInt(t / 60).ToString();

		if(int.Parse(mins) < 10)
			mins = "0" + mins;

		string secs = ((int)(t % 60)).ToString();

		if(int.Parse(secs) < 10)
			secs = "0" + secs;

		return mins + ":" + secs;
	}
}
