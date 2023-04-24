# ClockApplication
 A clock application made in Unity.
 This application has three core features:
 1. Clock to see the current time.
 2. A timer function.
 3. A stopwatch function.

 Future Developments:

    iOS Support: The current UI is flexible enough to still be functional on iOS screens however in order to ensure the best user experience on device testing must be done to make sure the buttons are easy to click, the text is easy to read, and there are no device specific issues with any of the application's functionality. There might also be some device specific changes that would be desirable such as using the iOS notification system to send a notification when the timer is finished.

    Possible refactors could include using a dependency injection plugin if that would have some benifit with integrating the clock app with other projects. This is likely a like to have unless there is some requriement for using dependency injection.

    Another possible refactor would be to use an event system that classes could use to subscribe to events like the timer went off or the stopwatch was stopped. This might be helpful in some applications that integrate the clock application. This is likely a like to have unless there is a need for this behavior in some app the clock application is being integrated into.

    If this application were to be used in VR then a rework of the UI for that platorm would be needed. The clock application would need to be displayed more like a HUD (Heads up display) and have all the information played to the edges of the screen with the expection of notification modals. The user input would also have to be reworked to work with that platform.

Build:

    There is a build of this application for Windows in the ClockAppBuild. 
