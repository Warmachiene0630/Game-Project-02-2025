using UnityEngine;

public interface IPickUp
{

    bool gainHealth(int amount);

    public void getGunStats(GunStats gun)
    {

    }

    bool gainFuel(float amount);
}