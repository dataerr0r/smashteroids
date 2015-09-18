﻿using UnityEngine;
using System.Collections;

public class EnemyShip : MonoBehaviour {

	public Tweakables.EnemyType enemyType = Tweakables.EnemyType.NONE;
	public GameObject explosionEffect;

	private Tweakables.BaseEnemyStats mBaseStats;
	public Tweakables.BaseEnemyStats baseStats {
		get { return mBaseStats; }
	}
	
	private int damageTaken;

	private EnemyShipBaseAI AI;

	public int Points;


	// Use this for initialization
	void Start () {
		mBaseStats = Tweakables.Instance.GetEnemyStatsForEnemy (enemyType);
		AI = GetComponent<EnemyShipBaseAI> ();
	}
	
	// Update is called once per frame
	void Update () {

		// Do AI stuff
		if (AI != null) {
			AI.Brain();
		}
	}

	public bool IsAlive() {
		return (damageTaken < baseStats.hp);
	}

	private void Die() {
		Dispatcher.FireEvent<EnemyDeathEvent> (this, new EnemyDeathEvent (this));

		if(explosionEffect != null)
		{
			var explosion = (GameObject) GameObject.Instantiate(explosionEffect);
			explosion.transform.position = transform.position;
		}

		GameManager.Instance.Points += Points;
		Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D collider) {
		DamageEnemy hazard = collider.GetComponent<DamageEnemy> ();
		
		if (hazard != null) {
			damageTaken += hazard.damage;
			
			if (damageTaken >= baseStats.hp) {
				Die();
			}
		}
	}
}
