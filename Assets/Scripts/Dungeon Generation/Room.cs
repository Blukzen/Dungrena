using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
    public Exits[] exits;
}

public enum Exits {
    UP, DOWN, LEFT, RIGHT
}
