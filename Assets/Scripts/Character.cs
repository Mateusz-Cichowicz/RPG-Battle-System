using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] private string characterName;
    [Header("Stats")]
    [SerializeField] private int maxHP;
    [SerializeField] private int damage;
    [SerializeField] private int defense;
    [SerializeField] private int range;
    private int currentHP;
    private HealthBar healthBar;
    private BattleManager battleManager;
    private bool isMoving = false;
    private float startingDistance;
    private Renderer renderer;
    private ButtonController buttonController;

    public int speed;
    public float moveDistance = 6f;
    public float avaibleMovement;

    public List<Ability> abilities;
    public NavMeshAgent agent;
    public NavMeshObstacle obstacle;
    public GameObject panelActions;

    public Sprite turnOrderSprite;
    public Avatar avatar { get; set; }
    public bool isWaitingForResponse;

    public int maxNumberOfAbilityCasts = 1;
    public int remainingNumberOfAbilityCasts;
    public Vector3 target;
    private void Start()
    {
        InitializeReferences();
        InitializeAttributes();
    }

    private void InitializeReferences() 
    {
        battleManager = FindAnyObjectByType<BattleManager>();
        healthBar = GetComponentInChildren<HealthBar>();
        renderer = GetComponent<Renderer>();
        buttonController = FindObjectOfType<ButtonController>();
    }
    private void InitializeAttributes()
    {
        currentHP = maxHP;
        healthBar.SetMaxHealth(maxHP);
        avaibleMovement = moveDistance;
        panelActions.SetActive(false);
        remainingNumberOfAbilityCasts = maxNumberOfAbilityCasts;
    }
    private void Update()
    {
        if (battleManager.currentTurnCharacter == this && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleMouseInput();
            CheckMovementCompletion();
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (isWaitingForResponse == true)
                {
                    HandleAbilityTargetSelection(hit);
                }
                else if (hit.collider.CompareTag("Character"))
                {
                    HandleCharacterInteraction(hit);
                }
                else
                {
                    Move(hit, 0);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            HandleRightMouseClick();
        }
    }
    private void HandleCharacterInteraction(RaycastHit hit)
    {
        if (Vector3.Distance(hit.collider.transform.position, transform.position) <= range)
        {
            Attack(hit);
        }
        else
        {
            Move(hit, range);
        }
    }
    private void HandleAbilityTargetSelection(RaycastHit hit)
    {
        target = hit.point;
        buttonController.targetSelected = true;
    }
    private void HandleRightMouseClick()
    {
        if (isMoving)
        {
            agent.isStopped = true;
            MovementControl();
        }

        CancelAbility();
    }

    public void CancelAbility() 
    {
        if (isWaitingForResponse)
        {
            buttonController.abilityCanceled = true;
        }
    }
    private void CheckMovementCompletion()
    {
        if (startingDistance - avaibleMovement >= agent.remainingDistance)
        {
            agent.isStopped = true;
        }
    }
    private IEnumerator GetStartingDistance() 
    {
        yield return new WaitForSeconds(0.1f);
        startingDistance = agent.remainingDistance;
    }

    
    private void MovementControl() 
    {
        if (agent.velocity.magnitude <=0.1f) 
        {
            avaibleMovement -= (startingDistance - agent.remainingDistance);
            isMoving = false;
            CancelInvoke();
        }
    }
    private void Move(RaycastHit hit, int stopingDistance)
    {
        agent.stoppingDistance = stopingDistance - 0.1f;
        agent.isStopped = false;
        agent.SetDestination(hit.point);
        StartCoroutine(GetStartingDistance());
        InvokeRepeating("MovementControl", 0.5f, 0.5f);
        isMoving = true;
    }
    public void ResetPath() 
    {
        agent.ResetPath();
    }
    public void TakeDamage(int damage)
    {
        int calculatedDamage = (damage - defense < 1) ? 1 : damage - defense;
        currentHP -= calculatedDamage;
        healthBar.SetHealth(currentHP);
        if (currentHP < 0)
        {
            currentHP = 0;
        }
    }
    private void Attack(RaycastHit hit)
    {
        if (hit.collider.GetComponent<Character>() != this)
        {
            hit.collider.GetComponent<Character>().TakeDamage(damage);
        }
    }
    public void OnMouseOver()
    {
        avatar.EnableHighlight();
    }

    public void OnMouseExit()
    {
        avatar.DisableHighlight();
    }

    public void Highlight(int value)
    {
        renderer.material.SetFloat("_Metallic", value);
    }
}

