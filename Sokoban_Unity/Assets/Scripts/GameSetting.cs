using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings")]
public class GameSetting : ScriptableObject 
{
	
	[Header("Player Settings")]
	public PlayerSettings Player;
	
	[Header("Boxes")]
	public GameObject[] Boxes;
	
	[Header("Walls")]
	public GameObject[] Walls;
	
	[Header("Goals")]
	public GameObject[] Goals;


	[Serializable]
	public struct PlayerSettings
	{
		public bool GameStart;
		public float Speed;
		public float GridSize;
		public Vector3 Movement;
		public int Move;
		public int Direction;
		public bool Push;
		public Vector3 Position;
		public PlayerController PlayerController;	
		public SpriteRenderer SpriteRenderer;
		public GameManager GameManager;
		public SpriteSettings Sprites;
		public KeycodeSettings Keycodes;
		public AudioSettings Audios;
	}
	
	[Serializable]
	public struct SpriteSettings
	{
		public Sprite Horizontal1;
		public Sprite Horizontal2;
		public Sprite Vertical1;
		public Sprite Vertical2;
		public Sprite Victory;
		public Sprite HorizontalPush1;
		public Sprite HorizontalPush2;
		public Sprite VerticalPush1;
		public Sprite VerticalPush2;
	}
	
	[Serializable]
	public struct KeycodeSettings
	{
		public KeyCode Up;
		public KeyCode Down;
		public KeyCode Left;
		public KeyCode Right;
		public KeyCode Back;
	}
	
	[Serializable]
	public struct AudioSettings
	{
		public AudioSource AudioSource;
		public AudioClip Bgm;
		public AudioClip WinAudio;
		public AudioClip PauseAudio;
	}
}
