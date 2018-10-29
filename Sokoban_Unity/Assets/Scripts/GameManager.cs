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
	public Text WinText;
	public Text SelectText;
	public GameObject WinBox;
	public GameObject Arrow;
	public Text MoveCount;

	private int _moveCount;

	public GameObject Player;
	private int _scene;

	private float _timer;
	private bool _pause;
	private bool _select;

	private AudioSource _audio;
	public AudioClip WinAudio;
	public AudioClip PauseAudio;

	private bool _bgmPlayed;
	private bool _audioPlayed;

	private bool _gameStart;



	// Use this for initialization
	
	void Start()
	{
		_gameStart = true;
		WinText.enabled = false;
		SelectText.enabled = false;
		WinBox.SetActive(false);
		Arrow.SetActive(false);
		MoveCount.enabled = false;
		_boxes = GameObject.FindGameObjectsWithTag("Box");
		_scene = SceneManager.GetActiveScene().buildIndex;
		_audio = GetComponent<AudioSource>();

	}

	// Update is called once per frame
	private void Update()
	{	
		if (_pause)
		{
			Select();
			if (!_audioPlayed)
			{
				_audio.Stop();
				_audio.PlayOneShot(PauseAudio);
				_audioPlayed = true;
			}
			if (!_select && Input.GetKeyUp("1"))
			{
				SceneManager.LoadScene(_scene);
			}
			else if (_select && Input.GetKeyUp("1"))
			{
				SceneManager.LoadScene(1);
			}
		}

		if (!AllSet() && _gameStart && Input.GetKeyUp("1"))
		{
			WinBox.SetActive(true);
			Arrow.SetActive(true);
			SelectText.enabled = true;
			_pause = true;
			Player.GetComponent<PlayerController>().enabled = false;
		}

		if (_gameStart)
		{
			if (!_bgmPlayed)
			{
				_audio.Play();
				_bgmPlayed = true;
			}
			MoveCount.enabled = true;
			Player.GetComponent<PlayerController>().GameStart = true;
		}
		else
		{
			_audio.Stop();
			MoveCount.enabled = false;
			Player.GetComponent<PlayerController>().GameStart = false;
			if (Input.GetKeyUp("1"))
			{
				_gameStart = true;
			}
		}


		if (AllSet())
		{
			Player.GetComponent<SpriteRenderer>().flipX = false;
			Player.GetComponent<SpriteRenderer>().flipY = false;
			Player.GetComponent<PlayerController>().GameStart = false;
			_timer += Time.deltaTime;
			if (_timer >= 2.0f)
			{
				WinBox.SetActive(true);
				Arrow.SetActive(true);
				WinText.enabled = true;
				Select();
				if (!_audioPlayed)
				{
					_audio.Stop();
					_audio.PlayOneShot(WinAudio);
					_audioPlayed = true;
				}
				if (Input.GetKeyUp("1"))
				{
					if (_scene + 1 < SceneManager.sceneCountInBuildSettings)
					{
						SceneManager.LoadScene(_scene + 1);
					}
					else
					{
						SceneManager.LoadScene(2);
					}
				}
			}
			Player.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Victory");
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

	private void Select()
	{
			if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
			{
				_select = !_select;
				Player.GetComponent<PlayerController>().GameStart = false;
			}

			if (_select)
			{
				Arrow.transform.position = new Vector3(Arrow.transform.position.x, -3.43f, 0f);
				;
			}
			else
			{
				Arrow.transform.position = new Vector3(Arrow.transform.position.x, -2.81f, 0f);
			}
	}
}
