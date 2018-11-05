using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoxController : MonoBehaviour {
	public GameSetting Settings;
	public Vector3 TargetPos;
	private Vector3 _expectTarget;

	private Vector3 _offset;
	private int _playerStatus;
	private GameObject _player;
	
	private SpriteRenderer _spriteRenderer;
	public Sprite DarkBox;
	public Sprite LightBox;
	public bool ReachGoal;

	private void Start()
	{
		_player = GameObject.FindWithTag("Player");
		_spriteRenderer = GetComponent<SpriteRenderer>();
		TargetPos = transform.position;
		_expectTarget = transform.position;
	}

	private void Update()
	{	
		_offset = _player.transform.position - transform.position;
		if (_offset.x < -0.5f && _offset.x >= -1.0f && Mathf.Abs(_offset.y) < 0.8f)
		{
			_expectTarget = transform.position + Vector3.right;
			_playerStatus = 3;
		} 
		else if (_offset.x > 0.5f && _offset.x <= 1.0f  && Mathf.Abs(_offset.y) < 0.8f)
		{
			_expectTarget = transform.position + Vector3.left;
			_playerStatus = 4;
		}
		else if (_offset.y < -0.5f && _offset.y >= -1.0f  && Mathf.Abs(_offset.x) < 0.8f)
		{
			_expectTarget = transform.position + Vector3.up;
			_playerStatus = 2;
		} 
		else if (_offset.y > 0.5f && _offset.y <= 1.0f  && Mathf.Abs(_offset.x) < 0.8f)
		{
			_expectTarget = transform.position + Vector3.down;
			_playerStatus = 1;
		}
		else
		{
			_expectTarget = transform.position;
			_playerStatus = 0;
		}

		if (!Blocked())
		{
		transform.position = Vector3.MoveTowards(transform.position, TargetPos, Time.deltaTime * 8);
		}
		else
		{
			transform.position = new Vector3(
				Mathf.Round(transform.position.x),
				Mathf.Round(transform.position.y),
				Mathf.Round(transform.position.z));
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
		if (Settings.Walls.Any(wall => wall.transform.position == _expectTarget))
		{
			return CheckBlockedStatus();
		}
		return Settings.Boxes.Any(box => box != this.gameObject && box.transform.position == _expectTarget) && CheckBlockedStatus();
	}

	private bool CheckBlockedStatus()
	{
		switch (_playerStatus)
		{
			case 0:
				return false;
			case 1:
				return Input.GetKey(Settings.Player.Keycodes.Down);
			case 2:
				return Input.GetKey(Settings.Player.Keycodes.Up);
			case 3:
				return Input.GetKey(Settings.Player.Keycodes.Right);
			case 4:
				return Input.GetKey(Settings.Player.Keycodes.Left);
		}
		return false;
	}
}

