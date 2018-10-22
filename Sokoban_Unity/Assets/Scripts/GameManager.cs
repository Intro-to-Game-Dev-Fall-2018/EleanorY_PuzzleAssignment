using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

	private GameObject[] _boxes;
	public GameObject WinText;
	public Text MoveCount;

	private int _moveCount;

	public GameObject Player;

	// Use this for initialization
	void Start()
	{
		_boxes = GameObject.FindGameObjectsWithTag("Box");
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.R))
		{
			SceneManager.LoadScene(0);
		}

		if (AllSet())
		{
			WinText.SetActive(true);
		}

		_moveCount = Player.GetComponent<PlayerController>().Move;
		MoveCount.text = _moveCount.ToString().PadLeft(4, '0');

	}


	private bool AllSet()
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
