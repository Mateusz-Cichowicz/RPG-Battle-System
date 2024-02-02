using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public List<Character> characters;
    public List<Character> playerTeam;
    public List<Character> enemyTeam;
    public Character currentTurnCharacter;

    [SerializeField] private Avatar imageCharacterPrefab;
    [SerializeField] private GameObject panelTurnOrder;
    [SerializeField] private ButtonController buttonController;

    private const float maxAlphaValue = 1f;
    private const float halfAlphaValue = 0.5f;

    private void Start()
    {
        StartBattle();
        if (buttonController == null) 
        {
            Debug.Log("ButtonCotnroller not assigned in Unity Editor.");
        }
    }
    private void StartBattle()
    {
        characters.AddRange(FindObjectsOfType<Character>());
        characters = characters.OrderByDescending(x => x.speed).ToList();
        foreach(Character character in characters) 
        {
            buttonController.CreateButtons(character);

            GameObject temp = Instantiate(imageCharacterPrefab.gameObject);
            temp.transform.SetParent(panelTurnOrder.transform, false);
            temp.GetComponent<Image>().sprite = character.turnOrderSprite;
            character.avatar = temp.GetComponent<Avatar>();
            temp.GetComponent<Avatar>().character = character;
            
        }
        currentTurnCharacter = characters.First();

        InitializeTurnCharacter();
    }

    private void InitializeTurnCharacter()
    {
        currentTurnCharacter.obstacle.enabled = false;
        currentTurnCharacter.agent.enabled = true;

        buttonController.character = currentTurnCharacter;
        currentTurnCharacter.panelActions.SetActive(true);
    }
    public void EndBattle()
    {
        // Check for win/loss conditions and end the battle
    }

    public void ProgressToNextTurn()
    {
        currentTurnCharacter.CancelAbility();
        currentTurnCharacter.isWaitingForResponse = false;
        int characterIndex = characters.IndexOf(currentTurnCharacter);
        currentTurnCharacter.ResetPath();
        currentTurnCharacter.agent.enabled = false;
        currentTurnCharacter.obstacle.enabled = true;
        currentTurnCharacter.avatar.ChangeAlpha(halfAlphaValue);
        currentTurnCharacter.panelActions.SetActive(false);

        if (characterIndex + 1 == characters.Count)
        {
            characterIndex = 0;
            foreach(Character character in characters) 
            {
                character.avatar.ChangeAlpha(maxAlphaValue);
            }
        }
        else 
        {
            characterIndex += 1;
        }

        currentTurnCharacter = characters[characterIndex];

        InitializeTurnCharacter();

        currentTurnCharacter.avaibleMovement = currentTurnCharacter.moveDistance;
        currentTurnCharacter.remainingNumberOfAbilityCasts = currentTurnCharacter.maxNumberOfAbilityCasts;
    }

}
