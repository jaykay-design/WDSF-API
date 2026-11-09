namespace Wdsf.Api.Client.Models
{
    /// <summary>
    /// Whether a competition entry has a confirmed WDSF coach for the competition's discipline.
    /// </summary>
    public enum ParticipantCoachStatus
    {
        /// <summary>
        /// No confirmed coach for the competition's discipline.
        /// </summary>
        No,

        /// <summary>
        /// At least one confirmed coach who may act as such at the event.
        /// </summary>
        Yes,

        /// <summary>
        /// Every confirmed coach is an adjudicator of a competition in the same event group and
        /// may not act as coach there.
        /// </summary>
        Blocked,
    }
}
