using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
	public GameObject Title;
	public GameObject Title2;
	public float Speed;
	

	private void Start()
	{
		Title.transform.position = new Vector3(0f, 6.6f, 0f);
	}

	private void Update()
	{
		if (Title.transform.position.y > 1.16f)
		{
			Title.transform.position += Vector3.down * Time.deltaTime * Speed;
		}
		else
		{
			Title.transform.position = new Vector3(0f, 1.16f, 0f);
			Title2Move();
		}
	}

	private void Title2Move()
	{
		if (Title2.transform.position.y < -1.6f)
		{
			Title2.transform.position += Vector3.up * Time.deltaTime * Speed;
		}
		else
		{
			Title2.transform.position = new Vector3(0f, -1.6f, 0f);
			if (Input.GetKeyUp("1"))
			{
				SceneManager.LoadScene(2);
			}
		}		
	}
}


