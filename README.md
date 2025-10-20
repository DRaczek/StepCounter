# StepUp

StepUp is an android application built with .NET MAUI designed to track your daily steps, convert them into distance and calories, and visualize your activity history. The app uses `Plugin.Maui.Pedometer` for step counting and some of Syncfusion components for UI.

---

## Features

-   Real-time step tracking.
-   Conversion of steps into distance (km) and calories burned.
-   Daily progress displayed with a **circular progress bar**.
-   Step history: view the last few days on the home page and browse the full history on the history page.
-   Ability to set a daily step goal on the Settings page.
-   Pedometer works in the background as a foreground service
-   Buffered data is saved to the database every 1 full minute

---

## Technologies

-   [.NET MAUI](https://learn.microsoft.com/dotnet/maui/)
-   [CommunityToolkit.MVVM](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/)
-   [Plugin.Maui.Pedometer](https://github.com/jfversluis/Plugin.Maui.Pedometer)
-   [SQLite-net-pcl](https://github.com/praeclarum/sqlite-net) for local step storage
-   [Syncfusion MAUI Components](https://github.com/syncfusion/maui-toolkit) – Carousel, Charts, ProgressBar, SfNumericEntry

> **Note:** `Plugin.Maui.Pedometer` works best on a physical device. Step readings may not work correctly on an emulator.

---
## Project Structure

#### `StepCounter/`
- `Views/`
- `ViewModels/`
- `Services/`
- `Data/`
- `Converters/`
- `Resources/`

---

## Settings

-   Set your daily step goal in the `SettingsPage`.
-   Daily progress, distance, and calorie calculations will update automatically based on your goal.

---

## Permissions

The app requests all necessary permissions.

-   `ACTIVITY_RECOGNITION`
-   `POST_NOTIFICATIONS`

---

## Usage Notes

-   Recent days are displayed on the main page.
-   Full History can be viewed on the `HistoryPage` with a carousel and chart visualizations.
-   Converters are used to calculate distance and calories from steps for UI display.
-   The circular progress bar reflects your daily steps versus the user-defined goal.

---

## Known bugs and issues

-   Foreground service is being terminated by the system at some point (don't know why yet)
-  Permissions on the first launch are not applied correctly; sometimes the app needs to be restarted twice along with the foreground service for it to work as intended.

> **Note:** UI was tested on only on 3 devices and the functionalities only on 1 (Galaxy M35). Since i don't have access to any IOS device I couldn't implement nor test the app on such device.
