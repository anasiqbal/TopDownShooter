using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
[RequireComponent (typeof (WeaponController))]
public class Player : LivingEntity
{
	#region Member Variables

	[Header ("Visual")]
	public Transform modelParent;
	public GameObject prefab_PlayerModel;

	[Header ("Controls")]
	public float moveSpeed;

	[Header ("Base Values")]
	public Vector3 startingPosition;

	// References
	Camera mainCamera;
	PlayerController controller;
	WeaponController weaponController;
	
	GameObject instantiatedPlayer;

	#endregion

	#region Unity Behaviours
	protected override void Start()
	{
		base.Start ();

		mainCamera = Camera.main;
		controller = GetComponent<PlayerController> ();
		weaponController = GetComponent<WeaponController> ();

		//SetupPlayerModel ();
	}

	void Update()
	{
		if(IsActive && !IsDead)
		{
			// Get movement input and pass the resultant velocity to the controller
			Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
			Vector3 moveVelocity = moveInput.normalized * moveSpeed;

			controller.Move (moveVelocity);

			// Get aim/ look direction and pass the look point to the controller
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			Plane groundPlane = new Plane (Vector3.up, Vector3.zero);
			float rayDistance;

			if (groundPlane.Raycast (ray, out rayDistance))
			{
				Vector3 point = ray.GetPoint (rayDistance);
				controller.LookAt (point);
			}

			// Trigger weapon to shoot
			if (Input.GetMouseButton (0))
				weaponController.Shoot ();
		}
	}

	#endregion

	#region Inherited Methods
	public override void Initialize()
	{
		base.Initialize ();
		transform.position = startingPosition;
	}

	protected override void Die()
	{
		base.Die ();

	}

	#endregion

	#region Helper Methods
	void SetupPlayerModel()
	{
		instantiatedPlayer = Instantiate (prefab_PlayerModel, modelParent);
		instantiatedPlayer.transform.localScale = Vector3.one;
		instantiatedPlayer.transform.localPosition = new Vector3 (0, 1, 0);
	}

	#endregion
}
