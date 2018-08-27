using System;

namespace NotAMetroidGame
{
    public class Weapon
    {
        public String name;
        //How long the invulnerability period is after being damaged by this weapon.
        public int invulnTime;
        public int damage;

        //Default invuln time for player
        private static readonly int PLAYER_INVULN = 800;

        public Weapon(String name, int invulnTime, int damage)
        {
            this.name = name;
            this.damage = damage;
            if (invulnTime < 0)
                this.invulnTime = PLAYER_INVULN;
            else
                this.invulnTime = invulnTime;
        }
    }
}

