using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Weather.Tests.Tools
{
  internal class CoroutineRunner : MonoBehaviour
  {
    private static readonly Dictionary<string, bool> CoroutineStatuses = new();
    private static CoroutineRunner _instance;
    private static CancellationToken _cancellationToken;

    public static CoroutineRunner Instance =>
      _instance ??= new GameObject("CoroutineRunner").AddComponent<CoroutineRunner>();

    public static bool IsRunning(Func<CancellationToken, IEnumerator> coroutine)
    {
      string coroutineName = nameof(coroutine);
      return CoroutineStatuses.ContainsKey(coroutineName) && CoroutineStatuses[coroutineName];
    }

    public void RunCoroutine(Func<CancellationToken, IEnumerator> coroutine, CancellationToken cancellationToken)
    {
      CoroutineStatuses[nameof(coroutine)] = true;
      Instance.StartCoroutine(CoroutineWrapper(coroutine, cancellationToken));
    }

    private static IEnumerator CoroutineWrapper(Func<CancellationToken, IEnumerator> coroutine,
      CancellationToken cancellationToken)
    {
      yield return coroutine(cancellationToken);
      CoroutineStatuses[nameof(coroutine)] = false;
    }
  }
}