using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    PlayerMovement PM;
    [Header("Text Settings")]
    public TextMeshPro textScript;
    public float textFlatTime;
    public float textMultiplierTime;
    IEnumerator OnGoingTextCoroutine;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        PM = PlayerMovement.instance;
    }

    private void Update()
    {
        if (UF.IsOnUI()) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            OnMouseLeftClick();
        }
    }

    void OnMouseLeftClick()
    {
        Vector2 mousePosition = UF.GetMousePos();
        GameObject somethingUnderMouse = UF.FetchGameObject(UF.GetMousePos(), LayerMask.GetMask("Interractable"));
        if (somethingUnderMouse == null) //Jos hiiren alla ei ole mitään niin liikutaan
        {
            PlayerMovement.instance.Move(mousePosition);
        }
        else
        {
            somethingUnderMouse.GetComponent<Interractable>().GiveActivationCommand();
        }
    }

    public void SayText(string text)
    {
        if (OnGoingTextCoroutine != null)
        {
            StopCoroutine(OnGoingTextCoroutine);
        }
        OnGoingTextCoroutine = TextCoroutine(text);
        StartCoroutine(OnGoingTextCoroutine);
    }

    IEnumerator TextCoroutine(string text)
    {
        textScript.text = text;
        yield return new WaitForSeconds(textFlatTime + textMultiplierTime * text.Length);
        textScript.text = "";
    }

    public void UseItem(Item item)
    {
        Vector2 mousePosition = UF.GetMousePos();
        GameObject somethingUnderMouse = UF.FetchGameObject(UF.GetMousePos(), LayerMask.GetMask("Interractable"));
        if (somethingUnderMouse != null)
        {
            Interractable targetInterractScript = somethingUnderMouse.GetComponent<Interractable>();
            if (targetInterractScript.key == item.name)
            {
                targetInterractScript.GiveActivationCommand();
            }
        }
    }
}
