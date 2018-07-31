
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
	Idle, Chasing, Attacking
}

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity
{
	#region Member Variables
	EnemyState currentState;
	public int damage = 1;

	public float attackDistanceThreshold = 0.5f;
	public float timeBetweenAttacks = 1;

	float nextAttackTime;
	float myCollisionRadius;
	float targetCollisionRadius;

	Color orignalColor;

	bool hasTarget;

	// References
	public GameObject enemyModel;
	NavMeshAgent pathFinder;
	Transform target;
	LivingEntity targetEntity;

	Material skinMaterial;
	
	#endregion

	#region Unity Methods
	protected override void Start()
	{
		pathFinder = GetComponent<NavMeshAgent> ();
		skinMaterial = enemyModel.GetComponent<Renderer> ().material;
		orignalColor = skinMaterial.color;

		if(GameObject.FindGameObjectWithTag ("Player") != null)
		{
			currentState = EnemyState.Chasing;
			hasTarget = true;

			target = GameObject.FindGameObjectWithTag ("Player").transform;
			targetEntity = target.GetComponent<LivingEntity> ();
			targetEntity.OnDeath += TargetEntity_OnDeath;

			myCollisionRadius = GetComponent<CapsuleCollider> ().radius;
			targetCollisionRadius = 0.5f;

			StartCoroutine (UpdatePath ());
		}
	}

	private void Update()
	{
		if(hasTarget && !IsDead)
		{
			if (Time.time > nextAttackTime)
			{
				// attack distance threshold is distance from the edge of the both parties rather than the center
				// but as the squared distance calculated below is from the center of both parties
				// therfore, add the radius of both parties
				float squaredDistanceToTarget = (target.position - transform.position).sqrMagnitude;
				if (squaredDistanceToTarget < Mathf.Pow (attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
				{
					nextAttackTime = Time.time + timeBetweenAttacks;
					StartCoroutine (PerformAttack ());
				}
			}
		}
	}

	#endregion

	#region Inherited Methods
	protected override void Die()
	{
		base.Die ();

		pathFinder.enabled = false;
		GetComponent<Collider> ().enabled = false;

		StartCoroutine (PlayDeathSequence ());
	}

	#endregion

	#region Helper Methods - Public
	public bool RemoveSelfFromField()
	{
		// only remove self if dead
		if(IsDead)
		{
			StartCoroutine (PlayDestoryCorpseSequence ());
			return true;
		}

		return false;
	}

	#endregion

	#region Helper Methods - Private
	void TargetEntity_OnDeath()
	{
		hasTarget = false;
		currentState = EnemyState.Idle;
	}

	protected IEnumerator UpdatePath()
	{
		float refreshRate = 0.25f;

		while(hasTarget && !IsDead)
		{
			if(currentState == EnemyState.Chasing)
			{
				Vector3 direction = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - direction * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);

				pathFinder.SetDestination (targetPosition);
			}
			yield return new WaitForSeconds (refreshRate);
		}

		if(!IsDead)
		{
			pathFinder.enabled = false;
		}
	}

	protected IEnumerator PerformAttack()
	{
		currentState = EnemyState.Attacking;
		pathFinder.enabled = false;
		Vector3 originalPosition = transform.position;

		Vector3 direction = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - direction * (myCollisionRadius);

		float attackSpeed = 3;
		float percent = 0;
		bool hasAppliedDamage = false;

		skinMaterial.color = Color.red;

		while(percent <= 1)
		{
			percent += Time.deltaTime * attackSpeed;

			if(percent >= 0.5 && !hasAppliedDamage)
			{
				targetEntity.TakeDamage (damage);
			}

			// using parabola equation y = 4(-x^2 + x)
			// as we need the enemy to perfom attack from original position to target position and back to original position
			float interpolation = (-Mathf.Pow (percent, 2) + percent) * 4;
			transform.position = Vector3.Lerp (originalPosition, attackPosition, interpolation);

			yield return null;
		}

		skinMaterial.color = orignalColor;

		pathFinder.enabled = true;
		currentState = EnemyState.Chasing;
	}

	protected IEnumerator PlayDeathSequence()
	{
		yield return null;
	}

	protected IEnumerator PlayDestoryCorpseSequence()
	{
		yield return null;
		Destroy (gameObject);
	}

	#endregion
}
