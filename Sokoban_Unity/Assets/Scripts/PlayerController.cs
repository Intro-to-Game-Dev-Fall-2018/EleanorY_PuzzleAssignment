using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public GameSetting Settings;	
	
	private GameObject _boxPushed;	
	private bool _back;
	private bool _keyPressed;
	private bool _setOffset;
	private Vector3 _lastPlayerPos;
	private Vector3 _lastBoxPos;
	private bool _animDir;
	
	private void Awake()
	{
		Settings.Player.PlayerController = this;
	}
	
	private void Start()
	{
		_back = true;
		Settings.Player.Move = 0;
		Settings.Player.Position = transform.position;
		Settings.Player.SpriteRenderer = GetComponent<SpriteRenderer>();
		Settings.Player.SpriteRenderer.sprite = Settings.Player.Sprites.Horizontal2;
	}


	private void Update()
	{
		Settings.Player.TransformPosition = transform.position;
		if (Settings.Player.GameStart)
		{
			if (Input.GetKey(Settings.Player.Keycodes.Up))
			{
				Settings.Player.Direction = 1;
				PlayerMove();
			}

			if (Input.GetKey(Settings.Player.Keycodes.Down))
			{
				Settings.Player.Direction = 2;
				PlayerMove();
			}
			
			if (Input.GetKey(Settings.Player.Keycodes.Right))
			{
				Settings.Player.Direction = 3;
				PlayerMove();
			}
			
			if (Input.GetKey(Settings.Player.Keycodes.Left))
			{
				Settings.Player.Direction = 4;
				PlayerMove();
			}
			if (Input.GetKeyUp(Settings.Player.Keycodes.Back))
			{
				Settings.Player.Position = _lastPlayerPos;
				if (!_back)
				{
					Settings.Player.Move--;
					if (Settings.Player.Push)
					{
						_boxPushed.GetComponent<BoxController>().TargetPos = _lastBoxPos;
					}
					_back = true;
				}

			}
			if (!Input.anyKeyDown && transform.position == Settings.Player.Position)
			{
				_keyPressed = false;
				Settings.Player.Movement = new Vector3(0, 0, 0);
				Settings.Player.Direction = 0;
			}
		}

		if (!Blocked())
		{
			transform.position = Vector3.MoveTowards(transform.position, Settings.Player.Position, Time.deltaTime * Settings.Player.Speed);
		}
		else
		{
			Settings.Player.Position = new Vector3(
				Mathf.Round(transform.position.x),
				Mathf.Round(transform.position.y),
				Mathf.Round(transform.position.z));
		}

		if (Input.anyKey && Settings.Player.GameStart)
		{
			switch (Settings.Player.Direction)
			{
				case 1:
					Settings.Player.SpriteRenderer.flipX = false;
					Settings.Player.SpriteRenderer.flipY = false;
					break;
				case 2:
					Settings.Player.SpriteRenderer.flipX = false;
					Settings.Player.SpriteRenderer.flipY = true;
					break;
				case 3:
					Settings.Player.SpriteRenderer.flipX = false;
					Settings.Player.SpriteRenderer.flipY = false;
					break;
				case 4:
					Settings.Player.SpriteRenderer.flipX = true;
					Settings.Player.SpriteRenderer.flipY = false;
					break;
				case 0:
					break;
			}
		}
		if (!Settings.Player.GameManager.AllSet())
		{
			if (_animDir)
			{
				if (!Settings.Player.Push)
				{
					Settings.Player.SpriteRenderer.sprite = (Settings.Player.Move % 2 == 0) ? 	Settings.Player.Sprites.Vertical1 : Settings.Player.Sprites.Vertical2;
				}
				else
				{
					Settings.Player.SpriteRenderer.sprite = (Settings.Player.Move % 2 == 0) ? Settings.Player.Sprites.VerticalPush1 : Settings.Player.Sprites.VerticalPush2;
				}
			}
			else
			{
				if (!Settings.Player.Push)
				{
					Settings.Player.SpriteRenderer.sprite = (Settings.Player.Move % 2 == 0) ? Settings.Player.Sprites.Horizontal1 : Settings.Player.Sprites.Horizontal2;
				}
				else
				{
					Settings.Player.SpriteRenderer.sprite = (Settings.Player.Move % 2 == 0) ? Settings.Player.Sprites.HorizontalPush1 : Settings.Player.Sprites.HorizontalPush2;
				}
			}
		}
		else
		{
			Settings.Player.SpriteRenderer.sprite = Settings.Player.Sprites.Victory;
			Settings.Player.Speed = 0.0f;
		}


		if (Settings.Player.Push && Vector3.Distance(_boxPushed.transform.position, transform.position) > 1.4f)
		{
			Settings.Player.Push = false;
		}		
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Box")) return;
		other.GetComponent<BoxController>().TargetPos += Settings.Player.Movement;
		_boxPushed = other.gameObject;
		Settings.Player.Push = true;
		_lastBoxPos = other.transform.position;
	}

	private bool Blocked()
	{
		return Settings.Walls.Any(wall => wall.transform.position == Settings.Player.Position) || Settings.Boxes.Any(box => box.GetComponent<BoxController>().Blocked());
	}


	private void PlayerMove()
	{
		switch (Settings.Player.Direction)
		{
			case 1:
				Settings.Player.Movement = Vector3.up;
				_animDir = true;
				break;
			case 2:
				Settings.Player.Movement = Vector3.down;
				_animDir = true;
				break;
			case 3:
				Settings.Player.Movement = Vector3.right;
				_animDir = false;
				break;
			case 4:
				Settings.Player.Movement = Vector3.left;
				_animDir = false;
				break;
			default:
				Settings.Player.Movement = Vector3.zero;
				break;
		}
		if (Settings.Player.Position != transform.position || _keyPressed) return;
		if (Settings.Player.Push && !_boxPushed.GetComponent<BoxController>().Blocked())
		{
			_lastBoxPos = new Vector3(
				Mathf.Round(_boxPushed.transform.position.x),
				Mathf.Round(_boxPushed.transform.position.y),
				Mathf.Round(_boxPushed.transform.position.z));
		}
		Settings.Player.Position += Settings.Player.Movement * Settings.Player.GridSize;
		_keyPressed = true;
		if (Blocked()) return;
		_lastPlayerPos = new Vector3(
			Mathf.Round(transform.position.x),
			Mathf.Round(transform.position.y),
			Mathf.Round(transform.position.z));
		Settings.Player.Move++;
		_back = false;
	}
}



