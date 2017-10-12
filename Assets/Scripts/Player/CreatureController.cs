using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureController : MonoBehaviour {
	public Animator animatorController;
	public Slider healthSlider;
	public Image fillSliderImage;
	public DialogScript dialogCanvas;
	public Transform creatureTransform;
	public GameObject shield;
	public GameObject HealingBox;
	public OppressCount oppressScript;
	public TileController occupiedTile;
	public float speed = 0.1f;

	[HideInInspector] public bool moved;
	[HideInInspector] public bool isTired;
	[HideInInspector] public PlayerController belongsToPlayer;
	[HideInInspector] public PlayerController influencedByPlayer;
	[HideInInspector] public PlayerController oppressedByPlayer;

	private float inDoubtBlinkTimeValue = 1f;
	private int health;
	private int defendingDamage;
	private bool finishedInteractionAnimation = false;
	private bool isDefending;
	private IEnumerator inDoubtBlinkLoop;
	private ParticleSystem explosionParticles;
	private ParticleSystem rocksParticles;
	private CreatureController enemy;
	private WaitForSeconds inDoubtBlinkTime;

	void Awake()
	{
		animatorController.SetTrigger ("IsIdle");

		TurnDefense (false);

		HealingBox.SetActive (false);

		inDoubtBlinkTime = new WaitForSeconds (inDoubtBlinkTimeValue);

		moved = false;
		isTired = false;
	}

	void Start()
	{
		health = GameManager.instance.maxHealth;
		healthSlider.maxValue = GameManager.instance.maxHealth;
		healthSlider.value = health;

		defendingDamage = ActionsManager.instance.defendingDamage;

		explosionParticles = Instantiate (GameManager.instance.boardScript.explosionParticles, transform.position, Quaternion.identity, transform) as ParticleSystem;
		rocksParticles = Instantiate (GameManager.instance.boardScript.rockExplorationParticles, transform.position, Quaternion.identity, transform) as ParticleSystem;
	}

	public IEnumerator Heal()
	{
		health = GameManager.instance.maxHealth;

		WaitForSeconds lerpTime = new WaitForSeconds (0.1f);


		while(healthSlider.value != health)
		{
			healthSlider.value += 1;
			yield return lerpTime;
		}

		GameManager.instance.player [GameManager.instance.activePlayerIndex].Spend (ActionsManager.instance.healingCost);

		HealingBox.SetActive (false);
	}

	public void ChangeTeam(PlayerController player)
	{
		if(belongsToPlayer!=null)
			belongsToPlayer.LoseCreature (this);

		player.ControllCreature (this);

		belongsToPlayer = player;
		influencedByPlayer = null;
		oppressedByPlayer = null;
		fillSliderImage.color = player.color;
	}

	public IEnumerator MoveToTarget (TileController targetTile)
	{
		Transform target = targetTile.spawnPoint;
		yield return StartCoroutine (Moving (target));

		creatureTransform.rotation = Quaternion.identity;

		// Tile x Creature assignments.
		occupiedTile.creature = null;
		targetTile.creature = this;
		occupiedTile = targetTile;

		SetToTired (true);

		ActionsManager.instance.FinishAction ();
	}

	public CreatureController DuplicateToTarget (TileController targetTile)
	{
		Transform target = targetTile.spawnPoint;

		Vector3 newPosition = transform.position + (0.3f * (target.position - transform.position));

		oppressScript.SetAllActive ();
		GameObject instance = Instantiate (gameObject, newPosition, Quaternion.identity) as GameObject;
		oppressScript.SetAllFalse ();

		CreatureController newCreature = instance.GetComponent <CreatureController> ();
		newCreature.ChangeTeam (belongsToPlayer);

		// Assign creature to new tile.
		newCreature.occupiedTile = targetTile;

		//SetToTired (false);

		return newCreature;
	}

	public IEnumerator Attack(TileController originTile, TileController targetTile)
	{
		enemy = targetTile.creature;

		yield return StartCoroutine (ApproachTarget (originTile, targetTile));

		animatorController.SetTrigger ("Attacks");
		finishedInteractionAnimation = false;
		while(!finishedInteractionAnimation)
		{
			yield return null;
		}

		if(enemy.isDefending)
		{
			StartCoroutine (TakeDamage (defendingDamage));
			StartCoroutine (enemy.TakeDamage (defendingDamage));
		}
		else
		{
			StartCoroutine (enemy.TakeDamage (GameManager.instance.maxHealth));
		}

		if (enemy.CheckIfDie ())
		{
			yield return StartCoroutine (enemy.Die ());
		}
		else
		{
			targetTile = originTile;
			enemy.creatureTransform.rotation = Quaternion.identity;
		}

		if(CheckIfDie ())
		{
			ActionsManager.instance.FinishAction ();
		}
		else
		{
			yield return StartCoroutine ( MoveToTarget (targetTile));
			SetToTired (false);
		}

	}

	public IEnumerator Convert(TileController originTile, TileController targetTile)
	{
		enemy = targetTile.creature;

		yield return StartCoroutine (ApproachTarget (originTile, targetTile));

		/* TODO: Set converting animation.
		animatorController.SetTrigger ("Converts");
		finishedInteractionAnimation = false;
		while(!finishedInteractionAnimation)
		{
			yield return null;
		}
		*/

		Color newColor = GameManager.instance.player [GameManager.instance.activePlayerIndex].color;
		string convertingMessage = "Come to the true " +  "<color=#" + ColorUtility.ToHtmlStringRGB(newColor) + ">COLOR" + "</color>" + "!";

		yield return StartCoroutine (dialogCanvas.DisplayMessageForTime (convertingMessage));

		enemy.CheckIfConverted (this);

		StartCoroutine(MoveToTarget(originTile));

		enemy.creatureTransform.rotation = Quaternion.identity;
	}

	public IEnumerator Oppress(TileController originTile, TileController targetTile)
	{
		enemy = targetTile.creature;

		yield return StartCoroutine (ApproachTarget (originTile, targetTile));

		/* TODO: Set oppressing animation.
		animatorController.SetTrigger ("Oppresses");
		finishedInteractionAnimation = false;
		while(!finishedInteractionAnimation)
		{
			yield return null;
		}
		*/

		string oppressingMessage = "RESPECT MA' AUTHORITA'!";

		yield return StartCoroutine (dialogCanvas.DisplayMessageForTime (oppressingMessage));

		PlayerController oppressorPlayer = GameManager.instance.player [GameManager.instance.activePlayerIndex];

		if(belongsToPlayer == enemy.belongsToPlayer)
		{
			enemy.oppressScript.Unoppress ();

			enemy.oppressedByPlayer.controlledCreatures.Remove (enemy);
			enemy.belongsToPlayer.GetBackOppressedCreature (enemy);

			enemy.oppressedByPlayer = null;
		}
		else
		{
			enemy.oppressScript.Oppress (oppressorPlayer.color);

			oppressorPlayer.controlledCreatures.Add (enemy);
			enemy.belongsToPlayer.CreatureGetOppressed (enemy);

			enemy.oppressedByPlayer = oppressorPlayer;
		}

		if (!GameManager.instance.oppressedCreatures.Contains (enemy)) 
		{
			GameManager.instance.oppressedCreatures.Add (enemy);
		}

		StartCoroutine(MoveToTarget(originTile));

		enemy.creatureTransform.rotation = Quaternion.identity;
	}

	public IEnumerator TakeDamage(int damage)
	{
		health -= damage;

		WaitForSeconds lerpTime = new WaitForSeconds (0.1f);

		while(healthSlider.value != health)
		{
			healthSlider.value -= 1;
			yield return lerpTime;
		}
	}

	public bool IsDefending()
	{
		return isDefending;
	}

	public void TurnDefense(bool defenseState)
	{
		shield.SetActive (defenseState);
		isDefending = defenseState;

		SetToTired (true);
	}

	public void Explore()
	{
		explosionParticles.Play ();
		rocksParticles.Play ();

		SetToTired (false);
	}

	public void FinishExploringAnimation()
	{
		animatorController.SetTrigger ("IsIdle");
		ActionsManager.instance.FinishAction ();
	}

	public void PlayExplosionParticles()
	{
		explosionParticles.Play ();
	}

	public void CheckIfConverted(CreatureController savior)
	{
		PlayerController saviorPlayer = savior.belongsToPlayer;
		if(savior.oppressedByPlayer!=null)
		{
			saviorPlayer = savior.oppressedByPlayer;
		}

		if(influencedByPlayer == null)
		{
			saviorPlayer.attemptingToConvert.Add (this);
			influencedByPlayer = saviorPlayer;
			inDoubtBlinkLoop = InDoubt ();
			StartCoroutine (inDoubtBlinkLoop);
		}
		else if (influencedByPlayer == saviorPlayer)
		{

			StopCoroutine (inDoubtBlinkLoop);

			saviorPlayer.attemptingToConvert.Remove (this);
			belongsToPlayer.LoseCreature (this);

			ChangeTeam (saviorPlayer);

			oppressScript.Unoppress ();
			oppressedByPlayer = null;
		}
		else if (savior.belongsToPlayer == belongsToPlayer)
		{
			influencedByPlayer.attemptingToConvert.Remove (this);
			StopCoroutine (inDoubtBlinkLoop);
			ChangeTeam (belongsToPlayer);
		}
		else
		{
			StopCoroutine (inDoubtBlinkLoop);

			influencedByPlayer.attemptingToConvert.Remove (this);
			saviorPlayer.attemptingToConvert.Add (this);
	
			influencedByPlayer = saviorPlayer;
			inDoubtBlinkLoop = InDoubt ();
			StartCoroutine (inDoubtBlinkLoop);	
		}
			
		StartCoroutine ( dialogCanvas.DisplayMessageForTime ("Oh Boy..."));
	}

	public void SetToTired(bool halfTired)
	{
		
		/*if(halfTired)
		{
			if(moved)
			{
				moved = false;
				isTired = true;
			}
			else
			{
				moved = true;
			}
		}
		else
		{
			isTired = true;
		}

		if(isTired)
			animatorController.SetTrigger ("IsTired");

		if(!GameManager.instance.tiredCreatures.Contains (this))
			GameManager.instance.tiredCreatures.Add (this);
*/

	}

	public void FinishedAnimation()
	{
		finishedInteractionAnimation = true;
	}

	private bool CheckIfDie ()
	{
		if (health == 0) 
		{
			if(GameManager.instance.oppressedCreatures.Contains (this))
			{
				GameManager.instance.oppressedCreatures.Remove (this);
			}
			GameManager.instance.tiredCreatures.Remove (this);
			return true;
		}
		else if(health < GameManager.instance.maxHealth)
		{
			HealingBox.SetActive (true);
			StartCoroutine (PleadForHelp ());
		}
		return false;
	}

	private IEnumerator Die()
	{
		// TODO: Play animation
		PlayExplosionParticles ();
		yield return new WaitForSeconds (0.8f);
		belongsToPlayer.LoseCreature (this);
		Destroy (gameObject);
	}

	private IEnumerator Moving(Transform target)
	{
		animatorController.SetTrigger ("Moves");

		creatureTransform.rotation = Quaternion.LookRotation (target.position - transform.position);

		while(transform.position!=target.position){
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, target.position, step);
			yield return null;
		}

		animatorController.SetTrigger ("IsIdle");
	}

	private IEnumerator ApproachTarget (TileController originTile, TileController targetTile)
	{
		Transform origin = originTile.spawnPoint;
		Transform target = targetTile.spawnPoint;

		enemy.creatureTransform.rotation = Quaternion.LookRotation (origin.position - target.position);

		Vector3 walkingPosition = target.position - ((target.position - origin.position) / GameManager.instance.boardScript.tiles.tileSideSize);

		animatorController.SetTrigger ("Moves");
		creatureTransform.rotation = Quaternion.LookRotation (target.position - walkingPosition);

		while((transform.position - walkingPosition).sqrMagnitude > 0.15)
		{
			transform.position = Vector3.Lerp (transform.position, walkingPosition, Time.deltaTime * speed);
			yield return null;
		}

		animatorController.SetTrigger ("IsIdle");
	}

	private IEnumerator PleadForHelp()
	{
		// TODO: Make creature plead for help until get healed in random time interval.
		WaitForSeconds randomTime;
		do {
			randomTime = new WaitForSeconds(Random.Range (5f, 30f));
			dialogCanvas.DisplayMessageForTime ("Don't let me die, bro!");
			yield return randomTime;
		} while(health != GameManager.instance.maxHealth);
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
