using System.Collections;
using System.Runtime.CompilerServices;

public static class GlobalID
{
    private static int id = 0;

    public static int GetID()
    {
        id++;
        return id;
    }
}
