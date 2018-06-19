using System;

public class Creature : Entity {
    //This may or may not stay as an int
    private int HP;

    //Reduces damage taken
    private int armor;

	public Creature() {
	}

    /** Adjust this creature's position by adding the movement vector.
     * 
     * This function will likely be where the logic for collision is implemented.
     * 
     **/
    public void Move(Vector2D movement) {

    }

    /** Deals X damage to this creature's HP, returns true if this damage is lethal.
     * 
     * Example: Player lands a hit on enemy x, code checks the stats on the 
     * player's weapon, charatcer, etc and calls x.Damage(howevermuch damage should be done).
     * This function takes into account armor and other stats we decide to consider.
     * 
     * Some enemies may have special effects or resistances when they take damage.
     * For this instance, that enemy can override this function.
     * 
     **/
    public bool Damage(long damage) {

    }

    //*Insert Zelda CD-I reference here*
    public void Die() {

    }
}
