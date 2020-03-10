using System;

public class MonoBehaviourRuntime : MonoSingleton<MonoBehaviourRuntime> 
{
    public Action OnUpdate;
    public Action OnFixedUpdate;
    public Action OnLateUpdate;

    private void Update()
    {
        OnUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate?.Invoke();
    }

    private void LateUpdate()
    {
        OnLateUpdate?.Invoke();
    }
}
