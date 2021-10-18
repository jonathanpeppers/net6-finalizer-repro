# net6-finalizer-repro

Repro for: https://github.com/xamarin/xamarin-android/pull/6363#issuecomment-937379007

This repro's for me with:

    > dotnet --version
    6.0.100-rc.2.21505.57

If you have the `android` workload installed, you should be able to just do:

    > dotnet build -t:Run

The app crashes with:

    10-18 11:28:50.825 15507 15538 I DOTNET  : Actual Values: i=98, d=False, f=True
    10-18 11:28:50.835 15507 15507 D app_process32: ---- DEBUG ASSERTION FAILED ----
    10-18 11:28:50.835 15507 15507 D app_process32: ---- Assert Short Message ----
    10-18 11:28:50.835 15507 15507 D app_process32: f should be true
    10-18 11:28:50.835 15507 15507 D app_process32: ---- Assert Long Message ----
    10-18 11:28:50.835 15507 15507 D app_process32:
    10-18 11:28:50.835 15507 15507 D app_process32:    at System.Diagnostics.DebugProvider.Fail(String message, String detailMessage)
    10-18 11:28:50.835 15507 15507 D app_process32:    at System.Diagnostics.Debug.Fail(String message, String detailMessage)
    10-18 11:28:50.835 15507 15507 D app_process32:    at System.Diagnostics.Debug.Assert(Boolean condition, String message, String detailMessage)
    10-18 11:28:50.835 15507 15507 D app_process32:    at System.Diagnostics.Debug.Assert(Boolean condition, String message)
    10-18 11:28:50.835 15507 15507 D app_process32:    at net6_finalizer_repro.MainActivity.Dispose_Finalized(Int32 i)
    10-18 11:28:50.835 15507 15507 D app_process32:    at net6_finalizer_repro.MainActivity.<OnCreate>b__0_0(Int32 i)
    10-18 11:28:50.835 15507 15507 D app_process32:    at System.Threading.Tasks.Parallel.<>c__DisplayClass19_0`1[[System.Object, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]].<ForWorker>b__1(RangeWorker& currentWorker, Int32 timeout, Boolean& replicationDelegateYieldedBeforeCompletion)

I used an x86_64 API 28 emulator, but I would guess anything x86_64 would hit this issue.

If you uncomment the `UpdateMonoRuntimePacks` MSBuild target, it uses
a version of the Mono runtime packs that don't have the issue.
