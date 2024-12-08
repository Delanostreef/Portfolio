using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{

	[SerializeField] private int _health;
	[SerializeField] private int _maxHealth;
	[SerializeField] private bool _isDead;
	[SerializeField] private bool _deleteOnDeath;

	//	Events

	[SerializeField] private IntEvent healEvent;
	[SerializeField] private IntEvent damageEvent;
	[SerializeField] private IntEvent deathEvent;

	//
	//	Getters
	//

	public int health { get => _health; }
	public int maxHealth { get => _maxHealth; }
	public bool dead { get => _isDead; }
	public bool deleteOnDeaht { get => _deleteOnDeath; }

	//
	//	Unity Events
	//

	private void Awake()
	{
		if (_health > _maxHealth) _health = _maxHealth;
	}

	//
	//	Methods
	//

	public void Heal( int amount )
	{
		if (amount < 0) throw new System.Exception("Cannot heal negative.");
		if (_isDead) return;


		if (amount + _health > _maxHealth)
			amount = _maxHealth - _health;


		_health += amount;

		healEvent.Invoke(amount);
	}



	public void Damage( int amount )
	{
		if (amount < 0) throw new System.Exception("Cannot damage negative.");
		if (_isDead) return;

		//	Death

		if (_health - amount <= 0)
		{
			amount = _health;
			_health = 0;
			_isDead = true;
			deathEvent.Invoke(amount);
			if (_deleteOnDeath) Destroy(gameObject);
		}

		//	Damage

		else
		{
			_health -= amount;
			damageEvent.Invoke(amount);
		}
	}
}


[System.Serializable]
public class IntEvent : UnityEvent<int> { }