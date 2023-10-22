using System.Collections;
using UnityEngine;

namespace Common.Infrastructure
{
  public interface ICoroutineRunner
  {
    Coroutine StartCoroutine(IEnumerator coroutine);
  }
}