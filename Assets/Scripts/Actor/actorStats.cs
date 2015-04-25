using UnityEngine;
using System.Collections;

public class actorStats : MonoBehaviour {
	//current value of main stats
	float stability; //health
	float energy; //energy
	float fluid; //speed

	//base stats
	float durability;
	float capacitance;
	float volume;
	float aptitude;
	float precision;

	//MAX value of main stats
	float stabilityMax; //health
	float energyMax; //energy
	float fluidMax; //speed
	
	//Gimp = exact amount of stat reduction 1:1
	float stabilityGimp;
	float energyGimp;
	float fluidGimp;

	void Start() {
		//TODO: get actor template index & set initial stat values

	}

	void calculateStats() {
		//TODO: poll all equipment & currently running buffs.
	}

}