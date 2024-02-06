public class OpenMenu : NotifyerEventBase<UIMenus> { };
public class ApplicationStateChanged : NotifyerEventBase<ApplicationStates> { }; // Notify only from the GameManager
public class InGamePlayStateChanged : NotifyerEventBase<InGamePlayStates> { };
