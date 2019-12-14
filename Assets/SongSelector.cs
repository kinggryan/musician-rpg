using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongSelector : MonoBehaviour
{
    NumericInventory inventory;
    public PlayerJamMenu playerJamMenu;

    void Start(){
        playerJamMenu = Object.FindObjectOfType<PlayerJamMenu>();
        inventory = Object.FindObjectOfType<NumericInventory>();
    }

    public void ChangeSong(string songToLoad){
        string newSongString = "Songs/" + songToLoad;
        playerJamMenu = Object.FindObjectOfType<PlayerJamMenu>();
        playerJamMenu.soloSong = newSongString;
        playerJamMenu.jamController.LoadSong(newSongString);
    }

    public void AddSongToInventory(string songToAdd){
        inventory.learnedSongs.Add(songToAdd);
        inventory.inventoryDisplay.UpdateMovesAndSongs();
    }
}
