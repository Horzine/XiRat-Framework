namespace Xi.Gameplay.Character
{
/*
ICharacterController 接口
这是一个角色控制器的接口，定义了一些控制角色移动和属性的方法和属性。

SetSpeed(float walkSpeed, float runSpeed, float tacticalSprintSpeed): 设置角色的移动速度。
ResetSpeed(): 重置角色的移动速度。
Sensitivity: 获取或设置角色的灵敏度。
SprintSpeed: 获取角色的冲刺速度。
WalkSpeed: 获取角色的行走速度。
TacticalSprintSpeed: 获取角色的战术冲刺速度。
TacticalSprintAmount: 获取角色的战术冲刺量。
MaxedCameraRotation(): 检查摄像机是否旋转到了最大角度。
*/
    public interface ICharacterController
    {
        void SetSpeed(float walkSpeed, float runSpeed, float tacticalSprintSpeed);
        void ResetSpeed();
        float Sensitivity { get; }
        float SprintSpeed { get; }
        float WalkSpeed { get; }
        float TacticalSprintSpeed { get; }
        float TacticalSprintAmount { get; }
        bool MaxedCameraRotation();
    }
}