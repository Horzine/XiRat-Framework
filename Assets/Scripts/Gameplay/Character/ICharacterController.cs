namespace Xi.Gameplay.Character
{
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
