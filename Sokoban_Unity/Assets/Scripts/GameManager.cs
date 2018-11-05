using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public GameSetting Settings;

	public float GameTime;
	public Text WinText;
	public Text SelectText;
	public Text LoseText;
	public Text Timer;
	public GameObject WinBox;
	public GameObject Arrow;
	public Text MoveCount;

	private int _scene;
	private float _timer;
	private bool _pause;
	private bool _select;

	private bool _audioPlayed;
	private bool _bgmPlayed;


	private void Awake()
	{
		Settings.Boxes = GameObject.FindGameObjectsWithTag("Box");
		Settings.Goals = GameObject.FindGameObjectsWithTag("Goal");
		Settings.Walls = GameObject.FindGameObjectsWithTag("Wall");
		Settings.Player.GameManager = this;
		Settings.Player.GameStart = true;
		Settings.Player.Audios.AudioSource = GetComponent<AudioSource>();
		Settings.Player.Audios.AudioSource.clip = Settings.Player.Audios.Bgm;
		Settings.Player.Audios.AudioSource.loop = true;

	}
	
	private void Start()
	{
		WinText.enabled = false;
		SelectText.enabled = false;
		LoseText.enabled = false;
		WinBox.SetActive(false);
		Arrow.SetActive(false);
		MoveCount.enabled = false;
		Settings.Player.Audios.AudioSource.Play();
		_scene = SceneManager.GetActiveScene().buildIndex;

	}

	private void Update()
	{
		if (Settings.Player.GameStart && GameTime > 0.0f)
		{
			GameTime -= Time.deltaTime;
		}
		if (GameTime <= 0.0f && !AllSet())
		{
			GameOver();
		}
		Timer.text = GameTime.ToString("f0");
		if (_pause)
		{
			Select();
			if (!_audioPlayed)
			{
				Settings.Player.Audios.AudioSource.Stop();
				Settings.Player.Audios.AudioSource.PlayOneShot(Settings.Player.Audios.PauseAudio);
				_audioPlayed = true;
			}
			if (!_select && Input.GetKeyUp("1"))
			{
				SceneManager.LoadScene(_scene);
			}
		}
		
		if (_select && Input.GetKeyUp("1"))
		{
			SceneManager.LoadScene(1);
		}

		if (!AllSet() && Settings.Player.GameStart && Input.GetKeyUp("1"))
		{
			WinBox.SetActive(true);
			Arrow.SetActive(true);
			SelectText.enabled = true;
			_pause = true;
			Settings.Player.PlayerController.enabled = false;
		}

		if (Settings.Player.GameStart)
		{
			if (!_bgmPlayed)
			{
				Settings.Player.Audios.AudioSource.Play();
				_bgmPlayed = true;
			}
			MoveCount.enabled = true;
		}
		else
		{
			Settings.Player.GameStart = Input.GetKeyUp("1");
		}
		
		if (AllSet())
		{
			Settings.Player.SpriteRenderer.flipX = false;
			Settings.Player.SpriteRenderer.flipY = false;
			Settings.Player.GameStart = false;
			_timer += Time.deltaTime;
			Settings.Player.SpriteRenderer.sprite = Settings.Player.Sprites.Victory;
			if (_timer >= 2.0f)
			{
				WinBox.SetActive(true);
				Arrow.SetActive(true);
				WinText.enabled = true;
				Select();
				if (!_audioPlayed)
				{
					Settings.Player.Audios.AudioSource.Stop();
					Settings.Player.Audios.AudioSource.PlayOneShot(Settings.Player.Audios.WinAudio);
					_audioPlayed = true;
				}
				if (!_select && Input.GetKeyUp("1"))
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
		}
		MoveCount.text = Settings.Player.Move.ToString().PadLeft(4, '0');
	}

	public bool AllSet()
	{
		return Settings.Boxes.All(box => box.GetComponent<BoxController>().ReachGoal);
	}

	private void Select()
	{
			if (Input.GetKeyUp(Settings.Player.Keycodes.Up) || Input.GetKeyUp(Settings.Player.Keycodes.Down))
			{
				_select = !_select;
			}

		Arrow.transform.position = (_select) ?  
				new Vector3(Arrow.transform.position.x, -3.43f, 0f): 
				new Vector3(Arrow.transform.position.x, -2.81f, 0f);
	}

	private void GameOver()
	{
		WinBox.SetActive(true);
		Arrow.SetActive(true);
		LoseText.enabled = true;
		Settings.Player.PlayerController.enabled = false;
		Select();
		if (!_audioPlayed)
		{
			Settings.Player.Audios.AudioSource.Stop();
			Settings.Player.Audios.AudioSource.PlayOneShot(Settings.Player.Audios.PauseAudio);
			_audioPlayed = true;
		}
		if (!_select && Input.GetKeyUp("1"))
		{
			SceneManager.LoadScene(_scene);
		}
		
	if (_select && Input.GetKeyUp("1"))
	{
		SceneManager.LoadScene(1);
	}
	}
}

