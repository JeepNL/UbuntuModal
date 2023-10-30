using Terminal.Gui;

static class Program
{
    public static ListView lstView = new ListView();
    public static List<string> logList = new List<string>();
    public static Dialog? dlg;

    static void Main()
    {
        Application.Init();
        var top = Application.Top;
        var btnStart = new Button("Start Process");
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
        var win = new Window("Background Process Sample");
        win.Add(btnStart, lstView);
        top.Add(win);

        bool PrepareBackgroundProcess(MainLoop _)
        {
            if (dlg != null)
            {
                Application.MainLoop.Invoke(async () => {
                    await RunBackgroundProcessAsync();
                });
            }
            return dlg == null;
        }

        Application.Run();
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
        dlg = new Dialog("This will be closed after the process is finished.");
        Application.Run(dlg);
        RegisterLog("Stopping the modal dialog...");
    }

    static void RegisterLog(string msg)
    {
        logList.Add(msg);
        lstView.MoveDown();
    }
}
