using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureController : MonoBehaviour {
	public Animator animatorController;
	public Slider healthSlider;
	public Image fillSliderImage;
	public int belongsToPlayer;

	private int health = 10;

	void Start()
	{
		healthSlider.value = health;
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
		if(health<=0)
		{
			health = 0;
			Die ();
		}
		healthSlider.value = health;
	}

	public void ChangeTeam(int newPlayerIndex)
	{
		belongsToPlayer = newPlayerIndex;
		fillSliderImage.color = GameManager.instance.playersColors[belongsToPlayer];
	}

	public void MoveToTarget (Transform target)
	{
		// TODO: Change animation and move smoothly.
		transform.position = target.position;
		ActionsManager.instance.FinishAction ();
	}

	private void Die ()
	{
		
	}
}
