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
        int counter = 0;
        foreach(string song in inventory.learnedSongs){
            if(song == songToAdd){
                counter++;
            }
        }
        if(counter == 0){
            inventory.learnedSongs.Add(songToAdd);
            inventory.inventoryDisplay.UpdateMovesAndSongs();
        }
        
    }
}
