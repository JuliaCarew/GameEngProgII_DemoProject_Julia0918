

public interface IState
{
    // called when the game state is entered. This method should handle initialization logic specific to the state
    void EnterState();
    // called once per physics frame to update the state. This method should handle physics-related updates and logic

    void FixedUpdateState();
    // called once per frame to update the state. This method should handle regular updates suchas input handling and game logic
    void UpdateState();

    // called once per frame after all UpdateState and FixedUpdateState calls. This method should handle any late updates that need to occur after the main update logic
    void LateUpdateState();
    
    void ExitState();
}
