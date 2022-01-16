using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CoroutineExtensions {
    public static IEnumerator WaitAll(this MonoBehaviour monoBehaviour, params IEnumerator[] enumerators) {
        return enumerators.Select(monoBehaviour.StartCoroutine).ToArray().GetEnumerator();
    }
}
