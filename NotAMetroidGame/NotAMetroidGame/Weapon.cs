using System;

namespace NotAMetroidGame
{
    public class Weapon
    {
        public String name;
        public int attackTime;
        public int damage;

        public Weapon(String name, int attackTime, int damage)
        {
            this.name = name;
            this.attackTime = attackTime;
            this.damage = damage;
        }
    }
}

