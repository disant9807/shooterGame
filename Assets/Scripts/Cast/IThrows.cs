using UnityEngine;

public interface IThrows
{
    bool SetAmmo(int ammo, int magazine);

    int GetMagazine();

    int GetAmmo();

    void Cast();
}

