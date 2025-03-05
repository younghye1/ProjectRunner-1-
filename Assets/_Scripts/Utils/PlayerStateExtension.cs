

public static class PlayerStateExtension
{
    public static bool HasAny(this PlayerState state, params PlayerState[] compares)
    {
        foreach( var c in compares )
            if((state & c) != 0)
                return true;

        return false;
    }
}
