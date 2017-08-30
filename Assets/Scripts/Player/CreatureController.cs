using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureController : MonoBehaviour {
	public Animator animatorController;
	public Slider healthSlider;
	public Image fillSliderImage;
	public DialogScript dialogCanvas;
	public GameObject shield;
	public GameObject HealingBox;

	public float speed = 0.1f;

	[HideInInspector] public bool moved;
	[HideInInspector] public bool isTired;
	[HideInInspector] public PlayerController belongsToPlayer = null;
	[HideInInspector] public PlayerController influencedByPlayer = null;

	private float inDoubtBlinkTimeValue = 1f;
	private int health;
	private int defendingDamage;
	private bool finishedInteractionAnimation = false;
	private bool isDefending;
	private IEnumerator inDoubtBlinkLoop;
	private GameObject creatureModel;
	private ParticleSystem explosionParticles;
	private ParticleSystem rocksParticles;
	private CreatureController enemy;
	private WaitForSeconds inDoubtBlinkTime;

	void Awake()
	{
		moved = false;
		isTired = false;

		creatureModel = gameObject;
		animatorController.SetTrigger ("IsIdle");


		TurnDefense (false);

		HealingBox.SetActive (false);

		inDoubtBlinkTime = new WaitForSeconds (inDoubtBlinkTimeValue);
	}

	void LateStart()
	{
		health = ActionsManager.instance.maxHealth;
		healthSlider.maxValue = ActionsManager.instance.maxHealth;
		healthSlider.value = health;

		defendingDamage = ActionsManager.instance.defendingDamage;

		explosionParticles = Instantiate (GameManager.instance.boardScript.explosionParticles, transform.position, Quaternion.identity, transform);
		rocksParticles = Instantiate (GameManager.instance.boardScript.rockExplorationParticles, transform.position, Quaternion.identity, transform);

	}

	public void FinishedAnimation()
	{
		finishedInteractionAnimation = true;
	}

	public bool IsDefending()
	{
		return isDefending;
	}

	public bool DiesWhenTakeDamage()
	{
		if (isDefending) 
		{
			health -= defendingDamage;
		}
		else 
		{
			health = 0;
		}

		return (health == 0);
	}

	public void ChangeTeam(int newPlayerIndex)
	{
		influencedByPlayer = null;
		belongsToPlayer = GameManager.instance.player[newPlayerIndex];
		fillSliderImage.color = GameManager.instance.playersColors[newPlayerIndex];
	}

	public void MoveToTarget (Transform target)
	{
		animatorController.SetTrigger ("Moves");
		StartCoroutine (Moving (target));
	}

	public void DuplicateToTarget (Transform target, out CreatureController newCreature)
	{
		Vector3 newPosition = transform.position + (0.3f * (target.position - transform.position));
		GameObject instance = Instantiate (creatureModel, newPosition, Quaternion.identity);
		newCreature = instance.GetComponent <CreatureController> ();

		newCreature.ChangeTeam (belongsToPlayer.playerNumber);

		newCreature.MoveToTarget (target);
	}

	public void Attack(Transform origin, Transform target, CreatureController enemy)
	{
		this.enemy = enemy;

		enemy.transform.rotation = Quaternion.LookRotation (origin.position - target.position);

		Vector3 walkingPosition = target.position - ((target.position - origin.position) / GameManager.instance.boardScript.tiles.tileSideSize);

		StartCoroutine (Attacking (origin, target, walkingPosition));
	}

	public void TurnDefense(bool defenseState)
	{
		shield.SetActive (defenseState);
		isDefending = defenseState;
	}

	public void Explore()
	{
		explosionParticles.Play ();
		rocksParticles.Play ();
	}

	public void Convert(Transform origin, Transform target, CreatureController enemy)
	{
		this.enemy = enemy;

		enemy.transform.rotation = Quaternion.LookRotation (origin.position - target.position);

		Vector3 walkingPosition = target.position - ((target.position - origin.position) / GameManager.instance.boardScript.tiles.tileSideSize);

		StartCoroutine (Converting (origin, target, walkingPosition));
	}

	public void FinishExploringAnimation()
	{
		ActionsManager.instance.FinishAction ();
	}

	public void PlayExplosionParticles()
	{
		explosionParticles.Play ();
	}

	public IEnumerator Heal()
	{
		health = ActionsManager.instance.maxHealth;

		WaitForSeconds lerpTime = new WaitForSeconds (0.1f);

		Debug.Log ("Health: " + health);

		while(healthSlider.value != health)
		{
			healthSlider.value += 1;
			Debug.Log ("HealthSlider: " + healthSlider);
			yield return lerpTime;
		}

		GameManager.instance.player [GameManager.instance.activePlayerIndex].Spend (ActionsManager.instance.healingCost);

		HealingBox.SetActive (false);
	}

	public IEnumerator CheckIfConverted(CreatureController savior)
	{
		
		if (influencedByPlayer == savior.belongsToPlayer)
		{
			StopCoroutine (inDoubtBlinkLoop);
			ChangeTeam (savior.belongsToPlayer.playerNumber);
		}
		else if (savior.belongsToPlayer == belongsToPlayer)
		{
			StopCoroutine (inDoubtBlinkLoop);
			ChangeTeam (belongsToPlayer.playerNumber);
		}
		else
		{
			if(inDoubtBlinkLoop != null)
				StopCoroutine (inDoubtBlinkLoop);
			influencedByPlayer = savior.belongsToPlayer;
			inDoubtBlinkLoop = InDoubt ();
			StartCoroutine (inDoubtBlinkLoop);	
			Debug.Log (influencedByPlayer);
		}

		WaitForSeconds exhibitMessageTime = new WaitForSeconds(3f);
		dialogCanvas.DisplayMessageForTime ("Oh Boy...");

		yield return exhibitMessageTime;

	}

	private IEnumerator CheckIfDie (CreatureController killer)
	{
		// TODO: Play animation
		WaitForSeconds lerpTime = new WaitForSeconds (0.1f);

		while(healthSlider.value != health)
		{
			healthSlider.value -= 1;
			yield return lerpTime;
		}

		if(health == 0)
			Destroy (gameObject);
		else 
		{
			HealingBox.SetActive (true);
			StartCoroutine (PleadForHelp ());
		}
		
		killer.FinishedAnimation ();
	}

	private IEnumerator Moving(Transform target)
	{
		transform.rotation = Quaternion.LookRotation (target.position - transform.position);
		while(transform.position!=target.position){
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
			yield return null;
		}
		animatorController.SetTrigger ("IsIdle");


		transform.rotation = Quaternion.identity;
		ActionsManager.instance.FinishAction ();

	}

	private IEnumerator Attacking(Transform origin, Transform target, Vector3 midTarget)
	{
		animatorController.SetTrigger ("Moves");

		transform.rotation = Quaternion.LookRotation (midTarget - transform.position);
		while((transform.position - midTarget).sqrMagnitude > 0.15)
		{
			transform.position = Vector3.Lerp (transform.position, midTarget, Time.deltaTime * speed);
			yield return null;
		}
			
		animatorController.SetTrigger ("Attacks");
		finishedInteractionAnimation = false;
		while(!finishedInteractionAnimation)
		{
			yield return null;
		}

		animatorController.SetTrigger ("IsIdle");
		finishedInteractionAnimation = false;

		StartCoroutine (enemy.CheckIfDie (this));

		while(!finishedInteractionAnimation)
		{
			yield return null;
		}

		if(enemy == null)
		{
			MoveToTarget (target);
		}
		else
		{
			MoveToTarget (origin);
			enemy.transform.rotation = Quaternion.identity;
		}
	}

	private IEnumerator Converting(Transform origin, Transform target, Vector3 midTarget)
	{
		animatorController.SetTrigger ("Moves");

		transform.rotation = Quaternion.LookRotation (midTarget - transform.position);
		while((transform.position - midTarget).sqrMagnitude > 0.15)
		{
			transform.position = Vector3.Lerp (transform.position, midTarget, Time.deltaTime * speed);
			yield return null;
		}
			
		animatorController.SetTrigger ("IsIdle");
		string convertingMessage = "Come to the true " +  "<color=#" + ColorUtility.ToHtmlStringRGB(belongsToPlayer.color) + ">COLOR" + "</color>" + "!";
		WaitForSeconds exhibitMessageTime = new WaitForSeconds(3f);
		dialogCanvas.DisplayMessageForTime (convertingMessage);

		yield return exhibitMessageTime;

		StartCoroutine (enemy.CheckIfConverted (this));

		MoveToTarget (origin);
		enemy.transform.rotation = Quaternion.identity;

	}

	private IEnumerator PleadForHelp()
	{
		// TODO: Make creature plead for help until get healed in random time interval.
		yield return null;

		dialogCanvas.DisplayMessageForTime ("Don't let me die, bro!");
	}

	private IEnumerator InDoubt()
	{
		while(true)
		{
			if(fillSliderImage.color == belongsToPlayer.color)
			{
				fillSliderImage.color = influencedByPlayer.color;
			}
			else
			{
				fillSliderImage.color = belongsToPlayer.color;
			}

			yield return inDoubtBlinkTime;
		}
	}
}
