public interface IGun
{
    bool SetAmmo(int ammo, int magazine);

    int GetMagazine();

    int GetAmmo();

    void Fire();
}
