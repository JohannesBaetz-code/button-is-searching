namespace StateManagement.PlayState
{
    /// <summary> An Interface which is implemented by all states to change between subscribed classes. </summary>
    /// <author> Johannes Bätz </author>
    public interface IPlayState
    {
        /// <summary> The Method to handle state switches in the classes. </summary>
        /// <param name="playStateManager"> the instance of the PlayStateManager. </param>
        /// <returns> The State which is called in the next frame. </returns>
        IPlayState DoState(PlayStateManager playStateManager);
    }
}