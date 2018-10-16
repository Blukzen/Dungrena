using UnityEngine;
using System.Collections;

public interface IEntitySensor {
    bool CanSeeTarget { get; }
}
