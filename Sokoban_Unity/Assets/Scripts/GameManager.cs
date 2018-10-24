using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	private GameObject[] _boxes;
//	public GameObject WinText;
	public GameObject WinText;
	public Text MoveCount;

	private int _moveCount;

	public GameObject Player;
	private int _scene;

	private float _timer;
	
	// Use this for initialization
	void Start()
	{
		WinText.SetActive(false);
		_boxes = GameObject.FindGameObjectsWithTag("Box");
		_scene = SceneManager.GetActiveScene().buildIndex;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.R))
		{
			SceneManager.LoadScene(_scene);
		}

		if (AllSet())
		{
			_timer += Time.deltaTime;
			Player.GetComponent<SpriteRenderer>().flipX = false;
			Player.GetComponent<SpriteRenderer>().flipY = false;
			
			Player.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Victory");
			if (_timer >= 2.0f)
			{
				WinText.SetActive(true);
				if (Input.GetKeyUp("1"))
				{
					if (_scene + 1 < SceneManager.sceneCountInBuildSettings)
					{
						SceneManager.LoadScene(_scene + 1);
					} else
					{
						SceneManager.LoadScene(0);
					}		
				}
			}
		}

		_moveCount = Player.GetComponent<PlayerController>().Move;
		MoveCount.text = _moveCount.ToString().PadLeft(4, '0');
	


	}


	public bool AllSet()
	{
		foreach (var box in _boxes)
		{
			if (!box.GetComponent<BoxController>().ReachGoal)
			{
				return false;
			}
		}
		return true;
	}
}
