using System;

/**
 * Holds the cost of a given stat upgrade.
 * 
 * @author Joshua Hooker
 * 7 March 2016
 */
public class Stat_Cost {

	public readonly int scrap_cost;
	public readonly int ecore_cost;

	/* Creates a Stat Cost with the given e.core and scrap cost. */
	public Stat_Cost(int ec_cost, int s_cost) {
		scrap_cost = s_cost;
		ecore_cost = ec_cost;
	}
}
