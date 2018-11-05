using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {
	public GameSetting Settings;
	private Vector3 _currentPos;
	public Vector3 TargetPos;
	private Vector3 _expectTarget;

	public GameObject Player;
	private Vector3 _offset;

	private int _playerStatus;

	private SpriteRenderer _spriteRenderer;
	public Sprite DarkBox;
	public Sprite LightBox;
	public bool ReachGoal;

	private void Start()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		TargetPos = transform.position;
		_expectTarget = transform.position;
	}

	private void Update()
	{	
		_currentPos = transform.position;
		_offset = Player.transform.position - transform.position;
		if (_offset.x < -0.5f && _offset.x >= -1.0f && Mathf.Abs(_offset.y) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x + 1,
				_currentPos.y,
				_currentPos.z);
			_playerStatus = 3;
		} 
		else if (_offset.x > 0.5f && _offset.x <= 1.0f  && Mathf.Abs(_offset.y) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x - 1,
				_currentPos.y,
				_currentPos.z);
			_playerStatus = 4;
		}
		else if (_offset.y < -0.5f && _offset.y >= -1.0f  && Mathf.Abs(_offset.x) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x,
				_currentPos.y + 1,
				_currentPos.z);
			_playerStatus = 2;
		} 
		else if (_offset.y > 0.5f && _offset.y <= 1.0f  && Mathf.Abs(_offset.x) < 0.8f)
		{
			_expectTarget = new Vector3(
				_currentPos.x,
				_currentPos.y - 1,
				_currentPos.z);
			_playerStatus = 1;
		}
		else
		{
			_expectTarget = transform.position;
			_playerStatus = 0;
		}

		if (!Blocked())
		{
		transform.position = Vector3.MoveTowards(_currentPos, TargetPos, Time.deltaTime * 8);
		}
		else
		{
			transform.position = new Vector3(
				Mathf.Round(_currentPos.x),
				Mathf.Round(_currentPos.y),
				Mathf.Round(_currentPos.z));
			TargetPos = transform.position;
		}

		foreach (var goal in Settings.Goals)
		{
			if (transform.position == goal.transform.position)
			{
				_spriteRenderer.sprite = DarkBox;
				ReachGoal = true;
				return;
			}
			ReachGoal = false;
			_spriteRenderer.sprite = LightBox;			
		}
		
	}

	public bool Blocked()
	{
		foreach (var wall in Settings.Walls)
		{
			if (wall.transform.position == _expectTarget)
			{
				return CheckBlockedStatus();
			}
		}

		foreach (var box in Settings.Boxes)
		{
			if (box != this.gameObject && box.transform.position == _expectTarget)
			{
				return CheckBlockedStatus();
			}
		}
			return false;
		}

	private bool CheckBlockedStatus()
	{
		switch (_playerStatus)
		{
			case 0:
				return false;
			case 1:
				if (Input.GetKey(Settings.Player.Keycodes.Down))
				{
					return true;
				}
				else
				{
					return false;
				}
			case 2:
				if (Input.GetKey(Settings.Player.Keycodes.Up))
				{
					return true;
				}
				else
				{
					return false;
				}
			case 3:
				if (Input.GetKey(Settings.Player.Keycodes.Right))
				{
					return true;
				}
				else
				{
					return false;
				}
			case 4:
				if (Input.GetKey(Settings.Player.Keycodes.Left))
				{
					return true;
				}
				else
				{
					return false;
				}
		}
		return false;
	}

}
