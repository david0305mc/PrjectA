using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
{
    private Coroutine produceCoroutine;
    public void StartGame()
    {
        return;
        if (produceCoroutine != null)
            StopCoroutine(produceCoroutine);
        produceCoroutine = StartCoroutine(ProduceGoldCoroutne());
    }
    IEnumerator ProduceGoldCoroutne()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            UserDataStore.Instance.ProduceGold();
        }
    }
}
