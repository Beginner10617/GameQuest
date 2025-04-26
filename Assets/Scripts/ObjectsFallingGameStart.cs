using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsFallingGameStart : MonoBehaviour
{
    public Spawner _spawner;
    public GameObject _boundaryOfGameArea;
    public Interaction _interaction;
    public GameObject timer;

    public void startGame()
    {
        _spawner.enabled = true;
        _boundaryOfGameArea.SetActive(true);
    }

    public void StopGame()
    {
        _spawner.enabled = false;
        _boundaryOfGameArea.SetActive(false);
        _interaction.StartDialogues();
        timer.SetActive(false);
    }

}
