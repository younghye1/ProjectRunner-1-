using System.Runtime.InteropServices;
using UnityEngine;
using System.Collections.Generic;

public static class PlayerStateExtension
{

public static bool HasAny(this PlayerState state, params PlayerState[] compares)
{
    foreach( var c in comparses)
        if((state & c))
        return true;

        return false;


}
}
