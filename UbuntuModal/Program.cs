using Terminal.Gui;

static class Program
{
    public static ListView lstView = new();
    public static List<string> logList = [];
    public static Dialog? dialog;

    static void Main()
    {
        Application.Init();
        Toplevel? top = Application.Top;
        Button? btnStart = new("Start Process");
        btnStart.Clicked += () => {
            RegisterLog("Starting logging...");
            Application.MainLoop.AddTimeout(TimeSpan.FromMilliseconds(100), PrepareBackgroundProcess);
            StartBackgroundProcessDialog();
            RegisterLog("Finished logging...");
        };
        lstView = new ListView(logList)
        {
            Y = Pos.Y(btnStart) + 1,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        Window? win = new("Background Process Sample");
        win.Add(btnStart, lstView);
        top.Add(win);

        static bool PrepareBackgroundProcess(MainLoop _)
        {
            if (dialog != null)
            {
                Application.MainLoop.Invoke(async () => {
                    await RunBackgroundProcessAsync();
                });
            }
            return dialog == null;
        }

        Application.Run();
        Application.Shutdown();
    }

    static async Task RunBackgroundProcessAsync()
    {
        RegisterLog("Background Process is running...");
        await Task.Delay(1000);
        Application.RequestStop();
        RegisterLog("Background Process is finished...");
    }

    static void StartBackgroundProcessDialog()
    {
        RegisterLog("Starting the modal dialog...");
        dialog = new Dialog("This will be closed after the process is finished.");
        Application.Run(dialog);
        RegisterLog("Stopping the modal dialog...");
    }

    static void RegisterLog(string msg)
    {
        logList.Add(msg);
        lstView.MoveDown();
    }
}
