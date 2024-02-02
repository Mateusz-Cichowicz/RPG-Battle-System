using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Character character { get; set; }
    public bool targetSelected { get; set; }
    public bool abilityCanceled {get; set;}

    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private CursorController cursorController;

    public void RangeAttack()
    {
        Debug.Log("Elo" + character.transform.position);
    }
    public void CreateButtons(Character character) 
    {
        foreach (Ability ability in character.abilities) 
        {
            if (buttonPrefab != null && character.panelActions != null)
            {
                GameObject button = Instantiate(buttonPrefab);
                button.transform.SetParent(character.panelActions.transform);
                button.GetComponent<Button>().onClick.AddListener(() => AbilityPressed(ability));
                button.GetComponent<Image>().sprite = ability.icon;
            }
            else
            {
                Debug.LogError("ButtonPrefab or PanelActions not assigned in Unity Editor.");
            }
        }
    }

    public void AbilityPressed(Ability ability)
    {
        Debug.Log("Firebal Presed");
        if (character.remainingNumberOfAbilityCasts > 0)
        {
            Debug.Log("Firebal Presed 2");
            StartCoroutine(MainAction(ability));
        }
    }
    public IEnumerator MainAction(Ability ability)
    {
        //change cursor
        if (cursorController != null)
        {
            cursorController.ChooseCursor(ability.cursor);
        }
        //disable other mouse1 controlls
        character.isWaitingForResponse = true;
        //wait for player to use mouse1 on target or mouse2 to cancel
        yield return new WaitUntil(() => targetSelected == true || abilityCanceled == true);

        //enable other mouse1 controlls
        character.isWaitingForResponse = false;

        //change cursor back
        if (cursorController != null)
        {
            cursorController.SetMainCursor();
        }
        //use previously chosen ability
        if (targetSelected == true)
        {
            ability.Activate(character);
            character.remainingNumberOfAbilityCasts -= 1;
            targetSelected = false;
        }//cancel ability
        else if(abilityCanceled == true)
        {
            abilityCanceled = false;
            yield break;
        }
    }
}
