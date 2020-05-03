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
            int skillId = 1010001;
            if (_character.CanCombo(skillId))
            {
                skillId = _character.CurSkill.NextSkillId;
                _moment.Attack(skillId, true);
            }
            else
            {
                _moment.Attack(skillId);
            }
        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.LongAttack)))
        {

        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Skill)))
        {

        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Jump)))
        {
            if (_character.IsGround)
                _moment.Jump(15, false, SceneConfigManager.Instance.PlayJumpDownEffect);
        }
        if (Input.GetKeyDown(GameData.Instance.GetKey(E_InputKey.Blink)))
        {
            _moment.Attack(1010011, true);
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


    /// <summary>
    /// 碰撞时判断是否跳跃
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        var target = collision.transform.GetComponent<Character>();
        if (target != null && _character.CurStatus == E_CharacterFsmStatus.Jump)
        {
            var hit = Physics2D.BoxCast((Vector2)transform.position + _character.BottomOffest, new Vector2(_character.BoxCollider.size.x + 0.1f, 0.2f), 0, Vector2.right, 0, GameConfig.Instance.EnemyMask);
            if (hit&&CanJumpHead(target) )
            {
                _moment.Jump(15, true, SceneConfigManager.Instance.PlayJumpDownEffect);
            }
        }
    }

    bool CanJumpHead(Character target)
    {
        bool onHead = _character.transform.position.y - target.transform.position.y > target.BoxCollider.size.y - 0.2f;
        bool isJumpDown = _character.CurStateInfo.IsName(E_AnimatorIndex.JumpingDown.ToString());
        return onHead && isJumpDown;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawCube(transform.position + (Vector3)_character.BottomOffest, new Vector3(_character.BoxCollider.size.x, 0.2f, 1));
        }
    }
}
