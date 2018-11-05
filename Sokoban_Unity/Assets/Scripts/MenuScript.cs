using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

	private int _level;
	private int _levelCount;
	public GameObject Arrow;
	private AudioSource _audio;
	public AudioClip SelectFx;
	
	private void Start()
	{
		_level = 1;
		_audio = GetComponent<AudioSource>();
		_levelCount = SceneManager.sceneCountInBuildSettings - 2;

	}
	
	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			_audio.PlayOneShot(SelectFx);
			_level = (_level + 1 > _levelCount) ? 1 : _level + 1;
		}

		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			_audio.PlayOneShot(SelectFx);
			_level = (_level - 1 < 1) ? _levelCount : _level - 1;
		}
		if (Input.GetKeyUp("1"))
		{
			SceneManager.LoadScene(_level + 1);
		}
		ArrowPos();
	}

	private void ArrowPos()
	{
		switch (_level)
		{
			case 1:
				Arrow.transform.position = new Vector3(2.0f, 1.9f, 0f);
				break;
			case 2:
				Arrow.transform.position = new Vector3(2.0f, -0.1f, 0f);
				break;
			case 3:
				Arrow.transform.position = new Vector3(2.0f, -2.1f, 0f);
				break;
		}
	}
}


