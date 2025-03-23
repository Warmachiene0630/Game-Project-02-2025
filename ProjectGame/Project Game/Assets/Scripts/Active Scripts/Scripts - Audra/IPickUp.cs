using UnityEngine;

public interface IPickUp
{

    bool gainHealth(int amount);
    public void getGunStats(GunStats gun)
    {

    }
    public void getMeleeStats(meleeStats melee)
    {

    }
    public bool gainFuel(int fuel);
}