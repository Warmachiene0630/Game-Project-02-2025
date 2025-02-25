using UnityEngine;

public interface IPickUp
{
    public void getGunStats(GunStats gun);

    bool gainHealth(int amount);
}