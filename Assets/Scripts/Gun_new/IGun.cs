using UnityEngine;


public interface IGunNews
{
    bool SetAmmo(int ammo, int magazine);

    int GetMagazine();

    int GetAmmo();

    void Fire();
}

