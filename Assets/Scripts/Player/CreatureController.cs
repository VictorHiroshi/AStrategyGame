using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureController : MonoBehaviour {
	public Animator animatorController;
	public Slider healthSlider;
	public Image fillSliderImage;
	public int belongsToPlayer;
	public float speed = 1.0f;

	[HideInInspector] public bool moved;
	[HideInInspector] public bool isTired;

	private int health = 10;
	private GameObject creatureModel;

	void Start()
	{
		moved = false;
		isTired = false;
		healthSlider.value = health;
		animatorController.SetTrigger ("IsIdle");
		creatureModel = GameManager.instance.player [belongsToPlayer].creature;
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
		StartCoroutine (Moving (target));
	}

	public void DuplicateToTarget (Transform target, out CreatureController newCreature)
	{
		Vector3 newPosition = transform.position + (0.3f * (target.position - transform.position));
		GameObject instance = Instantiate (creatureModel, newPosition, Quaternion.identity);
		newCreature = instance.GetComponent <CreatureController> ();

		newCreature.ChangeTeam (belongsToPlayer);

		newCreature.MoveToTarget (target);
	}

	private void Die ()
	{
		
	}

	private IEnumerator Moving(Transform target)
	{
		transform.rotation = Quaternion.LookRotation (target.position - transform.position);
		animatorController.SetTrigger ("Moves");
		while(transform.position!=target.position){
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
			yield return null;
		}
		animatorController.SetTrigger ("IsIdle");
		transform.rotation = Quaternion.identity;
		ActionsManager.instance.FinishAction ();
	}

}
