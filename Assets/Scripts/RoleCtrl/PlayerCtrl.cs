using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour 
{
    CharacterMovement _moment;
    Character _character;
    private void Awake()
    {
        _moment = GetComponent<CharacterMovement>();
        _character = GetComponent<Character>();
    }

    private void Update()
    {
        Ctrl();
    }

    void Ctrl()
    {
        //if (Input.GetKey(GameData.Instance.GetKey(E_InputKey.Up)))
        //{

        //}
        //if (Input.GetKey(GameData.Instance.GetKey(E_InputKey.Down)))
        //{

        //}
        if (Input.GetKey(GameData.Instance.GetKey(E_InputKey.Left)))
        {
            _moment.MoveToPoint(transform.position + Vector3.left);
        }
        if (Input.GetKey(GameData.Instance.GetKey(E_InputKey.Rigth)))
        {
            _moment.MoveToPoint(transform.position + Vector3.right);
        }
        if (Input.GetKeyUp(GameData.Instance.GetKey(E_InputKey.Left))||Input.GetKeyUp(GameData.Instance.GetKey(E_InputKey.Rigth)))
        {
            if (Input.GetKey(GameData.Instance.GetKey(E_InputKey.Left)) || Input.GetKey(GameData.Instance.GetKey(E_InputKey.Rigth)))
                return;
            _moment.MoveEnd();
        }

        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Attack)))
        {
            int skillId = 1010001;
            if (_character.CanCombo(skillId))
            {
                skillId = _character.CurSkill.NextSkillId;
            }
            _moment.Attack(skillId);
        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.LongAttack)))
        {

        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Skill)))
        {

        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Jump)))
        {

        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Blink)))
        {

        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Support)))
        {

        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Bag)))
        {

        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Map)))
        {

        }
    }
}
