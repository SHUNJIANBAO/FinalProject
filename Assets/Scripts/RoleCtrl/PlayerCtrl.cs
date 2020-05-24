using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public Vector3 SkillOffest;

    CharacterMovement _moment;
    Character _character;
    bool _jumpAttack = false;
    bool _jumpBlink = false;
    private void Awake()
    {
        _moment = GetComponent<CharacterMovement>();
        _character = GetComponent<Character>();
    }

    private void Update()
    {
        if (GameManager.IsLoading)
        {
            return;
        }
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
        if (_character.IsGround)
        {
            _jumpAttack = false;
            _jumpBlink = false;
        }
        if (Input.GetKey(GameData.Instance.GetKey(E_InputKey.Left)))
        {
            _moment.MoveToPoint(transform.position + Vector3.left, SceneConfigManager.Instance.PlayMoveEffect);
        }
        if (Input.GetKey(GameData.Instance.GetKey(E_InputKey.Rigth)))
        {
            _moment.MoveToPoint(transform.position + Vector3.right, SceneConfigManager.Instance.PlayMoveEffect);
        }
        if (Input.GetKeyUp(GameData.Instance.GetKey(E_InputKey.Left)) || Input.GetKeyUp(GameData.Instance.GetKey(E_InputKey.Rigth)))
        {
            if (Input.GetKey(GameData.Instance.GetKey(E_InputKey.Left)) || Input.GetKey(GameData.Instance.GetKey(E_InputKey.Rigth)))
                return;
            _moment.MoveEnd();
        }

        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Attack)))
        {
            int skillId = 0;
            bool beForce = false;
            if (_character.IsGround)
            {
                skillId = 1010001;
            }
            else
            {
                if (_jumpAttack == false)
                {
                    _jumpAttack = true;
                    skillId = 1010010;
                    beForce = true;
                }
                else
                {
                    return;
                }
            }
            if (_character.CanComboSkill(skillId))
            {
                beForce = true;
                skillId = _character.CurSkill.NextSkillId;
                _moment.Attack(skillId, beForce);
            }
            else
            {
                _moment.Attack(skillId, beForce);
            }
        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.LongAttack)))
        {
            int skillId = 0;
            if (_character.IsGround)
            {
                skillId = 1010005;
            }
            else
            {
                return;
            }
            if (_character.CanComboSkill(skillId))
            {
                skillId = _character.CurSkill.NextSkillId;
                _moment.Attack(skillId, true);
            }
            else
            {
                _moment.Attack(skillId);
            }
        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Skill)))
        {
            if (GameManager.IsTimeStop || !_character.CheckCanChangeStatus(E_CharacterFsmStatus.Attack, true)) return;
            if (_character.CurStatus == E_CharacterFsmStatus.Attack)
            {
                if (!_character.CanCombo())
                {
                    return;
                }
            }
            if (_character.IsGround)
            {
                SkillOffest.x = _character.IsFaceRight ? SkillOffest.x : -SkillOffest.x;

                _moment.Attack(1010020, true, ()=>
                {
                    GameManager.StopOtherTime(_character, true);
                    GameManager.TimeRatio = 0;
                    Util.RunLater(_character, () =>
                    {
                        GameManager.TimeRatio = 1;
                        GameManager.SetCurSceneGray(false);
                        GameManager.StopOtherTime(_character, false);
                    }, 5);
                }, () =>
                 {
                     CameraManager.Ripple(_character.transform.position + SkillOffest);
                     GameManager.SetCurSceneGray(true);
                 });
            }
        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Jump)))
        {
            if (_character.IsGround)
            {
                if (_character.CurStatus == E_CharacterFsmStatus.Attack)
                {
                    _moment.Jump(15, _character.CanCombo(), SceneConfigManager.Instance.PlayJumpDownEffect);
                }
                else
                {
                    _moment.Jump(15, false, SceneConfigManager.Instance.PlayJumpDownEffect);
                }
            }
        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Blink)))
        {
            if (!_character.CheckCanChangeStatus(E_CharacterFsmStatus.Attack, true)) return;
            if (_character.CurStatus == E_CharacterFsmStatus.Attack)
            {
                if (!_character.CanCombo())
                {
                    return;
                }
            }
            if (Input.GetKey(GameData.Instance.GetKey(E_InputKey.Left)))
            {
                _character.LookToTarget(_character.transform.position + Vector3.left);
            }
            if (Input.GetKey(GameData.Instance.GetKey(E_InputKey.Rigth)))
            {
                _character.LookToTarget(_character.transform.position + Vector3.right);
            }
            if (_character.IsGround)
            {
                _moment.Attack(1010011, true);
            }
            else
            {
                if (!_jumpBlink)
                {
                    _jumpBlink = true;
                    _moment.Attack(1010011, true);
                }
                else
                {
                    return;
                }
            }
            //if (_character.CanInterruptStatus())
            //{

            //    _character.ChangeStatus(E_CharacterFsmStatus.Blink, true);
            //}
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
