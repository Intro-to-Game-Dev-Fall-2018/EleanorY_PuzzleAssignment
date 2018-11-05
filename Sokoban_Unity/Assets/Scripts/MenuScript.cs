using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

	private int _level;
	public GameObject Arrow;
	private AudioSource _audio;
	public AudioClip SelectFx;
	
	// Update is called once per frame
	private void Start()
	{
		_level = 1;
		_audio = GetComponent<AudioSource>();

	}
	
	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.DownArrow))
		{
			_audio.PlayOneShot(SelectFx);
			if (_level + 1 > 3)
			{
				_level = 1;
			}
			else
			{
				_level++;
			}
		}
		if (Input.GetKeyUp(KeyCode.UpArrow))
		{
			_audio.PlayOneShot(SelectFx);
			if (_level - 1 < 1)
			{
				_level = 3;
			}
			else
			{
				_level--;
			}
		}
		if (Input.GetKeyUp("1"))
		{
			SceneManager.LoadScene(_level+1);
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

